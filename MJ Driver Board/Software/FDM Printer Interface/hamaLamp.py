import serial
import time
 
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

ser_uvlamp = serial.Serial( port     = "COM6",
                            baudrate = 9600,
                            parity   = serial.PARITY_NONE,
                            stopbits = serial.STOPBITS_ONE,
                            bytesize = serial.EIGHTBITS)


lamp_power(50)
lamp_on()
lamp_off()


#lamp_on()

#lamp_off()
