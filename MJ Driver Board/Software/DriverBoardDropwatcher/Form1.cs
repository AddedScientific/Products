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

namespace DriverBoardDropwatcher
{
    public partial class Form1 : Form
    {
        private OpenFileDialog ofd;
        bool valid_port_selected = false;
        string port_name;
        static SerialPort driver_board;
        //isConnected.Checked;
        private Thread trd;
        int activeDropWatch = 0;
        int activeSingleHead;
        int activeNozzleValue;
        int activeSpanValue;
        int activeImageMode;
        int headStatus1;
        int headStatus2;
        int headStatus3;
        int headStatus4;
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
        bool printing;

        public Form1()
        {
            InitializeComponent();
            InitializeUI("UIMode"); 
            dropWatchSelect.Text = "--Select Mode--"; // Placeholder Text
            serialPort.Text = "Select COM Port"; // Placeholder Text
            singleHead.Text = "--Select Head--"; // Placeholder Text
            ImageModeSelection.Text = "--Select Mode--"; //Placeholder Text
            DropWatchingStatus.Text = "Head Status"; // Placeholder Text;
            Thread trd = new Thread(new ThreadStart(this.ThreadTask));
            trd.IsBackground = true;
            trd.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
            textBox2.Text = "Disconnected"; //Displays Disconnected in Status Box
            ofd = new OpenFileDialog();
        }
        private void ThreadTask()
        {
            // Repeatedly checks if board is connected
            while (true)
            {
                if (isConnected.Checked)
                {
                    driver_board.Write("b\n");
                }

                Thread.Sleep(500);
            }
        }

        private void InitializeUI(string v)
        {
           
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
                    driver_board.DataReceived += new SerialDataReceivedEventHandler(DataRecievedHandler);
                    isConnected.Checked = true;
                    Console.WriteLine("Connected");
                    textBox2.Text = "Connected";
                }
                catch
                {
                    Console.WriteLine("Error opening port");
                    textBox2.Text = "Error opening port";
                }
            }
            else if (isConnected.Checked)
            {
                disconnect_board();
                textBox2.Text = "Disconnected";
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
                    parseJsonData(input);
                }
                else
                {
                    Console.WriteLine(input);
                }
            }

        }

        private void determineStatus()
        {
            if (power.Checked)
            {
                switch (headStatus1)
                {
                    case -2: // -2 means head is not connected to board
                        Head1TextStatus.Text = "Not Connected"; // Displays Message
                        voltage1.Visible = false; // Hides Voltage Box
                        temperature1.Visible = false; // Hides Set Temperature Box 
                        temperatureOutput1.Visible = false; //Hides Current Temperature Box
                        printCounter1.Visible = false; // Hides Print Counter Box
                        Head1TextStatus.BackColor = Color.Red; //Sets Status Box to Red indicating head is not connected
                        break;
                    case -3: // -3 means error with head
                        Head1TextStatus.Text = "Ready Error"; // Displays Message
                        voltage1.Visible = false; // Hides Voltage Box
                        temperature1.Visible = false; // Hides Set Temperature Box 
                        temperatureOutput1.Visible = false; //Hides Current Temperature Box
                        printCounter1.Visible = false; // Hides Print Counter Box
                        Head1TextStatus.BackColor = Color.Red; //Sets Status Box to Red indicating head error
                        break;
                    case 10: // Head is connected but idle
                        voltage1.Visible = true; // Displays Voltage Box
                        temperature1.Visible = true; // Dislays Set Temperature Box 
                        temperatureOutput1.Visible = true; //Displays Current Temperature Box
                        printCounter1.Visible = true; // Displays Print Counter Box

                        if (printCount1 > previousPrintCount1)
                        {
                            Head1TextStatus.Text = "Printing";
                            Head1TextStatus.BackColor = Color.Green;
                            break;
                        }
                        else
                        {
                            Head1TextStatus.Text = "Idle"; // Displays Message
                            Head1TextStatus.BackColor = Color.Orange; //Sets Status Box to Orange indicating all is fine but not printing
                            break;
                        }
                    default: 
                        Head1TextStatus.Text = "Printing"; // Displays Message
                        voltage1.Visible = true; // Displays Voltage Box
                        temperature1.Visible = true; // Dislays Set Temperature Box 
                        temperatureOutput1.Visible = true; //Displays Current Temperature Box
                        printCounter1.Visible = true; // Displays Print Counter Box
                        Head1TextStatus.BackColor = Color.Green;//Sets Status Box to Orange indicating all is fine and printing
                        break;
                }

                // Repeats above steps for each Head
                switch (headStatus2)
                {
                    case -2:
                        Head2TextStatus.Text = "Not Connected";
                        voltage2.Visible = false;
                        temperature2.Visible = false;
                        temperatureOutput2.Visible = false;
                        printCounter2.Visible = false;
                        Head2TextStatus.BackColor = Color.Red;
                        break;
                    case -3:
                        Head2TextStatus.Text = "Ready Error";
                        voltage2.Visible = false;
                        temperature2.Visible = false;
                        temperatureOutput2.Visible = false;
                        printCounter2.Visible = false;
                        Head2TextStatus.BackColor = Color.Red;
                        break;
                    case 10:
                        Head2TextStatus.Text = "Idle";
                        voltage2.Visible = true;
                        temperature2.Visible = true;
                        temperatureOutput2.Visible = true;
                        printCounter2.Visible = true;
                        Head2TextStatus.BackColor = Color.Orange;
                        break;
                    default:
                        Head2TextStatus.Text = "Printing";
                        voltage2.Visible = true;
                        temperature2.Visible = true;
                        temperatureOutput2.Visible = true;
                        printCounter2.Visible = true;
                        Head2TextStatus.BackColor = Color.Green;
                        break;
                }

                switch (headStatus3)
                {
                    case -2:
                        Head3TextStatus.Text = "Not Connected";
                        voltage3.Visible = false;
                        temperature3.Visible = false;
                        temperatureOutput3.Visible = false;
                        printCounter3.Visible = false;
                        Head3TextStatus.BackColor = Color.Red;
                        break;
                    case -3:
                        Head3TextStatus.Text = "Ready Error";
                        voltage3.Visible = false;
                        temperature3.Visible = false;
                        temperatureOutput3.Visible = false;
                        printCounter3.Visible = false;
                        Head3TextStatus.BackColor = Color.Red;
                        break;
                    case 10:
                        Head3TextStatus.Text = "Idle";
                        voltage3.Visible = true;
                        temperature3.Visible = true;
                        temperatureOutput3.Visible = true;
                        printCounter3.Visible = true;
                        Head3TextStatus.BackColor = Color.Orange;
                        break;
                    default:
                        Head3TextStatus.Text = "Printing";
                        voltage3.Visible = true;
                        temperature3.Visible = true;
                        temperatureOutput3.Visible = true;
                        printCounter3.Visible = true;
                        Head3TextStatus.BackColor = Color.Green;
                        break;
                }

                switch (headStatus4)
                {
                    case -2:
                        Head4TextStatus.Text = "Not Connected";
                        voltage4.Visible = false;
                        temperature4.Visible = false;
                        temperatureOutput4.Visible = false;
                        printCounter4.Visible = false;
                        Head4TextStatus.BackColor = Color.Red;
                        break;
                    case -3:
                        Head4TextStatus.Text = "Ready Error";
                        voltage4.Visible = false;
                        temperature4.Visible = false;
                        temperatureOutput4.Visible = false;
                        printCounter4.Visible = false;
                        Head4TextStatus.BackColor = Color.Red;
                        break;
                    case 10:
                        Head4TextStatus.Text = "Idle";
                        voltage4.Visible = true;
                        temperature4.Visible = true;
                        temperatureOutput4.Visible = true;
                        printCounter4.Visible = true;
                        Head1TextStatus.BackColor = Color.Orange;
                        break;
                    default:
                        Head4TextStatus.Text = "Printing";
                        voltage4.Visible = true;
                        temperature4.Visible = true;
                        temperatureOutput4.Visible = true;
                        printCounter4.Visible = true;
                        Head1TextStatus.BackColor = Color.Green;
                        break;
                }

            }

            // If Board is Powered Off, then all heads displays Powered Off and Hides all Settings
            else
            {
                Head1TextStatus.Text = "Powered off.";
                voltage1.Visible = false;
                temperature1.Visible = false;
                temperatureOutput1.Visible = false;
                printCounter1.Visible = false;
                Head2TextStatus.Text = "Powered off.";
                voltage2.Visible = false;
                temperature2.Visible = false;
                temperatureOutput2.Visible = false;
                printCounter2.Visible = false;
                Head3TextStatus.Text = "Powered off.";
                voltage3.Visible = false;
                temperature3.Visible = false;
                temperatureOutput3.Visible = false;
                printCounter3.Visible = false;
                Head4TextStatus.Text = "Powered off.";
                voltage4.Visible = false;
                temperature4.Visible = false;
                temperatureOutput4.Visible = false;
                printCounter4.Visible = false;
                Head1TextStatus.BackColor = Color.Red;
                Head2TextStatus.BackColor = Color.Red;
                Head3TextStatus.BackColor = Color.Red;
                Head4TextStatus.BackColor = Color.Red;
            }

        }

        private void parseJsonData(string input_string)
        {

            dynamic d = JObject.Parse(input_string);
            //--------------------------------------------------
            headStatus1 = d.heads[0].status; //Check status of head 1
            headStatus2 = d.heads[1].status; //Check status of head 2
            headStatus3 = d.heads[2].status; //Check status of head 3
            headStatus4 = d.heads[3].status; //Check status of head 4
            //--------------------------------------------------

            //Check Print Counts for Heads
            printCount1 = d.heads[0].printCounts; // Counts number of prints for head 1
            printCounter1.Text = (printCount1.ToString()); // Displays Number of Print Count on GUI
            printCount2 = d.heads[1].printCounts; // Counts number of prints for head 2
            printCounter2.Text = (printCount2.ToString()); // Displays Number of Print Count on GUI
            printCount3 = d.heads[2].printCounts; // Counts number of prints for head 3
            printCounter3.Text = (printCount3.ToString()); // Displays Number of Print Count on GUI
            printCount4 = d.heads[3].printCounts; // Counts number of prints for head 4
            printCounter4.Text = (printCount4.ToString()); // Displays Number of Print Count on GUI 

            determineStatus();

            // IF print count is increasing for any head, "Head Printing" will be displayed in Status Box
            if ((printCount1) > (previousPrintCount1) || (printCount2) > (previousPrintCount2) || (printCount3) > (previousPrintCount3) ||  (printCount4) > (previousPrintCount4))
            {
                DropWatchingStatus.Text = ("Head Printing"); 
                DropWatchingStatus.BackColor = Color.Green;
            }

            // IF print count is stationary for all heads, "Head Idle" will be displayed in Status Box
            else if (((printCount1) == (previousPrintCount1)) && ((printCount2) == (previousPrintCount2)) && ((printCount3) == (previousPrintCount3)) && ((printCount4) == (previousPrintCount4)))
            {
                DropWatchingStatus.Text = ("Head Idle");
                DropWatchingStatus.BackColor = Color.Orange;
            }

            //Sets Previous Count to current count to detect if value is going up or staying the same
            previousPrintCount1 = printCount1;
            previousPrintCount2 = printCount2;
            previousPrintCount3 = printCount3;
            previousPrintCount4 = printCount4;

            //--------------------------------------------------
            //Check Currrent Temeperatures for Heads
            temperatureOutput1.Text = d.heads[0].curTemperature.ToString();
            temperatureOutput2.Text = d.heads[1].curTemperature.ToString();
            temperatureOutput3.Text = d.heads[2].curTemperature.ToString();
            temperatureOutput4.Text = d.heads[3].curTemperature.ToString();
            //--------------------------------------------------
            //--------------------------------------------------
            // Check Current Voltage for Heads
            voltage1.Text = d.heads[0].voltage.ToString();
            voltage2.Text = d.heads[1].voltage.ToString();
            voltage3.Text = d.heads[2].voltage.ToString();
            voltage4.Text = d.heads[3].voltage.ToString();
            //--------------------------------------------------
            actFreq = d.printingParameters[0].internalPrintPeriod;

            if (actFreq > 0)
            {
                //Check Frequency for each Head
                frequency.Text = (1000000 / actFreq).ToString();
                frequencyDuplicate.Text = frequency.Text;
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
        private void button1_Click(object sender, EventArgs e)
        {
            connect_board();
        }

        private void powerOnOff_Click(object sender, EventArgs e)
        {
            if (isConnected.Checked)
            {
                if (power.Checked)
                {
                    powerOff();
                    textBox2.Text = "Power Off"; //Displays Disconnected in Status Box
                }
                else
                {
                    powerOn();
                    textBox2.Text = "Power On"; //Displays Connected in Status Box
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

        private void voltage1_ValueChanged(object sender, EventArgs e)
        {
            if (isConnected.Checked)
            {
                // Sets Head Voltage
                driver_board.Write($"v {(0 + 1).ToString()} {voltage1.Value.ToString()}");
                Console.WriteLine(voltage1.Value);
            }
        }

        private void voltage2_ValueChanged(object sender, EventArgs e)
        {
            if (isConnected.Checked)
            {
                // Sets Head Voltage
                driver_board.Write($"v {(1 + 1).ToString()} {voltage2.Value.ToString()}");
            }
        }

        private void voltage3_ValueChanged(object sender, EventArgs e)
        {
            if (isConnected.Checked)
            {
                // Sets Head Voltage
                driver_board.Write($"v {(2 + 1).ToString()} {voltage3.Value.ToString()}");
            }
        }

        private void voltage4_ValueChanged(object sender, EventArgs e)
        {
            if (isConnected.Checked)
            {
                // Sets Head Voltage
                driver_board.Write($"v {(3 + 1).ToString()} {voltage4.Value.ToString()}");
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void temperature1_ValueChanged(object sender, EventArgs e)
        {
            if (isConnected.Checked)
            {
                // Sets Head Temperature
                driver_board.Write($"T {(0 + 1).ToString()} {temperature1.Value.ToString()}");
            }

        }

        private void temperature2_ValueChanged(object sender, EventArgs e)
        {
            if (isConnected.Checked)
            {
                // Sets Head Temperature
                driver_board.Write($"T {(1 + 1).ToString()} {temperature2.Value.ToString()}");
            }

        }

        private void temperature3_ValueChanged(object sender, EventArgs e)
        {
            if (isConnected.Checked)
            {
                // Sets Head Temperature
                driver_board.Write($"T {(2 + 1).ToString()} {temperature3.Value.ToString()}");
            }

        }

        private void temperature4_ValueChanged(object sender, EventArgs e)
        {
            if (isConnected.Checked)
            {
                // Sets Head Temperature
                driver_board.Write($"T {(3 + 1).ToString()} {temperature4.Value.ToString()}");
            }

        }

        private void frequency_ValueChanged(object sender, EventArgs e)
        {
            if (isConnected.Checked)
            {
                //Sets Head Frequency
                driver_board.Write($"p {frequency.Value.ToString()}");
                Console.WriteLine(frequency.Value);
            }
        }


        private void label4_Click(object sender, EventArgs e)
        {

        }


        private void dropWatchSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            activeDropWatch = dropWatchSelect.SelectedIndex; //Stores selected mode

            //Checks if External Mode is Selected AND Board is Connected
            if ((activeDropWatch == 1) && (isConnected.Checked))
            {
                frequency.ReadOnly = true;
                frequencyLabel.ForeColor = Color.Gray;
                frequency.ForeColor = Color.Gray;
                frequencyDuplicate.Visible = false;
                frequencyDuplicateLabel.Visible = false;
                driver_board.Write("M2");
                Console.WriteLine("Drop Watching External Mode");
            }

            //Checks IF internal Mode is Selected AND Board is Connected
            else if ((activeDropWatch == 0) && (isConnected.Checked))
                {
                frequency.ReadOnly = false;
                frequencyLabel.ForeColor = Color.Black;
                frequency.ForeColor = Color.Black;
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
                this.pictureBox1.Image = Image.FromFile(ofd.FileName);
                FileName1.Text = ofd.SafeFileName;
                ImageSizeText1.Text = ((Image.FromFile(ofd.FileName).Width) + " x " + (Image.FromFile(ofd.FileName).Height));

                // If File Size has a width of more tha
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
                this.pictureBox2.Image = Image.FromFile(ofd.FileName);
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
                this.pictureBox3.Image = Image.FromFile(ofd.FileName);
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
                this.pictureBox4.Image = Image.FromFile(ofd.FileName);
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
        }

        private void SpanValue_ValueChanged(object sender, EventArgs e)
        {
            activeSpanValue = (int)SpanValue.Value;
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
                Console.WriteLine("Steper Motor Mode");
            }

            else if ((activeImageMode == 1) && (isConnected.Checked))
            {
                driver_board.Write("M4");
                Console.WriteLine("Quadrature Encoder Mode");
            }

            else if ((activeImageMode == 2) && (isConnected.Checked))
            {
                driver_board.Write("M5");
                Console.WriteLine("HW PD Mode");
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
            //if (isConnected.Checked)
            //{
            //    driver_board.Write($"I {(activeSingleHead + 1).ToString()}");
            //}
        }

        private void FillCycleB_Click(object sender, EventArgs e)
        {

        }

        private void FillCycleC_Click(object sender, EventArgs e)
        {

        }
    }
}
