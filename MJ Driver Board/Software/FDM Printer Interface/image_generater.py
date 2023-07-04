import numpy as np
from PIL import Image

NOZZLES = 1000

steps = 10

lengths = 100

ratio = 5

lengt = NOZZLES*ratio*2

array = np.ones([NOZZLES, lengt], dtype=np.uint8)*255
array[:,:lengths] = 0
for curPass in range(steps):
    for x in range(NOZZLES):
        if (x) % (steps) == curPass:
            star = (curPass+1) * lengths
            endd = star+lengths
            array[x,star:endd] = 0
            array[:,star:star+1] = 0
            



array[:,endd:endd+lengths] = 0

img = Image.fromarray(array)
img.save('PHNozzleTest_%d.bmp'%NOZZLES)
