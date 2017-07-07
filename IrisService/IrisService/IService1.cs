using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace IrisService
{
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        [WebGet(UriTemplate = "/loginUser/{studentNum}/{password}",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        int loginUser(String studentNum, String password);

        [OperationContract]
        [WebGet(UriTemplate = "/startEnrolment",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        bool startEnrolment();

        [OperationContract]
        [WebGet(UriTemplate = "/endEnrolment",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        bool endEnrolment();

        [OperationContract]
        [WebGet(UriTemplate = "/getEnrolmentStatus",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        bool getEnrolmentStatus();

        [OperationContract]
        [WebGet(UriTemplate = "/enrolUser/{studentNum}/{name}/{surname}/{password}/{type}/{cardUID}",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        int enrolUser(String studentNum, String name, String surname, String password, String type, String cardUID);

        [OperationContract]
        [WebGet(UriTemplate = "/enrolUserIris/{cardUID}/{irisHash}",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        int enrolUserIris(String cardUID, String irisHash);

        [OperationContract]
        [WebGet(UriTemplate = "/checkEnrolmentCompletion/{studentNum}",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string checkEnrolmentCompletion(String studentNum);

        [OperationContract]
        [WebGet(UriTemplate = "/takeAttendance/{cardUID}",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        int takeAttendance(String cardUID);

        [OperationContract]
        [WebGet(UriTemplate = "/updateStudentAttendance/{studentNum}/{attendance}",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        void updateStudentAttendance(String studentNum, String attendance);

        [OperationContract]
        [WebGet(UriTemplate = "/doesUserExist/{studentNum}",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        bool doesUserExist(String studentNum);

        [OperationContract]
        [WebGet(UriTemplate = "/deactivateUser/{studentNum}",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        void deactivateUser(String studentNum);

        [OperationContract]
        [WebGet(UriTemplate = "/removeUser/{studentNum}",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        void removeUser(String studentNum);

        [OperationContract]
        [WebGet(UriTemplate = "/getAttendance/",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<String> getAttendance();

        [OperationContract]
        [WebGet(UriTemplate = "/getEnrolledStudents/",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<String> getEnrolledStudents();

        [OperationContract]
        [WebGet(UriTemplate = "/getStudents/{date}",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<String> getStudents(String date);

        [OperationContract]
        [WebGet(UriTemplate = "/getDates/{studentNum}",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<String> getDates(String studentNum);

        [OperationContract]
        [WebGet(UriTemplate = "/getLectures/{studentNum}",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<Lecture> getLectures(string studentNum);

        [OperationContract]
        [WebGet(UriTemplate = "/getStudentLectures/{studentNum}",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<Lecture> getStudentLectures(string studentNum);
        
        [OperationContract]
        [WebGet(UriTemplate = "/startLecture/{moduleCode}/{lectureName}/{lecturerStudentNum}",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        void startLecture(string moduleCode, string lectureName, string lecturerStudentNum);

        [OperationContract]
        [WebGet(UriTemplate = "/checkStudentLatestLecture/{studentNum}",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        int checkStudentLatestLecture(string studentNum);
    }

    [DataContract]
    public class Lecture
    {
        string name = "";
        string attendanceDate = "";
        string lecturer = "";
        string moduleCode = "";

        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [DataMember]
        public string AttendanceDate
        {
            get { return attendanceDate; }
            set { attendanceDate = value; }
        }

        [DataMember]
        public string Lecturer
        {
            get { return lecturer; }
            set { lecturer = value; }
        }

        [DataMember]
        public string ModuleCode
        {
            get { return moduleCode; }
            set { moduleCode = value; }
        }

        public Lecture(string name, string attendanceDate, string lecturer, string moduleCode)
        {
            this.name = name;
            this.attendanceDate = attendanceDate;
            this.lecturer = lecturer;
            this.moduleCode = moduleCode;
        }
    }
}
