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
        void enrolUser(String studentNum, String name, String surname, String password, String type, String cardUID);

        [OperationContract]
        [WebGet(UriTemplate = "/enrolUserIris/{cardUID}/{irisHash}",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        void enrolUserIris(String cardUID, String irisHash);

        [OperationContract]
        [WebGet(UriTemplate = "/checkEnrolmentCompletion/{studentNum}",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string checkEnrolmentCompletion(String studentNum);

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
    }
}
