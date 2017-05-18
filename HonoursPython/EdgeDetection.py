import cv2

class EdgeDetection:

    #Constructor method that takes in a cropped image.
    def __init__(self, image):
        #The cropped image is already histogram equilized and should contain the iris.
        
        #We now want to apply CLAHE to illuminate the features that we want to extract.
		#I want to change this to add it inside Hough Circle or not use Histogram in Main function.
        CLAHE = cv2.createCLAHE()
        self.changedImage = CLAHE.apply(image)
        
        #After illuminating the features we need to apply the edge detection.
        #The two parameters are the thresholds of minValue and maxValue.
        self.changedImage = cv2.Canny(self.changedImage, 100, 200)