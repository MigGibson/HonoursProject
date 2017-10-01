#OpenCV
import cv2

#NFC/RFID Imports
import RPi.GPIO as GPIO
import MFRC522

import HoughCircle

#Pi Camera Imports
from picamera.array import PiRGBArray
from picamera import PiCamera
import time

running = True

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
        print currentUID
        
        ###############################################################
        #Writing Data
        # This is the default key for authentication
        key = [0xFF,0xFF,0xFF,0xFF,0xFF,0xFF]
        
        #Select the scanned tag
        MIFAREReader.MFRC522_SelectTag(uid)

        #Authenticate
        status = MIFAREReader.MFRC522_Auth(MIFAREReader.PICC_AUTHENT1A, 8, key, uid)
        print "\n"

        #Check if authenticated
        if status == MIFAREReader.MI_OK:

            #Variable for the data to write
            data = []

            #Fill the data with 0xFF
            for x in range(0,16):
                data.append(0x00)

            print "Sector 8 looked like this:"
            #Read block 8
            MIFAREReader.MFRC522_Read(8)
            print "\n"

            print "Sector 8 will now be filled with 0xFF:"
            #Write the data
            MIFAREReader.MFRC522_Write(8, data)
            print "\n"

            print "It now looks like this:"
            #Check to see if it was written
            MIFAREReader.MFRC522_Read(8)
            print "\n"
        
        #Need to turn on the LED
        GPIO.setmode(GPIO.BOARD)
        GPIO.setup(12, GPIO.OUT)
        GPIO.output(12, GPIO.HIGH)
        
        #####################
        GPIO.setup(33, GPIO.OUT)
        GPIO.output(33, GPIO.LOW)
        time.sleep(1)
        GPIO.output(33, GPIO.HIGH)
        #####################
        
        #Pi Camera instantiation.
        piCam = PiCamera()
        
        #Raw capture is the RGB array of the pixels that the camera sees.
        rawCapture = PiRGBArray(piCam)
        
        #Need to give the camera time to take the image.
        time.sleep(0.1)
        
        #Captures the image and stores it.
        #piCam.capture(rawCapture, format="bgr")
        #image = rawCapture.array
        
        piCam.capture('cam_test.jpg')
        image = cv2.imread('cam_test.jpg', 0)
        
        time.sleep(2)
        
        GPIO.output(12, GPIO.LOW)
        
        running = True
        GPIO.cleanup()
        
        #cv2.imshow('Original Image', image)
        #cv2.waitKey(0)
        #cv2.destroyAllWindows()
        
        #Calls the constructor method in HoughCircle.py
        hCircle = HoughCircle.HoughCircle(image)
        
        cv2.imshow('Hough Circle', hCircle.img)
        cv2.waitKey(0)
        cv2.destroyAllWindows()