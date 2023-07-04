import numpy as np
from PIL import Image


npi = 185

dist = 25.4 / npi
square = 1 / dist

ratio = (40 / 1)**(1/3)

spacx = 1+round(square*ratio)
spacy = spacx


lengt = 128*3

array = np.ones([128, lengt], dtype=np.uint8)*255

for x in range(128):
    if x % spacx == 0:
        array[x,:] = 0
        
for y in range(lengt):
    if y % spacy == 0:
        array[:,y] = 0


array[:,0] = 0
array[:,-1] = 0
array[0,:] = 0
array[-1,:] = 0

img = Image.fromarray(array)
img.save('Square_185_Scaled.bmp')
