using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApp_Honours
{
    public partial class ViewAttendance : System.Web.UI.Page
    {
        IrisService.Service1Client objReference = new IrisService.Service1Client();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Name.InnerText.Equals(""))
            {
                if (Session["type"].ToString().Equals("Student"))
                {
                    //Student Menu Options.
                    MenuOptions.InnerHtml += "<li><a href=\"ViewAttendance.aspx\"><i class=\"halflings-icon white file\"></i><span class=\"hidden-tablet\"> View Attendance</span></a></li>";
                }

                Name.InnerText += Session["username"].ToString();

                //Get all the attendance of this student of every lecture.
                List<String> attendance = objReference.getDates(Session["username"].ToString()).ToList();

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
            }
        }
    }
}