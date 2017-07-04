using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;

namespace IrisService
{
    public class Service1 : IService1
    {
        private MySqlConnection connection = new MySqlConnection("Server=localhost;Database=irisdb;Uid=root;Convert Zero Datetime=True;Allow Zero Datetime=True");
        private MySqlCommand cmd;
        private MySqlDataReader reader;

        private bool Enrol = false;

        //Login Processes
        public int loginUser(String studentNum, String password)
        {
            //Default value is being false.
            //9 = No match.
            //0 = Matched as student.
            //1 = Matched as lecturer.
            int result = 9;
            cmd = new MySqlCommand("SELECT * FROM usertb WHERE StudentNumber = @studentNum AND Password = @password AND Active=0", connection);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@studentNum", studentNum);
            cmd.Parameters.AddWithValue("@password", password);
            connection.Open();

            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                //Set to true because a match was found.
                if (reader["Type"].Equals("Student"))
                {
                    result = 0;
                }
                else
                {
                    result = 1;
                }
            }

            connection.Close();
            reader.Close();
            return result;
        }

        //Enrolment Processes
        public bool startEnrolment()
        {
            this.Enrol = true;
            return this.Enrol;
        }

        public bool endEnrolment()
        {
            this.Enrol = false;
            return this.Enrol;
        }

        public bool getEnrolmentStatus()
        {
            return this.Enrol;
        }

        //Step 1:
        //Enrols user with all their details.
        public void enrolUser(String studentNum, String name, String surname, String password, String type, String cardUID)
        {
            DateTime date = DateTime.Now;

            cmd = new MySqlCommand("INSERT INTO usertb VALUES ('@studentNum', '@pass', '@name', '@surname', '@type', '@card', '@date', 0);", connection);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@studentNum", studentNum);
            cmd.Parameters.AddWithValue("@pass", password);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@surname", surname);
            cmd.Parameters.AddWithValue("@type", type);
            cmd.Parameters.AddWithValue("@card", cardUID);
            cmd.Parameters.AddWithValue("@date", date.Date);

            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
        }

        //Step 2:
        //Enrols the student's iris
        public void enrolUserIris(String cardUID, String irisHash)
        {
            cmd = new MySqlCommand("INSERT INTO usertb VALUES ('@card', @irisHash);", connection);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@card", cardUID;
            cmd.Parameters.AddWithValue("@irisHash", irisHash);

            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
        }

        //Check the current enrolment status of a user.
        public string checkEnrolmentCompletion(String studentNum)
        {
            bool usertb = false;
            bool iristb = false;
            String cardUID = "";

            //Checking if the user is in the user table.
            cmd = new MySqlCommand("SELECT * FROM usertb WHERE StudentNumber = @studentNum AND Active = 0", connection);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@studentNum", studentNum);
            connection.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                //Return true because a match was found.
                usertb = true;
                cardUID = reader["Card_UID"].ToString();
            }
            connection.Close();
            reader.Close();

            if (cardUID == "")
            {
                return "No record of user!";
            }

            //Checking if the user is in the iris table.
            cmd = new MySqlCommand("SELECT * FROM iristb WHERE Card_UID = @cardUID", connection);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@cardUID", cardUID);
            connection.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                //Return true because a match was found.
                iristb = true;
            }
            connection.Close();
            reader.Close();

            if (usertb && iristb)
            {
                return "Enrolment Complete.";
            }

            if (usertb == true && iristb == false)
            {
                return "Details have been submitted. Awaiting iris enrolment.";
            }

            return "Unknown Error!";
        }

        //Attendance Process
        public int takeAttendance(String cardUID, String irisHash)
        {
            return 0;
        }

        public int takeAttendance(String cardUID, String secretHash)
        {
            return 0;
        }

        //User Management
        //Check to see if the user exists.
        public bool doesUserExist(String studentNum)
        {
            cmd = new MySqlCommand("SELECT * FROM usertb WHERE StudentNumber = @studentNum AND Active=0", connection);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@studentNum", studentNum);
            connection.Open();

            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                //Return true because a match was found.
                return true;
            }

            connection.Close();
            reader.Close();
            return false;
        }

        //Called by admin.
        public void deactivateUser(String studentNum)
        {
            cmd = new MySqlCommand("UPDATE usertb SET Active = 1 WHERE StudentNumber = @studentNum", connection);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@studentNum", studentNum);
            
            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
        }
    }
}
