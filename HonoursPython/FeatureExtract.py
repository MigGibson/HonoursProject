import cv2
import numpy as np

class FeatureExtract:
        
        #Constructor method takes in the image of the iris, and the circle (to get the center positions).
        def __init__(self, img, circle):
            print 'Center(x): ' + circle[0]
            print 'Center(y): ' + circle[1]
            print 'Radius: ' + circle[2]