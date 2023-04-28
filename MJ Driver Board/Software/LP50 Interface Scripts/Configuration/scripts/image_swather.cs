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


string sourceImage = Parameters.GetValue("Recipe.Bitmap[0]");
string sourceImage2 = Parameters.GetValue("Recipe.Bitmap[1]");
string sourceImage3 = Parameters.GetValue("Recipe.Bitmap[2]");
string sourceImage4 = Parameters.GetValue("Recipe.Bitmap[3]");

//so that scale bitmaps works
int flagForFIle =  Parameters.GetIntValue("Recipe.IgnoreRecipeSize[0]");
if(flagForFIle == 0){
sourceImage = Parameters.GetValue("DataFileConversion.OutputFile[0]");
}

string[] imageArray = {sourceImage, sourceImage2, sourceImage3, sourceImage4};
string[] labelArray = {"_A", "_B", "_C", "_D"};
//this only works for the first head


string outputFolder = @"C:\LP50\PrintGen\Output Files\";
int numberNozzles = getNozzleNumber(); 
//X is width of head
double dropSpacingX = Parameters.GetDoubleValue("Recipe.X_Resolution[0]");
//Y is print direction spacing
double dropSpacingY = Parameters.GetDoubleValue("Recipe.Y_Resolution[0]");

//delete all files in current folder
clearDirectory(outputFolder);

//need to select all images that may be related using headmask
for(int internalIndex = 0; internalIndex < 4; internalIndex++){
int imageIndex = Parameters.GetIntValue("PrintHead.ImgMask[" + internalIndex.ToString() + "]");
if(imageIndex != 0){
if(imageArray[imageIndex-1].Contains(".png") | imageArray[imageIndex-1].Contains(".bmp")){
sliceImage(numberNozzles, imageArray[imageIndex-1], outputFolder, labelArray[internalIndex]);
}
}
}
//take images and convert to data
convertImageToData(outputFolder, numberNozzles);

}

private int determinePadding(string filename){
	
	double dropSpacingY = Parameters.GetDoubleValue("Recipe.Y_Resolution[0]");
	
	double physical_spacing = 25; //mm
	
	double mm_pix = 25.4 / dropSpacingY;
	
	double pix_distance = physical_spacing / mm_pix;
	
	
	
	if(filename.Contains("_A")){
		double val = 3.0*pix_distance;
		return (int)Math.Round(val);
		}
	if(filename.Contains("_B")){
		double val = 2.0*pix_distance;
		return (int)Math.Round(val);
		}
	if(filename.Contains("_C")){
		double val = 1.0*pix_distance;
		return (int)Math.Round(val);
		}
	if(filename.Contains("_D")){
		return (int)0;
		}
	
	return 0;
}

private void convertImageToData(string folder, int numberNozzles){

	string[] files = Directory.GetFiles(folder);
	foreach(string file in files){
		Logger.Log("Image", "Loading sliced image to process data");
		using (Bitmap inputImage = new Bitmap(file)){
			int required_padding = determinePadding(file);
			Color pixel;
			List<byte> imageData = new List<byte>();
			
			for( int y = 0; y < required_padding; y++){
				for(int line = 0; line< 16; line++)
				{
					imageData.Add(0);
				}
			}
			
			
			for (int y = (inputImage.Height - 1); y > -1; y--){
				for(int byteWide = 0; byteWide < 16; byteWide++){
					byte curByte = 0;
					for(int bitIdx = 0; bitIdx < 8; bitIdx++){
						int x = 8*byteWide + bitIdx;
					
						if(x >= inputImage.Width){
							pixel = Color.FromName("White");
						}
						else{
							//x = (inputImage.Width-1) - x; 					
							//Logger.Log("Reference image at x=" + x.ToString() + " y=" + y.ToString());
							pixel = inputImage.GetPixel(x,y);
						}
						uint val = (uint) (pixel.ToArgb());
						//MessageBox.Show(val.ToString());
						if(val == 4278190080){
							curByte = (byte)(curByte + Math.Pow(2, (7-bitIdx)));
						}
					}

					imageData.Add(curByte);
				}
			}
			byte[] imageDataByteArray = imageData.ToArray();
			File.WriteAllBytes(file + ".printDat", imageDataByteArray);
		}
		Logger.Debug("Image", "Finished with sliced image to process data");

	}


}

private void sliceImage(int numberNozzles, string sourceImage, string outputFolder, string headStringIdx){
	Bitmap inputImage = new Bitmap(sourceImage);
	Logger.Debug("Slicing image " + numberNozzles.ToString() + " " + inputImage.Height.ToString());
	System.Drawing.Imaging.PixelFormat format = (System.Drawing.Imaging.PixelFormat)196865; //1bpp indexed
	int imageWidth = inputImage.Width;
	int rectStart = 0;
	int rectEnd = numberNozzles;
	int imageCount = 0;
	RectangleF cloneRect;
	//run until we start out of the image
	while(rectStart < imageWidth){
		//check that the end is bounded
		if(rectEnd < imageWidth){
			cloneRect = new RectangleF(rectStart, 0, numberNozzles, inputImage.Height);
		}
		else{
			cloneRect = new RectangleF(rectStart, 0, (imageWidth - rectStart), inputImage.Height);
		}
		Bitmap cloneBitmap = inputImage.Clone(cloneRect, format);
		double dropSpacingX = Parameters.GetDoubleValue("Recipe.X_Resolution[0]");
		int printIdx = (int)(imageCount * numberNozzles * (25400 / dropSpacingX));
		cloneBitmap.Save(outputFolder + printIdx.ToString("000000") + headStringIdx +  ".png");
		imageCount++;
		rectStart+= numberNozzles;
		rectEnd = rectStart + numberNozzles;
	}
}

private void clearDirectory(string dir){	
	string[] files = Directory.GetFiles(dir);
	foreach(string file in files){
		try{
			File.Delete(file);
		}
		catch(Exception e){
			Logger.Log("{0} Exception caught. In file delete.", e);
		}

	}
}

private int getNozzleNumber(){
	int headType = Parameters.GetIntValue("PrintHead.Type[0]"); //get current head type
	switch(headType){
		case 50:
		case 51:
		case 52:
		case 54:
		case 55:
		case 56:
			return 128;
		case 60:
			return 12;
		case 61:
			return 16;
	}

	Logger.Error("Print head not found in configuration");
	Helper.GenerateScriptError("Head", "Head type not found in list");

	return -1;

}