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
	int selectedHead = Parameters.GetIntValue("Recipe.Head");
	
    Position dwpos = new Position();
	
	double nozzleIndex = (Parameters.GetIntValue("Recipe.NozzleStart")-1) * 0.137;
	
	if(selectedHead == 1){
		dwpos.Change(Position.Axes.X, 473.373+nozzleIndex);
		dwpos.Change(Position.Axes.Y, 4);
		dwpos.Change(Position.Axes.Z, -5.8);
		dwpos.Change(Position.Axes.P, 90);
		dwpos.Change(Position.Axes.D, 2.65);
		
		
		 Motion.MoveTo(dwpos);
	}
	else if(selectedHead == 2){		
		dwpos.Change(Position.Axes.X, 473.373+nozzleIndex);
		dwpos.Change(Position.Axes.Y, 4);
		dwpos.Change(Position.Axes.Z, -5.8);
		dwpos.Change(Position.Axes.P, 90);
		dwpos.Change(Position.Axes.D, 2.65+25);
		
		 Motion.MoveTo(dwpos);
	}
	else{
		MessageBox.Show("Selected head is unreachable.", "Error");
	}

	Script.Run(Helper.GetScriptDir() + "Status.cs");

}