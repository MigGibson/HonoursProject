#To send a http RESTful request to service.
import urllib

class MatchingHam:
        
        #Constructor method takes in the hashed iris-code.
        def __init__(self, code, cardUID, size, answer):
            
            self.score = 0
            
            answer = np.array2string(self.code).replace(" ","")
            answer = answer.replace("[","")
            answer = answer.replace("]","")
            answer = answer.replace("."," ")
            answer = answer.replace("\n","")
            
            print answer
            
            #Run through the characters.
            for i in range(0, len(answer)):
                #Check if it's a match.
                if code[i] == answer[i]:
                    self.score += 1
            
            #If the score is above 80%
            if self.score >= (0.9 * size):
                #Then it's a match.
                self.outcome = "Match"
                #TODO Show rgb green
                print 'Student was matched.'
            else:
                #Then it's not a match
                #TODO rgb color must be red
                self.outcome = "No match"