from PIL import Image
import numpy
import serial
import math
import random
import time
import sys

 
def command_lamp(cmnd):
    cmnd = cmnd + "\r"
    print("Command: " + cmnd)
    ser_uvlamp.write((cmnd).encode())
    time.sleep(0.25)
    retrun="Reply: "
    while ser_uvlamp.in_waiting:
        retrun = retrun + ser_uvlamp.read().decode("utf-8")
    print(retrun)

def lamp_on():
    command_lamp("CNTQ") #sets to commands
    command_lamp("ON1")

def lamp_off():
    command_lamp("CNTQ") #sets to commands
    command_lamp("OFF1")

def lamp_power(power):
    commandString = "CURE1," + f"{power:03}" + ",01,001,01,001,01"
    command_lamp(commandString)


def sendImage(headIdx, imageToSend, whiteSpace):
    #this function sends image to a already connected board and has the option to pad white space (useful for ph alignment)
    #get image properties - this section only compatible with certain colour spaces
    width, height = imageToSend.size
    pixel_values = list(imageToSend.getdata())
    pixel_values = (255 - (numpy.array(pixel_values).reshape((height, width))))/255#turn image into binary
    #create empty byte array to store image data
    sum_line = 0
    imageData = bytearray()
    imageData.append(87) #87 = W
    imageData.append(100+headIdx) #head index 1-4 so 101 - 104

    #sum the bytes
    sumofval = 0
    #count the bytes
    copnt = 0
    #pixel_values
    #pre image whitespace
    for i in range(0,whiteSpace):
        for byt in range(0,16):
            curByte = 0
            imageData.append(curByte)
            lasvalu = curByte
            sumofval = sumofval + lasvalu
            copnt = copnt + 1
    
    
    #convert image data to binary string, reversal inside the byte to be noted
    for i in range(0,width):
        for byt in range(0,16):
            curByte = 0
            for bit in range(0,8):
                j = byt * 8 + bit            
                if (pixel_values[j][i] > 0):
                    curByte = curByte + 2**(7-bit)
                #print("At index %d the value is %d and sum is %d"%(j,pixel_values[j][i], curByte))
            #print(curByte)
            imageData.append(curByte)
            lasvalu = curByte
            sumofval = sumofval + lasvalu
            copnt = copnt + 1


        sum_line = 0



    #time the data send
    t = time.time()

    #send the image data
    ser_board.write(imageData)
    elapsed = time.time() - t

    #report how long it took to send data
    #print("Data sent in %fs."%elapsed)
    retrun = ""
    #recieve the "checksum from the head"
    
    time.sleep(0.1)

    while ser_board.in_waiting:
        retrun = retrun + ser_board.read().decode("utf-8")

    #print(retrun)
    midw = retrun.split(":")
    #print(midw)
    lv = midw[2].split("\n")
    lv = int(lv[0])

    rc = midw[3].split("\n")
    rc = int(rc[0])
    4
    dl = midw[4].split("\n")
    dl = int(dl[0])

    if(lv == lasvalu):
        if(rc == sumofval):
            if(dl == copnt):
                return True
            else:
                return False
        else:
            return False
    else:
         return False



def printer_clear_buffer():
    #to clear serial buffer on FDM printer - useful for synchronisation
    while ser_printer.in_waiting:
        ser_printer.readline()

def printer_command(command):
  #wrapper to send commands and wait for OK
  command = command + "\n"
  #start_time = datetime.now()
  ser_printer.write(str.encode(command)) 
  time.sleep(0.01)

  while True:
    line = ser_printer.readline()
    print(line)

    if line[0:2] == b'ok':
      break

  printer_poll_position()


def board_command(command):
    #wrapper for printhead board commands
    command = command + "\n"
    print("Sending %s"%command)
    ser_board.write(str.encode(command))
    time.sleep(0.3)
    retrun=""
    while ser_board.in_waiting:
        retrun = retrun + ser_board.read().decode("utf-8")
    print(retrun)
    return retrun

def check_board():
    #Sending a B will cause the board to reply with status for each head, -2 missing, -3 error
    #if error detected the board will be reset
    #after reset the temperatures will be written again
    ser_board.write("B\n".encode())
    time.sleep(0.1)
    retrun=""
    while ser_board.in_waiting:
        retrun = retrun + ser_board.read().decode("utf-8")
    print(retrun)
    val = retrun.find("-3")
    if val > 0:
        print("Boards need reseting")
        board_command("r")
        check_board()        
        board_command("T 1 50")
        board_command("T 2 60")


def printer_poll_position():
    #unused in the printing comand but to check where FDM printer thinks it is
    command = "M114\n"
    ser_printer.write(str.encode(command))
    time.sleep(0.01)
    print(ser_printer.readline())

def setStartPoint(loc):
    #sending this command zeros the current stepper tracker and sets an absolute start position
    board_command(", %d"%loc)
    val = board_command(">")
    if(val.find("Stepper current count: 0") < 0):
        setStartPoint(loc)

    return val
    


ser_printer = serial.Serial("COM7", 115200)
ser_board = serial.Serial("COM5", 1000000)
ser_uvlamp = serial.Serial( port     = "COM6",
                            baudrate = 9600,
                            parity   = serial.PARITY_NONE,
                            stopbits = serial.STOPBITS_ONE,
                            bytesize = serial.EIGHTBITS)



print("Serials connected")

printer_command("G28 0") #homes all untrusted axis
printer_command("G90")
printer_command("G1 X 125 Y 30 Z 10 F18000")
print("Machine homed and ready")


board_command("O") #turn board on
board_command("C")
print("Board on")

#power of lamp to 50
lamp_power(50)
lamp_off()

PrintSpeed = 100 ##100mm/s is 3khz
pSpacing = 722 #dpi
pSpacing = 25.4/722 #to mm
pFrequency = PrintSpeed / pSpacing #300 mm/s divide by drop spacing is Hz
secondHeadOffset = int(30 / pSpacing) #mount designed with 30 mm space
board_command("p %d"%pFrequency) #set printing frequency, internal clock used
board_command("D 1")
Xlocation = 175
Ystart = 80
Yend = 250
Zindex = 0.018
Zcurr = 3.5
total_layers = 5
PrintSpeed = PrintSpeed * 60 #set to mm/min

root = "\\Python Ender S1"

#images were generated from an stl file, then the support material was inverted using irfanview
fname1 = "verylowdense.bmp"

printer_command("G1 X %f Y %f Z %f F18000"%(Xlocation,Ystart,Zcurr))
printer_command("M400") #wait for move complete
time.sleep(2)

val = input("continue? ")
if(val == "n"):
    quit()


board_command("T 1 50") #set temps - this doesnt wait so head heating time should be allowed
    
for layers in range(total_layers):
    
    print("--------------")
    print("Layer %d of %d"%(layers+1, total_layers))
    print("Height %f"%(Zcurr))
    check_board()
    im1 = Image.open(fname1, 'r')
    sendImage(1, im1, 0)
    setStartPoint(8*30/0.1)
    #board_sendData(layers)


    lamp_on()
    printer_command("G1 X %f Y %f Z %f F%d"%(Xlocation,Yend,Zcurr,PrintSpeed))
    printer_clear_buffer() #clear serial buffer
    printer_command("M400") #wait for "OK"
    lamp_off()

    Zcurr += Zindex
    printer_command("G1 X %f Y %f Z %f F18000"%(Xlocation,Ystart,Zcurr))
    printer_clear_buffer()
    printer_command("M400")


printer_command("G1 X 2 Y 200 F18000") #move to park
board_command("b")
board_command("B")
board_command("p 1")
board_command("I 1")
ser_printer.close()
ser_board.close()
#board_command("F") #turn off board

