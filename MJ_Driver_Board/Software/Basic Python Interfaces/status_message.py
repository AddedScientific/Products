import numpy
import serial
import math
import random
import time
import json
import sys
import os

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



print("Please enter com port number (only number)")
ports = serial_ports()
print(ports)

if len(ports) == 1:
    comport = ports[0]
else:
    print("Please enter com port number (only number)")
    print(ports)
    comport = "COM" + input()


#create empty string for reply
retrun = ""


#if error on above line check bmp is binary

#open comport at correct baud
ser = serial.Serial(comport, 1000000)

while True:             # Loop continuously
    ser.write(("b\n").encode())

    time.sleep(0.01)
    retrun = ""
    while ser.in_waiting:
        retrun = retrun + ser.read().decode("utf-8")

    
    os.system('cls' if os.name == 'nt' else 'clear')
    #print(retrun)
    data = json.loads(retrun)
    print(data)
    #print(json.dumps(data, indent=4, sort_keys=True))
    time.sleep(1)
    

    
