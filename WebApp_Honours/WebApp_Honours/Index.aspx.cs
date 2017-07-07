using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApp_Honours
{
    public partial class Index : System.Web.UI.Page
    {
        IrisService.Service1Client objReference = new IrisService.Service1Client();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Name.InnerText.Equals(""))
            {
                if (Session["type"].ToString().Equals("Lecturer"))
                {
                    //Lecturer Menu Options.
                    MenuOptions.InnerHtml += "<li><a href=\"Enrolment.aspx\"><i class=\"icon-edit\"></i><span class=\"hidden-tablet\"> Enrolment</span></a></li>";
                    MenuOptions.InnerHtml += "<li><a href=\"ViewRecords.aspx\"><i class=\"halflings-icon white file\"></i><span class=\"hidden-tablet\"> View Records</span></a></li>";

                    lecturerForm.Style.Remove("visibility");
                }
                else
                {
                    //Student Menu Options.
                    MenuOptions.InnerHtml += "<li><a href=\"ViewAttendance.aspx\"><i class=\"halflings-icon white file\"></i><span class=\"hidden-tablet\"> View Attendance</span></a></li>";

                    studentForm.Style.Remove("visibility");

                    int result = objReference.checkStudentLatestLecture(Session["username"].ToString());

                    switch (result)
                    {
                        case 0:
                            //There is a lecture and student has attended.
                            attendanceIndex.InnerHtml = "<div class=\"alert alert-success \"> <button type=\"button\" class=\"close\" data-dismiss=\"alert\">×</button> <strong>Nice!</strong> You have successfully attended the lecture! </div>";
                            break;
                        case 1:
                            //There is a lecture but student hasn't attended.
                            attendanceIndex.InnerHtml = "<div class=\"alert alert-block \"> <button type=\"button\" class=\"close\" data-dismiss=\"alert\">×</button> <h4 class=\"alert-heading\" runat=\"server\" id=\"warningHeading\">There is a lecture!</h4> <p runat=\"server\" id=\"warningWriting\">You still need to attend the current lecture!</p> </div>";
                            break;
                        case 2:
                            //No lecture.
                            attendanceIndex.InnerHtml = "<div class=\"alert alert-error \"> <button type=\"button\" class=\"close\" data-dismiss=\"alert\">×</button> <strong>No lecture today!</strong> No need to worry about attendance today. Carry on studying! </div>";
                            break;
                    }
                }

                Name.InnerText += Session["username"].ToString();
            }
        }

        protected void btnStartLecture_ServerClick(object sender, EventArgs e)
        {
            startLecDiv.Style.Remove("visibility");
            Cancel();
        }

        protected void btnComplete_Click(object sender, EventArgs e)
        {
            String moduleCode = txtCode.Value.ToString();
            String lectureName = txtLectureName.Value.ToString();

            if (!moduleCode.Equals("") && !lectureName.Equals(""))
            {
                //If they aren't empty.
                objReference.startLecture(moduleCode, lectureName, Session["username"].ToString());

                step1_box.Style.Add("background-color", "green");
                Step1.InnerText = "Lecture has started!";

                btnGo.BackColor = Color.Green;
                btnGo.Text = "Started!";
                btnGo.Enabled = false;

                btnCancel.Visible = true;
                btnCancel.Text = "Restart";

                txtCode.Style.Add("background-color", "white");
                txtCode.Style.Add("color", "black");
                txtLectureName.Style.Add("background-color", "white");
                txtLectureName.Style.Add("color", "black");

                txtCode.Disabled = true;
                txtLectureName.Disabled = true;
            }
            else
            {
                if (moduleCode.Equals(""))
                {
                    txtCode.Style.Add("background-color", "red");
                    txtCode.Style.Add("color", "white");
                }
                else
                {
                    txtCode.Style.Add("background-color", "white");
                    txtCode.Style.Add("color", "black");
                }

                if (lectureName.Equals(""))
                {
                    txtLectureName.Style.Add("background-color", "red");
                    txtLectureName.Style.Add("color", "white");
                }
                else
                {
                    txtLectureName.Style.Add("background-color", "white");
                    txtLectureName.Style.Add("color", "black");
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //Cancel
            Cancel();
        }

        private void Cancel()
        {
            step1_box.Style.Remove("background-color");
            Step1.InnerText = "Start A Lecture! (Details)";

            btnGo.BackColor = Color.Blue;
            btnGo.Text = "Go!";
            btnGo.Enabled = true;

            btnCancel.Visible = false;

            txtCode.Style.Add("background-color", "white");
            txtCode.Style.Add("color", "black");
            txtLectureName.Style.Add("background-color", "white");
            txtLectureName.Style.Add("color", "black");

            txtCode.Disabled = false;
            txtLectureName.Disabled = false;
        }

        protected void btnCheckAttendance_ServerClick(object sender, EventArgs e)
        {

        }
    }
}