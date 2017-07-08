import cv2
import numpy as np

class TemplateGenerator:
        
        #Constructor method takes in the image of the iris that has been "rolled" out by the Feature Extractor.
        #Height is (for now) the height of the image which is (outer_radius - 15)
        def __init__(self, img, height):
            
            #An array of size 8 to get the iris-code.
            self.code = np.array([0, 7])
            
            #Gabor filter.
            gKernel = cv2.getGaborKernel((height, 360), 4.0, np.radians(180), 10.0, 0.5, 0, ktype=cv2.CV_32F)
            gKernel /= 1.5 * gKernel.sum()
            
            gOutput = np.zeros_like(img)
            filtered = cv2.filter2D(img, cv2.CV_8UC3, gKernel)
            np.maximum(gOutput, filtered, gOutput)
            
            #cv2.imshow('Gabor Filtered', gOutput)
            #cv2.waitKey(0)
            #cv2.destroyAllWindows()
            
            #Thinning
            iSize = np.size(gOutput)
            tOutput = np.zeros(gOutput.shape, np.uint8)
            
            ret,gOutput = cv2.threshold(gOutput, 50, 255, cv2.THRESH_BINARY)
            element = cv2.getStructuringElement(cv2.MORPH_CROSS, (3,3))
            done = False
            
            while(not done):
                
                eroded = cv2.erode(gOutput, element)
                temp = cv2.dilate(eroded, element)
                temp = cv2.subtract(gOutput, temp)
                tOutput = cv2.bitwise_or(tOutput, temp)
                gOutput = eroded.copy()
            
                total = iSize - cv2.countNonZero(gOutput)
                if total == iSize:
                    done = True
            
            #cv2.imshow('Thinned', tOutput)
            #cv2.waitKey(0)
            #cv2.destroyAllWindows()
            
            print 'Size:'
            print self.code.size
            
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
                mean = np.mean(cropped_image)
                
                print 'Mean:'
                print mean
                
                if mean > 0.5:
                    self.code[i - 1] = 1
                else:
                    self.code[i - 1] = 0