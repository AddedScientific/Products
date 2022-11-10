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
	bool success = false;
try
{
port.Open();
success = true;
}
catch( Exception otherProblem)
{
Logger.Log("Other exception");
}

if(success){
    string messageToSend = "B";
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
	
	
	port.Open();
    messageToSend = "b";
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
	
	s = s.Replace("{", "\n");
	s = s.Replace("\"", "");
	s = s.Replace("}", "");
	s = s.Replace("[", "");
	s = s.Replace("]", "\n");
	string[] lines = s.Split('\n');
	/*
	s = s.Replace("\"heads\"", "\"\nheads\"");
	s = s.Replace("\"isActive\"", "\"A\"");
	s = s.Replace("\"head\"", "\"\n  H\"");
	s = s.Replace("\"voltage\"", "\"V\"");
	s = s.Replace("\"printCounts\"", "\"C\"");
	s = s.Replace("\"status\"", "\"E\"");
	s = s.Replace("\"setTemperature\"", "\"S\"");
	s = s.Replace("\"isHeating\"", "\"H\"");
	s = s.Replace("\"curTemperature\"", "\"T\"");
	s = s.Replace("\"images\"", "\"\nimages\"");
	s = s.Replace("\"image\"", "\"\n I\"");
	s = s.Replace("\"length\"", "\"L\"");
	s = s.Replace("\"hasData\"", "\"D\"");
	s = s.Replace("\"progress\"", "\"P\"");
	s = s.Replace("\"remaining\"", "\"R\"");
	s = s.Replace("\"locations\"", "\"\nlocations\"");
	s = s.Replace("\"printingParameters\"", "\"\nprintingParameters\"");
	s = s.Replace("\"versions\"", "\"\nversions\"");
	s = s.Replace("\"software\"", "\"\nsoftware\"");
	s = s.Replace("\"hardware\"", "\"\nhardware\"");
	s = s.Replace("\"encoder\"", "\"\n encoder\"");
	s = s.Replace("\"stepper\"", "\"\n stepper\"");
	s = s.Replace("\"encoderDivider\"", "\"\n encoderDivider\"");
	s = s.Replace("\"internalPrintPeriod\"", "\"\n internalPrintPeriod\"");
	*/
	
	foreach( string line in lines){
		//Logger.Log(line);
		for(int headIndex = 1; headIndex < 5; headIndex++){
			int foundIndex = line.IndexOf("head: " + headIndex.ToString());
				if(foundIndex == 0){
					string[] elements = line.Split(',');
					foreach( string el in elements){
						string toFind = " voltage:";
						if(el.IndexOf(toFind) ==0){
							double res = double.Parse(el.Substring(toFind.Length));
							Parameters.SetValue("Printhead.X128.Voltage[" + (headIndex-1).ToString() + "]", res);
							//Logger.Log(res.ToString());
						}
						toFind = " curTemperature:";
						if(el.IndexOf(toFind) ==0){
							double res = double.Parse(el.Substring(toFind.Length));
							Parameters.SetValue("Printhead.X128.CurrentTemperature[" + (headIndex-1).ToString() + "]", res);
							//Logger.Log(res.ToString());
						}
						toFind = " setTemperature:";
						if(el.IndexOf(toFind) ==0){
							double res = double.Parse(el.Substring(toFind.Length));
							Parameters.SetValue("Printhead.X128.SetTemperature[" + (headIndex-1).ToString() + "]", res);
							//Logger.Log(res.ToString());
						}
						toFind = " printCounts:";
						if(el.IndexOf(toFind) ==0){
							double res = double.Parse(el.Substring(toFind.Length));
							Parameters.SetValue("Printhead.X128.HeadPrintCounts[" + (headIndex-1).ToString() + "]", res);
							//Logger.Log(res.ToString());
						}
						toFind = " status:";
						if(el.IndexOf(toFind) ==0){
							double res = double.Parse(el.Substring(toFind.Length));
							Parameters.SetValue("Printhead.X128.State[" + (headIndex-1).ToString() + "]", res);
							//Logger.Log(res.ToString());
						}
						
						
					}
				}
			
			foundIndex = line.IndexOf("image: " + headIndex.ToString());
				if(foundIndex == 0){
					string[] elements = line.Split(',');
					foreach( string el in elements){
						string toFind = " hasData:";
						if(el.IndexOf(toFind) ==0){
							double res = double.Parse(el.Substring(toFind.Length));
							Parameters.SetValue("Printhead.X128.ImageData[" + (headIndex-1).ToString() + "]", res);
							//Logger.Log(res.ToString());
						}						
						
					}
				}
				
			foundIndex = line.IndexOf("encoder:");
				if(foundIndex == 0){
					string[] elements = line.Split(',');
					foreach( string el in elements){
						string toFind = "encoder:";
						if(el.IndexOf(toFind) ==0){
							double res = double.Parse(el.Substring(toFind.Length));
							Parameters.SetValue("Printhead.X128.Encoder", res);
							//Logger.Log(res.ToString());
						}						
						
					}
				}	
				
			foundIndex = line.IndexOf("power:");
				if(foundIndex == 0){
					string[] elements = line.Split(',');
					foreach( string el in elements){
						string toFind = "power:";
						if(el.IndexOf(toFind) >=0){
							double res = double.Parse(el.Substring(toFind.Length));
							Parameters.SetValue("Printhead.X128.Power", res);
							//Logger.Log(res.ToString());
						}
						toFind = " timeOn:";
						if(el.IndexOf(toFind) >=0){
							double res = double.Parse(el.Substring(toFind.Length));
							Parameters.SetValue("Printhead.X128.OnTime", res);
							//Logger.Log(res.ToString());
						}		
						
					}
				}
			foundIndex = line.IndexOf("serialNumber:");
				if(foundIndex == 0){
					string[] elements = line.Split(',');
					foreach( string el in elements){
						string toFind = "serialNumber:";
						if(el.IndexOf(toFind) >=0){
							string res = (el.Substring(toFind.Length));
							Parameters.SetValue("Printhead.X128.SerialNumber", res);
						}	
						
					}
				}	
			foundIndex = line.IndexOf("date:");
				if(foundIndex == 0){
					string[] elements = line.Split(',');
					foreach( string el in elements){
						string toFind = "software:";
						if(el.IndexOf(toFind) >=0){
							string res = (el.Substring(toFind.Length));
							Parameters.SetValue("Printhead.X128.SoftwareVersion", res);
						}	
						
					}
				}
					
			
		}
	}
		
	string root_dir = Helper.GetScriptDir();
	string start = "<script src=\""+ root_dir +"\\run_prettify.js\"></script><pre class=\"prettyprint\">";
	
	s = start + s;
	s = s+ "<meta http-equiv=\"refresh\" content=\"1\" >";
	
	File.WriteAllText(root_dir + "\\index.html", s);
}
}