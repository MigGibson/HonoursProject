#This imports from HoughCircle
from user import HoughCircle
from user import EdgeDetection
from picamera.array import PiRGBArray
from picamera import PiCamera
import time
import urllib2
import requests
from PIL import Image

#Get image from the camera.
piCam = PiCamera()

#Raw capture is the RGB array of the pixels that the camera sees
rawCapture = PiRGBArray(piCam)

#Need to give the camera time to take the image.
time.sleep(0.1)

#Captures the image and stores it.
piCam.capture(rawCapture, format="bgr")
image = rawCapture.array

#Calls the constructor method in HoughCircle.py
houghCircle = HoughCircle(image)

#Get the cropped image from the identified circle
circle = houghCircle.circles[0]
iris = houghCircle.cimg.crop((circle[0] - circle[2], circle[1] - circle[2], circle[0] + circle[2], circle[1] + circle[2]))

#Histogram Equalize the cropped iris.
cv2.equalizeHist(iris, iris2)

#Feature Extraction
edge = EdgeDetection(iris2)
iris3 = edge.changedImage

#Changed now to only send generated template codes not whole image.

#Send image to service.
#url = 'http://192.168.43.193:9876/Service1.svc/UploadImage/post'
#files = {'file': open('iris.jpg', 'rb')}

#Get response.
#r = requests.post(url, files=files)
#r.text
