/******************************************************************************************
    Title:              LP50 Standard cure script
    Author:             GJ/AE/SD PixDro BV
    Date:               09 July 2009
    Description:        This script performs a default cure job on the LP50 inkjet printer. 
    Version:            $Revision: 4 $
*******************************************************************************************/

/*
50 - Xaar single (40)
51 - Xaar dual   (40)
52 - Xaar tripple(40)
54 - Xaar single (80)
55 - Xaar dual   (80)
56 - Xaar tripple(80)

60 - Dimatix (Samba)
61 - Dimatix (10 pL)
99 - single nozzle (shouldnt call this script)
*/
public void Initialize()
{
}

public static double EncoderDividerSet = 0.0064;
public static string currentFile;
public static double print_length = 1000; //Temporry values, overwritten when loading file
public static int printHeadNPI = 0;
public static string[] headIndexes = {"_A","_B","_C","_D"};
public static int currentHeadIndex = 1;
public static bool UVInUse = false;
public static string UVComPort = "COM21";
public static int UVComBaud = 9600;
public static string UVOnString = "N";
public static string UVOffString = "F";
public static string UVPowerString = "I 5\n\r"; //its out of 255

//This script is run instead of the normal printing script if it detects that a non-standard head is used
//This is detected in standard-print.cs

//The calling script has called the data generation script.
//This script reviews a folder to find all print data.
//It counts them up into an array and keeps track if there is 2 files instead of one (indicates dual)
//Also then runs the files, and checks if they are A or B
//Then loads files and sends them to board


public void Execute()
{
    //Check if UV curing exists
    if(Parameters.HasParameter("Recipe.UseUV")){
        if(Parameters.GetIntValue("Recipe.UseUV") == 1)
        {
            UVInUse = true;
			UVInUse = false;
        }
    }

    //check PH power
    turnOnPrintHead(); // only turns on if already off
	
	//Check head NPI
	printHeadNPI = getPrintheadNPI();

	//Determine start location
    double PR_X = Positions.Get("PrintReference[0]").GetAxisValue(Position.Axes.X);
    double PR_Y = Positions.Get("PrintReference[0]").GetAxisValue(Position.Axes.Y);   
    double ASP_X = Positions.Get("AlignedStartPosition[0]").GetAxisValue(Position.Axes.X);
    double ASP_Y = Positions.Get("AlignedStartPosition[0]").GetAxisValue(Position.Axes.Y);   
    double LEADIN = Parameters.GetIntValue("Motion.RunInLength[0]"); 
    double HAO_X = Positions.Get("HeadAssy.Offset[0]").GetAxisValue(Position.Axes.X);
    double HAO_Y = Positions.Get("HeadAssy.Offset[0]").GetAxisValue(Position.Axes.Y);
	
    double x_offset = PR_X + ASP_X + HAO_X;// - calculateXOffset();
    double y_offset = PR_Y - ASP_Y + HAO_Y + LEADIN;// - calculateYOffset();

    double layer_printed = (double)Parameters.GetIntValue("Recipe.Layer") * 0.017;

    double z_printheight = 5.0 	+ Positions.Get("SubstrateThickness[0]").GetAxisValue(Position.Axes.Z)
								+ Positions.Get("HeadAssy.Offset[0]").GetAxisValue(Position.Axes.Z)
                                + layer_printed;

    int runout = Parameters.GetIntValue("Motion.RunOutLength[0]");
    if(UVInUse){
        if(runout < 100){
            Helper.GenerateScriptWarning("UV Curing", "The movement to get light in place not correct, change RunOut."); 
        }
    }

	//Get print data
    string datafolder = @"C:\LP50\PrintGen\Output Files\";
    double print_speed = (double)Parameters.GetIntValue("Motion.PrintSpeed[0]");
    string[] files = Directory.GetFiles(datafolder);
    List<double> startLocations = new List<double>();
	
    foreach (string file in files)
    {
        Logger.Debug("Found: " + file);
        if (String.Equals(file.Substring(file.Length - 8), "printDat")) //Load CSV regardless of which heads - we check later
        {
            Logger.Debug("Data: " + file);
			//Taking the file name conversion
			int fiLenght = file.Length;
			int csLenght = 6;
			int diLength = datafolder.Length;
            string start_ = file.Substring(diLength, csLenght);
			
            Logger.Debug("String slice: " + start_);
			//At this point we have the X start location for each pass
			double star_ = double.Parse(start_);
			
			//Only add unique starts
			if(startLocations.Contains(star_))	{	}
			else
			{
                Logger.Debug("Start location added: " + star_.ToString());
				startLocations.Add(star_);
			}
        }
    }
	
	//Now have an array of start locations
	double[] startLocations_ = startLocations.ToArray();
	//Sort them so we are printing in consecutive order
    Array.Sort(startLocations_);

//To change this to create a sort algorithm with different methods
/*
    //*******************SIMON's CODE*******************
    // Splice Fws from B data and A Data to alternate rows of StartLocationsAll
    // StartLocationsAll is equivilant to  startLocations_
    //  DblArrayToMessageBox(startLocations_, "startLocations_");//Display Array to MessageBox
    //Get all Data
    List<double> StartLocationsB = new List<double>();
    List<double> StartLocationsA = new List<double>();
    foreach (string file in files)
    {                    
           if (String.Equals(file.Substring(file.Length - 5), "B.csv"))//Get B Data  
        {
            string PathNumberStr = file.Substring(datafolder.Length, file.Length-datafolder.Length-6);//6-get number only ("_B.csv"=6)            
            double PathNumberDbl = double.Parse(PathNumberStr);
            StartLocationsB.Add(PathNumberDbl);                        
        }          
        if (String.Equals(file.Substring(file.Length - 5), "A.csv"))// Get A Data
        {
            string PathNumberStr = file.Substring(datafolder.Length, file.Length-datafolder.Length-6);//6-get number only ("_B.csv"=6)            
            double PathNumberDbl = double.Parse(PathNumberStr);
            StartLocationsA.Add(PathNumberDbl);                        
        }          
    }
    double[] StartLocationsDblB = StartLocationsB.ToArray();//List --> double array
    Array.Sort(StartLocationsDblB);//sort data in increasing order
    double[] StartLocationsDblA = StartLocationsA.ToArray();//List --> double array
    Array.Sort(StartLocationsDblA);//sort data in increasing order
    
    //Create New array (StartLocationsAll) to contain A and B data
    double[] StartLocationsAll = new double[StartLocationsDblB.Length + StartLocationsDblA.Length];
    int Ntot = StartLocationsDblB.Length + StartLocationsDblA.Length;
    int incA = 0;
    int incB = 0;
    for (int i = 0; i < Ntot; i++)
    {
        if (IsOdd(i))
        {
            StartLocationsAll[i] = StartLocationsDblA[incA];
            incA++;        
        }    
        else
        {
            StartLocationsAll[i] = StartLocationsDblB[incB];
            incB++; 
        }            
    }
    // DblArrayToMessageBox(StartLocationsAll, "StartLocationsAll");//Display Array to MessageBox

    //Determine if Line-By-Line printing is required
    string LineByLine = Parameters.GetValue("Recipe.LineByLine[0]");
    if (String.Equals(Parameters.GetValue("Recipe.LineByLine[0]"), "Yes"))
    {    
        startLocations_ = StartLocationsAll;  
    }
    // MessageBox.Show("pause");  
    //***************************************************
    
*/

    //rotate print head to correct orientation
    rotateHead();
    SerialPort UVPort = new SerialPort(UVComPort, UVComBaud);
    if(UVInUse){
        UVPort.Open();
        Thread.Sleep(50);
        UVPort.Write(UVPowerString);
    }
	
    int pass_count = 1;
    foreach (double pos in startLocations_)
    {
        string partName = datafolder + pos.ToString("000000");
		
        AppendPass(pass_count, startLocations.Count);
		
        //move to start position
        Position startpos = new Position();

        startpos.Change(Position.Axes.X, x_offset + (pos / 1000.0));
        startpos.Change(Position.Axes.Y, y_offset);
        startpos.Change(Position.Axes.Z, z_printheight);
        Logger.Log("Moving to X = " + (x_offset + (pos / 1000.0).ToString()
                          + " Y = " + y_offset.ToString()
                          + " Z = " + z_printheight.ToString()));
        
        Motion.MoveTo(startpos);
	    Script.Run(Helper.GetScriptDir() + "Status.cs");
		bool dataSent = false; //to identify if data is sent
		int maxCounts = 3; // allowed maximum sends

        //for each index
            //check if a file exists at position
                //if file exists send it to head
                    //if data isnt sent right, resend
        //motion
		currentHeadIndex = 1;
		foreach(string headIdx in headIndexes){
            
			bool sendSuccess = false;
			int counts = 0;
            currentFile = partName + headIdx + ".png.printDat";
            Logger.Debug("Searching for file: " + currentFile);
            if(File.Exists(currentFile)){
				if (checkHeadPresence(currentHeadIndex) == false){
                    Helper.GenerateScriptError(currentFile, "Image loaded but head not found");
				}
            dataSent = true; //one of the files exist
			while(sendSuccess == false)
			{
                if (counts>0)
                {
                    Logger.Error("Data send retry " + counts.ToString());
                    Thread.Sleep(250);
                } 
				
                sendSuccess = encodePrintHead();
				counts++;
				if(counts > maxCounts){
		            Logger.Error("Error data send failed");
                    Helper.GenerateScriptError(currentFile, "Data send counts reached maximum = " + counts.ToString());
				}
			}
            }
            currentHeadIndex++;
			
            }

        //Do print move
        if(dataSent){
			
			logStatuses("printing pass");
			setVoltage();
			setTemperature();		
			while(checkTemperature()){
				Thread.Sleep(100);
			}
            Logger.Debug("Printing " + print_length.ToString());
            if(UVInUse){
                UVPort.Write(UVOnString);
            }
            Motion.RelMove(Position.Axes.Y, -print_length, print_speed, true);
						
            if(UVInUse){
                UVPort.Write(UVOffString);
            }
        }
        //if elements left repeat
        pass_count++;
	    Script.Run(Helper.GetScriptDir() + "Status.cs");
    }

    //Finished printing

    Parameters.SetValue("General.PrintCounter", Parameters.GetIntValue("General.PrintCounter") + 1);
    //Parameters.SetValue("General.ProgressText", "Print Finished");
    //turnOffPrintHead();
    if(UVInUse){
        UVPort.Close();
    }
}

public void rotateHead()
{
    double dspace = (25400.0 / (double)Parameters.GetIntValue("HeadAssy.DesiredNPI"));
	double hspace = (25400.0 / printHeadNPI);
    double angle = 90;
    if(dspace < hspace){        
    angle = 360.0 * Math.Acos(dspace / hspace) / (2.0 * Math.PI);
    Motion.AbsMove(Position.Axes.P, angle);
    }

    Logger.Debug("Rotating head to " + angle.ToString());

}

public double calculateYOffset()
{
    double dspace = (25400.0 / (double)Parameters.GetIntValue("HeadAssy.DesiredNPI"));
	double hspace = (25400.0 / printHeadNPI);
    double angle = Math.Acos(dspace / hspace);

    double offset = 18.2581 * Math.Sin(angle + 1.1408) - 9.9125;

    return 0;
}

public double calculateXOffset()
{
    double dspace = (25400.0 / (double)Parameters.GetIntValue("HeadAssy.DesiredNPI"));
	double hspace = (25400.0 / printHeadNPI);
    double angle = Math.Acos(dspace / hspace);

    double offset = 15.0166 * Math.Cos(angle + 1.6843) + 14.9413;

    return 0;
}

public bool checkHeadPresence(int headindex){
	string serial_port = Parameters.GetValue("HeadAssy.COMPORT");
    SerialPort port = new SerialPort(serial_port, 1000000);

    port.Open();
    string messageToSend = "B";
    messageToSend += "\r\n";
    port.Write(messageToSend);
    Logger.Debug(messageToSend);
    Thread.Sleep(50);
    //Check for any data on the port
    string s = port.ReadExisting();
    int drop_count = 0;
    while(s.Length < 1){
    s = port.ReadExisting();
    drop_count++;
    Thread.Sleep(1);
    if(drop_count > 1000){
        break;
    }
    }
    port.Close();
	
	string[] statuses = s.Split(':');
	
	string desiredIndex = statuses[headindex-1];

	
	bool result = desiredIndex.Contains("-2");
	if (result){
		return false;
	}	
		
	return true;
}


public void logStatuses(string appender){
	
    string serial_port = Parameters.GetValue("HeadAssy.COMPORT");
    SerialPort port = new SerialPort(serial_port, 1000000);

    port.Open();
    string messageToSend = "B";
    messageToSend += "\r\n";
    port.Write(messageToSend);
    Logger.Debug(messageToSend);
    Thread.Sleep(50);
    //Check for any data on the port
    string s = port.ReadExisting();
    int drop_count = 0;
    while(s.Length < 1){
    s = port.ReadExisting();
    drop_count++;
    Thread.Sleep(1);
    if(drop_count > 1000){
        break;
    }
    }
    Logger.Debug("RX " + s);
    port.Close();
	
	
	bool result = s.Contains("-3");
	if (result){
		resetPrintHead();
		Helper.GenerateScriptWarning("Status - reset", appender+s);
	}	
	
	Logger.Debug("Statuses", appender+s);
	
}

private void turnOffPrintHead(){
    string serial_port = Parameters.GetValue("HeadAssy.COMPORT");
    SerialPort port = new SerialPort(serial_port, 1000000);
    string messageToSend = "F";
    port.Open();
    port.WriteLine(messageToSend);
    Thread.Sleep(1);
    string reply = port.ReadExisting();
    port.Close();
}

private void resetPrintHead(){
    string serial_port = Parameters.GetValue("HeadAssy.COMPORT");
    SerialPort port = new SerialPort(serial_port, 1000000);

    string messageToSend = "r";
    port.Open();
    port.WriteLine(messageToSend);
    Thread.Sleep(1);
    string reply = port.ReadExisting();
    port.Close();
}

private void turnOnPrintHead(){
    
    string serial_port = Parameters.GetValue("HeadAssy.COMPORT");
    SerialPort port = new SerialPort(serial_port, 1000000);

    string messageToSend = "R";
    port.Open();
    string reply = port.ReadExisting();
    port.WriteLine(messageToSend);
    Thread.Sleep(10);
    reply = port.ReadExisting();
    port.Close();
    string[] subs = reply.Split(':');
	string powerString = subs[14];
	bool powerState = (powerString[1] == '1');

		if(powerState){
			Logger.Debug("Board already on.");
		}
		else{
			Logger.Debug("Board off, turning on");
            port.Open();
		    messageToSend = "O";
            port.WriteLine(messageToSend);
            Thread.Sleep(200); //boot sequence is about 150 ms
            reply = port.ReadExisting();
            port.Close();
		}
		
}

public bool encodePrintHead()
{
    
    string serial_port = Parameters.GetValue("HeadAssy.COMPORT");
    SerialPort port = new SerialPort(serial_port, 1000000);

    long lastValue = 0;
    long runningCount = 0;
    long dataLength = 0;
    
    //read all the data in
    byte[] readText = File.ReadAllBytes(currentFile);

    //compute our "CRC"
    foreach(byte dat in readText){
        lastValue = dat;
        runningCount+= dat;
        dataLength++;
    }

    //determine print pattern length
    double distance_a_pixel = 25.400 / Parameters.GetIntValue("Recipe.Y_Resolution[0]");
    print_length = (dataLength / 16) * distance_a_pixel ;
    print_length += Parameters.GetIntValue("Motion.RunInLength[0]"); //10 mm extra travel
    print_length += Parameters.GetIntValue("Motion.RunOutLength[0]"); //10 mm extra travel

    //send absolute start location
    int triggerCountsToStart = (int)(((Double)Parameters.GetIntValue("Motion.RunInLength[0]")) / EncoderDividerSet);
    string messageToSend = ",";
    messageToSend += (char)(100+currentHeadIndex); //set position for that head
    messageToSend += " " + triggerCountsToStart.ToString();
    messageToSend += "\r\n";
    
    port.Open();
    port.Write(messageToSend);
    Logger.Debug(messageToSend);
    Thread.Sleep(50);
    //Check for any data on the port
    string s = port.ReadExisting();
    int drop_count = 0;
    while(s.Length < 1){
    s = port.ReadExisting();
    drop_count++;
    Thread.Sleep(1);
    if(drop_count > 1000){
        break;
    }
    }
    Logger.Debug("RX " + s);
    port.Close();

    //make sure the head is clear
    port.Open();
    messageToSend = "C";
    messageToSend += "\r\n";
    port.Write(messageToSend);
    Logger.Debug(messageToSend);
    Thread.Sleep(50);
    //Check for any data on the port
    s = port.ReadExisting();
    drop_count = 0;
    while(s.Length < 1){
    s = port.ReadExisting();
    drop_count++;
    Thread.Sleep(1);
    if(drop_count > 1000){
        break;
    }
    }
    Logger.Debug("RX " + s);
    port.Close();
	
	
    //Reverse encoder counting direction
    port.Open();
    messageToSend = "M -1";
    messageToSend += "\r\n";
    port.Write(messageToSend);
    Logger.Debug(messageToSend);
    Thread.Sleep(50);
    //Check for any data on the port
    s = port.ReadExisting();
    drop_count = 0;
    while(s.Length < 1){
    s = port.ReadExisting();
    drop_count++;
    Thread.Sleep(1);
    if(drop_count > 1000){
        break;
    }
    }
    Logger.Debug("RX " + s);
    port.Close();

    //send the frequency for printing
    double YRes =  Parameters.GetIntValue("Recipe.Y_Resolution[0]");
    YRes = 25.400 / YRes;
    double Pspd =  Parameters.GetIntValue("Motion.PrintSpeed[0]");
    int frequency = (int)( Pspd / YRes);
    messageToSend = "p ";
    messageToSend+= frequency.ToString();
    messageToSend += "\r\n"; //not sent by defualt and using Arduino parseInt();
    
    if(frequency > 4000){
		Logger.Error("Frequency too high for head");
        Helper.GenerateScriptError("Head", "Freq = " + frequency.ToString() + " too high");
    }

    port.Open();
    port.Write(messageToSend);
    Logger.Debug(messageToSend);
    Thread.Sleep(50);
    //Check for any data on the port
    s = port.ReadExisting();
    drop_count = 0;
    while(s.Length < 1){
    s = port.ReadExisting();
    drop_count++;
    Thread.Sleep(1);
    if(drop_count > 1000){
        break;
    }
    }
    Logger.Debug("RX " + s);
    port.Close();

    //Create a new array with some padding
    byte[] dataToSend = new byte[readText.Length + 2];
    readText.CopyTo(dataToSend, 2);

    //Set to write array
    dataToSend[0] = (byte)'W';

    //set the head index
    dataToSend[1] = (byte)(100+currentHeadIndex);

    //Open the serial port
    port.Open();
    Thread.Sleep(50);
    //Check for any data on the port
    s = port.ReadExisting();
    Logger.Debug("RX " + s);

    port.Write(dataToSend, 0, dataToSend.Length);

    Thread.Sleep(100);
    s = port.ReadExisting();
    Logger.Debug("RX: " + s);
    port.Close();

    string[] s_ = s.Split('\n');

    int LV;
    int RC;
    int DL;

	if(s_.Length == 5){
        //LV RC DL
        int.TryParse(s_[1].Substring(3), out LV);
        int.TryParse(s_[2].Substring(3), out RC);
        int.TryParse(s_[3].Substring(3), out DL);
        if(LV == lastValue){
            if(RC == runningCount){
                if(DL == dataLength){
                    Logger.Debug("Data send correct");
                    return true;
                }
                else{
                    Logger.Error("DL incorrect");
                    return false;
                }
            }
            else{
                Logger.Error("RC incorrect");
                return false;
            }
        }
        else{
            Logger.Error("LV incorrect");
            return false;
        }

    }
    else{
        Logger.Debug("Not recieved the correct data format");
        Logger.Debug(" ");
        foreach(string lines in s_){
            Logger.Debug(lines);
        }
        Logger.Debug(" ");
        return false;
    }

}



public void DblArrayToMessageBox(double[] DblArray, string infoStr)
{
    string TestString3 = infoStr;
    foreach(double StartLocs in DblArray)
    {
        TestString3 = TestString3 + "\n" + StartLocs.ToString();
    }
    MessageBox.Show(TestString3);
}
public void DblListToMessageBox(List<double> DblArray, string infoStr)
{
    string TestString3 = infoStr;
    foreach(double StartLocs in DblArray)
    {
        TestString3 = TestString3 + "\n" + StartLocs.ToString();
    }
    MessageBox.Show(TestString3);
}

public bool IsOdd(int value)
{
    return value % 2 != 0;//if remainder of value/2 is not=0 it must be odd
}



private int getPrintheadNPI(){
    int headType = Parameters.GetIntValue("PrintHead.Type[0]"); //get current head type
	switch(headType){
		case 50:
		case 51:
		case 52:
		case 54:
		case 55:
		case 56:
			return 185;			
		case 60:
			return 75;			
		case 61:
			return 100;			
	}
	
	if(printHeadNPI == 0){
		Logger.Error("Error print head not found");
        Helper.GenerateScriptError("Head", "Head type not found in list");
	}

    return -1;
}

private void AppendPass(int layer, int layers){
   string currentMessage = Parameters.GetValue("General.ProgressText");
   string[] splits = currentMessage.Split(':');
   currentMessage = splits[0] + String.Format(": Pass {0} of {1}", layer, layers);
      
    Parameters.SetValue("General.ProgressText", currentMessage);
}


private void setVoltage(){
for(int index = 0; index < 4; index ++)
{
	
	string serial_port = Parameters.GetValue("HeadAssy.COMPORT");
    SerialPort port = new SerialPort(serial_port, 1000000);
	double voltage = Parameters.GetDoubleValue("PrintHead.PHVoltage[" + index.ToString() + "]");
	
	if(voltage > 35){
		voltage = 35;
	}
	if(voltage < 1){
		voltage = 1;
	}
	
	port.Open();
    string messageToSend = "v ";
	messageToSend += (index+1).ToString();
	messageToSend += " ";
    messageToSend += voltage.ToString();
    messageToSend += "\r\n";
    port.Write(messageToSend);
    Logger.Log(messageToSend);
    Thread.Sleep(50);
    //Check for any data on the port
    string s = port.ReadExisting();
    int drop_count = 0;
    while(s.Length < 1){
    s = port.ReadExisting();
    drop_count++;
    Thread.Sleep(1);
    if(drop_count > 1000){
        break;
    }
    }
    Logger.Log("RX " + s);
    port.Close();	
	
	
}	
}


private void setTemperature(){
for(int index = 0; index < 4; index ++)
{
	
	string serial_port = Parameters.GetValue("HeadAssy.COMPORT");
    SerialPort port = new SerialPort(serial_port, 1000000);
	int isInUse = Parameters.GetIntValue("PrintHead.ImgMask[" + index.ToString() + "]");
	
	double temper = 20;
	
	if(isInUse > 0){
		temper = Parameters.GetDoubleValue("PrintHead.PHTemperature[" + index.ToString() + "]");
	if(temper > 70){
		temper = 70;
	}
	if(temper < 21){
		temper = 20;
	}
	}
	else{
		temper = 0;
	}
	port.Open();
    string messageToSend = "T ";
	messageToSend += (index+1).ToString();
	messageToSend += " ";
    messageToSend += temper.ToString();
    messageToSend += "\r\n";
    port.Write(messageToSend);
    Logger.Log(messageToSend);
    Thread.Sleep(50);
    //Check for any data on the port
    string s = port.ReadExisting();
    int drop_count = 0;
    while(s.Length < 1){
    s = port.ReadExisting();
    drop_count++;
    Thread.Sleep(1);
    if(drop_count > 1000){
        break;
    }
    }
    Logger.Log("RX " + s);
    port.Close();	
	
}	
}
	



private bool checkTemperature(){
	string root_dir = Parameters.GetValue("Printhead.X128.ScriptFolder");
	Script.Run(Helper.GetScriptDir() + "Status.cs");
	for(int index = 0; index < 4; index ++){
		//SetTemperature
		//CurrentTemperature
		
	double curTemp = Parameters.GetDoubleValue("PrintHead.X128.CurrentTemperature[" + index.ToString() + "]");
	double setTemp = Parameters.GetDoubleValue("PrintHead.X128.SetTemperature[" + index.ToString() + "]");
	
	double error = (setTemp - curTemp)*(setTemp - curTemp);
	if(setTemp > 25){
	if(error > 10){
		
    Parameters.SetValue("General.ProgressText", "Waiting for head " + (index+1).ToString() + " to heat");
	return true;
	}
	}
		
}
return false;
}