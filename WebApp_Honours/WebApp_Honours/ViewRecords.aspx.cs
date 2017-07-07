using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApp_Honours
{
    public partial class ViewRecords : System.Web.UI.Page
    {
        IrisService.Service1Client objReference = new IrisService.Service1Client();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["type"].ToString().Equals("Lecturer"))
            {
                //Lecturer Menu Options.
                MenuOptions.InnerHtml += "<li><a href=\"Enrolment.aspx\"><i class=\"icon-edit\"></i><span class=\"hidden-tablet\"> Enrolment</span></a></li>";
                MenuOptions.InnerHtml += "<li><a href=\"ViewRecords.aspx\"><i class=\"halflings-icon white file\"></i><span class=\"hidden-tablet\"> View Records</span></a></li>";
            }
            else
            {
                //Student Menu Options.
            }

            Name.InnerHtml += Session["username"].ToString();

            //Get all the enrolled students.
            List<String> enrolledStudents = objReference.getEnrolledStudents().ToList();

            foreach (String student in enrolledStudents)
            {
                String[] details = student.Split(',');

                userTable.InnerHtml += "<tr> <td>" + details[0] + "</td> <td class=\"center\">" + details[1] + "</td> <td class=\"center\">" + details[2] + "</td> <td class=\"center\">" + details[3] + "</td> <td class=\"center\"><a class=\"btn btn-danger\" href=\"#\"><i class=\"halflings-icon white trash\"></i></a></td> </tr>";
            }

            //Get all the attendance of every student of every lecture.
            List<String> attendance = objReference.getAttendance().ToList();

            foreach (String student in attendance)
            {
                String[] details = student.Split(',');

                userAttendance.InnerHtml += "<tr> <td>" + details[0] + "</td> <td class=\"center\">" + details[1] + "</td> ";

                if (details[2].Equals("0"))
                {
                    userAttendance.InnerHtml += "<td><span class=\"label label-success\">Attended</span></td> ";
                }
                else
                {
                    userAttendance.InnerHtml += "<td><span class=\"label label-important\">Absent</span></td> ";
                }

                userAttendance.InnerHtml += "</tr>";
            }

            //Get all the lectures that this Lecturer has taken.
            List<IrisService.Lecture> lectures = objReference.getLectures(Session["username"].ToString()).ToList();

            foreach (IrisService.Lecture lecture in lectures)
            {
                lecturerLectures.InnerHtml += "<tr> <td>" + lecture.Name + "</td> <td class=\"center\">" + lecture.AttendanceDate + "</td> <td class=\"center\">" + lecture.ModuleCode + "</td></tr>";
            }
        }
    }
}