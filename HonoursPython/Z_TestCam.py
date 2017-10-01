#OpenCV
import cv2

#Pi Camera Imports
from picamera.array import PiRGBArray
from picamera import PiCamera
import time

import RPi.GPIO as GPIO

#Need to turn on the LED
GPIO.setmode(GPIO.BOARD)
GPIO.setup(12, GPIO.OUT)
GPIO.output(12, GPIO.HIGH)

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

GPIO.output(12, GPIO.LOW)
GPIO.cleanup()