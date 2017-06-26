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
            
            outer_radius = circle[2]
            
            #Apply histogram equalization to get the accuracy of the features.
            self.histo_img = cv2.equalizeHist(img)
            
            #Output array[radius][360 degrees].
            #Our new feature extracted image.
            output_image = np.zeros((outer_radius - 15, 360))
            
            #For each "row"/"ring"
            for r in range(15, outer_radius):
                #all 360 degrees around the center
                for degree in range(0, 360):
                    rad = np.radians(degree)
                    #Get the x and y of each point on the ring at 360 degrees
                    #Formulas are:
                        #x = a + r*cos(theta)
                        #y = b + r*sin(theta)
                    new_x = circle[0] + (r * np.cos(rad))
                    new_y = circle[1] + (r * np.sin(rad))
                    
                    #Add the new point to the output image.
                    output_image[r - 15][degree] = self.histo_img[new_y][new_x]
            
            self.rubber_output_image = np.array(output_image, dtype = np.uint8)
            #Display modified image.
            #cv2.imshow('Feature Extraction', self.rubber_output_image)
            #cv2.waitKey(0)
            #cv2.destroyAllWindows()