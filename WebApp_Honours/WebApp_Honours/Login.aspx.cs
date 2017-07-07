using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace WebApp_Honours
{
    public partial class Login : System.Web.UI.Page
    {
        IrisService.Service1Client objReference = new IrisService.Service1Client();

        protected void Page_Load(object sender, EventArgs e)
        {
            //Load
        }

        protected void btnLogin_Click1(object sender, EventArgs e)
        {
            String studentNum = txtStudentNum.Text.ToString();
            String password = txtPassword.Text.ToString();

            //////////////////////////
            if (!studentNum.Equals("") && !password.Equals(""))
            {
                int result = objReference.loginUser(studentNum, password);

                switch (result)
                {
                    case 0:
                        Session["type"] = "Student";
                        Session["username"] = txtStudentNum.Text.ToString();
                        Response.Redirect("Index.aspx");
                        break;
                    case 1:
                        Session["type"] = "Lecturer";
                        Session["username"] = txtStudentNum.Text.ToString();
                        Response.Redirect("Index.aspx");
                        break;
                    case 9:
                        txtStudentNum.BackColor = Color.Red;
                        txtStudentNum.ForeColor = Color.White;
                        txtPassword.BackColor = Color.Red;
                        txtPassword.ForeColor = Color.White;
                        break;
                }

                txtStudentNum.Text = "";
                txtPassword.Text = "";
            }
            else
            {
                if (studentNum.Equals(""))
                {
                    txtStudentNum.BackColor = Color.Red;
                    txtStudentNum.ForeColor = Color.White;
                }
                else
                {
                    txtStudentNum.BackColor = Color.White;
                    txtStudentNum.ForeColor = Color.Black;
                }

                if (password.Equals(""))
                {
                    txtPassword.BackColor = Color.Red;
                    txtPassword.ForeColor = Color.White;
                }
                else
                {
                    txtPassword.BackColor = Color.White;
                    txtPassword.ForeColor = Color.Black;
                }
            }
        }

    }
}