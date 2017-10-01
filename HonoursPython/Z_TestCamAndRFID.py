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

#Need to turn on the LED
GPIO.setmode(GPIO.BOARD)
GPIO.setup(12, GPIO.OUT)
GPIO.output(12, GPIO.HIGH)

print 'test 1'

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

print 'test 2'

image = cv2.imread('cam_test.jpg', 0)

print 'test 3'

time.sleep(2)

print 'test 4'

GPIO.output(12, GPIO.LOW)

GPIO.cleanup()

#cv2.imshow('Original Image', image)
#cv2.waitKey(0)
#cv2.destroyAllWindows()

print 'test 5'

#Calls the constructor method in HoughCircle.py
hCircle = HoughCircle.HoughCircle(image)

#cv2.imshow('Hough Circle', hCircle.img)
#cv2.waitKey(0)
#cv2.destroyAllWindows()

print 'test 6'