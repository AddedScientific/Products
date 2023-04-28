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
	int headStatus = Parameters.GetIntValue("PrintHead.State[0]");
	string messageToSend;
	string s;
    int drop_count = 0;
    string serial_port = Parameters.GetValue("HeadAssy.COMPORT");
    SerialPort port = new SerialPort(serial_port, 1000000);
	if(headStatus == (int)PrintheadState.Idle){
		//head is ready we can start
	port.Open();
    messageToSend = "M 2";
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
	
		PHD.StartJetting(DataGen.Active.HeadInfo, "all");
	}
	else if (headStatus != (int)PrintheadState.Jetting){
		//head is jetting we can stop
	port.Open();
    messageToSend = "M 4";
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
	
		PHD.StopJetting(DataGen.Active.HeadInfo);
	}
	
	
	
	
	
	
	


	int selectedHead = Parameters.GetIntValue("Recipe.Head");
	int startNozzle = Parameters.GetIntValue("Recipe.NozzleStart");
	int endNozzle = Parameters.GetIntValue("Recipe.NozzleEnd");
	int nozzleRange = 128;
	if((endNozzle - startNozzle < 128) & (endNozzle - startNozzle >0)){
		nozzleRange = endNozzle - startNozzle;
	}

	//nSelected Head [1-4], Nozzle [1-128], Span [1-128]
    port.Open();
    messageToSend = "N " + selectedHead.ToString() + " " + startNozzle.ToString() + " " + nozzleRange.ToString();
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
	
	
	
	
}