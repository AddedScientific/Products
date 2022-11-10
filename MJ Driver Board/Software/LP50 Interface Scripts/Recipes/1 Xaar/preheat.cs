/******************************************************************************************
	Title:		    		LP50 Standard printing script
	Author:		    	GJ Pixdro BV
	Date:		      	05 March 2008
	Description:	This script performs a default printing job on the LP50 inkjet printer. Also print
						simulation is handled
	Version:      		$Revision: 40 $
*******************************************************************************************/

public void Initialize()
{

string headAssy = Parameters.GetValue("General.HeadAssemblyFile[0]");
	HeadAssy.ActivateSet(headAssy, true);
Parameters.SetValue("General.NumberOfPCAs", 2);

	Script.Run(Helper.GetScriptDir()  + "\\Status.cs");
}

public void Execute()
{
	turnOn();
setTemperature();

	Script.Run(Helper.GetScriptDir()  + "\\Status.cs");
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
	bool connection_not_made = true; 	int counter = 0; 	while(connection_not_made){ 		try{ 			port.Open(); 			connection_not_made = false; 			} 		catch( Exception otherProblem){ 			Thread.Sleep(100); 			counter++; 			Logger.Log("COMPORT Stuck " + counter.ToString()); 		} 	}
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
private void turnOn(){
	
	string serial_port = Parameters.GetValue("HeadAssy.COMPORT");
    SerialPort port = new SerialPort(serial_port, 1000000);
	
	double temper = 20;
	bool connection_not_made = true; 	int counter = 0; 	while(connection_not_made){ 		try{ 			port.Open(); 			connection_not_made = false; 			} 		catch( Exception otherProblem){ 			Thread.Sleep(100); 			counter++; 			Logger.Log("COMPORT Stuck " + counter.ToString()); 		} 	}
    string messageToSend = "O";
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

private bool checkTemperature(){
	Script.Run(Helper.GetScriptDir()  + "\\Status.cs");
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