import cv2
import numpy as np
from picamera.array import PiRGBArray
from picamera import PiCamera

from PIL import Image

class HoughCircle:
        
        #Constructor method takes in the image.
        def __init__(self, img):
            #Get the image.
            self.img = img
            
            #Removes noise.
            self.img = cv2.medianBlur(self.img, 5)
            
            self.img = Image.fromarray(np.uint8(self.img))
            
            #Grayscales image.
            self.cimg = cv2.cvtColor(self.img, cv2.COLOR_GRAY2BGR)
            
            #Identify the circles. HoughCircles(image, detection method, inverse ratio, min distance between detected centers, upper threshold, center threshold, min radius, max radius) 
            self.circles = cv2.HoughCircles(self.img, cv2.HOUGH_GRADIENT, 1, 5, 50, 30, 0, 0)
            self.circles = np.uint16(np.around(circles))
            
            #Display the circles.
            for i in self.circles[0,:]:
                #Draw outer circle.
                #i[0] is x of center
                #i[1] is y of center
                #i[2] is radius
                cv2.circle(cimg,(i[0],i[1]),i[2],(0,255,0),2)
            
            #Display modified image.
            cv2.imshow('detected circles',cimg)
            cv2.waitKey(0)
            cv2.destroyAllWindows()