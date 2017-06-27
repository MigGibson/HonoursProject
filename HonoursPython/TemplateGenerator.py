import cv2
import numpy as np

class TemplateGenerator:
        
        #Constructor method takes in the image of the iris that has been "rolled" out by the Feature Extractor.
        #Height is (for now) the height of the image which is (outer_radius - 15)
        def __init__(self, img, height):
            
            #An array of size 8 to get the iris-code.
            self.code = np.zeros((0, 7))
            
            #Break up the image into 8 parts (45 degrees of the circle).
            for i in range(1, 8)
                
                #Calculating the places to cut the image.
                prev = (i - 1) * 45
                next = (i) * 45
                
                #Cutting the image.
                cropped_image = img[0:height, prev:next]
                
                print (np.mean(cropped_image))
                
                #Store the code after letting the mean go through checks.
                self.code[i - 1] = 0