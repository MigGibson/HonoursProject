import cv2
import numpy as np

#To send a http RESTful request to service.
import urllib

#To match
import Matching

class TemplateGenerator:
        
        #Constructor method takes in the image of the iris that has been "rolled" out by the Feature Extractor.
        #Height is (for now) the height of the image which is (outer_radius - 15)
        def __init__(self, img, height, typeOfProcess, cardUID):
            
            #An array of size 8 to get the iris-code.
            self.divisionSize = 24
            self.code = np.zeros(self.divisionSize)
            
            #Gabor filter.
            #The gabor filter highlights parallel lines according to the angle
            #Parameters:
            #ksize = Size of the filter (Shape of the input image)
            #sigma = Standard Deviation
            #theta = Orientation of the parallel stripes
            #lambd = Wavelength of the sin factor
            #gamma = Spatial aspect ratio
            #psi = Phase offset
            #ktype = Type of filter coefficients CV_32F or CV64F
            gKernel = cv2.getGaborKernel((height, 360), 4.0, np.radians(180), 10.0, 0.5, 0, ktype=cv2.CV_32F)
            #Not sure why /1.5 (Need to ask)
            gKernel /= 1.5 * gKernel.sum()
            
            gOutput = np.zeros_like(img)
            #Applies the kernel to an image.
            filtered = cv2.filter2D(img, cv2.CV_8UC3, gKernel)
            np.maximum(gOutput, filtered, gOutput)
            
            cv2.imshow('Gabor Filtered', gOutput)
            cv2.waitKey(0)
            cv2.destroyAllWindows()
            
            #Thinning
            iSize = np.size(gOutput)
            tOutput = np.zeros(gOutput.shape, np.uint8)
            
            ret,gOutput = cv2.threshold(gOutput, 75, 255, cv2.THRESH_BINARY)
            element = cv2.getStructuringElement(cv2.MORPH_CROSS, (3,3))
            done = False
            
            cv2.imshow('Binarized', gOutput)
            cv2.waitKey(0)
            cv2.destroyAllWindows()
            
            while(not done):
                
                #Two common morphology operators:
                #Erosion takes the minimal pixel value and replaces it's surroundings with that minimal pixel value.
                eroded = cv2.erode(gOutput, element)
                #Dilation takes the maximal pixel value and replaces the surroundings with that maximal pixel value.
                temp = cv2.dilate(eroded, element)
                #Subtract "removes" the found edges and reduces the main image.
                temp = cv2.subtract(gOutput, temp)
                tOutput = cv2.bitwise_or(tOutput, temp)
                gOutput = eroded.copy()
                
                total = iSize - cv2.countNonZero(gOutput)
                if total == iSize:
                    done = True
                
            cv2.imshow('Thinned', tOutput)
            cv2.waitKey(0)
            cv2.destroyAllWindows()
            
            #Break up the image into 8 parts (45 degrees of the circle).
            for i in range(1, self.divisionSize + 1):
                
                #Calculating the places to cut the image.
                prev = (i - 1) * (360 / self.divisionSize)
                next = ((i) * (360 / self.divisionSize)) - 1
                
                #Cutting the image.
                cropped_image = img[0:height - 1, prev:next]
                
                #cv2.imshow('Part', cropped_image)
                #cv2.waitKey(0)
                #cv2.destroyAllWindows()
                
                #Store the code after letting the mean go through checks.
                mean = np.mean(cropped_image)
                
                #print 'Mean:'
                #print mean
                
                if mean > 100:
                    self.code[i - 1] = 1
                else:
                    self.code[i - 1] = 0
            
            self.temp = np.array2string(self.code).replace(" ","_")
            self.temp = self.temp.replace("[","_")
            self.temp = self.temp.replace("]","_")
            self.temp = self.temp.replace(".","_")
            self.temp = self.temp.replace("_","")
            self.temp = self.temp.replace("\n","")
            
            #print "http://192.168.0.19:44556/Service1.svc/enrolUserIris/" + cardUID + "/" + self.temp
            
            #Check whether we should match or enrol the student.
            #0 = Match
            #1 = Enrol
            if typeOfProcess == 0:
                #Match the code.
                matching = Matching.Matching(self.temp, cardUID, self.divisionSize)
                
                self.outcome = matching.outcome
            else:
                #Enrol the student.
                #Initiates the opener to send the HTTP request.
                opener = urllib.FancyURLopener({})
                
                #Send the request.
                #request = opener.open("http://192.168.0.19:44556/Service1.svc/enrolUserIris/" + cardUID + "/" + self.temp)
                request = opener.open("http://192.168.43.114:44556/Service1.svc/enrolUserIris/" + cardUID + "/" + self.temp)
                response = request.read()
                
                #If the response is not empty.
                if response != "{\"enrolUserIrisResult\":\"\"}":
                    answer = response[23:-1]
                    
                    #TODO set rgb light to:
                    #0 = Blue
                    #1 = Green and Blue
                    if answer == "0":
                        self.outcome = "Enrolled Successfully!"
                    if answer == "1":
                        self.outcome = "User already exists!"
                    if answer == "2":
                        self.outcome = "User has the same iris as someone else!"