from PIL import Image
import numpy
import serial
import math
import random
import time


def sendImageCompressed(headIdx, imageToSend, whiteSpace):
    
    #get image properties
    width, height = imageToSend.size
    pixel_values = list(imageToSend.getdata())
    pixel_values = (255 - (numpy.array(pixel_values).reshape((height, width))))/255#turn image into binary
    #create empty byte array to store image data
    sum_line = 0
    imageData = bytearray()
    imageData.append(74) #87 = W
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


    curIdx = 2
    newArray = bytearray()
    newArray.append(imageData[0])
    newArray.append(imageData[1])
    byteCount = 1
    while curIdx <= (len(imageData)-32):
        
        if(imageData[curIdx:curIdx+16] == imageData[curIdx+16:curIdx+32]):
            byteCount+=1
        else:
            newArray.append(byteCount)
            for index in range(curIdx, curIdx+16):
                newArray.append(imageData[index])
            byteCount = 1

        ##if full byte or at the end        
        if(byteCount == 256):
            ##append count
            newArray.append(255)
            byteCount = 1
            ##append current row
            for index in range(curIdx, curIdx+16):
                newArray.append(imageData[index])

        if(curIdx == (len(imageData)-32)):
            ##append count this runs when EOF
            ##if byteCount > 1 then its not a new line
            ##if bteCount==1 then it is 
            newArray.append(byteCount)
            ##append current row

            if(byteCount == 1):
                for index in range(curIdx+16, curIdx+32):
                    newArray.append(imageData[index])
            else:
                for index in range(curIdx, curIdx+16):
                    newArray.append(imageData[index])

        curIdx+=16

    #time the data send
    t = time.time()
    
    raw_length = (len(imageData))/1024
    compressed_length = (len(newArray))/1024

    print(raw_length)
    print(compressed_length)
    print(raw_length/compressed_length)
    #send the image data
    ser.write(newArray)
    
    newFile = open("filenameR.txt", "wb")
    newFile.write(newArray[2:])

    retrun = ""
    #recieve the "checksum from the head"
    
    elapsed = time.time() - t
    #report how long it took to send data
    print("Data sent in %fs."%elapsed)
    time.sleep(1)

    while ser.in_waiting:
        retrun = retrun + ser.read().decode("utf-8")



    #print(retrun)
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
im = Image.open('input_image.bmp', 'r')

#open comport for driver board
comport = "COM5"

#create empty string for reply
retrun = ""


#if error on above line check bmp is binary

#open comport at correct baud
ser = serial.Serial(comport, 1000000)

#turn board on
ser.write("O".encode())
time.sleep(3)

#set to run in mode 4, encoder
ser.write("M 4".encode())
time.sleep(2)

#set a printing frequency (1 micron encoder, 50 micron drop spacing
ser.write("p 400".encode())
time.sleep(2)

#set start location to 1000 counts from current position
ser.write(", 1000".encode())
time.sleep(0.5)

while ser.in_waiting:
    retrun = retrun + ser.read().decode("utf-8")
    #clear serial buffer

print(retrun)
retrun = ""

result  = sendImageCompressed(1, im,0) #compressed send
result += sendImageCompressed(2, im,0)  #standard send
result += sendImageCompressed(3, im,0)
result += sendImageCompressed(4, im,0)

if(result == 1):
    print("All sent successfully")
    time.sleep(0.1)
else:
    print("Error sending")
    ser.write("F".encode()) # error went on so turn board off
    

#close port
ser.close()

