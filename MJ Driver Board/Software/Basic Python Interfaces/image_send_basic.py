from PIL import Image
import numpy
import serial
import math
import random
import time


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
            imageData.append(0)
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



#open the image to be sent, must be 128 pixels tall
im = Image.open('Xaar128 test image.bmp', 'r')

#open comport for driver board
comport = "COM4"

#create empty string for reply
retrun = ""


#if error on above line check bmp is binary

#open comport at correct baud
ser = serial.Serial(comport, 1000000)

#turn board on
ser.write("O".encode())
time.sleep(3)

#set an encoder divider (1 micron encoder, 50 micron drop spacing
ser.write("e 28".encode())
time.sleep(2)

#add encoder multipler/reverse
ser.write("M -1".encode())
time.sleep(2)

while ser.in_waiting:
    retrun = retrun + ser.read().decode("utf-8")
    #clear serial buffer

print(retrun)
retrun = ""
#as we wont send a start location "," the external PD will be used

result = sendImage(1, im)
result += sendImage(2, im)
result += sendImage(3, im)
result += sendImage(4, im)

if(result == 4):
    print("All sent successfully")
    #repeat the pattern 10 times
    ser.write(". 10".encode())
    time.sleep(0.1)
else:
    ser.write("F") # error went on so turn board off
    

#close port
ser.close()

