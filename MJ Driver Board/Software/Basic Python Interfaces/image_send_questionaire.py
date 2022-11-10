from PIL import Image
import numpy
import serial
import math
import random
import time
import sys

def serial_ports():
    """ Lists serial port names

        :raises EnvironmentError:
            On unsupported or unknown platforms
        :returns:
            A list of the serial ports available on the system
    """
    if sys.platform.startswith('win'):
        ports = ['COM%s' % (i + 1) for i in range(256)]
    elif sys.platform.startswith('linux') or sys.platform.startswith('cygwin'):
        # this excludes your current terminal "/dev/tty"
        ports = glob.glob('/dev/tty[A-Za-z]*')
    elif sys.platform.startswith('darwin'):
        ports = glob.glob('/dev/tty.*')
    else:
        raise EnvironmentError('Unsupported platform')

    result = []
    for port in ports:
        try:
            s = serial.Serial(port)
            s.close()
            result.append(port)
        except (OSError, serial.SerialException):
            pass
    return result


def sendImage(headIdx, imageToSend, whiteSpace):
    
    #get image properties
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
    ser.write(imageData)
    elapsed = time.time() - t

    #report how long it took to send data
    print("Data sent in %fs."%elapsed)
    retrun = ""
    #recieve the "checksum from the head"
    
    time.sleep(1)

    while ser.in_waiting:
        retrun = retrun + ser.read().decode("utf-8")

    print(retrun)
    midw = retrun.split(":")
    print(midw)
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



#open comport for driver board
print("Please enter com port number (only number)")
print(serial_ports())
comport = "COM" + input()


#create empty string for reply
retrun = ""


#if error on above line check bmp is binary

#open comport at correct baud
ser = serial.Serial(comport, 1000000)

#turn board on
ser.write(("O"+"\n").encode())
time.sleep(0.5)
ser.write(("r"+"\n").encode())
time.sleep(0.5)

print("Turning board on.")


print("Internal (1) or external encoder (0)?")
choice  = int(input())

print("Frequency/Divider?")
freq = int(input())

if choice == 0:
    #set an encoder divider (1 micron encoder, 50 micron drop spacing
    ser.write(("e "+str(freq)+"\n").encode())

    print("Printing normal (1), or print reverse direction (-1)?")
    em = input()
    ser.write(("M "+str(em)+"\n").encode())

if choice == 1:
    #set an printing freq
    ser.write(("p "+str(freq)+"\n").encode())
    
time.sleep(0.5)

print("Int(1) or Ext(0) PD?")
intext = int(input())
    
print("Product detect continuous(1) or single(0)?")
contPD = int(input())
print("K "+str(contPD))
ser.write(("K "+str(contPD)+"\n").encode())

time.sleep(0.5)

while ser.in_waiting:
    retrun = retrun + ser.read().decode("utf-8")
    #clear serial buffer

print(retrun)
retrun = ""
#as we wont send a start location "," the external PD will be used

print("name image 1 (dont add .bmp)?")
fname1 = input() + ".bmp"
print("whitespace padding (pixels)?")
ws1 = int(input())

print("name image 2 (dont add .bmp)?")
fname2 = input() + ".bmp"
print("whitespace padding (pixels)?")
ws2 = int(input())

print("name image 3 (dont add .bmp)?")
fname3 = input() + ".bmp"
print("whitespace padding (pixels)?")
ws3 = int(input())

print("name image 4 (dont add .bmp)?")
fname4 = input() + ".bmp"
print("whitespace padding (pixels)?")
ws4 = int(input())

#open the image to be sent, must be 128 pixels tall
im = Image.open(fname1, 'r')
result = sendImage(1, im, ws1)
im = Image.open(fname2, 'r')
result += sendImage(2, im, ws2)
im = Image.open(fname3, 'r')
result += sendImage(3, im, ws3)
im = Image.open(fname4, 'r')
result += sendImage(4, im, ws4)

if(result == 4):
    print("All sent successfully")
    #repeat the pattern 10 times
    print("How many repeats required?")
    reps = int(input())
    ser.write((". "+str(reps)+"\n").encode())
    
    if intext == 1:
        ser.write(("k"+"\n").encode())
    time.sleep(0.1)
else:
    ser.write(("F"+"\n").encode()) # error went on so turn board off
    print("board turning off due to error")
    

#close port
ser.close()

