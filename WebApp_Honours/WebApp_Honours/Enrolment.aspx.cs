using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace WebApp_Honours
{
    public partial class Enrolment : System.Web.UI.Page
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
                }
                else
                {
                    //Student Menu Options.
                }

                if (Session["enrolment_progress"] != null)
                {
                    if (Session["enrolment_progress"].ToString().Equals("1"))
                    {
                        step1_box.Style.Add("background-color", "green");
                        Step1.InnerText = "Step 1: Enrolment Process (Details) - Completed";

                        txtStudentNum.Disabled = true;
                        txtName.Disabled = true;
                        txtSurname.Disabled = true;
                        txtPassword.Disabled = true;
                        selectType.Disabled = true;
                        txtCardUID.Disabled = true;

                        btnComplete.BackColor = Color.Green;
                        btnComplete.Text = "Completed!";
                        btnComplete.Enabled = false;

                        btnCancel.Visible = true;
                    }
                }

                Name.InnerText += Session["username"].ToString();
            }
        }

        protected void btnComplete_Click(object sender, EventArgs e)
        {
            String studentNum = txtStudentNum.Value.ToString();
            String name = txtName.Value.ToString();
            String surname = txtSurname.Value.ToString();
            String password = txtPassword.Value.ToString();
            String type = selectType.Value.ToString();
            String cardUID = txtCardUID.Value.ToString();

            if (!studentNum.Equals("") && !name.Equals("") && !surname.Equals("") && !password.Equals("") && !cardUID.Equals(""))
            {
                //All of them have something.
                int result = objReference.enrolUser(studentNum, name, surname, password, type, cardUID);

                if (result == 0)
                {
                    step1_box.Style.Add("background-color", "green");
                    Step1.InnerText = "Step 1: Enrolment Process (Details) - Completed";

                    txtStudentNum.Style.Add("background-color", "white");
                    txtStudentNum.Style.Add("color", "black");
                    txtName.Style.Add("background-color", "white");
                    txtName.Style.Add("color", "black");
                    txtSurname.Style.Add("background-color", "white");
                    txtSurname.Style.Add("color", "black");
                    txtPassword.Style.Add("background-color", "white");
                    txtPassword.Style.Add("color", "black");
                    txtCardUID.Style.Add("background-color", "white");
                    txtCardUID.Style.Add("color", "black");

                    txtStudentNum.Disabled = true;
                    txtName.Disabled = true;
                    txtSurname.Disabled = true;
                    txtPassword.Disabled = true;
                    selectType.Disabled = true;
                    txtCardUID.Disabled = true;

                    btnComplete.BackColor = Color.Green;
                    btnComplete.Text = "Completed!";
                    btnComplete.Enabled = false;
                    
                    btnCancel.Visible = true;

                    btnRestart.Text = "Restart";
                    btnRestart.BackColor = Color.Blue;
                    btnRestart.Enabled = true;

                    Session["enrolment_progress"] = "1";

                    Session["currentEnrolment"] = studentNum;
                    Thread t1 = new Thread(new ThreadStart(StartLookingForIris));
                    t1.Start();
                }
                else
                {
                    if (result == 1)
                    {
                        //The person already exists in the db.
                        txtStudentNum.Value = "User Already Exists!";
                        txtName.Value = "";
                        txtSurname.Value = "";
                        txtPassword.Value = "";
                        selectType.SelectedIndex = 0;
                        txtCardUID.Value = "";
                    }
                }
            }
            else
            {
                //Highlight the ones that need to be entered in.
                if (studentNum.Equals(""))
                {
                    txtStudentNum.Style.Add("background-color", "red");
                    txtStudentNum.Style.Add("color", "white");
                }
                else
                {
                    txtStudentNum.Style.Add("background-color", "white");
                    txtStudentNum.Style.Add("color", "black");
                }

                if (name.Equals(""))
                {
                    txtName.Style.Add("background-color", "red");
                    txtName.Style.Add("color", "white");
                }
                else
                {
                    txtName.Style.Add("background-color", "white");
                    txtName.Style.Add("color", "black");
                }

                if (surname.Equals(""))
                {
                    txtSurname.Style.Add("background-color", "red");
                    txtSurname.Style.Add("color", "white");
                }
                else
                {
                    txtSurname.Style.Add("background-color", "white");
                    txtSurname.Style.Add("color", "black");
                }

                if (password.Equals(""))
                {
                    txtPassword.Style.Add("background-color", "red");
                    txtPassword.Style.Add("color", "white");
                }
                else
                {
                    txtPassword.Style.Add("background-color", "white");
                    txtPassword.Style.Add("color", "black");
                }

                if (cardUID.Equals(""))
                {
                    txtCardUID.Style.Add("background-color", "red");
                    txtCardUID.Style.Add("color", "white");
                }
                else
                {
                    txtCardUID.Style.Add("background-color", "white");
                    txtCardUID.Style.Add("color", "black");
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //CALL - call a function to delete the half enrolled user!
            objReference.removeUser(Session["currentEnrolment"].ToString());

            Cancel();
        }

        private void Cancel()
        {
            step1_box.Style.Remove("background-color");
            Step1.InnerText = "Step 1: Enrolment Process (Details)";

            txtStudentNum.Style.Add("background-color", "white");
            txtStudentNum.Style.Add("color", "black");
            txtName.Style.Add("background-color", "white");
            txtName.Style.Add("color", "black");
            txtSurname.Style.Add("background-color", "white");
            txtSurname.Style.Add("color", "black");
            txtPassword.Style.Add("background-color", "white");
            txtPassword.Style.Add("color", "black");
            txtCardUID.Style.Add("background-color", "white");
            txtCardUID.Style.Add("color", "black");

            txtStudentNum.Disabled = false;
            txtName.Disabled = false;
            txtSurname.Disabled = false;
            txtPassword.Disabled = false;
            selectType.Disabled = false;
            txtCardUID.Disabled = false;

            txtStudentNum.Value = "";
            txtName.Value = "";
            txtSurname.Value = "";
            txtPassword.Value = "";
            selectType.SelectedIndex = 0;
            txtCardUID.Value = "";

            btnComplete.BackColor = Color.Blue;
            btnComplete.Text = "Complete";
            btnComplete.Enabled = true;

            btnCancel.Visible = false;

            btnRestart.Text = "Cancel";
            btnRestart.BackColor = Color.Gray;
            btnRestart.Enabled = false;

            Session["enrolment_progress"] = "";
        }

        protected void btnRestart_Click(object sender, EventArgs e)
        {
            //CALL - call a function to delete the half enrolled user!
            objReference.removeUser(Session["currentEnrolment"].ToString());

            Cancel();
        }

        public void StartLookingForIris()
        {
            Console.WriteLine("Hello.");

            if (Session["currentEnrolment"] != null)
            {
                int count = 1;
                while (objReference.checkEnrolmentCompletion(Session["currentEnrolment"].ToString()).Equals("Details have been submitted. Awaiting iris enrolment."))
                {
                    Thread.Sleep(1000);

                    switch (count)
                    {
                        case 1:
                            lblWaiting.Text = "Awaiting Iris Scan.";
                            count++;
                            break;
                        case 2:
                            lblWaiting.Text = "Awaiting Iris Scan..";
                            count++;
                            break;
                        case 3:
                            lblWaiting.Text = "Awaiting Iris Scan...";
                            count = 1;
                            break;
                    }
                }
            }
        }
    }
}