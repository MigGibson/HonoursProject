#OpenCV
import cv2

#HoughCircle
import HoughCircleCam
import FeatureExtractCam
import TemplateGeneratorHam

#NFC/RFID Imports
import RPi.GPIO as GPIO
import MFRC522

#Pi Camera Imports
from picamera.array import PiRGBArray
from picamera import PiCamera
import time

from PIL import Image

#To send a http RESTful request to service.
import urllib

running = True
previousUID = ""
process = 0
data = None

MIFAREReader = MFRC522.MFRC522()

print "Waiting for RFID..."

while running:

    (status,TagType) = MIFAREReader.MFRC522_Request(MIFAREReader.PICC_REQIDL)
    
    #This allows the reading of the RFID/NFC card.
    #To get the unique identifier (UID).
    (status,uid) = MIFAREReader.MFRC522_Anticoll()
    
    #If the UID is able to be read.
    #Read the UID
    #Begin the processing of the iris.
    if status == MIFAREReader.MI_OK:
        
        #Check whether the previous UID matches the current.
        currentUID = str(uid[0]) + "," + str(uid[1]) + "," + str(uid[2]) + "," + str(uid[3])
    
        ########################################
        #Read data on the RFID card.
        #Need key for authentication.
        key = [0xFF,0xFF,0xFF,0xFF,0xFF,0xFF]
        
        #Select the tag.
        MIFAREReader.MFRC522_SelectTag(uid)
        
        #Authenticate
        status = MIFAREReader.MFRC522_Auth(MIFAREReader.PICC_AUTHENT1A, 8, key, uid)
        
        if status == MIFAREReader.MI_OK:
            data = MIFAREReader.MFRC522_Read(8)
            print data
            MIFAREReader.MFRC522_StopCrypto1()
        else:
            print "Authentication error"
            running = False
            GPIO.cleanup()
            break
        ########################################
    
        #If the previousUID is not the same then we can continue the capture process.
        #When the code reaches the end and the user has been authenticated then we will update the previous UID.
        if previousUID != currentUID:
            
            #Start the capture process.
            print currentUID
            previousUID = currentUID
            
            #Validate user and what their intended process is.
            
            ############################################
            #Initiates the opener to send the HTTP request.
            #opener = urllib.FancyURLopener({})
            # 
            #
            #Send the request.
            #request = opener.open("http://192.168.43.114:44556/Service1.svc/checkEnrolmentCompletion/" + currentUID)
            #response = request.read()
            #
            #If the response is not empty.
            #if response != "{\"checkEnrolmentCompletionResult\":\"\"}":
            #    answer = response[35:-2]
            #    
            #    #print answer
            #    
            #    if answer == "Enrolment Complete.":
            #        process = 0
            #        
            #    if answer == "Details have been submitted. Awaiting iris enrolment.":
            #        process = 1
            #        
            #    if process == -1:
            #        GPIO.cleanup()
            #        print 'No one found.'
            #        #TODO: Set the rgb light to red.
            #        break
            #
            ############################################
            
            #Testing an image from UBIRIS
            #image = cv2.imread('test.jpg', 0)
            
            ############################################
            #Need to turn on the LED
            #GPIO.setmode(GPIO.BOARD)
            #GPIO.setup(12, GPIO.OUT)
            #GPIO.output(12, GPIO.HIGH)
            
            #Pi Camera instantiation.
            #piCam = PiCamera()
            
            #Raw capture is the RGB array of the pixels that the camera sees.
            #rawCapture = PiRGBArray(piCam)
            
            #Need to give the camera time to take the image.
            #time.sleep(0.1)
            
            #Captures the image and stores it.
            #piCam.capture('cam_test.jpg')
            image = cv2.imread('cam_test.jpg', 0)
            
            #time.sleep(3)
            #
            #GPIO.output(12, GPIO.LOW)
            #
            #cv2.imshow('Original Image', image)
            #cv2.waitKey(0)
            #cv2.destroyAllWindows()
            
            ############################################
            
            #cv2.imwrite('Cam_Test.jpg', image)
            #image2 = cv2.imread('Cam_Test.jpg')
            
            #Calls the constructor method in HoughCircle.py
            hCircle = HoughCircleCam.HoughCircleCam(image)
            
            #cv2.imshow('Hough Circle', hCircle.img)
            #cv2.waitKey(0)
            #cv2.destroyAllWindows()
            
            #Get the cropped image from the identified circle
            fExtract = FeatureExtractCam.FeatureExtractCam(image, hCircle.circle)
            
            #Get the iris-code
            tGenerate = TemplateGeneratorHam.TemplateGeneratorHam(fExtract.rubber_output_image, 15, process, currentUID, data)
            
            #print tGenerate.outcome
            
            process = 1
            running = False
            GPIO.cleanup()