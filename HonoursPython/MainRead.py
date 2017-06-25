#OpenCV
import cv2

#HoughCircle
import HoughCircle

#NFC/RFID Imports
import RPi.GPIO as GPIO
import MFRC522

#Pi Camera Imports
from picamera.array import PiRGBArray
from picamera import PiCamera
import time

from PIL import Image

running = True
previousUID = ""

MIFAREReader = MFRC522.MFRC522()

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
    
        #If the previousUID is not the same then we can continue the capture process.
        #When the code reaches the end and the user has been authenticated then we will update the previous UID.
        if previousUID != currentUID:
            
            #Start the capture process.
            print currentUID
            previousUID = currentUID
            
            #Testing an image from UBIRIS
            image = cv2.imread('test.jpg', 0)
            
            #Pi Camera instantiation.
            #piCam = PiCamera()
            
            #Raw capture is the RGB array of the pixels that the camera sees.
            #rawCapture = PiRGBArray(piCam)
            
            #Need to give the camera time to take the image.
            #time.sleep(0.1)
            
            #Captures the image and stores it.
            #piCam.capture(rawCapture, format="bgr")
            #image = rawCapture.array
            
            #Calls the constructor method in HoughCircle.py
            hCircle = HoughCircle.HoughCircle(image)
            
            #Get the cropped image from the identified circle
            circle = houghCircle.circles[0]
            iris = houghCircle.img.crop((circle[0] - circle[2], circle[1] - circle[2], circle[0] + circle[2], circle[1] + circle[2]))
            
            #Display modified image.
            cv2.imshow('iris', iris)
            cv2.waitKey(0)
            cv2.destroyAllWindows()
            
            running = False