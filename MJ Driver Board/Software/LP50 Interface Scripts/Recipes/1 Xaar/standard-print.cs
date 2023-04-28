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

Parameters.SetValue("AUXOUT.Gate[0]", 0); //Enable gate?
Parameters.SetValue("AUXOUT.Enable[0]", 0); //Enable gate?
Parameters.SetValue("AUXOUT.Trigger[0]", 0); //Enable gate?
Parameters.SetValue("AUXOUT.OUT1Mode[0]", 8); //Match 1
Parameters.SetValue("AUXOUT.OUT2Mode[0]", 16);    //None

string headAssy = Parameters.GetValue("General.HeadAssemblyFile[0]");
	HeadAssy.ActivateSet(headAssy, true);
Parameters.SetValue("General.NumberOfPCAs", 2);
		Script.Run(Helper.GetScriptDir()  + "Status.cs");
	
	
	if (workerObject == null)
	{
		workerObject = new Worker(this);
	}
}

public void Execute()
{

  bool z_safe = true;
  bool simOnly = (Parameters.GetIntValue("Simulator.SimulationType")==1);
 // for( int repeat = 0; repeat < 5; repeat ++){
	// Only check z_safe if not in simulation mode

//check to see if leadIn is enough for printing
double leadInValue = (double)Parameters.GetDoubleValue("Motion.RunInLength[0]"); 
double printSpeed  = (double)Parameters.GetDoubleValue("Motion.PrintSpeed[0]"); 
double yAcceleration  = (double)Parameters.GetDoubleValue("Motion.Y.Acceleration[0]"); 

int spdCntrl = Parameters.GetIntValue("Recipe.PrintSpeedControl");
//PrintFrequency
//PrintScaler
if(spdCntrl == 1){
	//printSpeed = printSpeed;	
}
else if(spdCntrl == 2){
	printSpeed = (double)Parameters.GetDoubleValue("Recipe.PrintFrequency[0]") * (25.4/(double)Parameters.GetDoubleValue("Recipe.Y_Resolution[0]")); 
}   
else if(spdCntrl == 3){
	printSpeed = (double)Parameters.GetDoubleValue("Recipe.PrintScaler[0]") * 40 * (25.4/(double)Parameters.GetDoubleValue("Recipe.Y_Resolution[0]")); 
}   
if(printSpeed > 500){
	printSpeed = 500;
}
Logger.Log("Speed calculated to: " + printSpeed.ToString());
Parameters.SetValue("Motion.PrintSpeed", printSpeed); 

double accelDistance = 0.5 * (printSpeed * printSpeed) / yAcceleration;

if(leadInValue < accelDistance){
	Helper.GenerateScriptWarning("Speed", "Printing will not be steady state, set to <" +  accelDistance.ToString());
}


//cehck to see if a head assy is specified and activate

string headAssy = Parameters.GetValue("General.HeadAssemblyFile[0]");
	HeadAssy.ActivateSet(headAssy, true);





  if(!simOnly) 
	{
		Helper.CheckPrintConditions();	
		PHD.StopHead(DataGen.Active.HeadInfo);	
	  z_safe = Motion.CheckHomePos(Position.Axes.Z);
  }

  if(z_safe)
  {
		if ((!simOnly) & (Parameters.GetIntValue("Recipe.DoMaintenance") > 0))
		{
			Script.Run(Helper.GetScriptDir() + "maintenance.cs");
		}


    if ((!simOnly) & (Parameters.GetBoolValue("Alignment.Activated")))
    {
      AlignmentFunc.Align();
    	if (!Parameters.GetBoolValue("Alignment.Success"))
        // generate error!
        return;
    }

	int headType = Parameters.GetIntValue("PrintHead.Type[0]") ;
	int layer_counts = 1;
  if(Parameters.HasParameter("Recipe.Layers[0]")){
			layer_counts = Parameters.GetIntValue("Recipe.Layers[0]");
  }
  
	Parameters.SetValue("Recipe.Layer", 0);

	for(int lay = 0; lay < layer_counts; lay++){
	
	Parameters.SetValue("General.ProgressText", "Layer " + (lay+1).ToString() + " of " + layer_counts.ToString() + ": ");
	Logger.Log("Layer " + (lay+1).ToString() + " of " + layer_counts.ToString() + ": ");

	//Parameters.SetValue("Recipe.Bitmap[0]", @"C:\LP50\SampleBitmaps\STL\Gyroid_80x80x80\support\images" + lay + ".png" );
	//Parameters.SetValue("Recipe.Bitmap[1]", @"C:\LP50\SampleBitmaps\STL\Gyroid_80x80x80\build\images" + lay + ".png" );

	if( (headType > 49) && !(headType==99) ){
		//generate data
		Script.Run(Helper.GetScriptDir() + "image_swather.cs");
		//head is a xaar 128 head, single head
		Script.Run(Helper.GetScriptDir() + "print_xaar.cs");
	}
	else{
		Script.Run(Helper.GetScriptDir() + "print.cs");
	}
	

	Parameters.SetValue("Recipe.Layer", Parameters.GetIntValue("Recipe.Layer")+1);

  if(Parameters.HasParameter("Recipe.UseIR[0]")){
			bool runIR = (Parameters.GetIntValue("Recipe.UseIR[0]") == 1);
			if(runIR){
				Script.Run(Helper.GetScriptDir() + "runIR.cs");
			}
  }
	}

	  Parameters.SetValue("General.ProgressText", "$(Finished)");
		if(!simOnly){
			//Motion.MoveTo("Service");         
		Position endpos = new Position();

        endpos.Change(Position.Axes.Y, 10);
        
        Motion.MoveTo(endpos);
		}
		
  } 
//}
}