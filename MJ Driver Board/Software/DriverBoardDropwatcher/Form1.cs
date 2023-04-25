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

namespace DriverBoardDropwatcher
{
    public partial class Form1 : Form
    {
        private OpenFileDialog ofd;
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
        bool ImageHead1 = false, ImageHead2 = false, ImageHead3 = false, ImageHead4 = false;
        String CurrentFileName;
        String datafolder = Application.StartupPath.Replace("bin\\Debug", "Output Images\\File");
        String dataFolderPath = Application.StartupPath.Replace("bin\\Debug", "Output Images");
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
            // Upload Placeholer Image for 4 Picture Boxes
            pictureBox1.Image = Properties.Resources.upload;
            pictureBox2.Image = Properties.Resources.upload;
            pictureBox3.Image = Properties.Resources.upload;
            pictureBox4.Image = Properties.Resources.upload;
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

        private void cbSerialPort_DropDown(object sender, EventArgs e)
        {
            cbSerialPort.Items.Clear(); //Clears all items from serial port drop down
            string[] ports = SerialPort.GetPortNames();
            // Display each port name to the console.
            if (ports.Length < 1)
                cbSerialPort.Items.Add("No ports found");
            else
            {
                foreach (string port in ports)
                {
                    Console.WriteLine(port);
                    cbSerialPort.Items.Add(port); //Adds all available ports to drop down
                }
            }
        }
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
            else if (isConnected.Checked)
            {
                disconnect_board();
                txtbStatusBox.Text = "Disconnected"; //Updates Status Text
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

                else if (input.Substring(0,3).Contains("LV:"))
                {
                    if (ImageHead1 == true)
                        VerifyImageData(1, input);
                    if (ImageHead2 == true)
                        VerifyImageData(2, input);
                    if (ImageHead3 == true)
                        VerifyImageData(3, input);
                    if (ImageHead4 == true)
                        VerifyImageData(4, input);
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
            System.Windows.Forms.TextBox[] HeadTemperaturesOutputText = { txtbTemperatureOutput1, txtbTemperatureOutput2, txtbTemperatureOutput3, txtbTemperatureOutput4 };
            System.Windows.Forms.TextBox[] HeadPrintCounters = { txtbPrintCounter1, txtbPrintCounter2, txtbPrintCounter3, txtbPrintCounter4 };

            if (power.Checked)
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
                Head1TextStatus.Text = "Powered off.";
                nudVoltageHead1.Visible = false;
                nudTemperatureHead1.Visible = false;
                txtbTemperatureOutput1.Visible = false;
                txtbPrintCounter1.Visible = false;
                Head2TextStatus.Text = "Powered off.";
                nudVoltageHead2.Visible = false;
                nudTemperatureHead2.Visible = false;
                txtbTemperatureOutput2.Visible = false;
                txtbPrintCounter2.Visible = false;
                Head3TextStatus.Text = "Powered off.";
                nudVoltageHead3.Visible = false;
                nudTemperatureHead3.Visible = false;
                txtbTemperatureOutput3.Visible = false;
                txtbPrintCounter3.Visible = false;
                Head4TextStatus.Text = "Powered off.";
                nudVoltageHead4.Visible = false;
                nudTemperatureHead4.Visible = false;
                txtbTemperatureOutput4.Visible = false;
                txtbPrintCounter4.Visible = false;
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

            if (!power.Checked)
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
                txtbFrequencyDuplicate.Text = nudFrequency.Value.ToString();
            }
            power.Checked = d.board[0].power == 1 ? true : false;
            int newTime = d.board[0].timeOn;
            
            //Determines Board Up and Running Time 
            timeBoardOn = newTime;
            txtbBoardUpTime.Text = timeBoardOn.ToString(); // Displays Clock Time in GUI
        }
        private void btnConnectDisconnect_Click(object sender, EventArgs e)
        {
            //Toggle button to connect and disconnect from board
            connect_board();
            tcDropWatchingAndImageModes_SelectedIndexChanged(sender, e);
        }

        private void powerOnOff_Click(object sender, EventArgs e)
        {
            if (isConnected.Checked)
            {
                if (power.Checked)
                {
                    //Toggle button to power on and off
                    powerOff();
                    txtbStatusBox.Text = "Power Off"; //Displays Disconnected in Status Box
                }
                else
                {
                    powerOn();
                    txtbStatusBox.Text = "Power On"; //Displays Connected in Status Box
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
                if (voltageChanged.Tag == "voltageHead1") //Head 1 Voltage Tag is "voltageHead1"
                {
                    driver_board.Write($"v {(1).ToString()} {nudVoltageHead1.Value.ToString()}"); //Sends Command to modify Voltage Value
                    Console.WriteLine("Head 1 Voltage Changed to: {0}", nudVoltageHead1.Value); //Send Message to Console to confirm Voltage Change
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
            // This Function Checks for Tag Names to determine which Head Temperature is being changed.
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
            // This Function Checks for Tag Names to determine which Frequency Value is being changed.
            System.Windows.Forms.NumericUpDown nudFrequencyChanged = (System.Windows.Forms.NumericUpDown)sender;
            if ((isConnected.Checked) && (power.Checked))
            {
                if (nudFrequencyChanged.Tag == "nudFrequency")
                {
                    //Sets Head Frequency
                    driver_board.Write($"p {nudFrequency.Value.ToString()}");
                }
            }
        }

        private void cbDropWatchMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            activeDropWatch = cbDropWatchMode.SelectedIndex; //Stores selected mode

            //Checks if External Mode is Selected AND Board is Connected
            if ((activeDropWatch == 1) && (isConnected.Checked))
            {
                nudFrequency.ReadOnly = true;
                frequencyLabel.ForeColor = Color.Gray;
                nudFrequency.ForeColor = Color.Gray;
                txtbFrequencyDuplicate.Visible = false;
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
                txtbFrequencyDuplicate.Visible = true;
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


        private void cbDropWatchHeadSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            activeDropModeHead = cbDropWatchHeadSelection.SelectedIndex; //Stores Head Value
        }

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
                        FileName1.Text = ofd.SafeFileName; //Stores File Name removing Path of File
                        CurrentFileName =  (datafolder + 1 + ".png");
                        ImageSizeText1.Text = ((Image.FromFile(ofd.FileName).Width) + " x " + (Image.FromFile(ofd.FileName).Height));

                        // If File Size has a width of more that
                        if ((Image.FromFile(ofd.FileName).Width) > 128) //If image width exceed 128, warning message shows
                        {
                            MessageBox.Show("Maximum 128 Pixels Per Head!", "File Error",
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
                        FileName2.Text = ofd.SafeFileName; //Stores File Name removing Path of File
                        CurrentFileName = (datafolder + 2 + ".png");
                        ImageSizeText2.Text = ((Image.FromFile(ofd.FileName).Width) + " x " + (Image.FromFile(ofd.FileName).Height));

                        if ((Image.FromFile(ofd.FileName).Width) > 128) //If image width exceed 128, warning message shows
                        {
                            MessageBox.Show("Maximum 128 Pixels Per Head!", "File Error",
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
                        FileName3.Text = ofd.SafeFileName; //Stores File Name removing Path of File
                        CurrentFileName = (datafolder + 3 + ".png"); 
                        ImageSizeText3.Text = ((Image.FromFile(ofd.FileName).Width) + " x " + (Image.FromFile(ofd.FileName).Height));

                        if ((Image.FromFile(ofd.FileName).Width) > 128) //If image width exceed 128, warning message shows
                        {
                            MessageBox.Show("Maximum 128 Pixels Per Head!", "File Error",
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
                        FileName4.Text = ofd.SafeFileName; //Stores File Name removing Path of File
                        CurrentFileName = (datafolder + 4 + ".png");
                        ImageSizeText4.Text = ((Image.FromFile(ofd.FileName).Width) + " x " + (Image.FromFile(ofd.FileName).Height));

                        if ((Image.FromFile(ofd.FileName).Width) > 128) //If image width exceed 128, warning message shows
                        {
                            MessageBox.Show("Maximum 128 Pixels Per Head!", "File Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        convertImageToData(CurrentFileName); //Calls function to convert image to data
                        ImageHead4 = true;
                    }
                    break;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ImageBoxClicked(sender, e);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            ImageBoxClicked(sender, e);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            ImageBoxClicked(sender, e);
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            ImageBoxClicked(sender, e);
        }

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

        private void btnPrintImage_Click(object sender, EventArgs e)
        {
            if ((isConnected.Checked) && (power.Checked))
            {
                if (ImageHead1 == true)
                {
                    PrintingImage(1);
                    Thread.Sleep(50);
                }
                if (ImageHead2 == true)
                {
                    PrintingImage(2);
                    Thread.Sleep(50);
                }
                if (ImageHead3 == true)
                {
                    PrintingImage(3);
                    Thread.Sleep(50);
                }
                if (ImageHead4 == true)
                {
                    PrintingImage(4);
                    Thread.Sleep(50);
                }
            }
        }

        private void PrintingImage(int head)
        {
            try
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
                driver_board.Write(dataToSend, 0, dataToSend.Length);
                Thread.Sleep(100);
            }

            catch
            {
                MessageBox.Show("Error uploading file(s)");
            }
        }

        private void VerifyImageData(int head, string existing_lines)
        {
            String CurrentFile = datafolder + head + ".png.printDat";
            byte[] readText = File.ReadAllBytes(CurrentFile);

            long lastValue = 0;
            long runningCount = 0;
            long dataLength = 0;
            int LV, RC, DL;

            //Splits each new line from the output from board into a new item in an array
            string[] s_ = existing_lines.Split('\n');

            foreach (byte dat in readText)
            {
                lastValue = dat;
                runningCount += dat;
                dataLength++;
            }

            if (s_.Length == 4)
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
                            MessageBox.Show(("Data Length Incorrect for Head: " + head), "Printing Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        Console.WriteLine("RC incorrect");
                        MessageBox.Show(("Running Count Incorrect for Head: " + head), "Printing Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Console.WriteLine("RC Value Expected: " + runningCount);
                        Console.WriteLine("RC Value Received from Board: " + RC);
                    }
                }
                else
                {
                    Console.WriteLine("LV incorrect");
                    MessageBox.Show(("Last Value Incorrect for Head: " + head), "Printing Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Console.WriteLine("LV Value Expected: " + lastValue);
                    Console.WriteLine("LV Value Received from Board: " + LV);
                }
            }
            else
            {
                Console.WriteLine("Not recieved the correct data format");
                MessageBox.Show(("Not received the correct data format for Head: " + head), "Printing Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void NozzleValue_ValueChanged(object sender, EventArgs e)
        {
            activeNozzleValue = (int)nudNozzle.Value;
            if ((activeSpanValue + activeNozzleValue) > 128)
            {
                activeNozzleValue = 128 - activeSpanValue;
                MessageBox.Show("Maximum value of 128 Exceeded!", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                nudNozzle.Value = activeNozzleValue;
            }
        }

        private void SpanValue_ValueChanged(object sender, EventArgs e)
        {
            activeSpanValue = (int)nudSpan.Value;
            if ((activeSpanValue + activeNozzleValue) > 128)
            {
                activeSpanValue = 128 - activeNozzleValue;
                MessageBox.Show("Maximum value of 128 Exceeded!", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                nudSpan.Value = activeSpanValue;
            }
        }

        private void FillSingleNozzleButton_Click(object sender, EventArgs e)
        {
            if ((isConnected.Checked) && (power.Checked))
            {
                isFillNozzle.Checked = true;
                isFillHead.Checked = false;
                isFillSpan.Checked = false;
                driver_board.Write($"n {(activeDropModeHead + 1).ToString()} {activeNozzleValue.ToString()}");
            }
        }
        private void FillSpanNozzleButton_Click(object sender, EventArgs e)
        {
            if (isConnected.Checked)
            {
                isFillSpan.Checked = true;
                isFillHead.Checked = false;
                isFillNozzle.Checked = false;
                driver_board.Write($"N {(activeDropModeHead + 1).ToString()} {activeNozzleValue.ToString()} {activeSpanValue.ToString()}");
            }
        }

        private void ImageModeSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Determine relevent option from drop down menu selected by user
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
            if ((isConnected.Checked) && (power.Checked))
            {
                driver_board.Write("C");
                isFillNozzle.Checked = false;
                isFillSpan.Checked = false;
                isFillHead.Checked = false;
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
            // Tags used to identify which button has been pressed, to modify, change button tag name on Designer Panel
            System.Windows.Forms.Button b = (System.Windows.Forms.Button)sender;
            if (isConnected.Checked)
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

        }
        private void GapValue_ValueChanged(object sender, EventArgs e)
        {
            activeGapValue = (int)nudGap.Value;
        }

        private void FillGapButton_Click(object sender, EventArgs e)
        {
            isFillGap.Checked = true;
            isFillHead.Checked = false;
            isFillSpan.Checked = false;
            isFillNozzle.Checked = false;
        }

        private void fillHead_Click(object sender, EventArgs e)
        {
            if ((isConnected.Checked) && (power.Checked))
            {
                isFillHead.Checked = true;
                isFillNozzle.Checked = false;
                isFillSpan.Checked = false;
                driver_board.Write($"I {(activeDropModeHead + 1).ToString()}");
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
            //Selection changes for Encoder Tracked Poisition Modes (Normal or Reversed Modes)
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
            // Selection changes between pd modes
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
            // Function to determine setting changes by user between Image and Drp Watching Modes
            if (tcDropWatchingAndImageModes.SelectedIndex == 0)
            {
                cbDropWatchMode_SelectedIndexChanged(sender, e);
                cbDropWatchHeadSelection_SelectedIndexChanged(sender, e);
            }

            else if (tcDropWatchingAndImageModes.SelectedIndex == 1)
            {
                ImageModeSelection_SelectedIndexChanged(sender, e);
            }
        }

        private void DropWatchingTab_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                // Decrease Nozzle Value by -1 increment and send command to update board when left arrow key pressed
                case (Keys.Left):
                    nudNozzle.Value = nudNozzle.Value - 1;
                    activeNozzleValue = (int)nudNozzle.Value;

                    if (isFillNozzle.Checked)
                    {
                        driver_board.Write($"n {(activeDropModeHead + 1).ToString()} {activeNozzleValue.ToString()}");
                    }

                    else if (isFillSpan.Checked)
                    {
                        driver_board.Write($"N {(activeDropModeHead + 1).ToString()} {activeNozzleValue.ToString()} {activeSpanValue.ToString()}");
                    }
                    break;

                    // Increase Nozzle Value by +1 increment and send command to update board when right arrow key pressed
                case (Keys.Right):
                    nudNozzle.Value = nudNozzle.Value + 1;
                    activeNozzleValue = (int)nudNozzle.Value;

                    if (isFillNozzle.Checked)
                    {
                        driver_board.Write($"n {(activeDropModeHead + 1).ToString()} {activeNozzleValue.ToString()}");
                    }

                    else if (isFillSpan.Checked)
                    {
                        driver_board.Write($"N {(activeDropModeHead + 1).ToString()} {activeNozzleValue.ToString()} {activeSpanValue.ToString()}");
                    }
                    break;
            }
        }

        private void DropWatchingTab_Click(object sender, EventArgs e)
        {
            DropWatchingTab.Focus();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if ((isConnected.Checked) && (power.Checked))
            {
                driver_board.Write("C");
            }
        }

        private void DeleteAllFilesInFolder(object sender, EventArgs e)
        {
            // Delete All Files Saved in "Output Images" when closing Application
            System.IO.DirectoryInfo directory = new DirectoryInfo(dataFolderPath);

            foreach (FileInfo file in directory.GetFiles())
            {
                file.Delete();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Load Settings from Previous Saved Session
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
            ImageModeSelection.SelectedItem = Properties.Settings.Default.ImageMode;
            PD_Polarity_SelectedIndexChanged(sender, e);
            PD_Polarity.SelectedItem = Properties.Settings.Default.pdPolarity;
            EncoderTrackedPositionSelection_SelectedIndexChanged(sender, e);
            EncoderTrackedPositionSelection.SelectedItem = Properties.Settings.Default.Encoder_TrackedPosition;
            pdDirection.SelectedItem = Properties.Settings.Default.pd_direction;
            tcDropWatchingAndImageModes.SelectedIndex = Properties.Settings.Default.TabNumber;
            isFillNozzle.Checked = Properties.Settings.Default.FillNozzleCheckedStatus;
            isFillSpan.Checked = Properties.Settings.Default.FillSpanCheckedStatus;
            isFillGap.Checked = Properties.Settings.Default.FillGapCheckedStatus;
            isFillHead.Checked = Properties.Settings.Default.FillHeadCheckedStatus;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Save data to user settings
            Properties.Settings.Default.Serial_Port = cbSerialPort.SelectedItem.ToString();
            Properties.Settings.Default.Frequency = (int)nudFrequency.Value;
            Properties.Settings.Default.DropWatchMode = cbDropWatchMode.SelectedItem.ToString();
            Properties.Settings.Default.DropWatchHead = cbDropWatchHeadSelection.SelectedItem.ToString();
            Properties.Settings.Default.Index = (int)nudNozzle.Value;
            Properties.Settings.Default.Span = (int)nudSpan.Value;
            Properties.Settings.Default.Gap = (int)nudGap.Value;
            Properties.Settings.Default.TabNumber = tcDropWatchingAndImageModes.SelectedIndex;
            Properties.Settings.Default.FillNozzleCheckedStatus = isFillNozzle.Checked;
            Properties.Settings.Default.FillSpanCheckedStatus = isFillSpan.Checked;
            Properties.Settings.Default.FillGapCheckedStatus = isFillGap.Checked;
            Properties.Settings.Default.FillHeadCheckedStatus = isFillHead.Checked;
            DeleteAllFilesInFolder(sender, e);
            Properties.Settings.Default.Save();
        }
    }
}
