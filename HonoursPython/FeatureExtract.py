import cv2
import numpy as np

class FeatureExtract:
        
        #Constructor method takes in the image of the iris, and the circle (to get the center positions).
        def __init__(self, img, circle):
            print 'Center(x): '
            print circle[0]
            print 'Center(y): '
            print circle[1]
            print 'Radius: '
            print circle[2]