import cv2
import numpy as np

class TemplateGenerator:
        
        #Constructor method takes in the image of the iris that has been "rolled" out by the Feature Extractor.
        #Height is (for now) the height of the image which is (outer_radius - 15)
        def __init__(self, img, height):
            
            #An array of size 8 to get the iris-code.
            self.code = np.zeros((0, 7))
            
            #Gabor filter.
            kern = cv2.getGaborKernel((height, 360), 4.0, theta, 10.0, 0.5, 0, ktype=cv2.CV_32F)
            kern /= 1.5*kern.sum()
            
            accum = np.zeros_like(img)
            fimg = cv2.filter2D(img, cv2.CV_8UC3, kern)
            np.maximum(accum, fimg, accum)
            
            cv2.imshow('Part', cropped_image)
            cv2.waitKey(0)
            cv2.destroyAllWindows()
            
            #Break up the image into 8 parts (45 degrees of the circle).
            for i in range(1, 8):
                
                #Calculating the places to cut the image.
                prev = (i - 1) * 45
                next = ((i) * 45) - 1
                
                #Cutting the image.
                cropped_image = img[0:height - 1, prev:next]
                
                #cv2.imshow('Part', cropped_image)
                #cv2.waitKey(0)
                #cv2.destroyAllWindows()
                
                #Store the code after letting the mean go through checks.
                #self.code[i - 1] = 0