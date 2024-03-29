﻿
namespace DriverBoardDropwatcher
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnConnectDisconnect = new System.Windows.Forms.Button();
            this.ChbxIsConnected = new System.Windows.Forms.CheckBox();
            this.btnPowerOnOff = new System.Windows.Forms.Button();
            this.ChbxPower = new System.Windows.Forms.CheckBox();
            this.lbStatus = new System.Windows.Forms.Label();
            this.txtbHeadStatus1 = new System.Windows.Forms.TextBox();
            this.txtbTemperatureOutput1 = new System.Windows.Forms.TextBox();
            this.nudTemperatureHead1 = new System.Windows.Forms.NumericUpDown();
            this.nudVoltageHead1 = new System.Windows.Forms.NumericUpDown();
            this.txtbHeadStatus2 = new System.Windows.Forms.TextBox();
            this.txtbTemperatureOutput2 = new System.Windows.Forms.TextBox();
            this.nudTemperatureHead2 = new System.Windows.Forms.NumericUpDown();
            this.nudVoltageHead2 = new System.Windows.Forms.NumericUpDown();
            this.txtbHeadStatus3 = new System.Windows.Forms.TextBox();
            this.txtbTemperatureOutput3 = new System.Windows.Forms.TextBox();
            this.nudTemperatureHead3 = new System.Windows.Forms.NumericUpDown();
            this.nudVoltageHead3 = new System.Windows.Forms.NumericUpDown();
            this.txtbTemperatureOutput4 = new System.Windows.Forms.TextBox();
            this.nudTemperatureHead4 = new System.Windows.Forms.NumericUpDown();
            this.nudVoltageHead4 = new System.Windows.Forms.NumericUpDown();
            this.txtbStatusBox = new System.Windows.Forms.TextBox();
            this.lbTimeOn = new System.Windows.Forms.Label();
            this.txtbBoardUpTime = new System.Windows.Forms.TextBox();
            this.btnReset = new System.Windows.Forms.Button();
            this.StatusTable = new System.Windows.Forms.TableLayoutPanel();
            this.txtbHeadStatus4 = new System.Windows.Forms.TextBox();
            this.txtbPrintCounter4 = new System.Windows.Forms.TextBox();
            this.label40 = new System.Windows.Forms.Label();
            this.lbHead = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.label39 = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.lbCurrentTemperature = new System.Windows.Forms.Label();
            this.lbSetTemperature = new System.Windows.Forms.Label();
            this.lbSetVoltage = new System.Windows.Forms.Label();
            this.lbHeadStatus = new System.Windows.Forms.Label();
            this.lbPrintCount = new System.Windows.Forms.Label();
            this.txtbPrintCounter1 = new System.Windows.Forms.TextBox();
            this.txtbPrintCounter2 = new System.Windows.Forms.TextBox();
            this.txtbPrintCounter3 = new System.Windows.Forms.TextBox();
            this.lbFrequency = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.FillCycleBox = new System.Windows.Forms.GroupBox();
            this.btnFillCycleA = new System.Windows.Forms.Button();
            this.lbDropWatchingFillCycle = new System.Windows.Forms.Label();
            this.btnFillCycleB = new System.Windows.Forms.Button();
            this.btnFillCycleC = new System.Windows.Forms.Button();
            this.btnFillHead = new System.Windows.Forms.Button();
            this.btnFillGap = new System.Windows.Forms.Button();
            this.btnFillSpan = new System.Windows.Forms.Button();
            this.btnFillNozzle = new System.Windows.Forms.Button();
            this.btnClearHead = new System.Windows.Forms.Button();
            this.tcDropWatchingAndImageModes = new System.Windows.Forms.TabControl();
            this.DropWatchingTab = new System.Windows.Forms.TabPage();
            this.txtbNozzleSpanStatusBox = new System.Windows.Forms.TextBox();
            this.chbxIsFillSpan = new System.Windows.Forms.CheckBox();
            this.chbxIsFillGap = new System.Windows.Forms.CheckBox();
            this.chbxIsFillHead = new System.Windows.Forms.CheckBox();
            this.chbxIsFillNozzle = new System.Windows.Forms.CheckBox();
            this.nudGap = new System.Windows.Forms.NumericUpDown();
            this.lbDropWatchingGap = new System.Windows.Forms.Label();
            this.txtbHeadStatus = new System.Windows.Forms.TextBox();
            this.txtbFrequencyDuplicate = new System.Windows.Forms.TextBox();
            this.lbDropWatchingFrequencyDuplicate = new System.Windows.Forms.Label();
            this.lbDropWatchingSpan = new System.Windows.Forms.Label();
            this.lbDropWatchingNozzle = new System.Windows.Forms.Label();
            this.nudSpan = new System.Windows.Forms.NumericUpDown();
            this.lbHeadIndex = new System.Windows.Forms.Label();
            this.lbDropWatchingMode = new System.Windows.Forms.Label();
            this.nudNozzle = new System.Windows.Forms.NumericUpDown();
            this.cbDropWatchHeadSelection = new System.Windows.Forms.ComboBox();
            this.label24 = new System.Windows.Forms.Label();
            this.cbDropWatchMode = new System.Windows.Forms.ComboBox();
            this.ImageModeTab = new System.Windows.Forms.TabPage();
            this.txtbPrintStatus = new System.Windows.Forms.TextBox();
            this.txtbCurrentStepperPosition = new System.Windows.Forms.TextBox();
            this.lbCurrentStepperPosition = new System.Windows.Forms.Label();
            this.nudSetPosition = new System.Windows.Forms.NumericUpDown();
            this.txtbCurrentEncoderPosition = new System.Windows.Forms.TextBox();
            this.lbSetPosition = new System.Windows.Forms.Label();
            this.lbCurrentEncoderPosition = new System.Windows.Forms.Label();
            this.txtbImageHeadStatus = new System.Windows.Forms.TextBox();
            this.cdPDdirection = new System.Windows.Forms.ComboBox();
            this.lbPDdirection = new System.Windows.Forms.Label();
            this.cbEncoderTrackedPosition = new System.Windows.Forms.ComboBox();
            this.lbEncoderTrackedPosition = new System.Windows.Forms.Label();
            this.cbPDpolarity = new System.Windows.Forms.ComboBox();
            this.lbPDpolarity = new System.Windows.Forms.Label();
            this.lbImageMode = new System.Windows.Forms.Label();
            this.cbImageMode = new System.Windows.Forms.ComboBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnPrintImage = new System.Windows.Forms.Button();
            this.txtbDimensionsHead4 = new System.Windows.Forms.TextBox();
            this.lbDimensionsHead4 = new System.Windows.Forms.Label();
            this.txtbDimensionsHead3 = new System.Windows.Forms.TextBox();
            this.lbDimensionsHead3 = new System.Windows.Forms.Label();
            this.txtbDimensionsHead2 = new System.Windows.Forms.TextBox();
            this.lbDimensionsHead2 = new System.Windows.Forms.Label();
            this.txtbDimensionsHead1 = new System.Windows.Forms.TextBox();
            this.lbDimensionsHead1 = new System.Windows.Forms.Label();
            this.txtbFileNameHead4 = new System.Windows.Forms.TextBox();
            this.txtbFileNameHead3 = new System.Windows.Forms.TextBox();
            this.txtbFileNameHead2 = new System.Windows.Forms.TextBox();
            this.txtbFileNameHead1 = new System.Windows.Forms.TextBox();
            this.lbFileNameHead4 = new System.Windows.Forms.Label();
            this.lbFileNameHead3 = new System.Windows.Forms.Label();
            this.lbFileNameHead2 = new System.Windows.Forms.Label();
            this.lbFileNameHead1 = new System.Windows.Forms.Label();
            this.lbImageHead4 = new System.Windows.Forms.Label();
            this.lbImageHead3 = new System.Windows.Forms.Label();
            this.lbImageHead2 = new System.Windows.Forms.Label();
            this.lbImageHead1 = new System.Windows.Forms.Label();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.txtbJSONview = new System.Windows.Forms.RichTextBox();
            this.nudFrequency = new System.Windows.Forms.NumericUpDown();
            this.cbSerialPort = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudTemperatureHead1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudVoltageHead1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTemperatureHead2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudVoltageHead2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTemperatureHead3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudVoltageHead3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTemperatureHead4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudVoltageHead4)).BeginInit();
            this.StatusTable.SuspendLayout();
            this.FillCycleBox.SuspendLayout();
            this.tcDropWatchingAndImageModes.SuspendLayout();
            this.DropWatchingTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudGap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSpan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudNozzle)).BeginInit();
            this.ImageModeTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSetPosition)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudFrequency)).BeginInit();
            this.SuspendLayout();
            // 
            // btnConnectDisconnect
            // 
            this.btnConnectDisconnect.Location = new System.Drawing.Point(161, 4);
            this.btnConnectDisconnect.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.btnConnectDisconnect.Name = "btnConnectDisconnect";
            this.btnConnectDisconnect.Size = new System.Drawing.Size(127, 25);
            this.btnConnectDisconnect.TabIndex = 1;
            this.btnConnectDisconnect.Text = "Connect/Disconnect";
            this.btnConnectDisconnect.UseVisualStyleBackColor = true;
            this.btnConnectDisconnect.Click += new System.EventHandler(this.btnConnectDisconnect_Click);
            // 
            // ChbxIsConnected
            // 
            this.ChbxIsConnected.AutoSize = true;
            this.ChbxIsConnected.Enabled = false;
            this.ChbxIsConnected.Location = new System.Drawing.Point(296, 10);
            this.ChbxIsConnected.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.ChbxIsConnected.Name = "ChbxIsConnected";
            this.ChbxIsConnected.Size = new System.Drawing.Size(22, 21);
            this.ChbxIsConnected.TabIndex = 2;
            this.ChbxIsConnected.UseVisualStyleBackColor = true;
            // 
            // btnPowerOnOff
            // 
            this.btnPowerOnOff.Location = new System.Drawing.Point(161, 33);
            this.btnPowerOnOff.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.btnPowerOnOff.Name = "btnPowerOnOff";
            this.btnPowerOnOff.Size = new System.Drawing.Size(127, 23);
            this.btnPowerOnOff.TabIndex = 4;
            this.btnPowerOnOff.Text = "Power Toggle";
            this.btnPowerOnOff.UseVisualStyleBackColor = true;
            this.btnPowerOnOff.Click += new System.EventHandler(this.powerOnOff_Click);
            // 
            // ChbxPower
            // 
            this.ChbxPower.AutoSize = true;
            this.ChbxPower.Enabled = false;
            this.ChbxPower.Location = new System.Drawing.Point(296, 39);
            this.ChbxPower.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.ChbxPower.Name = "ChbxPower";
            this.ChbxPower.Size = new System.Drawing.Size(22, 21);
            this.ChbxPower.TabIndex = 5;
            this.ChbxPower.UseVisualStyleBackColor = true;
            // 
            // lbStatus
            // 
            this.lbStatus.AutoSize = true;
            this.lbStatus.Location = new System.Drawing.Point(16, 80);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(37, 13);
            this.lbStatus.TabIndex = 13;
            this.lbStatus.Text = "Status";
            // 
            // txtbHeadStatus1
            // 
            this.txtbHeadStatus1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtbHeadStatus1.Location = new System.Drawing.Point(54, 43);
            this.txtbHeadStatus1.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.txtbHeadStatus1.Name = "txtbHeadStatus1";
            this.txtbHeadStatus1.ReadOnly = true;
            this.txtbHeadStatus1.Size = new System.Drawing.Size(88, 20);
            this.txtbHeadStatus1.TabIndex = 22;
            // 
            // txtbTemperatureOutput1
            // 
            this.txtbTemperatureOutput1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtbTemperatureOutput1.Location = new System.Drawing.Point(338, 43);
            this.txtbTemperatureOutput1.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.txtbTemperatureOutput1.Name = "txtbTemperatureOutput1";
            this.txtbTemperatureOutput1.ReadOnly = true;
            this.txtbTemperatureOutput1.Size = new System.Drawing.Size(57, 20);
            this.txtbTemperatureOutput1.TabIndex = 14;
            // 
            // nudTemperatureHead1
            // 
            this.nudTemperatureHead1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.nudTemperatureHead1.DecimalPlaces = 1;
            this.nudTemperatureHead1.Location = new System.Drawing.Point(248, 43);
            this.nudTemperatureHead1.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.nudTemperatureHead1.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.nudTemperatureHead1.Minimum = new decimal(new int[] {
            21,
            0,
            0,
            0});
            this.nudTemperatureHead1.Name = "nudTemperatureHead1";
            this.nudTemperatureHead1.Size = new System.Drawing.Size(43, 20);
            this.nudTemperatureHead1.TabIndex = 11;
            this.nudTemperatureHead1.Tag = "temperatureHead1";
            this.nudTemperatureHead1.Value = new decimal(new int[] {
            21,
            0,
            0,
            0});
            this.nudTemperatureHead1.ValueChanged += new System.EventHandler(this.temperature_ValueChanged);
            // 
            // nudVoltageHead1
            // 
            this.nudVoltageHead1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.nudVoltageHead1.DecimalPlaces = 1;
            this.nudVoltageHead1.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudVoltageHead1.Location = new System.Drawing.Point(162, 43);
            this.nudVoltageHead1.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.nudVoltageHead1.Maximum = new decimal(new int[] {
            36,
            0,
            0,
            0});
            this.nudVoltageHead1.Minimum = new decimal(new int[] {
            18,
            0,
            0,
            0});
            this.nudVoltageHead1.Name = "nudVoltageHead1";
            this.nudVoltageHead1.Size = new System.Drawing.Size(44, 20);
            this.nudVoltageHead1.TabIndex = 12;
            this.nudVoltageHead1.Tag = "voltageHead1";
            this.nudVoltageHead1.Value = new decimal(new int[] {
            35,
            0,
            0,
            0});
            this.nudVoltageHead1.ValueChanged += new System.EventHandler(this.voltage_ValueChanged);
            // 
            // txtbHeadStatus2
            // 
            this.txtbHeadStatus2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtbHeadStatus2.Location = new System.Drawing.Point(54, 77);
            this.txtbHeadStatus2.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.txtbHeadStatus2.Name = "txtbHeadStatus2";
            this.txtbHeadStatus2.ReadOnly = true;
            this.txtbHeadStatus2.Size = new System.Drawing.Size(88, 20);
            this.txtbHeadStatus2.TabIndex = 20;
            // 
            // txtbTemperatureOutput2
            // 
            this.txtbTemperatureOutput2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtbTemperatureOutput2.Location = new System.Drawing.Point(338, 77);
            this.txtbTemperatureOutput2.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.txtbTemperatureOutput2.Name = "txtbTemperatureOutput2";
            this.txtbTemperatureOutput2.ReadOnly = true;
            this.txtbTemperatureOutput2.Size = new System.Drawing.Size(57, 20);
            this.txtbTemperatureOutput2.TabIndex = 14;
            // 
            // nudTemperatureHead2
            // 
            this.nudTemperatureHead2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.nudTemperatureHead2.DecimalPlaces = 1;
            this.nudTemperatureHead2.Location = new System.Drawing.Point(248, 77);
            this.nudTemperatureHead2.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.nudTemperatureHead2.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.nudTemperatureHead2.Minimum = new decimal(new int[] {
            21,
            0,
            0,
            0});
            this.nudTemperatureHead2.Name = "nudTemperatureHead2";
            this.nudTemperatureHead2.Size = new System.Drawing.Size(43, 20);
            this.nudTemperatureHead2.TabIndex = 11;
            this.nudTemperatureHead2.Tag = "temperatureHead2";
            this.nudTemperatureHead2.Value = new decimal(new int[] {
            21,
            0,
            0,
            0});
            this.nudTemperatureHead2.ValueChanged += new System.EventHandler(this.temperature_ValueChanged);
            // 
            // nudVoltageHead2
            // 
            this.nudVoltageHead2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.nudVoltageHead2.DecimalPlaces = 1;
            this.nudVoltageHead2.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudVoltageHead2.Location = new System.Drawing.Point(162, 77);
            this.nudVoltageHead2.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.nudVoltageHead2.Maximum = new decimal(new int[] {
            36,
            0,
            0,
            0});
            this.nudVoltageHead2.Minimum = new decimal(new int[] {
            18,
            0,
            0,
            0});
            this.nudVoltageHead2.Name = "nudVoltageHead2";
            this.nudVoltageHead2.Size = new System.Drawing.Size(44, 20);
            this.nudVoltageHead2.TabIndex = 12;
            this.nudVoltageHead2.Tag = "voltageHead2";
            this.nudVoltageHead2.Value = new decimal(new int[] {
            35,
            0,
            0,
            0});
            this.nudVoltageHead2.ValueChanged += new System.EventHandler(this.voltage_ValueChanged);
            // 
            // txtbHeadStatus3
            // 
            this.txtbHeadStatus3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtbHeadStatus3.Location = new System.Drawing.Point(54, 112);
            this.txtbHeadStatus3.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.txtbHeadStatus3.Name = "txtbHeadStatus3";
            this.txtbHeadStatus3.ReadOnly = true;
            this.txtbHeadStatus3.Size = new System.Drawing.Size(88, 20);
            this.txtbHeadStatus3.TabIndex = 22;
            // 
            // txtbTemperatureOutput3
            // 
            this.txtbTemperatureOutput3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtbTemperatureOutput3.Location = new System.Drawing.Point(338, 112);
            this.txtbTemperatureOutput3.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.txtbTemperatureOutput3.Name = "txtbTemperatureOutput3";
            this.txtbTemperatureOutput3.ReadOnly = true;
            this.txtbTemperatureOutput3.Size = new System.Drawing.Size(57, 20);
            this.txtbTemperatureOutput3.TabIndex = 14;
            // 
            // nudTemperatureHead3
            // 
            this.nudTemperatureHead3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.nudTemperatureHead3.DecimalPlaces = 1;
            this.nudTemperatureHead3.Location = new System.Drawing.Point(248, 112);
            this.nudTemperatureHead3.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.nudTemperatureHead3.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.nudTemperatureHead3.Minimum = new decimal(new int[] {
            21,
            0,
            0,
            0});
            this.nudTemperatureHead3.Name = "nudTemperatureHead3";
            this.nudTemperatureHead3.Size = new System.Drawing.Size(43, 20);
            this.nudTemperatureHead3.TabIndex = 11;
            this.nudTemperatureHead3.Tag = "temperatureHead3";
            this.nudTemperatureHead3.Value = new decimal(new int[] {
            21,
            0,
            0,
            0});
            this.nudTemperatureHead3.ValueChanged += new System.EventHandler(this.temperature_ValueChanged);
            // 
            // nudVoltageHead3
            // 
            this.nudVoltageHead3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.nudVoltageHead3.DecimalPlaces = 1;
            this.nudVoltageHead3.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudVoltageHead3.Location = new System.Drawing.Point(162, 112);
            this.nudVoltageHead3.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.nudVoltageHead3.Maximum = new decimal(new int[] {
            36,
            0,
            0,
            0});
            this.nudVoltageHead3.Minimum = new decimal(new int[] {
            18,
            0,
            0,
            0});
            this.nudVoltageHead3.Name = "nudVoltageHead3";
            this.nudVoltageHead3.Size = new System.Drawing.Size(44, 20);
            this.nudVoltageHead3.TabIndex = 12;
            this.nudVoltageHead3.Tag = "voltageHead3";
            this.nudVoltageHead3.Value = new decimal(new int[] {
            35,
            0,
            0,
            0});
            this.nudVoltageHead3.ValueChanged += new System.EventHandler(this.voltage_ValueChanged);
            // 
            // txtbTemperatureOutput4
            // 
            this.txtbTemperatureOutput4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtbTemperatureOutput4.Location = new System.Drawing.Point(338, 148);
            this.txtbTemperatureOutput4.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.txtbTemperatureOutput4.Name = "txtbTemperatureOutput4";
            this.txtbTemperatureOutput4.ReadOnly = true;
            this.txtbTemperatureOutput4.Size = new System.Drawing.Size(57, 20);
            this.txtbTemperatureOutput4.TabIndex = 14;
            // 
            // nudTemperatureHead4
            // 
            this.nudTemperatureHead4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.nudTemperatureHead4.DecimalPlaces = 1;
            this.nudTemperatureHead4.Location = new System.Drawing.Point(248, 148);
            this.nudTemperatureHead4.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.nudTemperatureHead4.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.nudTemperatureHead4.Minimum = new decimal(new int[] {
            21,
            0,
            0,
            0});
            this.nudTemperatureHead4.Name = "nudTemperatureHead4";
            this.nudTemperatureHead4.Size = new System.Drawing.Size(43, 20);
            this.nudTemperatureHead4.TabIndex = 11;
            this.nudTemperatureHead4.Tag = "temperatureHead4";
            this.nudTemperatureHead4.Value = new decimal(new int[] {
            21,
            0,
            0,
            0});
            this.nudTemperatureHead4.ValueChanged += new System.EventHandler(this.temperature_ValueChanged);
            // 
            // nudVoltageHead4
            // 
            this.nudVoltageHead4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.nudVoltageHead4.DecimalPlaces = 1;
            this.nudVoltageHead4.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudVoltageHead4.Location = new System.Drawing.Point(162, 148);
            this.nudVoltageHead4.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.nudVoltageHead4.Maximum = new decimal(new int[] {
            36,
            0,
            0,
            0});
            this.nudVoltageHead4.Minimum = new decimal(new int[] {
            18,
            0,
            0,
            0});
            this.nudVoltageHead4.Name = "nudVoltageHead4";
            this.nudVoltageHead4.Size = new System.Drawing.Size(44, 20);
            this.nudVoltageHead4.TabIndex = 12;
            this.nudVoltageHead4.Tag = "voltageHead4";
            this.nudVoltageHead4.Value = new decimal(new int[] {
            35,
            0,
            0,
            0});
            this.nudVoltageHead4.ValueChanged += new System.EventHandler(this.voltage_ValueChanged);
            // 
            // txtbStatusBox
            // 
            this.txtbStatusBox.Location = new System.Drawing.Point(63, 78);
            this.txtbStatusBox.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.txtbStatusBox.Name = "txtbStatusBox";
            this.txtbStatusBox.ReadOnly = true;
            this.txtbStatusBox.Size = new System.Drawing.Size(84, 20);
            this.txtbStatusBox.TabIndex = 16;
            // 
            // lbTimeOn
            // 
            this.lbTimeOn.AutoSize = true;
            this.lbTimeOn.Location = new System.Drawing.Point(13, 108);
            this.lbTimeOn.Name = "lbTimeOn";
            this.lbTimeOn.Size = new System.Drawing.Size(47, 13);
            this.lbTimeOn.TabIndex = 17;
            this.lbTimeOn.Text = "Time On";
            // 
            // txtbBoardUpTime
            // 
            this.txtbBoardUpTime.Location = new System.Drawing.Point(63, 106);
            this.txtbBoardUpTime.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.txtbBoardUpTime.Name = "txtbBoardUpTime";
            this.txtbBoardUpTime.ReadOnly = true;
            this.txtbBoardUpTime.Size = new System.Drawing.Size(84, 20);
            this.txtbBoardUpTime.TabIndex = 18;
            this.txtbBoardUpTime.Text = "N/A";
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(13, 32);
            this.btnReset.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(132, 24);
            this.btnReset.TabIndex = 20;
            this.btnReset.Text = "Reset Board";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.reset_Click);
            // 
            // StatusTable
            // 
            this.StatusTable.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.StatusTable.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.StatusTable.ColumnCount = 6;
            this.StatusTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30.131F));
            this.StatusTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 69.869F));
            this.StatusTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 71F));
            this.StatusTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 98F));
            this.StatusTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 93F));
            this.StatusTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 71F));
            this.StatusTable.Controls.Add(this.txtbHeadStatus4, 1, 4);
            this.StatusTable.Controls.Add(this.txtbPrintCounter4, 5, 4);
            this.StatusTable.Controls.Add(this.label40, 0, 4);
            this.StatusTable.Controls.Add(this.lbHead, 0, 0);
            this.StatusTable.Controls.Add(this.label37, 0, 1);
            this.StatusTable.Controls.Add(this.txtbHeadStatus1, 1, 1);
            this.StatusTable.Controls.Add(this.txtbHeadStatus3, 1, 3);
            this.StatusTable.Controls.Add(this.txtbHeadStatus2, 1, 2);
            this.StatusTable.Controls.Add(this.label39, 0, 2);
            this.StatusTable.Controls.Add(this.txtbTemperatureOutput1, 4, 1);
            this.StatusTable.Controls.Add(this.txtbTemperatureOutput2, 4, 2);
            this.StatusTable.Controls.Add(this.txtbTemperatureOutput3, 4, 3);
            this.StatusTable.Controls.Add(this.txtbTemperatureOutput4, 4, 4);
            this.StatusTable.Controls.Add(this.nudVoltageHead2, 2, 2);
            this.StatusTable.Controls.Add(this.label38, 0, 3);
            this.StatusTable.Controls.Add(this.nudTemperatureHead2, 3, 2);
            this.StatusTable.Controls.Add(this.nudVoltageHead3, 2, 3);
            this.StatusTable.Controls.Add(this.lbCurrentTemperature, 4, 0);
            this.StatusTable.Controls.Add(this.nudTemperatureHead3, 3, 3);
            this.StatusTable.Controls.Add(this.nudVoltageHead4, 2, 4);
            this.StatusTable.Controls.Add(this.lbSetTemperature, 3, 0);
            this.StatusTable.Controls.Add(this.lbSetVoltage, 2, 0);
            this.StatusTable.Controls.Add(this.lbHeadStatus, 1, 0);
            this.StatusTable.Controls.Add(this.lbPrintCount, 5, 0);
            this.StatusTable.Controls.Add(this.txtbPrintCounter1, 5, 1);
            this.StatusTable.Controls.Add(this.txtbPrintCounter2, 5, 2);
            this.StatusTable.Controls.Add(this.txtbPrintCounter3, 5, 3);
            this.StatusTable.Controls.Add(this.nudTemperatureHead4, 3, 4);
            this.StatusTable.Controls.Add(this.nudTemperatureHead1, 3, 1);
            this.StatusTable.Controls.Add(this.nudVoltageHead1, 2, 1);
            this.StatusTable.Location = new System.Drawing.Point(325, 8);
            this.StatusTable.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.StatusTable.Name = "StatusTable";
            this.StatusTable.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.StatusTable.RowCount = 5;
            this.StatusTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 52.17391F));
            this.StatusTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 47.82609F));
            this.StatusTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.StatusTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.StatusTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.StatusTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 13F));
            this.StatusTable.Size = new System.Drawing.Size(490, 181);
            this.StatusTable.TabIndex = 21;
            // 
            // txtbHeadStatus4
            // 
            this.txtbHeadStatus4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtbHeadStatus4.Location = new System.Drawing.Point(54, 148);
            this.txtbHeadStatus4.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.txtbHeadStatus4.Name = "txtbHeadStatus4";
            this.txtbHeadStatus4.ReadOnly = true;
            this.txtbHeadStatus4.Size = new System.Drawing.Size(88, 20);
            this.txtbHeadStatus4.TabIndex = 39;
            // 
            // txtbPrintCounter4
            // 
            this.txtbPrintCounter4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtbPrintCounter4.Location = new System.Drawing.Point(421, 148);
            this.txtbPrintCounter4.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.txtbPrintCounter4.Name = "txtbPrintCounter4";
            this.txtbPrintCounter4.ReadOnly = true;
            this.txtbPrintCounter4.Size = new System.Drawing.Size(57, 20);
            this.txtbPrintCounter4.TabIndex = 38;
            // 
            // label40
            // 
            this.label40.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label40.AutoSize = true;
            this.label40.Location = new System.Drawing.Point(19, 152);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(13, 13);
            this.label40.TabIndex = 32;
            this.label40.Text = "4";
            // 
            // lbHead
            // 
            this.lbHead.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbHead.AutoSize = true;
            this.lbHead.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbHead.Location = new System.Drawing.Point(10, 4);
            this.lbHead.Name = "lbHead";
            this.lbHead.Size = new System.Drawing.Size(30, 33);
            this.lbHead.TabIndex = 24;
            this.lbHead.Text = "Head";
            this.lbHead.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label37
            // 
            this.label37.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(19, 47);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(13, 13);
            this.label37.TabIndex = 29;
            this.label37.Text = "1";
            // 
            // label39
            // 
            this.label39.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label39.AutoSize = true;
            this.label39.Location = new System.Drawing.Point(19, 80);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(13, 13);
            this.label39.TabIndex = 31;
            this.label39.Text = "2";
            // 
            // label38
            // 
            this.label38.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(19, 115);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(13, 13);
            this.label38.TabIndex = 30;
            this.label38.Text = "3";
            // 
            // lbCurrentTemperature
            // 
            this.lbCurrentTemperature.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbCurrentTemperature.AutoSize = true;
            this.lbCurrentTemperature.Location = new System.Drawing.Point(323, 7);
            this.lbCurrentTemperature.Name = "lbCurrentTemperature";
            this.lbCurrentTemperature.Size = new System.Drawing.Size(87, 26);
            this.lbCurrentTemperature.TabIndex = 27;
            this.lbCurrentTemperature.Text = "Current Temperature (°C)";
            this.lbCurrentTemperature.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbSetTemperature
            // 
            this.lbSetTemperature.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbSetTemperature.AutoSize = true;
            this.lbSetTemperature.Location = new System.Drawing.Point(227, 7);
            this.lbSetTemperature.Name = "lbSetTemperature";
            this.lbSetTemperature.Size = new System.Drawing.Size(86, 26);
            this.lbSetTemperature.TabIndex = 26;
            this.lbSetTemperature.Text = "Set Temperature (°C)";
            this.lbSetTemperature.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbSetVoltage
            // 
            this.lbSetVoltage.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbSetVoltage.AutoSize = true;
            this.lbSetVoltage.Location = new System.Drawing.Point(155, 14);
            this.lbSetVoltage.Name = "lbSetVoltage";
            this.lbSetVoltage.Size = new System.Drawing.Size(59, 13);
            this.lbSetVoltage.TabIndex = 25;
            this.lbSetVoltage.Text = "Voltage (V)";
            this.lbSetVoltage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbHeadStatus
            // 
            this.lbHeadStatus.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbHeadStatus.AutoSize = true;
            this.lbHeadStatus.Location = new System.Drawing.Point(79, 14);
            this.lbHeadStatus.Name = "lbHeadStatus";
            this.lbHeadStatus.Size = new System.Drawing.Size(37, 13);
            this.lbHeadStatus.TabIndex = 33;
            this.lbHeadStatus.Text = "Status";
            this.lbHeadStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbPrintCount
            // 
            this.lbPrintCount.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbPrintCount.AutoSize = true;
            this.lbPrintCount.Location = new System.Drawing.Point(420, 14);
            this.lbPrintCount.Name = "lbPrintCount";
            this.lbPrintCount.Size = new System.Drawing.Size(59, 13);
            this.lbPrintCount.TabIndex = 34;
            this.lbPrintCount.Text = "Print Count";
            this.lbPrintCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtbPrintCounter1
            // 
            this.txtbPrintCounter1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtbPrintCounter1.Location = new System.Drawing.Point(421, 43);
            this.txtbPrintCounter1.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.txtbPrintCounter1.Name = "txtbPrintCounter1";
            this.txtbPrintCounter1.ReadOnly = true;
            this.txtbPrintCounter1.Size = new System.Drawing.Size(57, 20);
            this.txtbPrintCounter1.TabIndex = 35;
            // 
            // txtbPrintCounter2
            // 
            this.txtbPrintCounter2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtbPrintCounter2.Location = new System.Drawing.Point(421, 77);
            this.txtbPrintCounter2.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.txtbPrintCounter2.Name = "txtbPrintCounter2";
            this.txtbPrintCounter2.ReadOnly = true;
            this.txtbPrintCounter2.Size = new System.Drawing.Size(57, 20);
            this.txtbPrintCounter2.TabIndex = 36;
            // 
            // txtbPrintCounter3
            // 
            this.txtbPrintCounter3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtbPrintCounter3.Location = new System.Drawing.Point(421, 112);
            this.txtbPrintCounter3.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.txtbPrintCounter3.Name = "txtbPrintCounter3";
            this.txtbPrintCounter3.ReadOnly = true;
            this.txtbPrintCounter3.Size = new System.Drawing.Size(57, 20);
            this.txtbPrintCounter3.TabIndex = 37;
            // 
            // lbFrequency
            // 
            this.lbFrequency.AutoSize = true;
            this.lbFrequency.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lbFrequency.Location = new System.Drawing.Point(13, 148);
            this.lbFrequency.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbFrequency.Name = "lbFrequency";
            this.lbFrequency.Size = new System.Drawing.Size(169, 25);
            this.lbFrequency.TabIndex = 29;
            this.lbFrequency.Text = "Frequency (Hz):";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // FillCycleBox
            // 
            this.FillCycleBox.Controls.Add(this.btnFillCycleA);
            this.FillCycleBox.Controls.Add(this.lbDropWatchingFillCycle);
            this.FillCycleBox.Controls.Add(this.btnFillCycleB);
            this.FillCycleBox.Controls.Add(this.btnFillCycleC);
            this.FillCycleBox.Location = new System.Drawing.Point(593, 53);
            this.FillCycleBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.FillCycleBox.Name = "FillCycleBox";
            this.FillCycleBox.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.FillCycleBox.Size = new System.Drawing.Size(183, 60);
            this.FillCycleBox.TabIndex = 52;
            this.FillCycleBox.TabStop = false;
            this.toolTip1.SetToolTip(this.FillCycleBox, "Fill each cycle");
            // 
            // btnFillCycleA
            // 
            this.btnFillCycleA.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFillCycleA.Location = new System.Drawing.Point(11, 34);
            this.btnFillCycleA.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.btnFillCycleA.Name = "btnFillCycleA";
            this.btnFillCycleA.Size = new System.Drawing.Size(43, 23);
            this.btnFillCycleA.TabIndex = 27;
            this.btnFillCycleA.Tag = "fillACycle";
            this.btnFillCycleA.Text = "A";
            this.toolTip1.SetToolTip(this.btnFillCycleA, "Fill every 1st nozzle of 3");
            this.btnFillCycleA.UseVisualStyleBackColor = true;
            this.btnFillCycleA.Click += new System.EventHandler(this.FillCycleA_Click);
            // 
            // lbDropWatchingFillCycle
            // 
            this.lbDropWatchingFillCycle.AutoSize = true;
            this.lbDropWatchingFillCycle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbDropWatchingFillCycle.Location = new System.Drawing.Point(62, 14);
            this.lbDropWatchingFillCycle.Name = "lbDropWatchingFillCycle";
            this.lbDropWatchingFillCycle.Size = new System.Drawing.Size(97, 25);
            this.lbDropWatchingFillCycle.TabIndex = 51;
            this.lbDropWatchingFillCycle.Text = "Fill Cycle:";
            // 
            // btnFillCycleB
            // 
            this.btnFillCycleB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFillCycleB.Location = new System.Drawing.Point(72, 34);
            this.btnFillCycleB.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.btnFillCycleB.Name = "btnFillCycleB";
            this.btnFillCycleB.Size = new System.Drawing.Size(43, 23);
            this.btnFillCycleB.TabIndex = 49;
            this.btnFillCycleB.Tag = "fillBCycle";
            this.btnFillCycleB.Text = "B";
            this.toolTip1.SetToolTip(this.btnFillCycleB, "Fill every 2nd nozzle of 3");
            this.btnFillCycleB.UseVisualStyleBackColor = true;
            this.btnFillCycleB.Click += new System.EventHandler(this.FillCycleB_Click);
            // 
            // btnFillCycleC
            // 
            this.btnFillCycleC.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFillCycleC.Location = new System.Drawing.Point(131, 34);
            this.btnFillCycleC.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.btnFillCycleC.Name = "btnFillCycleC";
            this.btnFillCycleC.Size = new System.Drawing.Size(43, 23);
            this.btnFillCycleC.TabIndex = 50;
            this.btnFillCycleC.Tag = "fillCCycle";
            this.btnFillCycleC.Text = "C";
            this.toolTip1.SetToolTip(this.btnFillCycleC, "Fill every 3rd nozzle of 3");
            this.btnFillCycleC.UseVisualStyleBackColor = true;
            this.btnFillCycleC.Click += new System.EventHandler(this.FillCycleC_Click);
            // 
            // btnFillHead
            // 
            this.btnFillHead.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFillHead.Location = new System.Drawing.Point(696, 159);
            this.btnFillHead.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.btnFillHead.Name = "btnFillHead";
            this.btnFillHead.Size = new System.Drawing.Size(80, 23);
            this.btnFillHead.TabIndex = 56;
            this.btnFillHead.Text = "Fill Head";
            this.toolTip1.SetToolTip(this.btnFillHead, "Fill all Nozzles on Selected Head");
            this.btnFillHead.UseVisualStyleBackColor = true;
            this.btnFillHead.Click += new System.EventHandler(this.fillHead_Click);
            // 
            // btnFillGap
            // 
            this.btnFillGap.Location = new System.Drawing.Point(593, 159);
            this.btnFillGap.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.btnFillGap.Name = "btnFillGap";
            this.btnFillGap.Size = new System.Drawing.Size(80, 23);
            this.btnFillGap.TabIndex = 53;
            this.btnFillGap.Text = "Fill Gap";
            this.toolTip1.SetToolTip(this.btnFillGap, "Fill Selected Nozzles every internal determined by the Gap");
            this.btnFillGap.UseVisualStyleBackColor = true;
            this.btnFillGap.Click += new System.EventHandler(this.FillGapButton_Click);
            // 
            // btnFillSpan
            // 
            this.btnFillSpan.Location = new System.Drawing.Point(696, 125);
            this.btnFillSpan.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.btnFillSpan.Name = "btnFillSpan";
            this.btnFillSpan.Size = new System.Drawing.Size(80, 23);
            this.btnFillSpan.TabIndex = 46;
            this.btnFillSpan.Text = "Fill Span";
            this.toolTip1.SetToolTip(this.btnFillSpan, "Fill Selected Span");
            this.btnFillSpan.UseVisualStyleBackColor = true;
            this.btnFillSpan.Click += new System.EventHandler(this.FillSpanNozzleButton_Click);
            // 
            // btnFillNozzle
            // 
            this.btnFillNozzle.Location = new System.Drawing.Point(593, 125);
            this.btnFillNozzle.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.btnFillNozzle.Name = "btnFillNozzle";
            this.btnFillNozzle.Size = new System.Drawing.Size(80, 23);
            this.btnFillNozzle.TabIndex = 45;
            this.btnFillNozzle.Text = "Fill Nozzle";
            this.toolTip1.SetToolTip(this.btnFillNozzle, "Fill Selected Nozzle");
            this.btnFillNozzle.UseVisualStyleBackColor = true;
            this.btnFillNozzle.Click += new System.EventHandler(this.FillSingleNozzleButton_Click);
            // 
            // btnClearHead
            // 
            this.btnClearHead.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClearHead.Location = new System.Drawing.Point(696, 10);
            this.btnClearHead.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.btnClearHead.Name = "btnClearHead";
            this.btnClearHead.Size = new System.Drawing.Size(80, 23);
            this.btnClearHead.TabIndex = 44;
            this.btnClearHead.Text = "Clear Head";
            this.toolTip1.SetToolTip(this.btnClearHead, "Clear all the Heads and Stop Printing");
            this.btnClearHead.UseVisualStyleBackColor = true;
            this.btnClearHead.Click += new System.EventHandler(this.btnClearHead_Click);
            // 
            // tcDropWatchingAndImageModes
            // 
            this.tcDropWatchingAndImageModes.Controls.Add(this.DropWatchingTab);
            this.tcDropWatchingAndImageModes.Controls.Add(this.ImageModeTab);
            this.tcDropWatchingAndImageModes.Controls.Add(this.tabPage1);
            this.tcDropWatchingAndImageModes.DataBindings.Add(new System.Windows.Forms.Binding("TabIndex", global::DriverBoardDropwatcher.Properties.Settings.Default, "TabNumber", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tcDropWatchingAndImageModes.Location = new System.Drawing.Point(19, 192);
            this.tcDropWatchingAndImageModes.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tcDropWatchingAndImageModes.Name = "tcDropWatchingAndImageModes";
            this.tcDropWatchingAndImageModes.SelectedIndex = 0;
            this.tcDropWatchingAndImageModes.Size = new System.Drawing.Size(796, 417);
            this.tcDropWatchingAndImageModes.TabIndex = global::DriverBoardDropwatcher.Properties.Settings.Default.TabNumber;
            this.tcDropWatchingAndImageModes.SelectedIndexChanged += new System.EventHandler(this.tcDropWatchingAndImageModes_SelectedIndexChanged);
            // 
            // DropWatchingTab
            // 
            this.DropWatchingTab.Controls.Add(this.txtbNozzleSpanStatusBox);
            this.DropWatchingTab.Controls.Add(this.chbxIsFillSpan);
            this.DropWatchingTab.Controls.Add(this.chbxIsFillGap);
            this.DropWatchingTab.Controls.Add(this.chbxIsFillHead);
            this.DropWatchingTab.Controls.Add(this.chbxIsFillNozzle);
            this.DropWatchingTab.Controls.Add(this.btnFillHead);
            this.DropWatchingTab.Controls.Add(this.nudGap);
            this.DropWatchingTab.Controls.Add(this.lbDropWatchingGap);
            this.DropWatchingTab.Controls.Add(this.btnFillGap);
            this.DropWatchingTab.Controls.Add(this.FillCycleBox);
            this.DropWatchingTab.Controls.Add(this.txtbHeadStatus);
            this.DropWatchingTab.Controls.Add(this.btnFillSpan);
            this.DropWatchingTab.Controls.Add(this.btnFillNozzle);
            this.DropWatchingTab.Controls.Add(this.btnClearHead);
            this.DropWatchingTab.Controls.Add(this.txtbFrequencyDuplicate);
            this.DropWatchingTab.Controls.Add(this.lbDropWatchingFrequencyDuplicate);
            this.DropWatchingTab.Controls.Add(this.lbDropWatchingSpan);
            this.DropWatchingTab.Controls.Add(this.lbDropWatchingNozzle);
            this.DropWatchingTab.Controls.Add(this.nudSpan);
            this.DropWatchingTab.Controls.Add(this.lbHeadIndex);
            this.DropWatchingTab.Controls.Add(this.lbDropWatchingMode);
            this.DropWatchingTab.Controls.Add(this.nudNozzle);
            this.DropWatchingTab.Controls.Add(this.cbDropWatchHeadSelection);
            this.DropWatchingTab.Controls.Add(this.label24);
            this.DropWatchingTab.Controls.Add(this.cbDropWatchMode);
            this.DropWatchingTab.Location = new System.Drawing.Point(4, 22);
            this.DropWatchingTab.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.DropWatchingTab.Name = "DropWatchingTab";
            this.DropWatchingTab.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.DropWatchingTab.Size = new System.Drawing.Size(788, 391);
            this.DropWatchingTab.TabIndex = 1;
            this.DropWatchingTab.Text = "Drop Watching";
            this.DropWatchingTab.UseVisualStyleBackColor = true;
            this.DropWatchingTab.Click += new System.EventHandler(this.DropWatchingTab_Click);
            this.DropWatchingTab.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.DropWatchingTab_PreviewKeyDown);
            // 
            // txtbNozzleSpanStatusBox
            // 
            this.txtbNozzleSpanStatusBox.Location = new System.Drawing.Point(161, 79);
            this.txtbNozzleSpanStatusBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtbNozzleSpanStatusBox.Name = "txtbNozzleSpanStatusBox";
            this.txtbNozzleSpanStatusBox.ReadOnly = true;
            this.txtbNozzleSpanStatusBox.Size = new System.Drawing.Size(185, 20);
            this.txtbNozzleSpanStatusBox.TabIndex = 61;
            // 
            // chbxIsFillSpan
            // 
            this.chbxIsFillSpan.AutoSize = true;
            this.chbxIsFillSpan.Checked = global::DriverBoardDropwatcher.Properties.Settings.Default.FillSpanCheckedStatus;
            this.chbxIsFillSpan.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::DriverBoardDropwatcher.Properties.Settings.Default, "FillSpanCheckedStatus", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chbxIsFillSpan.Enabled = false;
            this.chbxIsFillSpan.Location = new System.Drawing.Point(679, 130);
            this.chbxIsFillSpan.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.chbxIsFillSpan.Name = "chbxIsFillSpan";
            this.chbxIsFillSpan.Size = new System.Drawing.Size(22, 21);
            this.chbxIsFillSpan.TabIndex = 60;
            this.chbxIsFillSpan.UseVisualStyleBackColor = true;
            // 
            // chbxIsFillGap
            // 
            this.chbxIsFillGap.AutoSize = true;
            this.chbxIsFillGap.Checked = global::DriverBoardDropwatcher.Properties.Settings.Default.FillGapCheckedStatus;
            this.chbxIsFillGap.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::DriverBoardDropwatcher.Properties.Settings.Default, "FillGapCheckedStatus", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chbxIsFillGap.Enabled = false;
            this.chbxIsFillGap.Location = new System.Drawing.Point(573, 164);
            this.chbxIsFillGap.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.chbxIsFillGap.Name = "chbxIsFillGap";
            this.chbxIsFillGap.Size = new System.Drawing.Size(22, 21);
            this.chbxIsFillGap.TabIndex = 59;
            this.chbxIsFillGap.UseVisualStyleBackColor = true;
            // 
            // chbxIsFillHead
            // 
            this.chbxIsFillHead.AutoSize = true;
            this.chbxIsFillHead.Checked = global::DriverBoardDropwatcher.Properties.Settings.Default.FillHeadCheckedStatus;
            this.chbxIsFillHead.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::DriverBoardDropwatcher.Properties.Settings.Default, "FillHeadCheckedStatus", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chbxIsFillHead.Enabled = false;
            this.chbxIsFillHead.Location = new System.Drawing.Point(679, 164);
            this.chbxIsFillHead.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.chbxIsFillHead.Name = "chbxIsFillHead";
            this.chbxIsFillHead.Size = new System.Drawing.Size(22, 21);
            this.chbxIsFillHead.TabIndex = 58;
            this.chbxIsFillHead.UseVisualStyleBackColor = true;
            // 
            // chbxIsFillNozzle
            // 
            this.chbxIsFillNozzle.AutoSize = true;
            this.chbxIsFillNozzle.Checked = global::DriverBoardDropwatcher.Properties.Settings.Default.FillNozzleCheckedStatus;
            this.chbxIsFillNozzle.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::DriverBoardDropwatcher.Properties.Settings.Default, "FillNozzleCheckedStatus", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chbxIsFillNozzle.Enabled = false;
            this.chbxIsFillNozzle.Location = new System.Drawing.Point(573, 130);
            this.chbxIsFillNozzle.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.chbxIsFillNozzle.Name = "chbxIsFillNozzle";
            this.chbxIsFillNozzle.Size = new System.Drawing.Size(22, 21);
            this.chbxIsFillNozzle.TabIndex = 57;
            this.chbxIsFillNozzle.UseVisualStyleBackColor = true;
            // 
            // nudGap
            // 
            this.nudGap.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::DriverBoardDropwatcher.Properties.Settings.Default, "Gap", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.nudGap.Location = new System.Drawing.Point(49, 151);
            this.nudGap.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.nudGap.Maximum = new decimal(new int[] {
            128,
            0,
            0,
            0});
            this.nudGap.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudGap.Name = "nudGap";
            this.nudGap.Size = new System.Drawing.Size(85, 20);
            this.nudGap.TabIndex = 55;
            this.nudGap.Value = global::DriverBoardDropwatcher.Properties.Settings.Default.Gap;
            this.nudGap.ValueChanged += new System.EventHandler(this.GapValue_ValueChanged);
            // 
            // lbDropWatchingGap
            // 
            this.lbDropWatchingGap.AutoSize = true;
            this.lbDropWatchingGap.Location = new System.Drawing.Point(11, 151);
            this.lbDropWatchingGap.Name = "lbDropWatchingGap";
            this.lbDropWatchingGap.Size = new System.Drawing.Size(30, 13);
            this.lbDropWatchingGap.TabIndex = 54;
            this.lbDropWatchingGap.Text = "Gap:";
            // 
            // txtbHeadStatus
            // 
            this.txtbHeadStatus.Location = new System.Drawing.Point(662, 36);
            this.txtbHeadStatus.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtbHeadStatus.Name = "txtbHeadStatus";
            this.txtbHeadStatus.ReadOnly = true;
            this.txtbHeadStatus.Size = new System.Drawing.Size(114, 20);
            this.txtbHeadStatus.TabIndex = 47;
            this.txtbHeadStatus.Text = "--Head Status--";
            // 
            // txtbFrequencyDuplicate
            // 
            this.txtbFrequencyDuplicate.Location = new System.Drawing.Point(306, 10);
            this.txtbFrequencyDuplicate.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtbFrequencyDuplicate.Name = "txtbFrequencyDuplicate";
            this.txtbFrequencyDuplicate.ReadOnly = true;
            this.txtbFrequencyDuplicate.Size = new System.Drawing.Size(121, 20);
            this.txtbFrequencyDuplicate.TabIndex = 43;
            // 
            // lbDropWatchingFrequencyDuplicate
            // 
            this.lbDropWatchingFrequencyDuplicate.AutoSize = true;
            this.lbDropWatchingFrequencyDuplicate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lbDropWatchingFrequencyDuplicate.Location = new System.Drawing.Point(223, 12);
            this.lbDropWatchingFrequencyDuplicate.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbDropWatchingFrequencyDuplicate.Name = "lbDropWatchingFrequencyDuplicate";
            this.lbDropWatchingFrequencyDuplicate.Size = new System.Drawing.Size(122, 20);
            this.lbDropWatchingFrequencyDuplicate.TabIndex = 42;
            this.lbDropWatchingFrequencyDuplicate.Text = "Frequency (Hz):";
            // 
            // lbDropWatchingSpan
            // 
            this.lbDropWatchingSpan.AutoSize = true;
            this.lbDropWatchingSpan.Location = new System.Drawing.Point(11, 115);
            this.lbDropWatchingSpan.Name = "lbDropWatchingSpan";
            this.lbDropWatchingSpan.Size = new System.Drawing.Size(35, 13);
            this.lbDropWatchingSpan.TabIndex = 40;
            this.lbDropWatchingSpan.Text = "Span:";
            // 
            // lbDropWatchingNozzle
            // 
            this.lbDropWatchingNozzle.AutoSize = true;
            this.lbDropWatchingNozzle.Location = new System.Drawing.Point(11, 81);
            this.lbDropWatchingNozzle.Name = "lbDropWatchingNozzle";
            this.lbDropWatchingNozzle.Size = new System.Drawing.Size(42, 13);
            this.lbDropWatchingNozzle.TabIndex = 38;
            this.lbDropWatchingNozzle.Text = "Nozzle:";
            // 
            // nudSpan
            // 
            this.nudSpan.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::DriverBoardDropwatcher.Properties.Settings.Default, "Span", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.nudSpan.Location = new System.Drawing.Point(49, 114);
            this.nudSpan.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.nudSpan.Maximum = new decimal(new int[] {
            128,
            0,
            0,
            0});
            this.nudSpan.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudSpan.Name = "nudSpan";
            this.nudSpan.Size = new System.Drawing.Size(85, 20);
            this.nudSpan.TabIndex = 37;
            this.nudSpan.Value = global::DriverBoardDropwatcher.Properties.Settings.Default.Span;
            this.nudSpan.ValueChanged += new System.EventHandler(this.SpanValue_ValueChanged);
            // 
            // lbHeadIndex
            // 
            this.lbHeadIndex.AutoSize = true;
            this.lbHeadIndex.Location = new System.Drawing.Point(11, 46);
            this.lbHeadIndex.Name = "lbHeadIndex";
            this.lbHeadIndex.Size = new System.Drawing.Size(65, 13);
            this.lbHeadIndex.TabIndex = 31;
            this.lbHeadIndex.Text = "Head Index:";
            // 
            // lbDropWatchingMode
            // 
            this.lbDropWatchingMode.AutoSize = true;
            this.lbDropWatchingMode.Location = new System.Drawing.Point(11, 12);
            this.lbDropWatchingMode.Name = "lbDropWatchingMode";
            this.lbDropWatchingMode.Size = new System.Drawing.Size(37, 13);
            this.lbDropWatchingMode.TabIndex = 30;
            this.lbDropWatchingMode.Text = "Mode:";
            // 
            // nudNozzle
            // 
            this.nudNozzle.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::DriverBoardDropwatcher.Properties.Settings.Default, "Index", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.nudNozzle.Location = new System.Drawing.Point(57, 79);
            this.nudNozzle.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.nudNozzle.Maximum = new decimal(new int[] {
            128,
            0,
            0,
            0});
            this.nudNozzle.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudNozzle.Name = "nudNozzle";
            this.nudNozzle.Size = new System.Drawing.Size(85, 20);
            this.nudNozzle.TabIndex = 27;
            this.nudNozzle.Value = global::DriverBoardDropwatcher.Properties.Settings.Default.Index;
            this.nudNozzle.ValueChanged += new System.EventHandler(this.NozzleValue_ValueChanged);
            // 
            // cbDropWatchHeadSelection
            // 
            this.cbDropWatchHeadSelection.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::DriverBoardDropwatcher.Properties.Settings.Default, "DropWatchHead", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cbDropWatchHeadSelection.FormattingEnabled = true;
            this.cbDropWatchHeadSelection.Items.AddRange(new object[] {
            "Head 1",
            "Head 2",
            "Head 3",
            "Head 4"});
            this.cbDropWatchHeadSelection.Location = new System.Drawing.Point(80, 43);
            this.cbDropWatchHeadSelection.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.cbDropWatchHeadSelection.Name = "cbDropWatchHeadSelection";
            this.cbDropWatchHeadSelection.Size = new System.Drawing.Size(123, 21);
            this.cbDropWatchHeadSelection.TabIndex = 21;
            this.cbDropWatchHeadSelection.Text = global::DriverBoardDropwatcher.Properties.Settings.Default.DropWatchHead;
            this.cbDropWatchHeadSelection.SelectedIndexChanged += new System.EventHandler(this.cbDropWatchHeadSelection_SelectedIndexChanged);
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(23, 46);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(0, 13);
            this.label24.TabIndex = 19;
            // 
            // cbDropWatchMode
            // 
            this.cbDropWatchMode.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::DriverBoardDropwatcher.Properties.Settings.Default, "DropWatchMode", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cbDropWatchMode.FormattingEnabled = true;
            this.cbDropWatchMode.Items.AddRange(new object[] {
            "Internal Mode",
            "External Mode"});
            this.cbDropWatchMode.Location = new System.Drawing.Point(58, 10);
            this.cbDropWatchMode.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.cbDropWatchMode.Name = "cbDropWatchMode";
            this.cbDropWatchMode.Size = new System.Drawing.Size(123, 21);
            this.cbDropWatchMode.TabIndex = 1;
            this.cbDropWatchMode.Text = global::DriverBoardDropwatcher.Properties.Settings.Default.DropWatchMode;
            this.cbDropWatchMode.SelectedIndexChanged += new System.EventHandler(this.cbDropWatchMode_SelectedIndexChanged);
            // 
            // ImageModeTab
            // 
            this.ImageModeTab.Controls.Add(this.txtbPrintStatus);
            this.ImageModeTab.Controls.Add(this.txtbCurrentStepperPosition);
            this.ImageModeTab.Controls.Add(this.lbCurrentStepperPosition);
            this.ImageModeTab.Controls.Add(this.nudSetPosition);
            this.ImageModeTab.Controls.Add(this.txtbCurrentEncoderPosition);
            this.ImageModeTab.Controls.Add(this.lbSetPosition);
            this.ImageModeTab.Controls.Add(this.lbCurrentEncoderPosition);
            this.ImageModeTab.Controls.Add(this.txtbImageHeadStatus);
            this.ImageModeTab.Controls.Add(this.cdPDdirection);
            this.ImageModeTab.Controls.Add(this.lbPDdirection);
            this.ImageModeTab.Controls.Add(this.cbEncoderTrackedPosition);
            this.ImageModeTab.Controls.Add(this.lbEncoderTrackedPosition);
            this.ImageModeTab.Controls.Add(this.cbPDpolarity);
            this.ImageModeTab.Controls.Add(this.lbPDpolarity);
            this.ImageModeTab.Controls.Add(this.lbImageMode);
            this.ImageModeTab.Controls.Add(this.cbImageMode);
            this.ImageModeTab.Controls.Add(this.btnCancel);
            this.ImageModeTab.Controls.Add(this.btnPrintImage);
            this.ImageModeTab.Controls.Add(this.txtbDimensionsHead4);
            this.ImageModeTab.Controls.Add(this.lbDimensionsHead4);
            this.ImageModeTab.Controls.Add(this.txtbDimensionsHead3);
            this.ImageModeTab.Controls.Add(this.lbDimensionsHead3);
            this.ImageModeTab.Controls.Add(this.txtbDimensionsHead2);
            this.ImageModeTab.Controls.Add(this.lbDimensionsHead2);
            this.ImageModeTab.Controls.Add(this.txtbDimensionsHead1);
            this.ImageModeTab.Controls.Add(this.lbDimensionsHead1);
            this.ImageModeTab.Controls.Add(this.txtbFileNameHead4);
            this.ImageModeTab.Controls.Add(this.txtbFileNameHead3);
            this.ImageModeTab.Controls.Add(this.txtbFileNameHead2);
            this.ImageModeTab.Controls.Add(this.txtbFileNameHead1);
            this.ImageModeTab.Controls.Add(this.lbFileNameHead4);
            this.ImageModeTab.Controls.Add(this.lbFileNameHead3);
            this.ImageModeTab.Controls.Add(this.lbFileNameHead2);
            this.ImageModeTab.Controls.Add(this.lbFileNameHead1);
            this.ImageModeTab.Controls.Add(this.lbImageHead4);
            this.ImageModeTab.Controls.Add(this.lbImageHead3);
            this.ImageModeTab.Controls.Add(this.lbImageHead2);
            this.ImageModeTab.Controls.Add(this.lbImageHead1);
            this.ImageModeTab.Controls.Add(this.pictureBox4);
            this.ImageModeTab.Controls.Add(this.pictureBox3);
            this.ImageModeTab.Controls.Add(this.pictureBox2);
            this.ImageModeTab.Controls.Add(this.pictureBox1);
            this.ImageModeTab.Location = new System.Drawing.Point(4, 22);
            this.ImageModeTab.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ImageModeTab.Name = "ImageModeTab";
            this.ImageModeTab.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ImageModeTab.Size = new System.Drawing.Size(788, 391);
            this.ImageModeTab.TabIndex = 2;
            this.ImageModeTab.Text = "Image";
            this.ImageModeTab.UseVisualStyleBackColor = true;
            // 
            // txtbPrintStatus
            // 
            this.txtbPrintStatus.Location = new System.Drawing.Point(663, 65);
            this.txtbPrintStatus.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtbPrintStatus.Name = "txtbPrintStatus";
            this.txtbPrintStatus.ReadOnly = true;
            this.txtbPrintStatus.Size = new System.Drawing.Size(114, 20);
            this.txtbPrintStatus.TabIndex = 62;
            this.txtbPrintStatus.Text = "--Print Status--";
            // 
            // txtbCurrentStepperPosition
            // 
            this.txtbCurrentStepperPosition.Location = new System.Drawing.Point(423, 64);
            this.txtbCurrentStepperPosition.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtbCurrentStepperPosition.Name = "txtbCurrentStepperPosition";
            this.txtbCurrentStepperPosition.ReadOnly = true;
            this.txtbCurrentStepperPosition.Size = new System.Drawing.Size(114, 20);
            this.txtbCurrentStepperPosition.TabIndex = 61;
            // 
            // lbCurrentStepperPosition
            // 
            this.lbCurrentStepperPosition.AutoSize = true;
            this.lbCurrentStepperPosition.Location = new System.Drawing.Point(291, 64);
            this.lbCurrentStepperPosition.Name = "lbCurrentStepperPosition";
            this.lbCurrentStepperPosition.Size = new System.Drawing.Size(124, 13);
            this.lbCurrentStepperPosition.TabIndex = 60;
            this.lbCurrentStepperPosition.Text = "Current Stepper Position:";
            // 
            // nudSetPosition
            // 
            this.nudSetPosition.Location = new System.Drawing.Point(369, 6);
            this.nudSetPosition.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.nudSetPosition.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nudSetPosition.Name = "nudSetPosition";
            this.nudSetPosition.Size = new System.Drawing.Size(80, 20);
            this.nudSetPosition.TabIndex = 59;
            this.nudSetPosition.Tag = "nudSetStartPosition";
            this.nudSetPosition.ValueChanged += new System.EventHandler(this.nudSetPosition_ValueChanged);
            // 
            // txtbCurrentEncoderPosition
            // 
            this.txtbCurrentEncoderPosition.Location = new System.Drawing.Point(423, 31);
            this.txtbCurrentEncoderPosition.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtbCurrentEncoderPosition.Name = "txtbCurrentEncoderPosition";
            this.txtbCurrentEncoderPosition.ReadOnly = true;
            this.txtbCurrentEncoderPosition.Size = new System.Drawing.Size(114, 20);
            this.txtbCurrentEncoderPosition.TabIndex = 58;
            // 
            // lbSetPosition
            // 
            this.lbSetPosition.AutoSize = true;
            this.lbSetPosition.Location = new System.Drawing.Point(291, 8);
            this.lbSetPosition.Name = "lbSetPosition";
            this.lbSetPosition.Size = new System.Drawing.Size(66, 13);
            this.lbSetPosition.TabIndex = 57;
            this.lbSetPosition.Text = "Set Position:";
            // 
            // lbCurrentEncoderPosition
            // 
            this.lbCurrentEncoderPosition.AutoSize = true;
            this.lbCurrentEncoderPosition.Location = new System.Drawing.Point(291, 33);
            this.lbCurrentEncoderPosition.Name = "lbCurrentEncoderPosition";
            this.lbCurrentEncoderPosition.Size = new System.Drawing.Size(127, 13);
            this.lbCurrentEncoderPosition.TabIndex = 56;
            this.lbCurrentEncoderPosition.Text = "Current Encoder Position:";
            // 
            // txtbImageHeadStatus
            // 
            this.txtbImageHeadStatus.Location = new System.Drawing.Point(663, 8);
            this.txtbImageHeadStatus.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtbImageHeadStatus.Name = "txtbImageHeadStatus";
            this.txtbImageHeadStatus.ReadOnly = true;
            this.txtbImageHeadStatus.Size = new System.Drawing.Size(114, 20);
            this.txtbImageHeadStatus.TabIndex = 55;
            this.txtbImageHeadStatus.Text = "--Head Status--";
            // 
            // cdPDdirection
            // 
            this.cdPDdirection.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::DriverBoardDropwatcher.Properties.Settings.Default, "pd_direction", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cdPDdirection.FormattingEnabled = true;
            this.cdPDdirection.Items.AddRange(new object[] {
            "Continuous",
            "Single"});
            this.cdPDdirection.Location = new System.Drawing.Point(119, 64);
            this.cdPDdirection.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cdPDdirection.Name = "cdPDdirection";
            this.cdPDdirection.Size = new System.Drawing.Size(110, 21);
            this.cdPDdirection.TabIndex = 54;
            this.cdPDdirection.Text = global::DriverBoardDropwatcher.Properties.Settings.Default.pd_direction;
            this.cdPDdirection.SelectedIndexChanged += new System.EventHandler(this.pdDirection_SelectedIndexChanged);
            // 
            // lbPDdirection
            // 
            this.lbPDdirection.AutoSize = true;
            this.lbPDdirection.Location = new System.Drawing.Point(47, 65);
            this.lbPDdirection.Name = "lbPDdirection";
            this.lbPDdirection.Size = new System.Drawing.Size(70, 13);
            this.lbPDdirection.TabIndex = 53;
            this.lbPDdirection.Text = "PD Direction:";
            // 
            // cbEncoderTrackedPosition
            // 
            this.cbEncoderTrackedPosition.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::DriverBoardDropwatcher.Properties.Settings.Default, "Encoder_TrackedPosition", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cbEncoderTrackedPosition.FormattingEnabled = true;
            this.cbEncoderTrackedPosition.Items.AddRange(new object[] {
            "Normal",
            "Reverse"});
            this.cbEncoderTrackedPosition.Location = new System.Drawing.Point(181, 88);
            this.cbEncoderTrackedPosition.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cbEncoderTrackedPosition.Name = "cbEncoderTrackedPosition";
            this.cbEncoderTrackedPosition.Size = new System.Drawing.Size(110, 21);
            this.cbEncoderTrackedPosition.TabIndex = 52;
            this.cbEncoderTrackedPosition.Text = global::DriverBoardDropwatcher.Properties.Settings.Default.Encoder_TrackedPosition;
            this.cbEncoderTrackedPosition.SelectedIndexChanged += new System.EventHandler(this.EncoderTrackedPositionSelection_SelectedIndexChanged);
            // 
            // lbEncoderTrackedPosition
            // 
            this.lbEncoderTrackedPosition.AutoSize = true;
            this.lbEncoderTrackedPosition.Location = new System.Drawing.Point(47, 90);
            this.lbEncoderTrackedPosition.Name = "lbEncoderTrackedPosition";
            this.lbEncoderTrackedPosition.Size = new System.Drawing.Size(133, 13);
            this.lbEncoderTrackedPosition.TabIndex = 51;
            this.lbEncoderTrackedPosition.Text = "Encoder Tracked Position:";
            // 
            // cbPDpolarity
            // 
            this.cbPDpolarity.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::DriverBoardDropwatcher.Properties.Settings.Default, "pdPolarity", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cbPDpolarity.FormattingEnabled = true;
            this.cbPDpolarity.Items.AddRange(new object[] {
            "High",
            "Low"});
            this.cbPDpolarity.Location = new System.Drawing.Point(111, 33);
            this.cbPDpolarity.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cbPDpolarity.Name = "cbPDpolarity";
            this.cbPDpolarity.Size = new System.Drawing.Size(110, 21);
            this.cbPDpolarity.TabIndex = 50;
            this.cbPDpolarity.Text = global::DriverBoardDropwatcher.Properties.Settings.Default.pdPolarity;
            this.cbPDpolarity.SelectedIndexChanged += new System.EventHandler(this.PD_Polarity_SelectedIndexChanged);
            // 
            // lbPDpolarity
            // 
            this.lbPDpolarity.AutoSize = true;
            this.lbPDpolarity.Location = new System.Drawing.Point(47, 35);
            this.lbPDpolarity.Name = "lbPDpolarity";
            this.lbPDpolarity.Size = new System.Drawing.Size(62, 13);
            this.lbPDpolarity.TabIndex = 49;
            this.lbPDpolarity.Text = "PD Polarity:";
            // 
            // lbImageMode
            // 
            this.lbImageMode.AutoSize = true;
            this.lbImageMode.Location = new System.Drawing.Point(47, 8);
            this.lbImageMode.Name = "lbImageMode";
            this.lbImageMode.Size = new System.Drawing.Size(37, 13);
            this.lbImageMode.TabIndex = 48;
            this.lbImageMode.Text = "Mode:";
            // 
            // cbImageMode
            // 
            this.cbImageMode.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::DriverBoardDropwatcher.Properties.Settings.Default, "ImageMode", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cbImageMode.FormattingEnabled = true;
            this.cbImageMode.Items.AddRange(new object[] {
            "Stepper Motor",
            "Quadrature Encoder",
            "HW PD"});
            this.cbImageMode.Location = new System.Drawing.Point(87, 6);
            this.cbImageMode.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cbImageMode.Name = "cbImageMode";
            this.cbImageMode.Size = new System.Drawing.Size(159, 21);
            this.cbImageMode.TabIndex = 47;
            this.cbImageMode.Text = global::DriverBoardDropwatcher.Properties.Settings.Default.ImageMode;
            this.cbImageMode.DropDown += new System.EventHandler(this.ImageModeSelection_SelectedIndexChanged);
            this.cbImageMode.SelectedIndexChanged += new System.EventHandler(this.ImageModeSelection_SelectedIndexChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCancel.Location = new System.Drawing.Point(699, 34);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(81, 23);
            this.btnCancel.TabIndex = 46;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnPrintImage
            // 
            this.btnPrintImage.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnPrintImage.Location = new System.Drawing.Point(605, 34);
            this.btnPrintImage.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.btnPrintImage.Name = "btnPrintImage";
            this.btnPrintImage.Size = new System.Drawing.Size(81, 23);
            this.btnPrintImage.TabIndex = 45;
            this.btnPrintImage.Text = "Print";
            this.btnPrintImage.UseVisualStyleBackColor = true;
            this.btnPrintImage.Click += new System.EventHandler(this.btnPrintImage_Click);
            // 
            // txtbDimensionsHead4
            // 
            this.txtbDimensionsHead4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtbDimensionsHead4.Location = new System.Drawing.Point(585, 368);
            this.txtbDimensionsHead4.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtbDimensionsHead4.Name = "txtbDimensionsHead4";
            this.txtbDimensionsHead4.ReadOnly = true;
            this.txtbDimensionsHead4.Size = new System.Drawing.Size(151, 20);
            this.txtbDimensionsHead4.TabIndex = 23;
            this.txtbDimensionsHead4.Tag = "DimensionsHead4";
            // 
            // lbDimensionsHead4
            // 
            this.lbDimensionsHead4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbDimensionsHead4.AutoSize = true;
            this.lbDimensionsHead4.Location = new System.Drawing.Point(611, 353);
            this.lbDimensionsHead4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbDimensionsHead4.Name = "lbDimensionsHead4";
            this.lbDimensionsHead4.Size = new System.Drawing.Size(102, 13);
            this.lbDimensionsHead4.TabIndex = 22;
            this.lbDimensionsHead4.Text = "Dimensions in Pixels";
            // 
            // txtbDimensionsHead3
            // 
            this.txtbDimensionsHead3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtbDimensionsHead3.Location = new System.Drawing.Point(403, 368);
            this.txtbDimensionsHead3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtbDimensionsHead3.Name = "txtbDimensionsHead3";
            this.txtbDimensionsHead3.ReadOnly = true;
            this.txtbDimensionsHead3.Size = new System.Drawing.Size(151, 20);
            this.txtbDimensionsHead3.TabIndex = 21;
            this.txtbDimensionsHead3.Tag = "DimensionsHead3";
            // 
            // lbDimensionsHead3
            // 
            this.lbDimensionsHead3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbDimensionsHead3.AutoSize = true;
            this.lbDimensionsHead3.Location = new System.Drawing.Point(425, 353);
            this.lbDimensionsHead3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbDimensionsHead3.Name = "lbDimensionsHead3";
            this.lbDimensionsHead3.Size = new System.Drawing.Size(102, 13);
            this.lbDimensionsHead3.TabIndex = 20;
            this.lbDimensionsHead3.Text = "Dimensions in Pixels";
            // 
            // txtbDimensionsHead2
            // 
            this.txtbDimensionsHead2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtbDimensionsHead2.Location = new System.Drawing.Point(226, 368);
            this.txtbDimensionsHead2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtbDimensionsHead2.Name = "txtbDimensionsHead2";
            this.txtbDimensionsHead2.ReadOnly = true;
            this.txtbDimensionsHead2.Size = new System.Drawing.Size(151, 20);
            this.txtbDimensionsHead2.TabIndex = 19;
            this.txtbDimensionsHead2.Tag = "DimensionsHead2";
            // 
            // lbDimensionsHead2
            // 
            this.lbDimensionsHead2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbDimensionsHead2.AutoSize = true;
            this.lbDimensionsHead2.Location = new System.Drawing.Point(246, 353);
            this.lbDimensionsHead2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbDimensionsHead2.Name = "lbDimensionsHead2";
            this.lbDimensionsHead2.Size = new System.Drawing.Size(102, 13);
            this.lbDimensionsHead2.TabIndex = 18;
            this.lbDimensionsHead2.Text = "Dimensions in Pixels";
            // 
            // txtbDimensionsHead1
            // 
            this.txtbDimensionsHead1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtbDimensionsHead1.Location = new System.Drawing.Point(46, 368);
            this.txtbDimensionsHead1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtbDimensionsHead1.Name = "txtbDimensionsHead1";
            this.txtbDimensionsHead1.ReadOnly = true;
            this.txtbDimensionsHead1.Size = new System.Drawing.Size(151, 20);
            this.txtbDimensionsHead1.TabIndex = 17;
            this.txtbDimensionsHead1.Tag = "DimensionsHead1";
            // 
            // lbDimensionsHead1
            // 
            this.lbDimensionsHead1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbDimensionsHead1.AutoSize = true;
            this.lbDimensionsHead1.Location = new System.Drawing.Point(69, 353);
            this.lbDimensionsHead1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbDimensionsHead1.Name = "lbDimensionsHead1";
            this.lbDimensionsHead1.Size = new System.Drawing.Size(102, 13);
            this.lbDimensionsHead1.TabIndex = 16;
            this.lbDimensionsHead1.Text = "Dimensions in Pixels";
            // 
            // txtbFileNameHead4
            // 
            this.txtbFileNameHead4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtbFileNameHead4.Location = new System.Drawing.Point(585, 329);
            this.txtbFileNameHead4.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtbFileNameHead4.Name = "txtbFileNameHead4";
            this.txtbFileNameHead4.ReadOnly = true;
            this.txtbFileNameHead4.Size = new System.Drawing.Size(151, 20);
            this.txtbFileNameHead4.TabIndex = 15;
            this.txtbFileNameHead4.Tag = "FileNameHead4";
            // 
            // txtbFileNameHead3
            // 
            this.txtbFileNameHead3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtbFileNameHead3.Location = new System.Drawing.Point(403, 329);
            this.txtbFileNameHead3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtbFileNameHead3.Name = "txtbFileNameHead3";
            this.txtbFileNameHead3.ReadOnly = true;
            this.txtbFileNameHead3.Size = new System.Drawing.Size(151, 20);
            this.txtbFileNameHead3.TabIndex = 14;
            this.txtbFileNameHead3.Tag = "FileNameHead3";
            // 
            // txtbFileNameHead2
            // 
            this.txtbFileNameHead2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtbFileNameHead2.Location = new System.Drawing.Point(226, 329);
            this.txtbFileNameHead2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtbFileNameHead2.Name = "txtbFileNameHead2";
            this.txtbFileNameHead2.ReadOnly = true;
            this.txtbFileNameHead2.Size = new System.Drawing.Size(151, 20);
            this.txtbFileNameHead2.TabIndex = 13;
            this.txtbFileNameHead2.Tag = "FileNameHead2";
            // 
            // txtbFileNameHead1
            // 
            this.txtbFileNameHead1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtbFileNameHead1.Location = new System.Drawing.Point(46, 329);
            this.txtbFileNameHead1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtbFileNameHead1.Name = "txtbFileNameHead1";
            this.txtbFileNameHead1.ReadOnly = true;
            this.txtbFileNameHead1.Size = new System.Drawing.Size(151, 20);
            this.txtbFileNameHead1.TabIndex = 12;
            this.txtbFileNameHead1.Tag = "FileNameHead1";
            // 
            // lbFileNameHead4
            // 
            this.lbFileNameHead4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbFileNameHead4.AutoSize = true;
            this.lbFileNameHead4.Location = new System.Drawing.Point(635, 314);
            this.lbFileNameHead4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbFileNameHead4.Name = "lbFileNameHead4";
            this.lbFileNameHead4.Size = new System.Drawing.Size(57, 13);
            this.lbFileNameHead4.TabIndex = 11;
            this.lbFileNameHead4.Text = "File Name:";
            // 
            // lbFileNameHead3
            // 
            this.lbFileNameHead3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbFileNameHead3.AutoSize = true;
            this.lbFileNameHead3.Location = new System.Drawing.Point(453, 314);
            this.lbFileNameHead3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbFileNameHead3.Name = "lbFileNameHead3";
            this.lbFileNameHead3.Size = new System.Drawing.Size(57, 13);
            this.lbFileNameHead3.TabIndex = 10;
            this.lbFileNameHead3.Text = "File Name:";
            // 
            // lbFileNameHead2
            // 
            this.lbFileNameHead2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbFileNameHead2.AutoSize = true;
            this.lbFileNameHead2.Location = new System.Drawing.Point(278, 314);
            this.lbFileNameHead2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbFileNameHead2.Name = "lbFileNameHead2";
            this.lbFileNameHead2.Size = new System.Drawing.Size(57, 13);
            this.lbFileNameHead2.TabIndex = 9;
            this.lbFileNameHead2.Text = "File Name:";
            // 
            // lbFileNameHead1
            // 
            this.lbFileNameHead1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbFileNameHead1.AutoSize = true;
            this.lbFileNameHead1.Location = new System.Drawing.Point(95, 314);
            this.lbFileNameHead1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbFileNameHead1.Name = "lbFileNameHead1";
            this.lbFileNameHead1.Size = new System.Drawing.Size(57, 13);
            this.lbFileNameHead1.TabIndex = 8;
            this.lbFileNameHead1.Text = "File Name:";
            // 
            // lbImageHead4
            // 
            this.lbImageHead4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbImageHead4.AutoSize = true;
            this.lbImageHead4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lbImageHead4.Location = new System.Drawing.Point(636, 133);
            this.lbImageHead4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbImageHead4.Name = "lbImageHead4";
            this.lbImageHead4.Size = new System.Drawing.Size(90, 29);
            this.lbImageHead4.TabIndex = 7;
            this.lbImageHead4.Text = "Head 4";
            // 
            // lbImageHead3
            // 
            this.lbImageHead3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbImageHead3.AutoSize = true;
            this.lbImageHead3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lbImageHead3.Location = new System.Drawing.Point(453, 133);
            this.lbImageHead3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbImageHead3.Name = "lbImageHead3";
            this.lbImageHead3.Size = new System.Drawing.Size(90, 29);
            this.lbImageHead3.TabIndex = 6;
            this.lbImageHead3.Text = "Head 3";
            // 
            // lbImageHead2
            // 
            this.lbImageHead2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbImageHead2.AutoSize = true;
            this.lbImageHead2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lbImageHead2.Location = new System.Drawing.Point(272, 133);
            this.lbImageHead2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbImageHead2.Name = "lbImageHead2";
            this.lbImageHead2.Size = new System.Drawing.Size(90, 29);
            this.lbImageHead2.TabIndex = 5;
            this.lbImageHead2.Text = "Head 2";
            // 
            // lbImageHead1
            // 
            this.lbImageHead1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbImageHead1.AutoSize = true;
            this.lbImageHead1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lbImageHead1.Location = new System.Drawing.Point(94, 133);
            this.lbImageHead1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbImageHead1.Name = "lbImageHead1";
            this.lbImageHead1.Size = new System.Drawing.Size(90, 29);
            this.lbImageHead1.TabIndex = 4;
            this.lbImageHead1.Text = "Head 1";
            // 
            // pictureBox4
            // 
            this.pictureBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox4.Location = new System.Drawing.Point(625, 154);
            this.pictureBox4.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(86, 148);
            this.pictureBox4.TabIndex = 3;
            this.pictureBox4.TabStop = false;
            this.pictureBox4.Tag = "ImageHead4";
            this.pictureBox4.Click += new System.EventHandler(this.pictureBox4_Click);
            // 
            // pictureBox3
            // 
            this.pictureBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox3.Location = new System.Drawing.Point(441, 154);
            this.pictureBox3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(86, 148);
            this.pictureBox3.TabIndex = 2;
            this.pictureBox3.TabStop = false;
            this.pictureBox3.Tag = "ImageHead3";
            this.pictureBox3.Click += new System.EventHandler(this.pictureBox3_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox2.Location = new System.Drawing.Point(262, 154);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(86, 148);
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Tag = "ImageHead2";
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox1.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.InitialImage")));
            this.pictureBox1.Location = new System.Drawing.Point(85, 159);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(86, 148);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Tag = "ImageHead1";
            this.pictureBox1.WaitOnLoad = true;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.txtbJSONview);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPage1.Size = new System.Drawing.Size(788, 391);
            this.tabPage1.TabIndex = 3;
            this.tabPage1.Text = "View JSON";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // txtbJSONview
            // 
            this.txtbJSONview.Location = new System.Drawing.Point(13, 14);
            this.txtbJSONview.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtbJSONview.Name = "txtbJSONview";
            this.txtbJSONview.ReadOnly = true;
            this.txtbJSONview.Size = new System.Drawing.Size(772, 372);
            this.txtbJSONview.TabIndex = 0;
            this.txtbJSONview.Text = "";
            // 
            // nudFrequency
            // 
            this.nudFrequency.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::DriverBoardDropwatcher.Properties.Settings.Default, "Frequency", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.nudFrequency.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.nudFrequency.Location = new System.Drawing.Point(141, 147);
            this.nudFrequency.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.nudFrequency.Maximum = new decimal(new int[] {
            8000,
            0,
            0,
            0});
            this.nudFrequency.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudFrequency.Name = "nudFrequency";
            this.nudFrequency.Size = new System.Drawing.Size(78, 30);
            this.nudFrequency.TabIndex = 28;
            this.nudFrequency.Tag = "nudFrequency";
            this.nudFrequency.Value = global::DriverBoardDropwatcher.Properties.Settings.Default.Frequency;
            this.nudFrequency.ValueChanged += new System.EventHandler(this.frequencyValue_ValueChanged);
            // 
            // cbSerialPort
            // 
            this.cbSerialPort.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::DriverBoardDropwatcher.Properties.Settings.Default, "Serial_Port", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cbSerialPort.FormattingEnabled = true;
            this.cbSerialPort.Location = new System.Drawing.Point(13, 8);
            this.cbSerialPort.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.cbSerialPort.Name = "cbSerialPort";
            this.cbSerialPort.Size = new System.Drawing.Size(133, 21);
            this.cbSerialPort.TabIndex = 0;
            this.cbSerialPort.Text = global::DriverBoardDropwatcher.Properties.Settings.Default.Serial_Port;
            this.cbSerialPort.DropDown += new System.EventHandler(this.cbSerialPort_DropDown);
            this.cbSerialPort.SelectedIndexChanged += new System.EventHandler(this.cbSerialPort_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(823, 617);
            this.Controls.Add(this.lbFrequency);
            this.Controls.Add(this.StatusTable);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.tcDropWatchingAndImageModes);
            this.Controls.Add(this.txtbBoardUpTime);
            this.Controls.Add(this.lbTimeOn);
            this.Controls.Add(this.txtbStatusBox);
            this.Controls.Add(this.lbStatus);
            this.Controls.Add(this.ChbxPower);
            this.Controls.Add(this.btnPowerOnOff);
            this.Controls.Add(this.ChbxIsConnected);
            this.Controls.Add(this.btnConnectDisconnect);
            this.Controls.Add(this.nudFrequency);
            this.Controls.Add(this.cbSerialPort);
            this.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Driver Board";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudTemperatureHead1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudVoltageHead1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTemperatureHead2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudVoltageHead2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTemperatureHead3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudVoltageHead3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTemperatureHead4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudVoltageHead4)).EndInit();
            this.StatusTable.ResumeLayout(false);
            this.StatusTable.PerformLayout();
            this.FillCycleBox.ResumeLayout(false);
            this.FillCycleBox.PerformLayout();
            this.tcDropWatchingAndImageModes.ResumeLayout(false);
            this.DropWatchingTab.ResumeLayout(false);
            this.DropWatchingTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudGap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSpan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudNozzle)).EndInit();
            this.ImageModeTab.ResumeLayout(false);
            this.ImageModeTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSetPosition)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudFrequency)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbSerialPort;
        private System.Windows.Forms.Button btnConnectDisconnect;
        private System.Windows.Forms.CheckBox ChbxIsConnected;
        private System.Windows.Forms.Button btnPowerOnOff;
        private System.Windows.Forms.CheckBox ChbxPower;
        private System.Windows.Forms.Label lbStatus;
        private System.Windows.Forms.TextBox txtbStatusBox;
        private System.Windows.Forms.Label lbTimeOn;
        private System.Windows.Forms.TextBox txtbBoardUpTime;
        private System.Windows.Forms.TextBox txtbTemperatureOutput1;
        private System.Windows.Forms.NumericUpDown nudTemperatureHead1;
        private System.Windows.Forms.NumericUpDown nudVoltageHead1;
        private System.Windows.Forms.TextBox txtbTemperatureOutput2;
        private System.Windows.Forms.NumericUpDown nudTemperatureHead2;
        private System.Windows.Forms.NumericUpDown nudVoltageHead2;
        private System.Windows.Forms.TextBox txtbTemperatureOutput3;
        private System.Windows.Forms.NumericUpDown nudTemperatureHead3;
        private System.Windows.Forms.NumericUpDown nudVoltageHead3;
        private System.Windows.Forms.TextBox txtbTemperatureOutput4;
        private System.Windows.Forms.NumericUpDown nudTemperatureHead4;
        private System.Windows.Forms.NumericUpDown nudVoltageHead4;
        private System.Windows.Forms.TextBox txtbHeadStatus2;
        private System.Windows.Forms.TextBox txtbHeadStatus3;
        private System.Windows.Forms.TextBox txtbHeadStatus1;
        private System.Windows.Forms.TabControl tcDropWatchingAndImageModes;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.TabPage DropWatchingTab;
        private System.Windows.Forms.NumericUpDown nudFrequency;
        private System.Windows.Forms.NumericUpDown nudNozzle;
        private System.Windows.Forms.ComboBox cbDropWatchHeadSelection;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.ComboBox cbDropWatchMode;
        private System.Windows.Forms.TabPage ImageModeTab;
        private System.Windows.Forms.NumericUpDown nudSpan;
        private System.Windows.Forms.Label lbHeadIndex;
        private System.Windows.Forms.Label lbDropWatchingMode;
        private System.Windows.Forms.Label lbDropWatchingSpan;
        private System.Windows.Forms.Label lbDropWatchingNozzle;
        private System.Windows.Forms.Button btnFillCycleA;
        private System.Windows.Forms.TableLayoutPanel StatusTable;
        private System.Windows.Forms.Label lbCurrentTemperature;
        private System.Windows.Forms.Label lbSetTemperature;
        private System.Windows.Forms.Label lbHead;
        private System.Windows.Forms.Label lbSetVoltage;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.Label lbHeadStatus;
        private System.Windows.Forms.TextBox txtbPrintCounter4;
        private System.Windows.Forms.Label lbPrintCount;
        private System.Windows.Forms.TextBox txtbPrintCounter1;
        private System.Windows.Forms.TextBox txtbPrintCounter2;
        private System.Windows.Forms.TextBox txtbPrintCounter3;
        private System.Windows.Forms.Label lbFrequency;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label lbImageHead4;
        private System.Windows.Forms.Label lbImageHead3;
        private System.Windows.Forms.Label lbImageHead2;
        private System.Windows.Forms.Label lbImageHead1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox txtbFileNameHead4;
        private System.Windows.Forms.TextBox txtbFileNameHead3;
        private System.Windows.Forms.TextBox txtbFileNameHead2;
        private System.Windows.Forms.TextBox txtbFileNameHead1;
        private System.Windows.Forms.Label lbFileNameHead4;
        private System.Windows.Forms.Label lbFileNameHead3;
        private System.Windows.Forms.Label lbFileNameHead2;
        private System.Windows.Forms.Label lbFileNameHead1;
        private System.Windows.Forms.TextBox txtbDimensionsHead4;
        private System.Windows.Forms.Label lbDimensionsHead4;
        private System.Windows.Forms.TextBox txtbDimensionsHead3;
        private System.Windows.Forms.Label lbDimensionsHead3;
        private System.Windows.Forms.TextBox txtbDimensionsHead2;
        private System.Windows.Forms.Label lbDimensionsHead2;
        private System.Windows.Forms.TextBox txtbDimensionsHead1;
        private System.Windows.Forms.Label lbDimensionsHead1;
        private System.Windows.Forms.TextBox txtbHeadStatus4;
        private System.Windows.Forms.Label lbDropWatchingFrequencyDuplicate;
        private System.Windows.Forms.TextBox txtbFrequencyDuplicate;
        private System.Windows.Forms.Button btnClearHead;
        private System.Windows.Forms.Button btnPrintImage;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnFillNozzle;
        private System.Windows.Forms.Button btnFillSpan;
        private System.Windows.Forms.Label lbImageMode;
        private System.Windows.Forms.ComboBox cbImageMode;
        private System.Windows.Forms.TextBox txtbHeadStatus;
        private System.Windows.Forms.Label lbDropWatchingFillCycle;
        private System.Windows.Forms.Button btnFillCycleC;
        private System.Windows.Forms.Button btnFillCycleB;
        private System.Windows.Forms.GroupBox FillCycleBox;
        private System.Windows.Forms.Button btnFillGap;
        private System.Windows.Forms.NumericUpDown nudGap;
        private System.Windows.Forms.Label lbDropWatchingGap;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnFillHead;
        private System.Windows.Forms.ComboBox cbPDpolarity;
        private System.Windows.Forms.Label lbPDpolarity;
        private System.Windows.Forms.ComboBox cbEncoderTrackedPosition;
        private System.Windows.Forms.Label lbEncoderTrackedPosition;
        private System.Windows.Forms.ComboBox cdPDdirection;
        private System.Windows.Forms.Label lbPDdirection;
        private System.Windows.Forms.CheckBox chbxIsFillSpan;
        private System.Windows.Forms.CheckBox chbxIsFillGap;
        private System.Windows.Forms.CheckBox chbxIsFillHead;
        private System.Windows.Forms.CheckBox chbxIsFillNozzle;
        private System.Windows.Forms.TextBox txtbImageHeadStatus;
        private System.Windows.Forms.TextBox txtbCurrentEncoderPosition;
        private System.Windows.Forms.Label lbSetPosition;
        private System.Windows.Forms.Label lbCurrentEncoderPosition;
        private System.Windows.Forms.NumericUpDown nudSetPosition;
        private System.Windows.Forms.TextBox txtbCurrentStepperPosition;
        private System.Windows.Forms.Label lbCurrentStepperPosition;
        private System.Windows.Forms.TextBox txtbNozzleSpanStatusBox;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.RichTextBox txtbJSONview;
        private System.Windows.Forms.TextBox txtbPrintStatus;
    }
}

