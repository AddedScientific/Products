using System;
using System.IO;
//using System.Data; Doesn't exist?
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using OTB.Engineering.Alignment;
using OTB.Engineering.Conversions;
using OTB.Engineering.Data;
using OTB.Engineering.Facilities;
using OTB.Engineering.HMIConfiguration;
using OTB.Engineering.HMIControls;
using OTB.Engineering.IGalvoJob;
using OTB.Engineering.Interfaces;
using OTB.Engineering.Motion;
using OTB.Engineering.OtbProcess;
using OTB.Engineering.Parameters;
using OTB.Engineering.Printhead;
using OTB.Engineering.Script;
using OTB.Engineering.ScriptHelp;
using OTB.Engineering.Scripts;
using OTB.Engineering.ScriptSupport;
using OTB.Engineering.Vision;

public class Script : BaseScript, IScript {

        //Parameters.Get("PrintHead.State").ParamChanged -= new ParamChangedEventHandler(PrintheadStateChanged);

        private Worker workerObject;

        public class Worker {
            // Reference to owner	
            private Script m_Owner;

            // Create the thread object
            private volatile Thread workerThread;

            // Stop flag
            private volatile bool Stop;

            // Constructor
            public Worker (Script owner) {
                    // Reference to owner
                    m_Owner = owner;

                    // Create the thread object
                    workerThread = new Thread (DoWork);

                    // Create an unsignalled wait handle for the change event for the parameter
                    IParameter parameter = m_Owner.Parameters.Get ("General.ActiveRecipe");
                    IParameter state = ParameterHandler.Instance.Get ("General.MachineState");

                    // Hook to the events
                    parameter.ParamChanged += new ParamChangedEventHandler (ActiveSetFileChanged);
                    state.ParamChanged += new ParamChangedEventHandler (MachineStateChanged);
                    // Start the worker thread
                    workerThread.Start ();

                    // Loop until worker thread activates
                    while (!workerThread.IsAlive);
                }

                // Destructor
                ~Worker () {
                    // Create an unsignalled wait handle for the change event for the parameter
                    IParameter parameter = m_Owner.Parameters.Get ("General.ActiveRecipe");

                    // Cleanup the event
                    ActiveSetFileChanged (parameter);
                }

            private void MachineStateChanged (IParameter e) {
                // If machine-state is changed to shutdown, stop thread
                if (e.Value == "10") {
                    // Create an unsignalled wait handle for the change event for the parameter
                    IParameter parameter = m_Owner.Parameters.Get ("General.ActiveRecipe");

                    // Cleanup the event
                    ActiveSetFileChanged (parameter);
                }
            }

            // Callback for recipe-change
            private void ActiveSetFileChanged (IParameter e) {
                // Always unhook the event since it is not needed after the first change
                e.ParamChanged -= new ParamChangedEventHandler (ActiveSetFileChanged);

                // Request that the worker thread stop itself
                Stop = true;

                ParameterHandler.Instance.SetValue ("General.ProgressText", "Worker Stopped");
                // Block the current thread until the worker-thread terminates
                workerThread.Join ();
            }

            // This method will be called when the thread is started
            public void DoWork () {
                while (true) {
                    Thread.Sleep (1000);
                    int state = m_Owner.Parameters.GetIntValue ("General.MachineStateID");
                    if (state == 5) {
                        TheWork ();
                    }
                }
            }

            public void TheWork () {

                string serial_port = m_Owner.Parameters.GetValue ("HeadAssy.COMPORT");
                SerialPort port = new SerialPort (serial_port, 1000000);
                bool success = false;
                try {
                    port.Open ();
                    success = true;
                } catch (Exception otherProblem) {
                    Logger.Log ("Other exception");
                }

                if (success) {
                    string messageToSend = "B";
                    messageToSend += "\r\n";
                    port.Write (messageToSend);
                    Logger.Log (messageToSend);
                    Thread.Sleep (50);
                    //Check for any data on the port
                    string s = port.ReadExisting ();
                    int drop_count = 0;
                    while (s.Length < 1) {
                        s = port.ReadExisting ();
                        drop_count++;
                        Thread.Sleep (1);
                        if (drop_count > 1000) {
                            break;
                        }
                    }
                    Logger.Log ("RX " + s);
                    port.Close ();

                    port.Open ();
                    messageToSend = "b";
                    messageToSend += "\r\n";
                    port.Write (messageToSend);
                    Logger.Log (messageToSend);
                    Thread.Sleep (50);
                    //Check for any data on the port
                    s = port.ReadExisting ();
                    drop_count = 0;
                    while (s.Length < 1) {
                        s = port.ReadExisting ();
                        drop_count++;
                        Thread.Sleep (1);
                        if (drop_count > 1000) {
                            break;
                        }
                    }
                    Logger.Log ("RX " + s);
                    port.Close ();

                    s = s.Replace ("{", "\n");
					s = s.Replace ("\"", "");
					s = s.Replace ("}", "");
                    s = s.Replace ("[", "");
                    s = s.Replace ("]", "\n");
                    string[] lines = s.Split ('\n');
                    
                    string root_dir = "C:\\LP50\\Configuration\\scripts";
                    string start = "<script src=\"" + root_dir + "\\run_prettify.js\"></script><pre class=\"prettyprint\">";

                    s = start + s;
                    s = s + "<meta http-equiv=\"refresh\" content=\"1\" >";

                    File.WriteAllText (root_dir + "\\index.html", s);

                    foreach (string line in lines) {
                        //Logger.Log(line);
                        for (int headIndex = 1; headIndex < 5; headIndex++) {
                            int foundIndex = line.IndexOf ("head: " + headIndex.ToString ());
                            if (foundIndex == 0) {
                                string[] elements = line.Split (',');
                                foreach (string el in elements) {
                                    string toFind = " voltage:";
                                    if (el.IndexOf (toFind) == 0) {
                                        double res = double.Parse (el.Substring (toFind.Length));
                                        m_Owner.Parameters.SetValue ("Printhead.X128.Voltage[" + (headIndex - 1).ToString () + "]", res);
                                        //Logger.Log(res.ToString());
                                    }
                                    toFind = " curTemperature:";
                                    if (el.IndexOf (toFind) == 0) {
                                        double res = double.Parse (el.Substring (toFind.Length));
                                        m_Owner.Parameters.SetValue ("Printhead.X128.CurrentTemperature[" + (headIndex - 1).ToString () + "]", res);
                                        //Logger.Log(res.ToString());
                                    }
                                    toFind = " setTemperature:";
                                    if (el.IndexOf (toFind) == 0) {
                                        double res = double.Parse (el.Substring (toFind.Length));
                                        m_Owner.Parameters.SetValue ("Printhead.X128.SetTemperature[" + (headIndex - 1).ToString () + "]", res);
                                        //Logger.Log(res.ToString());
                                    }
                                    toFind = " printCounts:";
                                    if (el.IndexOf (toFind) == 0) {
                                        double res = double.Parse (el.Substring (toFind.Length));
                                        m_Owner.Parameters.SetValue ("Printhead.X128.HeadPrintCounts[" + (headIndex - 1).ToString () + "]", res);
                                        //Logger.Log(res.ToString());
                                    }
                                    toFind = " status:";
                                    if (el.IndexOf (toFind) == 0) {
                                        double res = double.Parse (el.Substring (toFind.Length));
                                        m_Owner.Parameters.SetValue ("Printhead.X128.State[" + (headIndex - 1).ToString () + "]", res);
                                        //Logger.Log(res.ToString());
                                    }

                                }
                            }

                            foundIndex = line.IndexOf ("image: " + headIndex.ToString ());
                            if (foundIndex == 0) {
                                string[] elements = line.Split (',');
                                foreach (string el in elements) {
                                    string toFind = " hasData:";
                                    if (el.IndexOf (toFind) == 0) {
                                        double res = double.Parse (el.Substring (toFind.Length));
                                        m_Owner.Parameters.SetValue ("Printhead.X128.ImageData[" + (headIndex - 1).ToString () + "]", res);
                                        //Logger.Log(res.ToString());
                                    }

                                }
                            }

                            foundIndex = line.IndexOf ("encoder:");
                            if (foundIndex == 0) {
                                string[] elements = line.Split (',');
                                foreach (string el in elements) {
                                    string toFind = "encoder:";
                                    if (el.IndexOf (toFind) == 0) {
                                        double res = double.Parse (el.Substring (toFind.Length));
                                        m_Owner.Parameters.SetValue ("Printhead.X128.Encoder", res);
                                        //Logger.Log(res.ToString());
                                    }

                                }
                            }

                            foundIndex = line.IndexOf ("power:");
                            if (foundIndex == 0) {
                                string[] elements = line.Split (',');
                                foreach (string el in elements) {
                                    string toFind = "power:";
                                    if (el.IndexOf (toFind) >= 0) {
                                        double res = double.Parse (el.Substring (toFind.Length));
                                        m_Owner.Parameters.SetValue ("Printhead.X128.Power", res);
                                        //Logger.Log(res.ToString());
                                    }
                                    toFind = " timeOn:";
                                    if (el.IndexOf (toFind) >= 0) {
                                        double res = double.Parse (el.Substring (toFind.Length));
                                        m_Owner.Parameters.SetValue ("Printhead.X128.OnTime", res);
                                        //Logger.Log(res.ToString());
                                    }

                                }
                            }
                            foundIndex = line.IndexOf ("serialNumber:");
                            if (foundIndex == 0) {
                                string[] elements = line.Split (',');
                                foreach (string el in elements) {
                                    string toFind = "serialNumber:";
                                    if (el.IndexOf (toFind) >= 0) {
                                        string res = (el.Substring (toFind.Length));
                                        m_Owner.Parameters.SetValue ("Printhead.X128.SerialNumber", res);
                                    }

                                }
                            }
                            foundIndex = line.IndexOf ("date:");
                            if (foundIndex == 0) {
                                string[] elements = line.Split (',');
                                foreach (string el in elements) {
                                    string toFind = "software:";
                                    if (el.IndexOf (toFind) >= 0) {
                                        string res = (el.Substring (toFind.Length));
                                        m_Owner.Parameters.SetValue ("Printhead.X128.SoftwareVersion", res);
                                    }

                                }
                            }

                        }
                    }

                }
            }
        }