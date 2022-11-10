#import matplotlib.pyplot as plt
#import matplotlib.animation as animation
#from matplotlib import style
import numpy as np
import random
import serial
import time

#initialize serial port
ser = serial.Serial()
ser.port = 'COM25' #Arduino serial port
ser.baudrate = 1000000
ser.timeout = 10 #specify timeout when using readline()
ser.open()
if ser.is_open==True:
	print("\nAll right, serial port now open. Configuration:\n")
	print(ser, "\n") #print serial parameters


#ser.write('O'.encode())
#ser.write('T 1 25'.encode())
#ser.write('T 2 30'.encode())
#ser.write('T 3 35'.encode())
#ser.write('T 4 40'.encode())

time.sleep(3)

while ser.in_waiting:
        print(ser.readline())

# Create figure for plotting
#fig = plt.figure()
#ax = fig.add_subplot(1, 1, 1)
xs = [] #store trials here (n)
t1s = [] #store relative frequency here
t2s = [] #for theoretical probability
t3s = [] #for theoretical probability
t4s = [] #for theoretical probability

s1s = [] #store relative frequency here
s2s = [] #for theoretical probability
s3s = [] #for theoretical probability
s4s = [] #for theoretical probability

# This function is called periodically from FuncAnimation
def animate(i, xs, t1s, t2s, t3s, t4s, s1s, s2s, s3s, s4s):

    #Aquire and parse data from serial port
    ser.write('t'.encode())
    line=ser.readline()      #ascii
    #print(line)
    line=ser.readline()      #ascii
    line= line + ser.readline()      #ascii
    line= line + ser.readline()      #ascii
    line= line + ser.readline()      #ascii
    #print(line)
    line_as_list = line.split(b' ')

    #print(line_as_list)

    #for el in line_as_list:
    #    print(el)
    #    print()
    
    t1 = float(line_as_list[4][:-6])
    t2 = float(line_as_list[8][:-6])
    t3 = float(line_as_list[12][:-6])
    t4 = float(line_as_list[16][:-2])
    
    s1 = float(line_as_list[3])
    s2 = float(line_as_list[7])
    s3 = float(line_as_list[11])
    s4 = float(line_as_list[15])

    if t1 < 0:
        t1 = 0
    
    if t2 < 0:
        t2 = 0
        
    if t3 < 0:
        t3 = 0
    
    if t4 < 0:
        t4 = 0
    
    
    i = i + 1
	
	# Add x and y to lists
    xs.append(i)
    
    t1s.append(t1)
    t2s.append(t2)
    t3s.append(t3)
    t4s.append(t4)
    
    s1s.append(s1)
    s2s.append(s2)
    s3s.append(s3)
    s4s.append(s4)

    # Limit x and y lists to 20 items
    #xs = xs[-20:]
    #ys = ys[-20:]

    # Draw x and y lists
    #ax.clear()
    
    #ax.plot(xs, t1s, label="T1")
    #ax.plot(xs, s1s, label="Set T1")
    
    #ax.plot(xs, t2s, label="T2")
    #ax.plot(xs, s2s, label="Set T2")
    
    #ax.plot(xs, t3s, label="T3")
    #ax.plot(xs, s3s, label="Set T3")
    
    #ax.plot(xs, t4s, label="T4")    
    #ax.plot(xs, s4s, label="Set T4")

    # Format plot
    #plt.xticks(rotation=45, ha='right')
    #plt.subplots_adjust(bottom=0.30)
    #plt.ylabel('Temperature (C)')
    #plt.legend()
    #plt.axis([1, None, 0, 1.1]) #Use for arbitrary number of trials
    #plt.axis([1, 100, 0, 1.1]) #Use for 100 trial demo
    print("%f\t%f\t%f\t%f"%(t1,t2,t3,t4))

# Set up plot to call animate() function periodically
#ani = animation.FuncAnimation(fig, animate, fargs=(xs, t1s, t2s, t3s, t4s, s1s, s2s, s3s, s4s), interval=100)
#plt.show()
while 1:
        animate(1, xs, t1s, t2s, t3s, t4s, s1s, s2s, s3s, s4s)
        time.sleep(0)
        
