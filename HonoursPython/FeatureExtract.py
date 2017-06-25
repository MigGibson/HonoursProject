import cv2
import numpy as np

class FeatureExtract:
        
        #Constructor method takes in the image of the iris, and the circle (to get the center positions).
        def __init__(self, img, circle):
            print img.shape