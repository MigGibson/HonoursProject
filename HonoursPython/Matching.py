#To send a http RESTful request to service.
import urllib

class Matching:
        
        #Constructor method takes in the hashed iris-code.
        def __init__(self, code, cardUID, size):
            
            self.score = 0
            
            #Need to get the iris template from the DB.
            #Initiates the opener to send the HTTP request.
            opener = urllib.FancyURLopener({})
            
            #Send the request.
            request = opener.open("http://192.168.0.19:44556/Service1.svc/getIrisHash/" + cardUID)
            response = request.read()
            
            #If the response is not empty.
            if response != "{\"getIrisHash\":\"\"}":
                answer = response[17:-2]
                
                #Run through the characters.
                for i in range(0, len(answer) - 1):
                    print i
                    
                    #If any are a 1 or 0
                    if answer[i] == '1' or answer[i] == '0':
                        #Then we know its part of the code.
                        #Check if it's a match.
                        if code[i] == answer[i]:
                            self.score += 1
                
                #If the score is above 80%
                if self.score >= (0.8 * size):
                    #Then it's a match.
                    self.outcome = "Match"
                    
                    #Attendance must then be recorded.
                    #Initiates the opener to send the HTTP request.
                    opener = urllib.FancyURLopener({})
                    
                    #Send the request.
                    request = opener.open("http://192.168.0.19:44556/Service1.svc/takeAttendance/" + cardUID)
                    response = request.read()
                    
                    #If the response is not empty.
                    if response != "{\"takeAttendance\":\"\"}":
                        answer = response[20:-2]
                    
                        if answer == '0':
                            #TODO Show rgb green
                            print 'RGB Green'
                        else:
                            #TODO Show rgb red
                            print 'RGB Red'
                    
                else:
                    #Then it's not a match
                    #TODO rgb color must be red
                    self.outcome = "No match"