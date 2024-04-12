/**
 * @file
 * @brief This C# Code contains the Source Code for the Driver Board Dropwatcher Application.
 * 
 * @author Written by Kajeban Baskaran for Added Scientific Limited
 * 
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Threading;
using Newtonsoft.Json.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Runtime.InteropServices;
using System.Collections;
using System.Drawing.Imaging;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using System.Linq.Expressions;
using System.IO.Compression;
using Newtonsoft.Json.Converters;
using System.Management;

namespace DriverBoardDropwatcher
{
    public partial class Form1 : Form
    {
        private OpenFileDialog ofd; /*!< Creates an OpenFileDialog variable called ofd */
        bool valid_port_selected = false;
        string port_name;
        int failCounter = 0;
        static SerialPort driver_board;
        int activeDropWatch;
        int activeDropModeHead;
        int activeNozzleValue;
        int activeSpanValue;
        int activeImageMode;
        int activeGapValue;
        int actFreq;
        int timeBoardOn = -1;
        int activePD_Polarity;
        int activeEncoderPosition;
        int activePDdirection;
        int activeImageHeadIndex;
        bool ImageHead1 = false;
        bool ImageHead2 = false;
        bool ImageHead3 = false;
        bool ImageHead4 = false;
        bool Head1ImageSend = false;
        bool Head2ImageSend = false;
        bool Head3ImageSend = false;
        bool Head4ImageSend = false;    
        bool isRunning = true;
        String CurrentFileName;
        //String datafolder = Application.StartupPath.Replace("bin\\Debug", "Output Images\\File");
        String datafolder = System.IO.Path.Combine(Application.StartupPath, "Output Images\\File");
        String outputFolderPath = System.IO.Path.Combine(Application.StartupPath, "Output Images");
        int[] HeadPrintCountersStoredAsInt = new int[4];
        int[] PreviousHeadPrintCounters = new int[4];
        int[] HeadStatus = new int[4];
        byte[] A_Bits = { 0b10010010, 0b01001001, 0b00100100 };
        byte[] B_Bits = { 0b01001001, 0b00100100, 0b10010010 };
        byte[] C_Bits = { 0b00100100, 0b10010010, 0b01001001 };
        byte[] BitsArray;
        byte[] BytesToSend;

        public Form1()
        {
            InitializeComponent();
            // Upload Placeholer Image for 4 Picture Boxes
            pictureBox1.Image = Properties.Resources.upload;
            pictureBox2.Image = Properties.Resources.upload;
            pictureBox3.Image = Properties.Resources.upload;
            pictureBox4.Image = Properties.Resources.upload;

            if (!Directory.Exists(outputFolderPath))
            {
                Directory.CreateDirectory(outputFolderPath);
            }
            this.FormClosing += Form1_FormClosing;
            Thread trd = new Thread(new ThreadStart(this.ThreadTask));
            trd.IsBackground = true;
            trd.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
            txtbStatusBox.Text = "Disconnected"; //Displays Disconnected in Status Box
            ofd = new OpenFileDialog();

            //Filter Image Files only when opening File Dialog
            ofd.Filter = "Image Files(*.jpg; *.jpeg; *.bmp); *.png|*.jpg; *.jpeg; *.bmp; *.png";
        }

        /**
         * @brief A Thread Task Function
         * 
         * This function constantly runs in the background to ensure the board is connected at all times.
         * It sends the command 'b' to receive relevent information such as voltage, temperature etc 
         * If this fails to run, then Error Message Box pops up signalling an error.
         */
        
        private void ThreadTask()
        {
            // Repeatedly checks if board is connected
            while (true)
            {
                if (ChbxIsConnected.Checked && isRunning)
                {
                    try
                    {
                        driver_board.Write("b\n");
                    }

                    catch
                    {
                        //Error Message Dialog when Board is Disconnected Unexpectedly
                        MessageBoxButtons BoxButtons = MessageBoxButtons.RetryCancel;
                        DialogResult results = MessageBox.Show("Error opening port", "Port Error", BoxButtons, MessageBoxIcon.Error);
                        if (results == DialogResult.Retry)
                        {
                            Application.Restart();
                            Environment.Exit(0);
                        }

                    }
                }
                Thread.Sleep(500);
            }
        }

        /**
         * @brief Grey Scale Image Function
         * 
         * This function grey scales any image passed to it
         * 
         * @param original Original Image (Coloured) 
         * @return Grey-Scaled Image in Bitmap format
         */

        public static Bitmap MakeGrayscale3(Bitmap original)
        {
            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            //get a graphics object from the new image
            using (Graphics g = Graphics.FromImage(newBitmap))
            {

                //create the grayscale ColorMatrix
                ColorMatrix colorMatrix = new ColorMatrix(
                   new float[][]
                   {
             new float[] {.299f, .299f, .299f, 0, 0},
             new float[] {.587f, .587f, .587f, 0, 0},
             new float[] {.114f, .114f, .114f, 0, 0},
             new float[] {0, 0, 0, 1, 0},
             new float[] {0, 0, 0, 0, 1}
                   });

                //create some image attributes
                using (ImageAttributes attributes = new ImageAttributes())
                {

                    //set the color matrix attribute
                    attributes.SetColorMatrix(colorMatrix);

                    //draw the original image on the new image
                    //using the grayscale color matrix
                    g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
                                0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);
                }
            }
            return newBitmap;
        }

        /**
        * @brief Detects Active Serial COM Ports from device
        * 
        * This function searches and detects any available COM Ports available to open.
        * 
        * @param sender The object that contains the reference to the object that raised the event
        * @param e The event data
        */

        private void cbSerialPort_DropDown(object sender, EventArgs e)
        {        // List to hold all matching COM port names
            List<string> comPorts = new List<string>();

            // Query WMI for devices with a specific VID and PID
            string vid = "16C0";
            string pid = "0483";
            string query = $"SELECT * FROM Win32_PnPEntity WHERE PNPDeviceID LIKE '%VID_{vid}&PID_{pid}%'";

            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
            {
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    string caption = queryObj["Caption"].ToString();
                    Console.WriteLine("Found Device: " + caption);

                    // Extract COM port name from the caption
                    int startIndex = caption.IndexOf("(COM");
                    if (startIndex != -1)
                    {
                        int endIndex = caption.IndexOf(')', startIndex);
                        if (endIndex != -1)
                        {
                            string comPort = caption.Substring(startIndex + 1, endIndex - (startIndex+1));
                            comPorts.Add(comPort);
                            Console.WriteLine("COM Port: " + comPort);
                        }
                    }
                }
            }

            // Optional: Convert List to Array
            string[] comPortArray = comPorts.ToArray();
            Console.WriteLine("Array of COM Ports:");
            foreach (var port in comPortArray)
            {
                Console.WriteLine(port);
            }

            cbSerialPort.Items.Clear();
            // Display each port name to the console.
            if (comPortArray.Length < 1)
                cbSerialPort.Items.Add("No X128 ports found");
            else
            {
                foreach (string port in comPortArray)
                {
                    Console.WriteLine(port);
                    cbSerialPort.Items.Add(port); //Adds all available ports to drop down
                }
            }


        }

        /**
        * @brief Detects Changes in Serial Ports
        * 
        * This function runs when the Serial COM Port is changed.
        * 
        * @param sender The object that contains the reference to the object that raised the event
        * @param e The event data
        */
        private void cbSerialPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            port_name = cbSerialPort.SelectedItem.ToString();
            Console.WriteLine(port_name);
            if (port_name == "No ports found")
            {
                Console.WriteLine("Cannot connect.");
                valid_port_selected = false;
            }

            else
                valid_port_selected = true;
        }

        /**
        * @brief Disconnect Driver Board
        * 
        * This function sends command to driver board to disconnect and unchecks the checkbox in the GUI.
        */
        private void disconnect_board()
        {
            Console.WriteLine("Disconnecting.");
            if (driver_board != null && driver_board.IsOpen)
                driver_board.Close();
            ChbxIsConnected.Checked = false;
            tcDropWatchingAndImageModes.Enabled = false;
            StatusTable.Enabled = false;
            nudFrequency.ReadOnly = true;
            lbFrequency.ForeColor = Color.Gray;
            lbStatus.ForeColor = Color.Gray;
            lbTimeOn.ForeColor = Color.Gray;
        }

        /**
        * @brief Connect Driver Board 
        * 
        * This function sends command to driver board to connect and checks the checkbox in the GUI.
        * Only runs if a valid port is selected and is connected successfully to the driver board.
        * If this fails to run, then an Error Dialog Box shows up.
        * 
        */

        private void connect_board()
        {
            if (valid_port_selected && !ChbxIsConnected.Checked)
            {
                driver_board = new SerialPort();
                driver_board.PortName = port_name;
                try
                {
                    driver_board.Open();
                    driver_board.ReadExisting();
                    driver_board.DataReceived += new SerialDataReceivedEventHandler(DataRecievedHandler);
                    ChbxIsConnected.Checked = true;
                    tcDropWatchingAndImageModes.Enabled = true;
                    StatusTable.Enabled = true;
                    nudFrequency.ReadOnly = false;
                    lbFrequency.ForeColor = Color.Black;
                    lbStatus.ForeColor = Color.Black;
                    lbTimeOn.ForeColor = Color.Black;
                    Console.WriteLine("Connected");
                    txtbStatusBox.Text = "Connected";
                    
                }
                catch
                {
                    Console.WriteLine("Error opening port");
                    txtbStatusBox.Text = "Error opening port";
                    MessageBoxButtons BoxButtons = MessageBoxButtons.RetryCancel;
                    DialogResult results = MessageBox.Show("Error opening port", "Port Error", BoxButtons, MessageBoxIcon.Error);
                    if (results == DialogResult.Retry)
                    {
                        Application.Restart();
                        Environment.Exit(0);
                    }                    
                }
            }
            else if (ChbxIsConnected.Checked)
            {
                disconnect_board();
                txtbStatusBox.Text = "Disconnected"; //Updates Status Text
                Console.WriteLine("Disconnected");
            }
        }

        /**
        * @brief Processes data received from driver board.
        * 
        * This function reads the data received from the driver board. If the substring matches to the expected character, then the data is sent to parse.
        * If this fails to run multipe times, then an Error Dialog Box shows up.
        * 
        * @param sender The object that contains the reference to the object that raised the event
        * @param e The event data
        */

        private void DataRecievedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            while (driver_board.BytesToRead > 0)
            {
                string input = driver_board.ReadExisting();
                //Console.WriteLine(input);
                if (input.Substring(0, 1) == "{")
                {
                    if (!input.Substring(10).Contains("board"))
                    {
                        try
                        {
                            parseJsonData(input);
                            failCounter = 0;
                            txtbJSONview.Text += input;
                        }
                        catch
                        {
                            failCounter++;
                            Console.WriteLine("Parse JSON Failed for the {0} time.", failCounter);

                            //If JSON Fails to Load, Error Dialog Pops up
                            if (failCounter > 3)
                            {
                                MessageBoxButtons BoxButtons = MessageBoxButtons.RetryCancel;
                                DialogResult results = MessageBox.Show("Parse JSON Failed", "Parse JSON Error", BoxButtons, MessageBoxIcon.Error);
                                if (results == DialogResult.Retry)
                                {
                                    Application.Restart();
                                    Environment.Exit(0);
                                }
                            }
                        }
                    }
                }

                if (input.Substring(0, 3).Contains("LV:"))
                {
                    if (Head1ImageSend == true)
                    {
                        VerifyImageData(1, input);
                        Head1ImageSend = false;
                    }
                    else if (Head2ImageSend == true)
                    {
                        VerifyImageData(2, input);
                        Head2ImageSend = false;
                    }
                    else if (Head3ImageSend == true)
                    {
                        VerifyImageData(3, input);
                        Head3ImageSend = false;

                    }
                    else if (Head4ImageSend == true)
                    {
                        VerifyImageData(4, input);
                        Head4ImageSend = false;
                    }

                    txtbJSONview.Text += "\n";
                    txtbJSONview.Text += input;
                    txtbJSONview.Text += "\n";
                }

                else
                {
                    //Console.WriteLine(input);
                }
            }
        }

        /**
        * @brief Determine the Status of Voltages and Temperatures
        * 
        * This function determines what state each Head is in and outputs the Voltage and Temperature Values to the GUI.
        * Only runs if a valid port is selected and is connected  and powered on successfully to the driver board.
        */
        private void determineStatus()
        {
            System.Windows.Forms.TextBox[] HeadTextStatus = { txtbHeadStatus1, txtbHeadStatus2, txtbHeadStatus3, txtbHeadStatus4 };
            NumericUpDown[] nudHeadVoltages = { nudVoltageHead1, nudVoltageHead2, nudVoltageHead3, nudVoltageHead4 };
            NumericUpDown[] nudHeadTemperatures = { nudTemperatureHead1, nudTemperatureHead2, nudTemperatureHead3, nudTemperatureHead4 };
            System.Windows.Forms.TextBox[] HeadTemperaturesOutputText = { txtbTemperatureOutput1, txtbTemperatureOutput2, txtbTemperatureOutput3, txtbTemperatureOutput4 };
            System.Windows.Forms.TextBox[] HeadPrintCounters = { txtbPrintCounter1, txtbPrintCounter2, txtbPrintCounter3, txtbPrintCounter4 };

            if (ChbxPower.Checked)
            { 
                //FOR loop for 4 Heads, to determine it status and update the UI accordingly
                for (int i = 0; i <= 3; i++)
                {
                    switch (HeadStatus[i])
                    {
                        case -2:
                            HeadTextStatus[i].Text = "Not Connected";
                            HeadTextStatus[i].BackColor = Color.Red;
                            nudHeadVoltages[i].Visible = false;
                            nudHeadTemperatures[i].Visible = false;
                            HeadTemperaturesOutputText[i].Visible = false;
                            HeadPrintCounters[i].Visible = false;
                            break;
                        case -3:
                            HeadTextStatus[i].Text = "Ready Error";
                            HeadTextStatus[i].BackColor = Color.Red;
                            nudHeadVoltages[i].Visible = false;
                            nudHeadTemperatures[i].Visible = false;
                            HeadTemperaturesOutputText[i].Visible = false;
                            HeadPrintCounters[i].Visible = false;
                            break;
                        case 10:
                            HeadTextStatus[i].Text = "Idle";
                            HeadTextStatus[i].BackColor = Color.Orange;
                            nudHeadVoltages[i].Visible = true;
                            nudHeadTemperatures[i].Visible = true;
                            HeadTemperaturesOutputText[i].Visible = true;
                            HeadPrintCounters[i].Visible = true;
                            if (HeadPrintCountersStoredAsInt[i] > PreviousHeadPrintCounters[i])
                            {
                                HeadTextStatus[i].Text = "Printing";
                                HeadTextStatus[i].BackColor = Color.Green;
                                txtbHeadStatus.Text = "Printing";
                                txtbHeadStatus.BackColor = Color.Green;
                                txtbImageHeadStatus.Text = "Printing";
                                txtbImageHeadStatus.BackColor = Color.Green;
                                break;
                            }
                            else
                            {
                                HeadTextStatus[i].Text = "Idle"; // Displays Message
                                HeadTextStatus[i].BackColor = Color.Orange; //Sets Status Box to Orange indicating all is fine but not printing
                                txtbHeadStatus.Text = "Head Idle";
                                txtbHeadStatus.BackColor = Color.Orange;
                                txtbImageHeadStatus.Text = "Head Idle";
                                txtbImageHeadStatus.BackColor = Color.Orange;
                                break;
                            }
                        default:
                            HeadTextStatus[i].Text = "Printing";
                            HeadTextStatus[i].BackColor = Color.Green;
                            nudHeadVoltages[i].Visible = true;
                            nudHeadTemperatures[i].Visible = true;
                            HeadTemperaturesOutputText[i].Visible = true;
                            HeadPrintCounters[i].Visible = true;
                            break;
                    }
                }
            }

            else
            {
                //IF Board is not connected, this code will run
                txtbHeadStatus1.Text = "Powered off.";
                nudVoltageHead1.Visible = false;
                nudTemperatureHead1.Visible = false;
                txtbTemperatureOutput1.Visible = false;
                txtbPrintCounter1.Visible = false;
                txtbHeadStatus2.Text = "Powered off.";
                nudVoltageHead2.Visible = false;
                nudTemperatureHead2.Visible = false;
                txtbTemperatureOutput2.Visible = false;
                txtbPrintCounter2.Visible = false;
                txtbHeadStatus3.Text = "Powered off.";
                nudVoltageHead3.Visible = false;
                nudTemperatureHead3.Visible = false;
                txtbTemperatureOutput3.Visible = false;
                txtbPrintCounter3.Visible = false;
                txtbHeadStatus4.Text = "Powered off.";
                nudVoltageHead4.Visible = false;
                nudTemperatureHead4.Visible = false;
                txtbTemperatureOutput4.Visible = false;
                txtbPrintCounter4.Visible = false;
                txtbHeadStatus1.BackColor = Color.Red;
                txtbHeadStatus2.BackColor = Color.Red;
                txtbHeadStatus3.BackColor = Color.Red;
                txtbHeadStatus4.BackColor = Color.Red;
            }
        }

        /**
        * @brief Parse the JSON Data 
        * 
        * This function parses the data received from the driver board.
        * Stores data such as Voltage, Temperature and Print Count into variables that can be called later
        * 
        * @param input_string The Data Lines received from the driver board in JSON Format
        */
        private void parseJsonData(string input_string)
        {
            dynamic d = JObject.Parse(input_string);
            //--------------------------------------------------
            //Retrieves Head Status from JSON
            for (int i = 0; i <= 3; i++)
            {
                HeadStatus[i] = d.heads[i].status;
            }
            //--------------------------------------------------

            //Check Print Counts for Heads
            System.Windows.Forms.TextBox[] HeadPrintCounters = { txtbPrintCounter1, txtbPrintCounter2, txtbPrintCounter3, txtbPrintCounter4 };

            //Updates Text Box to view current print count
            for (int i = 0; i <= 3; i++)
            {
                HeadPrintCounters[i].Text = d.heads[i].printCounts.ToString();
                HeadPrintCountersStoredAsInt[i] = d.heads[i].printCounts;
            }

            determineStatus();

            if (!ChbxPower.Checked)
            {
                txtbHeadStatus.Text = "Powered Off";
                txtbHeadStatus.BackColor = Color.Red;
                txtbImageHeadStatus.Text = "Powered Off";
                txtbImageHeadStatus.BackColor = Color.Red;
            }

            // Replaces previous count with current count to determine if value is increasing or stationary
            for (int i = 0; i <= 3; i++)
            {
                PreviousHeadPrintCounters[i] = HeadPrintCountersStoredAsInt[i];
            }

            //--------------------------------------------------

            //Check Currrent Temeperatures for Heads
            System.Windows.Forms.TextBox[] HeadTemperatures = { txtbTemperatureOutput1, txtbTemperatureOutput2, txtbTemperatureOutput3, txtbTemperatureOutput4 };

            for (int i = 0; i <= 3; i++)
            {
                HeadTemperatures[i].Text = d.heads[i].curTemperature.ToString();

                //Changes Current Temperature Text Box Colour if its heating
                if ((d.heads[i].isHeating) == 1)
                {
                    HeadTemperatures[i].ForeColor = Color.Red;
                }
                else
                {
                    HeadTemperatures[i].BackColor = Color.WhiteSmoke;
                }

            }

            if (activeImageMode == 0)
                txtbCurrentStepperPosition.Text = d.locations[0].stepper.ToString();
            if (activeImageMode == 1)
                txtbCurrentEncoderPosition.Text = d.locations[0].encoder.ToString();


            //--------------------------------------------------
            //Updates Voltage Values in GUI 
            NumericUpDown[] nudHeadVoltages = { nudVoltageHead1, nudVoltageHead2, nudVoltageHead3, nudVoltageHead4 };
            for (int i = 0; i <= 3; i++)
            {
                nudHeadVoltages[i].Value = d.heads[i].voltage;
            }
            //--------------------------------------------------
            actFreq = d.printingParameters[0].internalPrintPeriod;

            if (actFreq > 0)
            {
                //Check Frequency for each Head
                nudFrequency.Value = (1000000 / actFreq);
                nudFrequency.Value = (1000000 / actFreq);
                txtbFrequencyDuplicate.Text = nudFrequency.Value.ToString();
            }
            ChbxPower.Checked = d.board[0].power == 1 ? true : false;
            int newTime = d.board[0].timeOn;
            
            //Determines Board Up and Running Time 
            timeBoardOn = newTime;
            txtbBoardUpTime.Text = timeBoardOn.ToString(); // Displays Clock Time in GUI
        }

        /**
        * @brief Controls the Connect/Disconnect Toggle
        * 
        * This function controls the connection state of the driver board and modifies the checkbox as checked if connected or unchecked if disconnected
        * 
        * @param sender The object that contains the reference to the object that raised the event
        * @param e The event data
        */
        private void btnConnectDisconnect_Click(object sender, EventArgs e)
        {
            //Toggle button to connect and disconnect from board
            connect_board();
            tcDropWatchingAndImageModes_SelectedIndexChanged(sender, e);
        }

        /**
        * @brief Controls the Power On/Off Toggle
        * 
        * This function controls the power state of the driver board and modifies the checkbox as checked if powered on or unchecked if powered off.
        * 
        * @param sender The object that contains the reference to the object that raised the event
        * @param e The event data
        */
        private void powerOnOff_Click(object sender, EventArgs e)
        {
            if (ChbxIsConnected.Checked)
            {
                if (ChbxPower.Checked)
                {
                    //Toggle button to power on and off
                    powerOff();
                    txtbStatusBox.Text = "Power Off"; //Displays Disconnected in Status Box
                    tcDropWatchingAndImageModes.Enabled = false;
                    StatusTable.Enabled = false;
                    nudFrequency.ReadOnly = true;
                    lbFrequency.ForeColor = Color.Gray;
                    lbStatus.ForeColor = Color.Gray;
                    lbTimeOn.ForeColor = Color.Gray;

                }
                else
                {
                    powerOn();
                    tcDropWatchingAndImageModes.Enabled = true;
                    StatusTable.Enabled = true;
                    nudFrequency.ReadOnly = false;
                    lbFrequency.ForeColor = Color.Black;
                    lbStatus.ForeColor = Color.Black;
                    lbTimeOn.ForeColor = Color.Black;
                    txtbStatusBox.Text = "Power On"; //Displays Connected in Status Box
                    tcDropWatchingAndImageModes_SelectedIndexChanged(sender, e);
                    frequencyChange(sender, e);
                }
            }
            else
            {
                MessageBox.Show("Not Connected!", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /**
        * @brief Power On Function
        * 
        * This function sends the command to turn on the driver board. Only runs if correct COM Port is selected.
        *
        */
        private void powerOn()
        {
            if (ChbxIsConnected.Checked)
            {
                driver_board.Write("O"); // Turns on Board
            }
            else
            {
                MessageBox.Show("Not Connected!", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /**
        * @brief Power Off Function
        * 
        * This function is called when the toggle mode is set to Power Off
        * 
        * Sends command to driver board.
        * 
        * @param sender The object that contains the reference to the object that raised the event
        * @param e The event data
        */
        private void powerOff()
        {
            if (ChbxIsConnected.Checked)
            {
                driver_board.Write("F"); // Turns off Board
            }
            else
            {
                MessageBox.Show("Not Connected!", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /**
        * @brief Fetches Voltage Data from GUI 
        * 
        * This function reads data parsed from driverboard and outputs the required variables into the GUI.
        * Only runs if driver board is connected
        * 
        * Uses tags to find relevent Numeric Up Downs and Labels in GUI.
        * 
        * @param sender The object that contains the reference to the object that raised the event
        * @param e The event data
        */
        private void voltage_ValueChanged(object sender, EventArgs e)
        {
            // This Function Checks for Tag Names to determine which Head Voltage is being changed.
            System.Windows.Forms.NumericUpDown voltageChanged = (System.Windows.Forms.NumericUpDown)sender;

            if (ChbxIsConnected.Checked)
            {
                if (voltageChanged.Tag == "voltageHead1") //Head 1 Voltage Tag is "voltageHead1"
                {
                    driver_board.Write($"v {(1).ToString()} {nudVoltageHead1.Value.ToString()}"); //Sends Command to modify Voltage Value
                    Console.WriteLine("Head 1 Voltage Changed to: {0}", nudVoltageHead1.Value); //Send Message to Console to confirm Voltage Change
                }
                else if (voltageChanged.Tag == "voltageHead2")
                {
                    driver_board.Write($"v {(2).ToString()} {nudVoltageHead2.Value.ToString()}");
                    Console.WriteLine("Head 2 Voltage Changed to: {0}", nudVoltageHead2.Value);

                }
                else if (voltageChanged.Tag == "voltageHead3")
                {
                    driver_board.Write($"v {(3).ToString()} {nudVoltageHead3.Value.ToString()}");
                    Console.WriteLine("Head 3 Voltage Changed to: {0}", nudVoltageHead3.Value);
                }
                else if (voltageChanged.Tag == "voltageHead4")
                {
                    driver_board.Write($"v {(4).ToString()} {nudVoltageHead4.Value.ToString()}");
                    Console.WriteLine("Head 4 Voltage Changed to: {0}", nudVoltageHead4.Value);
                }
            }
            else
            {
                MessageBox.Show("Not Connected!", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /**
        * @brief Fetches Temperature Data from GUI 
        * 
        * This function reads data parsed from driverboard and outputs the required variables into the GUI.
        * Only runs if driver board is connected
        * 
        * Uses tags to find relevent Numeric Up Downs and Labels in GUI.
        * 
        * @param sender The object that contains the reference to the object that raised the event
        * @param e The event data
        */
        private void temperature_ValueChanged(object sender, EventArgs e)
        {
            // This Function Checks for Tag Names to determine which Head Temperature is being changed.
            System.Windows.Forms.NumericUpDown temperatureChanged = (System.Windows.Forms.NumericUpDown)sender;
            if (ChbxIsConnected.Checked)
            {
                if (temperatureChanged.Tag == "temperatureHead1")
                {
                    driver_board.Write($"T {(1).ToString()} {nudTemperatureHead1.Value.ToString()}");
                    Console.WriteLine("Head 1 Temperature Changed to: {0}", nudTemperatureHead1.Value);
                }
                else if (temperatureChanged.Tag == "temperatureHead2")
                {
                    driver_board.Write($"T {(2).ToString()} {nudTemperatureHead2.Value.ToString()}");
                    Console.WriteLine("Head 2 Temperature Changed to: {0}", nudTemperatureHead2.Value);
                }
                else if (temperatureChanged.Tag == "temperatureHead3")
                {
                    driver_board.Write($"T {(3).ToString()} {nudTemperatureHead3.Value.ToString()}");
                    Console.WriteLine("Head 3 Temperature Changed to: {0}", nudTemperatureHead3.Value);
                }
                else if (temperatureChanged.Tag == "temperatureHead4")
                {
                    driver_board.Write($"T {(4).ToString()} {nudTemperatureHead4.Value.ToString()}");
                    Console.WriteLine("Head 4 Temperature Changed to: {0}", nudTemperatureHead4.Value);
                }
            }
            else
            {
                MessageBox.Show("Not Connected!", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frequencyValue_ValueChanged(object sender, EventArgs e)
        {
            frequencyChange(sender, e);
        }

        /**
        * @brief Fetches Frequency Data from GUI 
        * 
        * This function runs when the Frequency Value is changed in the GUI so the new value is updated on the board.
        * Only runs if driver board is connected and is powered on.
        * 
        * Uses tags to find relevent Numeric Up Downs and Labels in GUI.
        * 
        * @param sender The object that contains the reference to the object that raised the event
        * @param e The event data
        */
        private void frequencyChange(object sender, EventArgs e)
        {
            try
            {
                // This Function Checks for Tag Names to determine which Frequency Value is being changed.
                System.Windows.Forms.NumericUpDown nudFrequencyChanged = (System.Windows.Forms.NumericUpDown)sender;
                if ((ChbxPower.Checked) && (ChbxIsConnected.Checked))
                {
                    if (nudFrequencyChanged.Tag == "nudFrequency")
                    {
                        //Sets Head Frequency
                        driver_board.Write($"p {nudFrequency.Value.ToString()}");
                    }
                }
            }
            catch
            {
                Console.WriteLine("Error");
            }
        }

        /**
        * @brief Fetches User Selected Drop Watch Mode Selection from GUI
        * 
        * This function is called when the Drop Watch Mode Seection is changed between Internal and External Modes.
        * Stores user selected mode in a variable called "activeDropWatch"
        * Only runs if driver board is connected
        * 
        * @param sender The object that contains the reference to the object that raised the event
        * @param e The event data
        */
        private void cbDropWatchMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            activeDropWatch = cbDropWatchMode.SelectedIndex; //Stores selected mode

            //Checks if External Mode is Selected AND Board is Connected
            if ((activeDropWatch == 1) && (ChbxIsConnected.Checked))
            {
                nudFrequency.ReadOnly = true;
                lbFrequency.ForeColor = Color.Gray;
                nudFrequency.ForeColor = Color.Gray;
                txtbFrequencyDuplicate.Visible = false;
                lbDropWatchingFrequencyDuplicate.Visible = false;
                driver_board.Write("M2");
                Console.WriteLine("Drop Watching External Mode");
            }

            //Checks IF internal Mode is Selected AND Board is Connected
            else if ((activeDropWatch == 0) && (ChbxIsConnected.Checked))
                {
                nudFrequency.ReadOnly = false;
                lbFrequency.ForeColor = Color.Black;
                nudFrequency.ForeColor = Color.Black;
                txtbFrequencyDuplicate.Visible = true;
                lbDropWatchingFrequencyDuplicate.Visible = true;
                driver_board.Write("M1");
                Console.WriteLine("Drop Watching Internal Mode");
            }
        }

        /**
        * @brief Reset Board Button
        * 
        * This function is called when user presses Reset Board Button in GUI
        * Only runs if driver board is connected
        * Sends relevent command to the driver board to reset it.
        * 
        * @param sender The object that contains the reference to the object that raised the event
        * @param e The event data
        */

        private void reset_Click(object sender, EventArgs e)
        {
            if (ChbxIsConnected.Checked && ChbxPower.Checked)
            {
                driver_board.Write("r"); //Resets Board
                tcDropWatchingAndImageModes_SelectedIndexChanged(sender, e);
            }
            else
            {
                MessageBox.Show("Not Connected!", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /**
        * @brief Fetches User Selected Head from GUI
        * 
        * This function is called when user modifies the head selection in the drop watching mode.
        * Stores selected head in a variable called "activeDropModeHead".
        * 
        * 
        * @param sender The object that contains the reference to the object that raised the event
        * @param e The event data
        */
        private void cbDropWatchHeadSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            activeDropModeHead = cbDropWatchHeadSelection.SelectedIndex; //Stores Head Value
        }

        /**
        * @brief Image Box Selection in Image Mode
        * 
        * This function is called when user presses the picture box in GUI to upload image
        * 
        * Opens up file dialog and filters it out to only allow upload of image files.
        * Clones the image and saves it into a folder called "Output Images" in the .png format
        * 
        * Uses tags to find relevent Picture Boxes (1-4).
        * 
        * @param sender The object that contains the reference to the object that raised the event
        * @param e The event data
        */
        private void ImageBoxClicked(object sender, EventArgs e)
        {
            PictureBox ImageBox = (System.Windows.Forms.PictureBox)sender;

            switch (ImageBox.Tag)
            {
                case ("ImageHead1"):
                    activeImageHeadIndex = 1;
                    // Opens File Dialog and Replaces Current Image with New Image
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        Bitmap Picture1 = (Bitmap)new Bitmap(ofd.FileName); // Clones uploaded image
                        Picture1.Save(datafolder + 1 + ".png");  //Saves uploaded folder to project folder 
                        pictureBox1.Image = MakeGrayscale3(Picture1); //Grey Scales Image in GUI
                        txtbFileNameHead1.Text = ofd.SafeFileName; //Stores File Name removing Path of File
                        CurrentFileName =  (datafolder + 1 + ".png");
                        txtbDimensionsHead1.Text = ((Image.FromFile(ofd.FileName).Width) + " x " + (Image.FromFile(ofd.FileName).Height));

                        // If File Size has a width of more that
                        if ((Image.FromFile(ofd.FileName).Width) > 128) //If image width exceed 128, warning message shows
                        {
                            MessageBox.Show("Maximum 128 Pixels Per Head!", "File Warning",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        convertImageToData(CurrentFileName); //Calls function to convert image to data
                        ImageHead1 = true;

                    }
                    break;

                case ("ImageHead2"):
                    activeImageHeadIndex = 2;
                    if (ofd.ShowDialog() == DialogResult.OK) //Opens File Dialog
                    {
                        Bitmap Picture2 = (Bitmap)new Bitmap(ofd.FileName);  // Clones uploaded image
                        Picture2.Save(datafolder + 2 + ".png"); //Saves uploaded folder to project folder 
                        pictureBox2.Image = MakeGrayscale3(Picture2); //Grey Scales Image in GUI
                        txtbFileNameHead2.Text = ofd.SafeFileName; //Stores File Name removing Path of File
                        CurrentFileName = (datafolder + 2 + ".png");
                        txtbDimensionsHead2.Text = ((Image.FromFile(ofd.FileName).Width) + " x " + (Image.FromFile(ofd.FileName).Height));

                        if ((Image.FromFile(ofd.FileName).Width) > 128) //If image width exceed 128, warning message shows
                        {
                            MessageBox.Show("Maximum 128 Pixels Per Head!", "File Warning",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        convertImageToData(CurrentFileName); //Calls function to convert image to data
                        ImageHead2 = true;
                    }
                    break;

                case ("ImageHead3"):
                    activeImageHeadIndex = 3;
                    if (ofd.ShowDialog() == DialogResult.OK) //Opens File Dialog
                    {
                        Bitmap Picture3 = (Bitmap)new Bitmap(ofd.FileName); // Clones uploaded image
                        Picture3.Save(datafolder + 3 + ".png");  //Saves uploaded folder to project folder 
                        pictureBox3.Image = MakeGrayscale3(Picture3); //Grey Scales Image in GUI
                        txtbFileNameHead3.Text = ofd.SafeFileName; //Stores File Name removing Path of File
                        CurrentFileName = (datafolder + 3 + ".png"); 
                        txtbDimensionsHead3.Text = ((Image.FromFile(ofd.FileName).Width) + " x " + (Image.FromFile(ofd.FileName).Height));

                        if ((Image.FromFile(ofd.FileName).Width) > 128) //If image width exceed 128, warning message shows
                        {
                            MessageBox.Show("Maximum 128 Pixels Per Head!", "File Warning",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        convertImageToData(CurrentFileName); //Calls function to convert image to data
                        ImageHead3 = true;
                    }
                    break;

                case ("ImageHead4"):
                    activeImageHeadIndex = 4;
                    if (ofd.ShowDialog() == DialogResult.OK) //Opens File Dialog
                    {
                        Bitmap Picture4 = (Bitmap)new Bitmap(ofd.FileName); // Clones uploaded image
                        Picture4.Save(datafolder + 4 + ".png"); //Saves uploaded folder to project folder 
                        pictureBox4.Image = MakeGrayscale3(Picture4); //Grey Scales Image in GUI
                        txtbFileNameHead4.Text = ofd.SafeFileName; //Stores File Name removing Path of File
                        CurrentFileName = (datafolder + 4 + ".png");
                        txtbDimensionsHead4.Text = ((Image.FromFile(ofd.FileName).Width) + " x " + (Image.FromFile(ofd.FileName).Height));

                        if ((Image.FromFile(ofd.FileName).Width) > 128) //If image width exceed 128, warning message shows
                        {
                            MessageBox.Show("Maximum 128 Pixels Per Head!", "File Warning",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        convertImageToData(CurrentFileName); //Calls function to convert image to data
                        ImageHead4 = true;
                    }
                    break;
            }
        }

        /**
        * @brief Picture Box 1 is pressed
        * 
        * This function calls the ImageBoxClicked function when Head 1 Image is modified
        * 
        * @param sender The object that contains the reference to the object that raised the event
        * @param e The event data
        */
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ImageBoxClicked(sender, e);
        }

        /**
        * @brief Picture Box 2 is pressed
        * 
        * This function calls the ImageBoxClicked function when Head 2 Image is modified
        * 
        * @param sender The object that contains the reference to the object that raised the event
        * @param e The event data
        */

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            ImageBoxClicked(sender, e);
        }

        /**
        * @brief Picture Box 3 is pressed
        * 
        * This function calls the ImageBoxClicked function when Head 3 Image is modified
        * 
        * @param sender The object that contains the reference to the object that raised the event
        * @param e The event data
        */

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            ImageBoxClicked(sender, e);
        }

        /**
        * @brief Picture Box 4 is pressed
        * 
        * This function calls the ImageBoxClicked function when Head 4 Image is modified
        * 
        * @param sender The object that contains the reference to the object that raised the event
        * @param e The event data
        */
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            ImageBoxClicked(sender, e);
        }

        /**
        * @brief Convert Image to Data
        * 
        * This function is called when the Image is ready to be converted into data in order to send it to the driver board.
        * 
        * Stores image as data in a file with an extension of ".printDat" in a Folder Called "Output Images" in Project Folder.
        * 
        * @param file_name This is the full path of where the file is stored in the system.
        */
        private void convertImageToData(string file_name)
        {
            using (Bitmap inputImage = new Bitmap(file_name))
            {
                Color pixel;
                List <byte> imageData = new List <byte>();

                for (int y = (inputImage.Height - 1); y > -1; y--)
                {
                    for (int byteWide = 0; byteWide < 16; byteWide++)
                    {
                        byte curByte = 0;
                        for (int bitIdx = 0; bitIdx < 8; bitIdx++)
                        {
                            int x = 8 * byteWide + bitIdx;

                            if (x >= inputImage.Width)
                            {
                                pixel = Color.FromName("White"); //Sets Pixel Colour to White if Input Image Width is smaller than x-width
                            }
                            else
                            {
                                pixel = inputImage.GetPixel(x, y);
                            }
                            uint val = (uint)(pixel.ToArgb());

                            if (val == 4278190080) // Equivalent of Hex 000000 (Colour: Black)
                            {
                                curByte = (byte)(curByte + Math.Pow(2, (7 - bitIdx)));
                            }
                        }

                        imageData.Add(curByte);
                    }
                }
                byte[] imageDataByteArray = imageData.ToArray();
                File.WriteAllBytes(file_name + ".printDat", imageDataByteArray); //Writes new image file into .printDat format and saves it to folder
            }
        }

        /**
        * @brief Print Image
        * 
        * This function is called when the Print Image Button is pressed.
        * 
        * Only runs if driver board is connected and powered on.
        * Sends only the relevent images to print. For example, if images are uploaded to Head 1 and Head 3 and Head 2 and 4 are left blank, only Heads 1 and 3 will print.
        * 
        * @param sender The object that contains the reference to the object that raised the event
        * @param e The event data
        */
        private void btnPrintImage_Click(object sender, EventArgs e)
        {
            if ((ChbxIsConnected.Checked) && (ChbxPower.Checked))
            {
                isRunning = false;
                //If Images are uploaded to any of the print heads, send the relevent image to the board to print
                if (ImageHead1 == true)
                {
                    Head1ImageSend = true;
                    PrintingImage(1);
                    txtbPrintStatus.Text = "Print Successful";
                }
                if (ImageHead2 == true)
                {
                    Head2ImageSend = true;
                    PrintingImage(2);
                    txtbPrintStatus.Text = "Print Successful";
                }
                if (ImageHead3 == true)
                {
                    Head3ImageSend = true;
                    PrintingImage(3);
                    txtbPrintStatus.Text = "Print Successful";
                }
                if (ImageHead4 == true)
                {
                    Head4ImageSend = true;
                    PrintingImage(4);
                    txtbPrintStatus.Text = "Print Successful";
                }
 
                isRunning = true;
            }

            else
            {
                MessageBox.Show("Not Connected!", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /**
        * @brief Printing Image Function
        * 
        * This function sends the relevent command with the relevent print head to the driver board.
        * 
        * Read the image data file and stores it in an array ready to send in bytes.
        * 
        * @param head The Active Head with the Image Loaded onto it.
        */
        private void PrintingImage(int head)
        {
            String CurrentFile = datafolder + head + ".png.printDat";
            byte[] readText = File.ReadAllBytes(CurrentFile);

            byte[] dataToSend = new byte[readText.Length + 2];
            readText.CopyTo(dataToSend, 2);

            //Set to write array
            dataToSend[0] = (byte)'W';

            //set the head index
            dataToSend[1] = (byte)(100 + head);

            //Send to driver board
            Thread.Sleep(50);
            driver_board.Write(dataToSend, 0, dataToSend.Length);

        }

        /**
        * @brief Picture Box 2
        * 
        * This function calls the ImageBoxClicked function when Head 2 Image is modified
        * 
        * @param sender The object that contains the reference to the object that raised the event
        * @param e The event data
        */
        private void VerifyImageData(int head, string existing_lines)
        {
            String CurrentFile = datafolder + head + ".png.printDat";
            byte[] readText = File.ReadAllBytes(CurrentFile);

            long lastValue = 0;
            long runningCount = 0;
            long dataLength = 0;
            int LV = 0, RC = 0, DL = 0;

            //Splits each new line from the output from board into a new item in an array
            string[] s_ = existing_lines.Split('\n');

            foreach (byte dat in readText)
            {
                lastValue = dat;
                runningCount += dat;
                dataLength++;
            }

            if (s_.Length == 4) //Checks if the array length is 4
            {
                //Parse LV, RC and DL Values outputted from Board
                int.TryParse(s_[0].Substring(3), out LV);
                int.TryParse(s_[1].Substring(3), out RC); 
                int.TryParse(s_[2].Substring(3), out DL);
                if (LV == lastValue)
                {
                    if (RC == runningCount)
                    {
                        if (DL == dataLength)
                        {
                            Console.WriteLine("Data send correct");
                        }
                        else
                        {
                            Console.WriteLine("DL incorrect");
                            //MessageBox.Show("DL Incorrect for Head: " + head);
                        }
                    }
                    else
                    {
                        Console.WriteLine("RC incorrect");
                        //MessageBox.Show("RC Incorrect for Head: " + head);
                    }
                }
                else
                {
                    Console.WriteLine("LV incorrect");
                    //MessageBox.Show("LV Incorrect for Head: " + head);
                }
            }
            else
            {
                Console.WriteLine("Not recieved the correct data format");
                MessageBox.Show(("Not received the correct data format for Head: " + head), "Printing Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /**
        * @brief Nozzle Value Modified in GUI
        * 
        * This function is called when user changed value of the Nozzle (Numeric Up Down)
        * 
        * If span value + nozzle value is greater than 128, then a warning message will pop up automatically rectifying the error in the numeric up down.
        * 
        * @param sender The object that contains the reference to the object that raised the event
        * @param e The event data
        */
        private void NozzleValue_ValueChanged(object sender, EventArgs e)
        {
            activeNozzleValue = (int)nudNozzle.Value;
            if ((activeSpanValue + activeNozzleValue) > 128)
            {
                activeNozzleValue = 128 - activeSpanValue;
                nudNozzle.Value = activeNozzleValue;

                txtbNozzleSpanStatusBox.Text = "Maximum value of 128 Exceeded!";
            }
        }

        /**
        * @brief Span Value Modified in GUI
        * 
        * This function is called when user changed value of the Span (Numeric Up Down)
        * 
        * If span value + nozzle value is greater than 128, then a warning message will pop up automatically rectifying the error in the numeric up down.
        * 
        * @param sender The object that contains the reference to the object that raised the event
        * @param e The event data
        */
        private void SpanValue_ValueChanged(object sender, EventArgs e)
        {
            activeSpanValue = (int)nudSpan.Value;
            if ((activeSpanValue + activeNozzleValue) > 128)
            {
                activeSpanValue = 128 - activeNozzleValue;
                nudSpan.Value = activeSpanValue;
                txtbNozzleSpanStatusBox.Text = "Maximum value of 128 Exceeded!";
            }
        }

        /**
        * @brief Fill Nozzle Push Button 
        * 
        * This function is called when the "Fill Nozzle" button is pushed 
        * 
        * It sends the relevent command to the driver board whilst checking the checkbox to show that it is running.
        * 
        * @param sender The object that contains the reference to the object that raised the event
        * @param e The event data
        */
        private void FillSingleNozzleButton_Click(object sender, EventArgs e)
        {
            if ((ChbxIsConnected.Checked) && (ChbxPower.Checked))
            {
                chbxIsFillNozzle.Checked = true;
                chbxIsFillHead.Checked = false;
                chbxIsFillSpan.Checked = false;
                driver_board.Write($"n {(activeDropModeHead + 1).ToString()} {activeNozzleValue.ToString()}");
                txtbNozzleSpanStatusBox.Text = "";
            }
            else
            {
                MessageBox.Show("Not Connected!", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /**
        * @brief Fill Span Push Button 
        * 
        * This function is called when the "Fill Span" button is pushed 
        * 
        * It sends the relevent command to the driver board whilst checking the checkbox to show that it is running.
        * 
        * @param sender The object that contains the reference to the object that raised the event
        * @param e The event data
        */
        private void FillSpanNozzleButton_Click(object sender, EventArgs e)
        {
            if (ChbxIsConnected.Checked)
            {
                chbxIsFillSpan.Checked = true;
                chbxIsFillHead.Checked = false;
                chbxIsFillNozzle.Checked = false;
                txtbNozzleSpanStatusBox.Text = "";
                driver_board.Write($"N {(activeDropModeHead + 1).ToString()} {activeNozzleValue.ToString()} {activeSpanValue.ToString()}");
            }
            else
            {
                MessageBox.Show("Not Connected!", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /**
        * @brief Runs if Image Mode Selection is Modified
        * 
        * This function is user modified the image mode selection in GUI. 
        * 
        * It stores the image mode into a variable called "activeImageMode" 
        * This function greys out other irrelevent combo box. For example, when Stepper Motor Mode is selected, the PD Polarity Settings are greyed out as they are irrelevent in this mode.
        * 
        * @param sender The object that contains the reference to the object that raised the event
        * @param e The event data
        */
        private void ImageModeSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Determine relevent option from drop down menu selected by user
            activeImageMode = cbImageMode.SelectedIndex; //Stores Head Value
            if ((activeImageMode == 0) && (ChbxIsConnected.Checked))
            {
                driver_board.Write("M3");
                Console.WriteLine("Stepper Motor Mode");
                lbPDpolarity.ForeColor = Color.Gray;
                cbPDpolarity.Visible = false;
                cbEncoderTrackedPosition.Visible = true;
                lbEncoderTrackedPosition.ForeColor = Color.Black;
                cdPDdirection.Visible = false;
                lbPDdirection.ForeColor = Color.Gray;
                txtbCurrentEncoderPosition.Visible = false;
                nudSetPosition.Visible = true;
                lbCurrentEncoderPosition.Visible = true;
                lbCurrentEncoderPosition.ForeColor = Color.Gray;
                lbSetPosition.Visible = true;
                txtbCurrentStepperPosition.Visible = true;
                lbCurrentStepperPosition.Visible = true;
                lbCurrentStepperPosition.ForeColor = Color.Black;
            }

            else if ((activeImageMode == 1) && (ChbxIsConnected.Checked))
            {
                driver_board.Write("M4");
                Console.WriteLine("Quadrature Encoder Mode");
                lbPDpolarity.ForeColor = Color.Gray;
                cbPDpolarity.Visible = false;
                cbEncoderTrackedPosition.Visible = true;
                lbEncoderTrackedPosition.ForeColor = Color.Black;
                cdPDdirection.Visible = false;
                lbPDdirection.ForeColor = Color.Gray;
                txtbCurrentEncoderPosition.Visible = true;
                nudSetPosition.Visible = true;
                lbCurrentEncoderPosition.Visible = true;
                lbCurrentEncoderPosition.ForeColor = Color.Black;
                lbSetPosition.Visible = true;
                txtbCurrentStepperPosition.Visible = false;
                lbCurrentStepperPosition.Visible = true;
                lbCurrentStepperPosition.ForeColor = Color.Gray;
            }

            else if ((activeImageMode == 2) && (ChbxIsConnected.Checked))
            {
                driver_board.Write("M5");
                Console.WriteLine("HW PD Mode");
                lbPDpolarity.Visible = true;
                lbPDpolarity.ForeColor = Color.Black;
                cbPDpolarity.Visible = true;
                cbEncoderTrackedPosition.Visible = false;
                lbEncoderTrackedPosition.Visible = true;
                lbEncoderTrackedPosition.ForeColor = Color.Gray;
                cdPDdirection.Visible = true;
                lbPDdirection.Visible = true;
                lbPDdirection.ForeColor = Color.Black;
                txtbCurrentEncoderPosition.Visible = false;
                nudSetPosition.Visible = false;
                lbCurrentEncoderPosition.Visible = false;
                lbSetPosition.Visible = false;
                txtbCurrentStepperPosition.Visible = false;
                lbCurrentStepperPosition.Visible = false;
            }
        }

        /**
        * @brief Clear Heads Push Button 
        * 
        * This function is called when "Clear Heads" button is pushed 
        * 
        * It sends the relevent command to the driver board whilst unchecking all other relevent checkboxes to indicate that all heads are cleared.
        * 
        * @param sender The object that contains the reference to the object that raised the event
        * @param e The event data
        */
        private void btnClearHead_Click(object sender, EventArgs e)
        {
            if ((ChbxIsConnected.Checked) && (ChbxPower.Checked))
            {
                driver_board.Write("C");
                chbxIsFillNozzle.Checked = false;
                chbxIsFillSpan.Checked = false;
                chbxIsFillHead.Checked = false;
                chbxIsFillGap.Checked = false;
                txtbNozzleSpanStatusBox.Text = "";
            }
            else
            {
                MessageBox.Show("Not Connected!", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /**
        * @brief Fill Cycle A Button Press
        * 
        * This function is called Fill Cycle A Button is Pressed. Calls the Fill Cycle function.
        * 
        * @param sender The object that contains the reference to the object that raised the event
        * @param e The event data
        */
        private void FillCycleA_Click(object sender, EventArgs e)
        {
            if (ChbxPower.Checked)
            {
                FillCycle(sender, e);
                txtbNozzleSpanStatusBox.Text = "";
            }
            else
            {
                MessageBox.Show("Not Connected!", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /**
        * @brief Fill Cycle B Button Press
        * 
        * This function is called Fill Cycle B Button is Pressed. Calls the Fill Cycle function.
        * 
        * @param sender The object that contains the reference to the object that raised the event
        * @param e The event data
        */
        private void FillCycleB_Click(object sender, EventArgs e)
        {
            if (ChbxPower.Checked)
            {
                FillCycle(sender, e);
                txtbNozzleSpanStatusBox.Text = "";
            }
            else
            {
                MessageBox.Show("Not Connected!", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /**
        * @brief Fill Cycle C Button Press
        * 
        * This function is called Fill Cycle C Button is Pressed. Calls the Fill Cycle function.
        * 
        * @param sender The object that contains the reference to the object that raised the event
        * @param e The event data
        */
        private void FillCycleC_Click(object sender, EventArgs e)
        {
            if (ChbxPower.Checked)
            {
                FillCycle(sender, e);
                txtbNozzleSpanStatusBox.Text = "";
            }
            else
            {
                MessageBox.Show("Not Connected!", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /**
        * @brief Fill Cycle Function
        * 
        * This function is called by either Fill Cycle A,B or C.
        * 
        * Sends relevent commands in bytes to driver board in order to fill cycles.
        * 
        * Contains tags to identify which push button has been triggered.
        * 
        * @param sender The object that contains the reference to the object that raised the event
        * @param e The event data
        */
        private void FillCycle(object sender, EventArgs e)
        {
            // Tags used to identify which button has been pressed, to modify, change button tag name on Designer Panel
            System.Windows.Forms.Button b = (System.Windows.Forms.Button)sender;
            if (ChbxIsConnected.Checked && ChbxPower.Checked)
            {
                BitsArray = new byte[18];

                BitsArray[0] = (byte)('s');
                BitsArray[1] = (byte)(activeDropModeHead + 1);

                for (int index = 2; index < 18; index++)
                {
                    if (b.Tag == "fillACycle")
                    {
                        BitsArray[index] = A_Bits[(index + 1) % 3];
   
                    }
                    else if (b.Tag == "fillBCycle")
                    {
                        BitsArray[index] = B_Bits[(index + 1) % 3];
                      
                    }
                    else if (b.Tag == "fillCCycle")
                    {
                        BitsArray[index] = C_Bits[(index + 1) % 3];
                  
                    }
                }

                driver_board.Write(BitsArray, 0, 18);

            }
            else
            {
                MessageBox.Show("Not Connected!", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        /**
        * @brief Gap Value Modified
        * 
        * This function is called the Gap Value Numeric Up Down Box is modified.
        * 
        * Stores new value in variable called "activeGapValue"
        * 
        * @param sender The object that contains the reference to the object that raised the event
        * @param e The event data
        */
        private void GapValue_ValueChanged(object sender, EventArgs e)
        {
            activeGapValue = (int)nudGap.Value;
        }

        /**
        * @brief Fill Gap Button 
        * 
        * This function is called the Fill Gap Button is pressed
        * 
        * Calls the FillGap() function
        * 
        * @param sender The object that contains the reference to the object that raised the event
        * @param e The event data
        */
        private void FillGapButton_Click(object sender, EventArgs e)
        {
            fillGap();
            chbxIsFillGap.Checked = true;
            chbxIsFillHead.Checked = false;
            chbxIsFillNozzle.Checked = false;
            chbxIsFillSpan.Checked = false;
            txtbNozzleSpanStatusBox.Text = "";
        }

        /**
        * @brief Fill Gap Function 
        * 
        * This function is called the Fill Gap Button is pressed
        * 
        * Prints in every nth nozzle gap.
        * 
        * @param sender The object that contains the reference to the object that raised the event
        * @param e The event data
        */
        private void fillGap()
        {
            if (ChbxPower.Checked)
            {
                BytesToSend = new byte[18];

                BytesToSend[0] = (byte)('s');
                BytesToSend[1] = (byte)(activeDropModeHead + 1);
                BytesToSend[2] = (byte)1;
                for (int x = 0; x < activeSpanValue; x += (activeGapValue + 1))
                {

                    byte bit_value = (byte)(x % 8);
                    byte byte_value = (byte)((x / 8) + 2);
                    BytesToSend[byte_value] |= (byte)(1 << bit_value);
                }
                Console.WriteLine("");

                foreach (byte b in BytesToSend)
                {
                    Console.Write(Convert.ToString(b, 2).PadLeft(8, '0') + " ");
                }

                Console.WriteLine("");

                driver_board.Write(BytesToSend, 0, 18);
            }
        }

        /**
        * @brief Fill Head Button Press
        * 
        * This function is called when the "Fill Head" button is pushed. 
        * 
        * Sends relevent command to driver board and sets other checkboxes to unchecked to indicate which mode is running.
        * 
        * @param sender The object that contains the reference to the object that raised the event
        * @param e The event data
        */
        private void fillHead_Click(object sender, EventArgs e)
        {
            if ((ChbxIsConnected.Checked) && (ChbxPower.Checked))
            {
                chbxIsFillHead.Checked = true;
                chbxIsFillNozzle.Checked = false;
                chbxIsFillSpan.Checked = false;
                txtbNozzleSpanStatusBox.Text = "";
                driver_board.Write($"I {(activeDropModeHead + 1).ToString()}");
            }
            else
            {
                MessageBox.Show("Not Connected!", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /**
        * @brief PD Polarity Mode Selection
        * 
        * This function is called when the PD Polarity Mode is changed.
        * 
        * Stores active mode in variable called "activePD_Polarity"
        * Sends relevent command to driver board.
        * Only runs if driver baord is connected.
        * 
        * @param sender The object that contains the reference to the object that raised the event
        * @param e The event data
        */
        private void PD_Polarity_SelectedIndexChanged(object sender, EventArgs e)
        {
            activePD_Polarity = cbPDpolarity.SelectedIndex; 
            if ((activePD_Polarity == 0) && (ChbxIsConnected.Checked))
            {
                driver_board.Write("q1");
                Console.WriteLine("Product Detect Polarity Changed to Active HIGH!");
            }

            else if ((activePD_Polarity == 1) && (ChbxIsConnected.Checked))
            {
                driver_board.Write("q2");
                Console.WriteLine("Product Detect Polarity Changed to Active LOW!");
            }
        }

        /**
        * @brief Encoder Position Selection Change
        * 
        * This function is called when the user modifies the Encoer Position Mode
        * 
        * Sends relevent command to driver board if Encoder Position is changed between (Stepper Normal and Stepper Reverse)
        * 
        * Only runs if board is connected.
        * 
        * @param sender The object that contains the reference to the object that raised the event
        * @param e The event data
        */
        private void EncoderTrackedPositionSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Selection changes for Encoder Tracked Poisition Modes (Normal or Reversed Modes)
            activeEncoderPosition = cbEncoderTrackedPosition.SelectedIndex; 
            if ((activeEncoderPosition == 0) && (ChbxIsConnected.Checked))
            {
                driver_board.Write("D1");
                Console.WriteLine("Stepper Normal");
            }

            else if ((activeEncoderPosition == 1) && (ChbxIsConnected.Checked))
            {
                driver_board.Write("D0");
                Console.WriteLine("Stepper Reverse");
            }
        }

        /**
        * @brief PD Direction Selection Change
        * 
        * This function is called when the user modifes the PD Direction Setting
        * Sends relevent command to driver board depending on which mode they have selected (continuous or single)
        * 
        * @param sender The object that contains the reference to the object that raised the event
        * @param e The event data
        */
        private void pdDirection_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Selection changes between pd modes
            activePDdirection = cdPDdirection.SelectedIndex; 
            if ((activePDdirection == 0) && (ChbxIsConnected.Checked))
            {
                driver_board.Write("K1");
                Console.WriteLine("Continuous Mode");
            }

            else if ((activePDdirection == 1) && (ChbxIsConnected.Checked))
            {
                driver_board.Write("K0");
                Console.WriteLine("Single Mode");
            }
        }

        /**
        * @brief Mode Selection
        * 
        * This function is called when the user switches between Drop Watching Mode and Image Mode
        * 
        * @param sender The object that contains the reference to the object that raised the event
        * @param e The event data
        */
        private void tcDropWatchingAndImageModes_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Function to determine setting changes by user between Image and Drp Watching Modes
            if (tcDropWatchingAndImageModes.SelectedIndex == 0)
            {
                cbDropWatchMode_SelectedIndexChanged(sender, e);
                cbDropWatchHeadSelection_SelectedIndexChanged(sender, e);
            }

            else if (tcDropWatchingAndImageModes.SelectedIndex == 1)
            {
                ImageModeSelection_SelectedIndexChanged(sender, e);
                cbDropWatchHeadSelection_SelectedIndexChanged(sender, e);

                if (activeImageMode == 2)
                {
                    nudSetPosition.Visible = false;
                    lbSetPosition.Visible = false;
                    txtbCurrentEncoderPosition.Visible = false;
                    txtbCurrentStepperPosition.Visible = false;
                    lbCurrentEncoderPosition.Visible = false;
                    lbCurrentStepperPosition.Visible = false;
                }
            }
        }

        /**
        * @brief Arrow Keys Pushed
        * 
        * This function is called when the user presses down on an arrow key
        * If right arrow key, then increase nozzle value by increments of 1 and send command to driver board.
        * If left arrow key, then decrease nozzle value by increments of 1 and send command to driver board.
        * 
        * @param sender The object that contains the reference to the object that raised the event
        * @param e The event data
        */
        private void DropWatchingTab_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                // Decrease Nozzle Value by -1 increment and send command to update board when left arrow key pressed
                case (Keys.Left):
                    nudNozzle.Value = nudNozzle.Value - 1;
                    activeNozzleValue = (int)nudNozzle.Value;

                    if (chbxIsFillNozzle.Checked)
                    {
                        driver_board.Write($"n {(activeDropModeHead + 1).ToString()} {activeNozzleValue.ToString()}");
                    }

                    else if (chbxIsFillSpan.Checked)
                    {
                        driver_board.Write($"N {(activeDropModeHead + 1).ToString()} {activeNozzleValue.ToString()} {activeSpanValue.ToString()}");
                    }
                    break;

                    // Increase Nozzle Value by +1 increment and send command to update board when right arrow key pressed
                case (Keys.Right):
                    nudNozzle.Value = nudNozzle.Value + 1;
                    activeNozzleValue = (int)nudNozzle.Value;

                    if (chbxIsFillNozzle.Checked)
                    {
                        driver_board.Write($"n {(activeDropModeHead + 1).ToString()} {activeNozzleValue.ToString()}");
                    }

                    else if (chbxIsFillSpan.Checked)
                    {
                        driver_board.Write($"N {(activeDropModeHead + 1).ToString()} {activeNozzleValue.ToString()} {activeSpanValue.ToString()}");
                    }
                    break;
            }
        }

        /**
        * @brief Drop Watching Tab pressed
        * 
        * This function is called when the user clicks anywhere withinthe drop watching tab
        * 
        * This will set focus onto the tab allowing for other functions to run such as right and left keys increasing and decreasing the nozzle value.
        * 
        * @param sender The object that contains the reference to the object that raised the event
        * @param e The event data
        */
        private void DropWatchingTab_Click(object sender, EventArgs e)
        {
            DropWatchingTab.Focus();
        }

        /**
        * @brief Cancel Button Pushed
        * 
        * This function is called when the "Cancel" button is pushed.
        * This is to cancel all printing and clear all heads
        * 
        * Runs only if driver board is connected and powered on.
        * 
        * @param sender The object that contains the reference to the object that raised the event
        * @param e The event data
        */
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if ((ChbxIsConnected.Checked) && (ChbxPower.Checked))
            {
                driver_board.Write("C");
                txtbPrintStatus.Text = "Print Cancelled";
            }
        }

        /**
        * @brief Delete all Images and Data Files from Folder
        * 
        * This function is called when the user closes the application. 
        * THis is to delete all files stored in the "Output Images" folder created to save memory.
        * 
        * @param sender The object that contains the reference to the object that raised the event
        * @param e The event data
        */
        private void DeleteAllFilesInFolder(object sender, EventArgs e)
        {
            // Delete All Files Saved in "Output Images" when closing Application
            System.IO.DirectoryInfo directory = new DirectoryInfo(outputFolderPath);

            foreach (FileInfo file in directory.GetFiles())
            {
                file.Delete();
            }
        }

        /**
        * @brief Loads all data when opening application
        * 
        * This function is called when the user starts application
        * 
        * It loads all stored values/data from the last session. 
        * This is to make everything more convenient for the user saving repetitive actions.
        * 
        * @param sender The object that contains the reference to the object that raised the event
        * @param e The event data
        */

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                // Load Settings from Previous Saved Session
                tcDropWatchingAndImageModes.Enabled = false;
                StatusTable.Enabled = false;
                nudFrequency.ReadOnly = true;
                lbFrequency.ForeColor = Color.Gray;
                lbStatus.ForeColor = Color.Gray;
                lbTimeOn.ForeColor = Color.Gray;
                cbSerialPort_DropDown(sender, e);
                cbSerialPort.SelectedItem = Properties.Settings.Default.Serial_Port;
                nudFrequency.Value = Properties.Settings.Default.Frequency;
                cbDropWatchMode_SelectedIndexChanged(sender, e);
                cbDropWatchMode.SelectedItem = Properties.Settings.Default.DropWatchMode;
                cbDropWatchHeadSelection_SelectedIndexChanged(sender, e);
                cbDropWatchHeadSelection.SelectedItem = Properties.Settings.Default.DropWatchHead;
                nudNozzle.Value = Properties.Settings.Default.Index;
                activeNozzleValue = (int)nudNozzle.Value;
                nudSpan.Value = Properties.Settings.Default.Span;
                activeSpanValue = (int)nudSpan.Value;
                nudGap.Value = Properties.Settings.Default.Gap;
                activeGapValue = (int)nudGap.Value;
                ImageModeSelection_SelectedIndexChanged(sender, e);
                cbImageMode.SelectedItem = Properties.Settings.Default.ImageMode;
                PD_Polarity_SelectedIndexChanged(sender, e);
                cbPDpolarity.SelectedItem = Properties.Settings.Default.pdPolarity;
                EncoderTrackedPositionSelection_SelectedIndexChanged(sender, e);
                cbEncoderTrackedPosition.SelectedItem = Properties.Settings.Default.Encoder_TrackedPosition;
                cdPDdirection.SelectedItem = Properties.Settings.Default.pd_direction;
                tcDropWatchingAndImageModes.SelectedIndex = Properties.Settings.Default.TabNumber;
                chbxIsFillNozzle.Checked = Properties.Settings.Default.FillNozzleCheckedStatus;
                chbxIsFillSpan.Checked = Properties.Settings.Default.FillSpanCheckedStatus;
                chbxIsFillGap.Checked = Properties.Settings.Default.FillGapCheckedStatus;
                chbxIsFillHead.Checked = Properties.Settings.Default.FillHeadCheckedStatus;
            }

            catch
            {
                Console.WriteLine("Error saving");
            }
        }

        /**
        * @brief Saves all data when closing application
        * 
        * This function is called when the User presses the close button 
        * 
        * It stores all the user modified values/data and stores it ready to load up the next time the user opens it. 
        * This is to make everything more convenient for the user the next time they use this application instead of re-selecting all relevent options.
        * 
        * @param sender The object that contains the reference to the object that raised the event
        * @param e The event data
        */
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Save data to user settings
            try
            {
                Properties.Settings.Default.Serial_Port = cbSerialPort.SelectedItem.ToString();
                Properties.Settings.Default.Frequency = (int)nudFrequency.Value;
                Properties.Settings.Default.DropWatchMode = cbDropWatchMode.SelectedItem.ToString();
                Properties.Settings.Default.DropWatchHead = cbDropWatchHeadSelection.SelectedItem.ToString();
                Properties.Settings.Default.Index = (int)nudNozzle.Value;
                Properties.Settings.Default.Span = (int)nudSpan.Value;
                Properties.Settings.Default.Gap = (int)nudGap.Value;
                Properties.Settings.Default.TabNumber = tcDropWatchingAndImageModes.SelectedIndex;
                Properties.Settings.Default.FillNozzleCheckedStatus = chbxIsFillNozzle.Checked;
                Properties.Settings.Default.FillSpanCheckedStatus = chbxIsFillSpan.Checked;
                Properties.Settings.Default.FillGapCheckedStatus = chbxIsFillGap.Checked;
                Properties.Settings.Default.FillHeadCheckedStatus = chbxIsFillHead.Checked;
                DeleteAllFilesInFolder(sender, e);
                Properties.Settings.Default.Save();
            }

            catch
            {
                Console.WriteLine("Error saving");
            }
        }

        /**
        * @brief Sets the Position of Print Head for Image Printing
        * 
        * Function is called when user enters value into Set Position Numeric Up Down
        * 
        * Sends relevent command to the driver board. 
        * 
        * @param sender The object that contains the reference to the object that raised the event
        * @param e The event data
        */

        private void SetPosition(object sender, EventArgs e)
        {
            System.Windows.Forms.NumericUpDown setPosition = (System.Windows.Forms.NumericUpDown)sender;
            if ((ChbxIsConnected.Checked) && (ChbxPower.Checked))
            {
                if (setPosition.Tag == "nudSetStartPosition")
                {
                    //Set Position of Head
                    string messageToSend = ",";
                    messageToSend += " " + nudSetPosition.Value;
                    messageToSend += "\r\n";

                    driver_board.Write(messageToSend);

                }
            }
        }

        /**
        * @brief Sets the Position of Print Head for Image Printing
        * 
        * Function is called when user modifies value in numeric up down
        * 
        * Calls the SetPosition function.
        * 
        * @param sender The object that contains the reference to the object that raised the event
        * @param e The event data
        */
        private void nudSetPosition_ValueChanged(object sender, EventArgs e)
        {
            SetPosition(sender, e);
        }
    }
}
