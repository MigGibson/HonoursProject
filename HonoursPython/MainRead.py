import RPi.GPIO as GPIO
import MFRC522

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