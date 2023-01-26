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


}

public void Execute()
{

    string serial_port = Parameters.GetValue("HeadAssy.COMPORT");
    SerialPort port = new SerialPort(serial_port, 1000000);

	int selectedHead = Parameters.GetIntValue("Recipe.Head");
	int startNozzle = Parameters.GetIntValue("Recipe.NozzleStart");
	int endNozzle = Parameters.GetIntValue("Recipe.NozzleEnd");
	int nozzleRange = 128;
	if((endNozzle - startNozzle < 128) & (endNozzle - startNozzle >0)){
		nozzleRange = endNozzle - startNozzle;
	}

	//nSelected Head [1-4], Nozzle [1-128], Span [1-128]
    //port.Open();
	
	bool success = false;
	while(!success){
                try {
                    port.Open ();
                    success = true;
                } catch (Exception otherProblem) {
                    Logger.Log ("Other exception: " + otherProblem);
                }
	}
    string messageToSend = "N " + selectedHead.ToString() + " " + startNozzle.ToString() + " " + nozzleRange.ToString();
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
	
	double voltage = Parameters.GetDoubleValue("Recipe.TuningVoltage");
	double temperature = Parameters.GetDoubleValue("Recipe.TuningTemperature");
	double tuningClock = Parameters.GetDoubleValue("Recipe.TuningClock");
	
	if(voltage > 35){
		voltage = 35;
	}
	if(voltage < 10){
		voltage = 10;
	}
	if(temperature < 20){
		temperature = 20;
	}
	if(temperature > 70){
		temperature = 70;
	}
	if(tuningClock > 150){
		tuningClock = 150;
	}
	if(tuningClock < 50){
		tuningClock = 50;
	}	
	
	tuningClock = 1000000 * tuningClock / 100.0;
	
	port.Open();
    messageToSend = "T ";
	messageToSend += selectedHead.ToString();
	messageToSend += " ";
    messageToSend += temperature.ToString();
    messageToSend += "\r\n";
    port.Write(messageToSend);
    Logger.Log(messageToSend);
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
    Logger.Log("RX " + s);
    port.Close();
		
	port.Open();
    messageToSend = "v ";
	messageToSend += selectedHead.ToString();
	messageToSend += " ";
    messageToSend += voltage.ToString();
    messageToSend += "\r\n";
    port.Write(messageToSend);
    Logger.Log(messageToSend);
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
    Logger.Log("RX " + s);
    port.Close();	
	
	port.Open();
    messageToSend = "G ";
    messageToSend += tuningClock.ToString();
    messageToSend += "\r\n";
    port.Write(messageToSend);
    Logger.Log(messageToSend);
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
    Logger.Log("RX " + s);
    port.Close();
	
	
	
	port.Open();
    messageToSend = "U 1";
    messageToSend += "\r\n";
    port.Write(messageToSend);
    Logger.Log(messageToSend);
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
    Logger.Log("RX " + s);
    port.Close();
	
	
	
	
	Script.Run(Helper.GetScriptDir()  + "Status.cs");
	
	
}