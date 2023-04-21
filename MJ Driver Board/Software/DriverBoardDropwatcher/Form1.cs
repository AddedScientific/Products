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

namespace DriverBoardDropwatcher
{
    public partial class Form1 : Form
    {
        private PictureBox GrayScaleImage1;
        private OpenFileDialog ofd;
        bool valid_port_selected = false;
        string port_name;
        int failCounter = 0;
        static SerialPort driver_board;
        private Thread trd;
        int activeDropWatch = 0;
        int activeSingleHead;
        int activeNozzleValue = 1;
        int activeSpanValue = 1;
        int activeImageMode;
        int headStatus1;
        int headStatus2;
        int headStatus3;
        int headStatus4;
        int Gap_Value = 0;
        int printCount1 = 0;
        int printCount2 = 0;
        int printCount3 = 0;
        int printCount4 = 0;
        int previousPrintCount1 = 0;
        int previousPrintCount2 = 0;
        int previousPrintCount3 = 0;
        int previousPrintCount4 = 0;
        int actFreq;
        int timeBoardOn = -1;
        int activePD_Polarity;
        int activeEncoderPosition;
        int activePDdirection;
        bool goLeft;
        bool goRight;
        int[] HeadPrintCountersStoredAsInt = new int[4];
        int[] PreviousHeadPrintCounters = new int[4];
        int[] HeadStatus = new int[4];
        byte[] A_Bits = { 0b10010010, 0b01001001, 0b00100100 };
        byte[] B_Bits = { 0b01001001, 0b00100100, 0b10010010 };
        byte[] C_Bits = { 0b00100100, 0b10010010, 0b01001001 };
        byte[] BitsArray;

        public Form1()
        {
            InitializeComponent();
            pictureBox1.Image = Properties.Resources.upload;
            pictureBox2.Image = Properties.Resources.upload;
            pictureBox3.Image = Properties.Resources.upload;
            pictureBox4.Image = Properties.Resources.upload;
            this.FormClosing += Form1_FormClosing;
            Thread trd = new Thread(new ThreadStart(this.ThreadTask));
            trd.IsBackground = true;
            trd.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
            statusTextBox.Text = "Disconnected"; //Displays Disconnected in Status Box
            ofd = new OpenFileDialog();
            ofd.Filter = "Image Files(*.jpg; *.jpeg; *.bmp); *.png|*.jpg; *.jpeg; *.bmp; *.png";
        }
        private void ThreadTask()
        {
            // Repeatedly checks if board is connected
            while (true)
            {
                if (isConnected.Checked)
                {
                    try
                    {
                        driver_board.Write("b\n");
                    }

                    catch
                    {
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

        private void serialPort_DropDown(object sender, EventArgs e)
        {
            serialPort.Items.Clear(); //Clears all items from serial port drop down
            string[] ports = SerialPort.GetPortNames();
            // Display each port name to the console.
            if (ports.Length < 1)
                serialPort.Items.Add("No ports found");
            else
            {
                foreach (string port in ports)
                {
                    Console.WriteLine(port);
                    serialPort.Items.Add(port); //Adds all available ports to drop down
                }
            }
        }
        private void serialPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            port_name = serialPort.SelectedItem.ToString();
            Console.WriteLine(port_name);
            if (port_name == "No ports found")
            {
                Console.WriteLine("Cannot connect.");
                valid_port_selected = false;
            }

            else
                valid_port_selected = true;
        }
        private void disconnect_board()
        {
            Console.WriteLine("Disconnecting.");
            if (driver_board != null && driver_board.IsOpen)
                driver_board.Close();
            isConnected.Checked = false;
        }
        private void connect_board()
        {
            if (valid_port_selected && !isConnected.Checked)
            {
                driver_board = new SerialPort();
                driver_board.PortName = port_name;
                try
                {
                    driver_board.Open();
                    driver_board.ReadExisting();
                    driver_board.DataReceived += new SerialDataReceivedEventHandler(DataRecievedHandler);
                    isConnected.Checked = true;
                    Console.WriteLine("Connected");
                    statusTextBox.Text = "Connected";
                }
                catch
                {
                    Console.WriteLine("Error opening port");
                    statusTextBox.Text = "Error opening port";
                    MessageBoxButtons BoxButtons = MessageBoxButtons.RetryCancel;
                    DialogResult results = MessageBox.Show("Error opening port", "Port Error", BoxButtons, MessageBoxIcon.Error);
                    if (results == DialogResult.Retry)
                    {
                        Application.Restart();
                        Environment.Exit(0);
                    }                    
                }
            }
            else if (isConnected.Checked)
            {
                disconnect_board();
                statusTextBox.Text = "Disconnected";
                Console.WriteLine("Disconnected");
            }
        }

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
                        }
                        catch
                        {
                            failCounter++;
                            Console.WriteLine("Parse JSON Failed for the {0} time.", failCounter);

                            //MessageBoxButtons BoxButtons = MessageBoxButtons.RetryCancel;
                            //DialogResult results = MessageBox.Show("Parse JSON Failed", "Parse JSON Error", BoxButtons, MessageBoxIcon.Error);
                            //if (results == DialogResult.Retry)
                            //{
                            //    Application.Restart();
                            //    Environment.Exit(0);
                            //}
                        }
                    }
                }

                else
                {
                    Console.WriteLine(input);
                }
            }

        }

        private void determineStatus()
        {
            System.Windows.Forms.TextBox[] HeadTextStatus = { Head1TextStatus, Head2TextStatus, Head3TextStatus, Head4TextStatus };
            NumericUpDown[] nudHeadVoltages = { nudVoltageHead1, nudVoltageHead2, nudVoltageHead3, nudVoltageHead4 };
            NumericUpDown[] nudHeadTemperatures = { nudTemperatureHead1, nudTemperatureHead2, nudTemperatureHead3, nudTemperatureHead4 };
            System.Windows.Forms.TextBox[] HeadTemperaturesOutputText = { temperatureOutput1, temperatureOutput2, temperatureOutput3, temperatureOutput4 };
            System.Windows.Forms.TextBox[] HeadPrintCounters = { printCounter1, printCounter2, printCounter3, printCounter4 };

            if (power.Checked)
            {
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
                                DropWatchingStatus.Text = "Printing";
                                DropWatchingStatus.BackColor = Color.Green;
                                break;
                            }
                            else
                            {
                                HeadTextStatus[i].Text = "Idle"; // Displays Message
                                HeadTextStatus[i].BackColor = Color.Orange; //Sets Status Box to Orange indicating all is fine but not printing
                                DropWatchingStatus.Text = "Head Idle";
                                DropWatchingStatus.BackColor = Color.Orange;
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
                Head1TextStatus.Text = "Powered off.";
                nudVoltageHead1.Visible = false;
                nudTemperatureHead1.Visible = false;
                temperatureOutput1.Visible = false;
                printCounter1.Visible = false;
                Head2TextStatus.Text = "Powered off.";
                nudVoltageHead2.Visible = false;
                nudTemperatureHead2.Visible = false;
                temperatureOutput2.Visible = false;
                printCounter2.Visible = false;
                Head3TextStatus.Text = "Powered off.";
                nudVoltageHead3.Visible = false;
                nudTemperatureHead3.Visible = false;
                temperatureOutput3.Visible = false;
                printCounter3.Visible = false;
                Head4TextStatus.Text = "Powered off.";
                nudVoltageHead4.Visible = false;
                nudTemperatureHead4.Visible = false;
                temperatureOutput4.Visible = false;
                printCounter4.Visible = false;
                Head1TextStatus.BackColor = Color.Red;
                Head2TextStatus.BackColor = Color.Red;
                Head3TextStatus.BackColor = Color.Red;
                Head4TextStatus.BackColor = Color.Red;
            }
        }



        //    switch (headStatus1)
        //    {
        //        case -2: // -2 means head is not connected to board
        //            Head1TextStatus.Text = "Not Connected"; // Displays Message
        //            nudVoltageHead1.Visible = false; // Hides Voltage Box
        //            temperature1.Visible = false; // Hides Set Temperature Box 
        //            temperatureOutput1.Visible = false; //Hides Current Temperature Box
        //            printCounter1.Visible = false; // Hides Print Counter Box
        //            Head1TextStatus.BackColor = Color.Red; //Sets Status Box to Red indicating head is not connected
        //            break;
        //        case -3: // -3 means error with head
        //            Head1TextStatus.Text = "Ready Error"; // Displays Message
        //            nudVoltageHead1.Visible = false; // Hides Voltage Box
        //            temperature1.Visible = false; // Hides Set Temperature Box 
        //            temperatureOutput1.Visible = false; //Hides Current Temperature Box
        //            printCounter1.Visible = false; // Hides Print Counter Box
        //            Head1TextStatus.BackColor = Color.Red; //Sets Status Box to Red indicating head error
        //            break;
        //        case 10: // Head is connected but idle
        //            nudVoltageHead1.Visible = true; // Displays Voltage Box
        //            temperature1.Visible = true; // Dislays Set Temperature Box 
        //            temperatureOutput1.Visible = true; //Displays Current Temperature Box
        //            printCounter1.Visible = true; // Displays Print Counter Box

        //            if (printCount1 > previousPrintCount1)
        //            {
        //                Head1TextStatus.Text = "Printing";
        //                Head1TextStatus.BackColor = Color.Green;
        //                break;
        //            }
        //            else
        //            {
        //                Head1TextStatus.Text = "Idle"; // Displays Message
        //                Head1TextStatus.BackColor = Color.Orange; //Sets Status Box to Orange indicating all is fine but not printing
        //                break;
        //            }
        //        default: 
        //            Head1TextStatus.Text = "Printing"; // Displays Message
        //            nudVoltageHead1.Visible = true; // Displays Voltage Box
        //            temperature1.Visible = true; // Dislays Set Temperature Box 
        //            temperatureOutput1.Visible = true; //Displays Current Temperature Box
        //            printCounter1.Visible = true; // Displays Print Counter Box
        //            Head1TextStatus.BackColor = Color.Green;//Sets Status Box to Orange indicating all is fine and printing
        //            break;
        //    }

        //    // Repeats above steps for each Head
        //    switch (headStatus2)
        //    {
        //        case -2:
        //            Head2TextStatus.Text = "Not Connected";
        //            nudVoltageHead2.Visible = false;
        //            temperature2.Visible = false;
        //            temperatureOutput2.Visible = false;
        //            printCounter2.Visible = false;
        //            Head2TextStatus.BackColor = Color.Red;
        //            break;
        //        case -3:
        //            Head2TextStatus.Text = "Ready Error";
        //            nudVoltageHead2.Visible = false;
        //            temperature2.Visible = false;
        //            temperatureOutput2.Visible = false;
        //            printCounter2.Visible = false;
        //            Head2TextStatus.BackColor = Color.Red;
        //            break;
        //        case 10:
        //            Head2TextStatus.Text = "Idle";
        //            nudVoltageHead2.Visible = true;
        //            temperature2.Visible = true;
        //            temperatureOutput2.Visible = true;
        //            printCounter2.Visible = true;
        //            Head2TextStatus.BackColor = Color.Orange;
        //            break;
        //        default:
        //            Head2TextStatus.Text = "Printing";
        //            nudVoltageHead2.Visible = true;
        //            temperature2.Visible = true;
        //            temperatureOutput2.Visible = true;
        //            printCounter2.Visible = true;
        //            Head2TextStatus.BackColor = Color.Green;
        //            break;
        //    }

        //    switch (headStatus3)
        //    {
        //        case -2:
        //            Head3TextStatus.Text = "Not Connected";
        //            nudVoltageHead3.Visible = false;
        //            temperature3.Visible = false;
        //            temperatureOutput3.Visible = false;
        //            printCounter3.Visible = false;
        //            Head3TextStatus.BackColor = Color.Red;
        //            break;
        //        case -3:
        //            Head3TextStatus.Text = "Ready Error";
        //            nudVoltageHead3.Visible = false;
        //            temperature3.Visible = false;
        //            temperatureOutput3.Visible = false;
        //            printCounter3.Visible = false;
        //            Head3TextStatus.BackColor = Color.Red;
        //            break;
        //        case 10:
        //            Head3TextStatus.Text = "Idle";
        //            nudVoltageHead3.Visible = true;
        //            temperature3.Visible = true;
        //            temperatureOutput3.Visible = true;
        //            printCounter3.Visible = true;
        //            Head3TextStatus.BackColor = Color.Orange;
        //            break;
        //        default:
        //            Head3TextStatus.Text = "Printing";
        //            nudVoltageHead3.Visible = true;
        //            temperature3.Visible = true;
        //            temperatureOutput3.Visible = true;
        //            printCounter3.Visible = true;
        //            Head3TextStatus.BackColor = Color.Green;
        //            break;
        //    }

        //    switch (headStatus4)
        //    {
        //        case -2:
        //            Head4TextStatus.Text = "Not Connected";
        //            nudVoltageHead4.Visible = false;
        //            temperature4.Visible = false;
        //            temperatureOutput4.Visible = false;
        //            printCounter4.Visible = false;
        //            Head4TextStatus.BackColor = Color.Red;
        //            break;
        //        case -3:
        //            Head4TextStatus.Text = "Ready Error";
        //            nudVoltageHead4.Visible = false;
        //            temperature4.Visible = false;
        //            temperatureOutput4.Visible = false;
        //            printCounter4.Visible = false;
        //            Head4TextStatus.BackColor = Color.Red;
        //            break;
        //        case 10:
        //            Head4TextStatus.Text = "Idle";
        //            nudVoltageHead4.Visible = true;
        //            temperature4.Visible = true;
        //            temperatureOutput4.Visible = true;
        //            printCounter4.Visible = true;
        //            Head1TextStatus.BackColor = Color.Orange;
        //            break;
        //        default:
        //            Head4TextStatus.Text = "Printing";
        //            nudVoltageHead4.Visible = true;
        //            temperature4.Visible = true;
        //            temperatureOutput4.Visible = true;
        //            printCounter4.Visible = true;
        //            Head1TextStatus.BackColor = Color.Green;
        //            break;
        //    }
        //}

        //If Board is Powered Off, then all heads displays Powered Off and Hides all Settings
        //else
        //{
        //    Head1TextStatus.Text = "Powered off.";
        //    nudVoltageHead1.Visible = false;
        //    temperature1.Visible = false;
        //    temperatureOutput1.Visible = false;
        //    printCounter1.Visible = false;
        //    Head2TextStatus.Text = "Powered off.";
        //    nudVoltageHead2.Visible = false;
        //    temperature2.Visible = false;
        //    temperatureOutput2.Visible = false;
        //    printCounter2.Visible = false;
        //    Head3TextStatus.Text = "Powered off.";
        //    nudVoltageHead3.Visible = false;
        //    temperature3.Visible = false;
        //    temperatureOutput3.Visible = false;
        //    printCounter3.Visible = false;
        //    Head4TextStatus.Text = "Powered off.";
        //    nudVoltageHead4.Visible = false;
        //    temperature4.Visible = false;
        //    temperatureOutput4.Visible = false;
        //    printCounter4.Visible = false;
        //    Head1TextStatus.BackColor = Color.Red;
        //    Head2TextStatus.BackColor = Color.Red;
        //    Head3TextStatus.BackColor = Color.Red;
        //    Head4TextStatus.BackColor = Color.Red;
        //}


        private void parseJsonData(string input_string)
        {
            dynamic d = JObject.Parse(input_string);
            //--------------------------------------------------
            for (int i = 0; i <= 3; i++)
            {
                HeadStatus[i] = d.heads[i].status;
            }
            //--------------------------------------------------

            //Check Print Counts for Heads
            System.Windows.Forms.TextBox[] HeadPrintCounters = { printCounter1, printCounter2, printCounter3, printCounter4 };

            for (int i = 0; i <= 3; i++)
            {
                HeadPrintCounters[i].Text = d.heads[i].printCounts.ToString();
                HeadPrintCountersStoredAsInt[i] = d.heads[i].printCounts;
            }

            determineStatus();

            if (!power.Checked)
            {
                DropWatchingStatus.Text = "Powered Off";
                DropWatchingStatus.BackColor = Color.Red;
            }

            for (int i = 0; i <= 3; i++)
            {
                PreviousHeadPrintCounters[i] = HeadPrintCountersStoredAsInt[i];
            }

            //printCount1 = d.heads[0].printCounts; // Counts number of prints for head 1
            //printCounter1.Text = (printCount1.ToString()); // Displays Number of Print Count on GUI
            //printCount2 = d.heads[1].printCounts; // Counts number of prints for head 2
            //printCounter2.Text = (printCount2.ToString()); // Displays Number of Print Count on GUI
            //printCount3 = d.heads[2].printCounts; // Counts number of prints for head 3
            //printCounter3.Text = (printCount3.ToString()); // Displays Number of Print Count on GUI
            //printCount4 = d.heads[3].printCounts; // Counts number of prints for head 4
            //printCounter4.Text = (printCount4.ToString()); // Displays Number of Print Count on GUI 

            //determineStatus();

            //// IF print count is increasing for any head, "Head Printing" will be displayed in Status Box
            //if ((power.Checked) && ((printCount1) > (previousPrintCount1) || (printCount2) > (previousPrintCount2) || (printCount3) > (previousPrintCount3) || (printCount4) > (previousPrintCount4)))
            //{
            //    DropWatchingStatus.Text = ("Head Printing");
            //    DropWatchingStatus.BackColor = Color.Green;
            //}

            //// IF print count is stationary for all heads, "Head Idle" will be displayed in Status Box
            //else if ((power.Checked) && (((printCount1) == (previousPrintCount1)) && ((printCount2) == (previousPrintCount2)) && ((printCount3) == (previousPrintCount3)) && ((printCount4) == (previousPrintCount4))))
            //{
            //    DropWatchingStatus.Text = ("Head Idle");
            //    DropWatchingStatus.BackColor = Color.Orange;
            //}
            //else if (!power.Checked)
            //{
            //    DropWatchingStatus.Text = ("Powered Off");
            //    DropWatchingStatus.BackColor = Color.Red;
            //}

            ////Sets Previous Count to current count to detect if value is going up or staying the same
            //previousPrintCount1 = printCount1;
            //previousPrintCount2 = printCount2;
            //previousPrintCount3 = printCount3;
            //previousPrintCount4 = printCount4;

            //--------------------------------------------------
            //Check Currrent Temeperatures for Heads
            System.Windows.Forms.TextBox[] HeadTemperatures = { temperatureOutput1, temperatureOutput2, temperatureOutput3, temperatureOutput4 };

            for (int i = 0; i <= 3; i++)
            {
                HeadTemperatures[i].Text = d.heads[i].curTemperature.ToString();

                if ((d.heads[i].isHeating) == 1)
                {
                    HeadTemperatures[i].ForeColor = Color.Red;
                }
                else
                {
                    HeadTemperatures[i].BackColor = Color.WhiteSmoke;
                }

            }

            //--------------------------------------------------
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
                frequencyDuplicate.Text = nudFrequency.Value.ToString();
            }
            power.Checked = d.board[0].power == 1 ? true : false;
            int newTime = d.board[0].timeOn;
            if (timeBoardOn != -1)
            {
                // Determines if clock is increasing
                if (newTime > timeBoardOn)
                {
                    //Console.WriteLine("Clock increasing.");
                }
                else
                {
                    //Console.WriteLine("Clock error.");
                }
            }
            timeBoardOn = newTime;
            textBox3.Text = timeBoardOn.ToString(); // Displays Clock Time in GUI
        }
        private void btnConnectDisconnect_Click(object sender, EventArgs e)
        {
            connect_board();
            tcDropWatchingAndImageModes_SelectedIndexChanged(sender, e);
        }

        private void powerOnOff_Click(object sender, EventArgs e)
        {
            if (isConnected.Checked)
            {
                if (power.Checked)
                {
                    powerOff();
                    statusTextBox.Text = "Power Off"; //Displays Disconnected in Status Box
                }
                else
                {
                    powerOn();
                    statusTextBox.Text = "Power On"; //Displays Connected in Status Box
                    tcDropWatchingAndImageModes_SelectedIndexChanged(sender, e);
                }
            }
        }
        private void powerOn()
        {
            if (isConnected.Checked)
            {
                driver_board.Write("O"); // Turns on Board
            }
        }
        private void powerOff()
        {
            if (isConnected.Checked)
            {
                driver_board.Write("F"); // Turns off Board
            }
        }

        private void voltage_ValueChanged(object sender, EventArgs e)
        {
            // This Function Checks for Tag Names to determine which Head Voltage is being changed.
            System.Windows.Forms.NumericUpDown voltageChanged = (System.Windows.Forms.NumericUpDown)sender;

            if (isConnected.Checked)
            {
                if (voltageChanged.Tag == "voltageHead1")
                {
                    driver_board.Write($"v {(1).ToString()} {nudVoltageHead1.Value.ToString()}");
                    Console.WriteLine("Head 1 Voltage Changed to: {0}", nudVoltageHead1.Value);
                }
                else if (voltageChanged.Tag == "voltageHead2")
                {
                    driver_board.Write($"v {(2).ToString()} {nudVoltageHead2.Value.ToString()}");
                    Console.WriteLine("Head 2 Voltage Changed to: {0}", nudVoltageHead1.Value);

                }
                else if (voltageChanged.Tag == "voltageHead3")
                {
                    driver_board.Write($"v {(3).ToString()} {nudVoltageHead3.Value.ToString()}");
                    Console.WriteLine("Head 3 Voltage Changed to: {0}", nudVoltageHead1.Value);
                }
                else if (voltageChanged.Tag == "voltageHead4")
                {
                    driver_board.Write($"v {(4).ToString()} {nudVoltageHead4.Value.ToString()}");
                    Console.WriteLine("Head 4 Voltage Changed to: {0}", nudVoltageHead1.Value);
                }
            }
        }

        private void temperature_ValueChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.NumericUpDown temperatureChanged = (System.Windows.Forms.NumericUpDown)sender;
            if (isConnected.Checked)
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
        }

        private void frequencyValue_ValueChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.NumericUpDown nudFrequencyChanged = (System.Windows.Forms.NumericUpDown)sender;
            if (isConnected.Checked)
            {
                if (nudFrequencyChanged.Tag == "nudFrequency")
                {
                    //Sets Head Frequency
                    driver_board.Write($"p {nudFrequency.Value.ToString()}");
                }
            }
        }

        private void dropWatchSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            activeDropWatch = dropWatchSelect.SelectedIndex; //Stores selected mode

            //Checks if External Mode is Selected AND Board is Connected
            if ((activeDropWatch == 1) && (isConnected.Checked))
            {
                nudFrequency.ReadOnly = true;
                frequencyLabel.ForeColor = Color.Gray;
                nudFrequency.ForeColor = Color.Gray;
                frequencyDuplicate.Visible = false;
                frequencyDuplicateLabel.Visible = false;
                driver_board.Write("M2");
                Console.WriteLine("Drop Watching External Mode");
            }

            //Checks IF internal Mode is Selected AND Board is Connected
            else if ((activeDropWatch == 0) && (isConnected.Checked))
                {
                nudFrequency.ReadOnly = false;
                frequencyLabel.ForeColor = Color.Black;
                nudFrequency.ForeColor = Color.Black;
                frequencyDuplicate.Visible = true;
                frequencyDuplicateLabel.Visible = true;
                driver_board.Write("M1");
                Console.WriteLine("Drop Watching Internal Mode");
            }
        }

        private void reset_Click(object sender, EventArgs e)
        {
            if (isConnected.Checked)
            {
                driver_board.Write("r"); //Resets Board
                tcDropWatchingAndImageModes_SelectedIndexChanged(sender, e);
            }
        }


        private void singleHead_SelectedIndexChanged(object sender, EventArgs e)
        {
            activeSingleHead = singleHead.SelectedIndex; //Stores Head Value
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // Opens File Dialog and Replaces Current Image with New Image
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Bitmap Picture1 = (Bitmap) new Bitmap(ofd.FileName);
                //GrayScaleImage1.Image = new Bitmap(ofd.FileName);
                pictureBox1.Image = MakeGrayscale3(Picture1);
                FileName1.Text = ofd.SafeFileName;
                ImageSizeText1.Text = ((Image.FromFile(ofd.FileName).Width) + " x " + (Image.FromFile(ofd.FileName).Height));
                
                // If File Size has a width of more that
                if ((Image.FromFile(ofd.FileName).Width) > 128)
                {
                    MessageBox.Show("Maximum 128 Pixels Per Head!", "File Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pictureBox2.Image = new Bitmap(ofd.FileName);
                FileName2.Text = ofd.SafeFileName;
                ImageSizeText2.Text = ((Image.FromFile(ofd.FileName).Width) + " x " + (Image.FromFile(ofd.FileName).Height));

                if ((Image.FromFile(ofd.FileName).Width) > 128)
                {
                    MessageBox.Show("Maximum 128 Pixels Per Head!", "File Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pictureBox3.Image = new Bitmap(ofd.FileName);
                FileName3.Text = ofd.SafeFileName;
                ImageSizeText3.Text = ((Image.FromFile(ofd.FileName).Width) + " x " + (Image.FromFile(ofd.FileName).Height));

                if ((Image.FromFile(ofd.FileName).Width) > 128)
                {
                    MessageBox.Show("Maximum 128 Pixels Per Head!", "File Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pictureBox4.Image = new Bitmap(ofd.FileName);
                FileName4.Text = ofd.SafeFileName;
                ImageSizeText4.Text = ((Image.FromFile(ofd.FileName).Width) + " x " + (Image.FromFile(ofd.FileName).Height));

                if ((Image.FromFile(ofd.FileName).Width) > 128)
                {
                    MessageBox.Show("Maximum 128 Pixels Per Head!", "File Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        private void NozzleValue_ValueChanged(object sender, EventArgs e)
        {
            activeNozzleValue = (int)NozzleValue.Value;
            if ((activeSpanValue + activeNozzleValue) > 128)
            {
                activeNozzleValue = 128 - activeSpanValue;
                MessageBox.Show("Maximum value of 128 Exceeded!", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                NozzleValue.Value = activeNozzleValue;
            }
        }

        private void SpanValue_ValueChanged(object sender, EventArgs e)
        {
            activeSpanValue = (int)SpanValue.Value;
            if ((activeSpanValue + activeNozzleValue) > 128)
            {
                activeSpanValue = 128 - activeNozzleValue;
                MessageBox.Show("Maximum value of 128 Exceeded!", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                SpanValue.Value = activeSpanValue;
            }
        }

        private void FillSingleNozzleButton_Click(object sender, EventArgs e)
        {
            if (isConnected.Checked)
            {
                driver_board.Write($"n {(activeSingleHead + 1).ToString()} {activeNozzleValue.ToString()}");
            }
        }
        private void FillSpanNozzleButton_Click(object sender, EventArgs e)
        {
            if (isConnected.Checked)
            {
                driver_board.Write($"N {(activeSingleHead + 1).ToString()} {activeNozzleValue.ToString()} {activeSpanValue.ToString()}");
            }
        }

        private void ImageModeSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            activeImageMode = ImageModeSelection.SelectedIndex; //Stores Head Value
            if ((activeImageMode == 0) && (isConnected.Checked))
            {
                driver_board.Write("M3");
                Console.WriteLine("Stepper Motor Mode");
                polarityLabel.ForeColor = Color.Gray;
                PD_Polarity.Visible = false;
                EncoderTrackedPositionSelection.Visible = true;
                EncoderPositionLabel.ForeColor = Color.Black;
                pdDirection.Visible = false;
                pdDirectionLabel.ForeColor = Color.Gray;
            }

            else if ((activeImageMode == 1) && (isConnected.Checked))
            {
                driver_board.Write("M4");
                Console.WriteLine("Quadrature Encoder Mode");
                polarityLabel.ForeColor = Color.Gray;
                PD_Polarity.Visible = false;
                EncoderTrackedPositionSelection.Visible = false;
                EncoderPositionLabel.ForeColor = Color.Gray;
                pdDirection.Visible = false;
                pdDirectionLabel.ForeColor = Color.Gray;
            }

            else if ((activeImageMode == 2) && (isConnected.Checked))
            {
                driver_board.Write("M5");
                Console.WriteLine("HW PD Mode");
                polarityLabel.Visible = true;
                polarityLabel.ForeColor = Color.Black;
                PD_Polarity.Visible = true;
                EncoderTrackedPositionSelection.Visible = false;
                EncoderPositionLabel.Visible = true;
                EncoderPositionLabel.ForeColor = Color.Gray;
                pdDirection.Visible = true;
                pdDirectionLabel.Visible = true;
                pdDirectionLabel.ForeColor = Color.Black;
            }
        }

        private void ClearHeadsButton_Click(object sender, EventArgs e)
        {
            if (isConnected.Checked)
            {
                driver_board.Write("C");
            }
        }

        private void FillCycleA_Click(object sender, EventArgs e)
        {
            FillCycle(sender, e);
        }

        private void FillCycleB_Click(object sender, EventArgs e)
        {
            FillCycle(sender, e);
        }

        private void FillCycleC_Click(object sender, EventArgs e)
        {
            FillCycle(sender, e);
        }

        private void FillCycle(object sender, EventArgs e)
        {
            System.Windows.Forms.Button b = (System.Windows.Forms.Button)sender;
            if (isConnected.Checked)
            {
                BitsArray = new byte[18];

                BitsArray[0] = (byte)('s');
                BitsArray[1] = (byte)(activeSingleHead + 1);

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

        }
        private void GapValue_ValueChanged(object sender, EventArgs e)
        {
            Gap_Value = (int)GapValue.Value;
        }

        private void FillGapButton_Click(object sender, EventArgs e)
        {

        }

        private void fillHead_Click(object sender, EventArgs e)
        {
            driver_board.Write($"I {(activeSingleHead + 1).ToString()}");
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            serialPort_DropDown(sender, e);
            serialPort.SelectedItem = Properties.Settings.Default.Serial_Port;
            nudFrequency.Value = Properties.Settings.Default.Frequency;
            dropWatchSelect_SelectedIndexChanged(sender, e);
            dropWatchSelect.SelectedItem = Properties.Settings.Default.DropWatchMode;
            singleHead_SelectedIndexChanged(sender, e);
            singleHead.SelectedItem = Properties.Settings.Default.DropWatchHead;
            NozzleValue.Value = Properties.Settings.Default.Index;
            SpanValue.Value = Properties.Settings.Default.Span;
            GapValue.Value = Properties.Settings.Default.Gap;
            ImageModeSelection_SelectedIndexChanged(sender, e);
            ImageModeSelection.SelectedItem = Properties.Settings.Default.ImageMode;
            PD_Polarity_SelectedIndexChanged(sender, e);
            PD_Polarity.SelectedItem = Properties.Settings.Default.pdPolarity;
            EncoderTrackedPositionSelection_SelectedIndexChanged(sender, e);
            EncoderTrackedPositionSelection.SelectedItem = Properties.Settings.Default.Encoder_TrackedPosition;
            pdDirection.SelectedItem = Properties.Settings.Default.pd_direction;
            tcDropWatchingAndImageModes.SelectedIndex = Properties.Settings.Default.TabNumber;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Save data to user settings
            Properties.Settings.Default.Serial_Port = serialPort.SelectedItem.ToString();
            Properties.Settings.Default.Frequency = (int)nudFrequency.Value;
            Properties.Settings.Default.DropWatchMode = dropWatchSelect.SelectedItem.ToString();
            Properties.Settings.Default.DropWatchHead = singleHead.SelectedItem.ToString();
            Properties.Settings.Default.Index = (int)NozzleValue.Value;
            Properties.Settings.Default.Span = (int)SpanValue.Value;
            Properties.Settings.Default.Gap = (int)GapValue.Value;
            Properties.Settings.Default.TabNumber = tcDropWatchingAndImageModes.SelectedIndex;
            //Properties.Settings.Default.ImageMode = ImageModeSelection.SelectedItem.ToString();
            //Properties.Settings.Default.pdPolarity = PD_Polarity.SelectedItem.ToString();
            //Properties.Settings.Default.Encoder_TrackedPosition = EncoderTrackedPositionSelection.SelectedItem.ToString();
            //Properties.Settings.Default.pd_direction = pdDirection.SelectedItem.ToString();
            Properties.Settings.Default.Save();
        }

        private void tabPage6_DoubleClick(object sender, EventArgs e)
        {
            MessageBox.Show("Double Clicked");
            NozzleValue.Update();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                NozzleValue.Value = NozzleValue.Value - 1;
                goLeft = true;
                goRight = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                NozzleValue.Value = NozzleValue.Value + 1;
                goRight = true;
                goLeft = false;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (isConnected.Checked)
            {
                Console.WriteLine("Write Print Head Data");
                driver_board.Write("W");
            }
        }

        private void PD_Polarity_SelectedIndexChanged(object sender, EventArgs e)
        {
            activePD_Polarity = PD_Polarity.SelectedIndex; 
            if ((activePD_Polarity == 0) && (isConnected.Checked))
            {
                driver_board.Write("q1");
                Console.WriteLine("Product Detect Polarity Changed to Active HIGH!");
            }

            else if ((activePD_Polarity == 1) && (isConnected.Checked))
            {
                driver_board.Write("q2");
                Console.WriteLine("Product Detect Polarity Changed to Active LOW!");
            }
        }

        private void EncoderTrackedPositionSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            activeEncoderPosition = EncoderTrackedPositionSelection.SelectedIndex; 
            if ((activeEncoderPosition == 0) && (isConnected.Checked))
            {
                driver_board.Write("D1");
                Console.WriteLine("Stepper Normal");
            }

            else if ((activeEncoderPosition == 1) && (isConnected.Checked))
            {
                driver_board.Write("D0");
                Console.WriteLine("Stepper Reverse");
            }
        }

        private void pdDirection_SelectedIndexChanged(object sender, EventArgs e)
        {
            activePDdirection = pdDirection.SelectedIndex; 
            if ((activePDdirection == 0) && (isConnected.Checked))
            {
                driver_board.Write("K1");
                Console.WriteLine("Continuous Mode");
            }

            else if ((activePDdirection == 1) && (isConnected.Checked))
            {
                driver_board.Write("K0");
                Console.WriteLine("Single Mode");
            }
        }

        private void tcDropWatchingAndImageModes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tcDropWatchingAndImageModes.SelectedIndex == 0)
            {
                dropWatchSelect_SelectedIndexChanged(sender, e);
                singleHead_SelectedIndexChanged(sender, e);
            }

            else if (tcDropWatchingAndImageModes.SelectedIndex == 1)
            {
                ImageModeSelection_SelectedIndexChanged(sender, e);
            }
        }
    }
}
