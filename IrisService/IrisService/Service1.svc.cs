﻿using System;
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
        //0 - Successful
        //1 - Already exists in DB.
        public int enrolUser(String studentNum, String name, String surname, String password, String type, String cardUID)
        {
            if (doesUserExist(studentNum))
            {
                return 1;
            }

            DateTime date = DateTime.Now;

            cmd = new MySqlCommand("INSERT INTO usertb VALUES (@studentNum, @pass, @name, @surname, @type, @card, @date, 0);", connection);
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

            return 0;
        }

        //Step 2:
        //Enrols the student's iris
        //0 - Successful
        //1 - Already exists in DB.
        public int enrolUserIris(String cardUID, String irisHash)
        {
            String studentNum = "";

            //Get the student number.
            cmd = new MySqlCommand("SELECT * FROM usertb WHERE Card_UID = @cardUID AND Active = 0", connection);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@cardUID", cardUID);
            connection.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                //Getting the student number.
                studentNum = reader["StudentNumber"].ToString();
            }
            connection.Close();
            reader.Close();

            if (studentNum == "")
            {
                return 1;
            }

            cmd = new MySqlCommand("INSERT INTO usertb VALUES (@card, @irisHash);", connection);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@card", cardUID);
            cmd.Parameters.AddWithValue("@irisHash", irisHash);

            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();

            return 0;
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
        //Takes attendance using the cardUID
        public void takeAttendance(String cardUID)
        {
            String studentNum = "";

            //Get the student number.
            cmd = new MySqlCommand("SELECT * FROM usertb WHERE Card_UID = @cardUID AND Active = 0", connection);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@cardUID", cardUID);
            connection.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                //Getting the student number.
                studentNum = reader["StudentNumber"].ToString();
            }
            connection.Close();
            reader.Close();

            if (studentNum == "")
            {
                return;
            }

            //Todays date.
            String date = DateTime.Now.Date.ToString();

            //Takes attendance
            cmd = new MySqlCommand("INSERT INTO attendancetb VALUES (@studentNum, @date, 0);", connection);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@studentNum", studentNum);
            cmd.Parameters.AddWithValue("@date", date);

            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
        }

        //Update student's attendance
        //Admin
        public void updateStudentAttendance(String studentNum, String attendance)
        {
            cmd = new MySqlCommand("UPDATE attendancetb SET Attended = @attendance WHERE StudentNumber = @studentNum", connection);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@studentNum", studentNum);
            cmd.Parameters.AddWithValue("@attendance", attendance);

            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
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

        //Admin
        public void deactivateUser(String studentNum)
        {
            cmd = new MySqlCommand("UPDATE usertb SET Active = 1 WHERE StudentNumber = @studentNum", connection);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@studentNum", studentNum);
            
            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
        }

        //View Records.
        //Get the students that attended a certain day.
        public List<String> getStudents(String date)
        {
            List<String> students = new List<String>();

            cmd = new MySqlCommand("SELECT * FROM attendancetb WHERE AttendanceDate = @date ORDER BY StudentNumber ASC;", connection);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@date", date);
            connection.Open();

            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                //Add all the students to the list.
                students.Add(reader["StudentNumber"].ToString());
            }

            connection.Close();
            reader.Close();

            return students;
        }

        //Get all the dates with that a student has attended.
        public List<String> getDates(String studentNum)
        {
            List<String> dates = new List<String>();

            cmd = new MySqlCommand("SELECT DISTINCT AttendanceDate FROM attendancetb WHERE StudentNumber = @studentNum ORDER BY AttendanceDate DESC;", connection);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@studentNum", studentNum);
            connection.Open();

            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                //Add all the students to the list.
                dates.Add(reader["AttendanceDate"].ToString());
            }

            connection.Close();
            reader.Close();

            return dates;
        }

        //Get all the lectures that a lecturer has attended
        public List<Lecture> getLectures(string studentNum)
        {
            List<Lecture> lectures = new List<Lecture>();

            cmd = new MySqlCommand("SELECT * FROM lecturetb WHERE Lecturer_StudentNumber = @studentNum ORDER BY Lecture_ModuleCode ASC;", connection);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@studentNum", studentNum);
            connection.Open();

            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                //Add all the lectures to the list.
                lectures.Add(new Lecture(reader["Lecture_Name"].ToString(), reader["Lecture_AttendanceDate"].ToString(), reader["Lecturer_StudentNumber"].ToString(), reader["Lecture_ModuleCode"].ToString()));
            }

            connection.Close();
            reader.Close();

            return lectures;
        }

        //Get all the lectures that a student has attended.
        public List<Lecture> getStudentLectures(string studentNum)
        {
            List<Lecture> lectures = new List<Lecture>();

            cmd = new MySqlCommand("SELECT * FROM lecturetb, attendancetb WHERE attendancetb.StudentNumber = @studentNum AND attendancetb.AttendanceDate = lecturetb.Lecture_AttendanceDate ORDER BY Lecture_ModuleCode ASC;", connection);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@studentNum", studentNum);
            connection.Open();

            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                //Add all the lectures to the list.
                lectures.Add(new Lecture(reader["Lecture_Name"].ToString(), reader["Lecture_AttendanceDate"].ToString(), reader["Lecturer_StudentNumber"].ToString(), reader["Lecture_ModuleCode"].ToString()));
            }

            connection.Close();
            reader.Close();

            return lectures;
        }

        //Start a lecture.
        public void startLecture(string moduleCode, string lectureName, string lecturerStudentNum)
        {
            string date = DateTime.Now.Date.ToString();

            cmd = new MySqlCommand("INSERT INTO lecturetb VALUES (@moduleCode, @lectureName, @date, @lecturerStudentNum);", connection);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@moduleCode", moduleCode);
            cmd.Parameters.AddWithValue("@lectureName", lectureName);
            cmd.Parameters.AddWithValue("@date", date);
            cmd.Parameters.AddWithValue("@lecturerStudentNum", lecturerStudentNum);

            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
        }

        //Check if student has attended the latest lecture.
        //0 - Student has attended
        //1 - Student has not attended
        //2 - There is no lecture.
        public int checkStudentLatestLecture(string studentNum)
        {
            string today = DateTime.Now.Date.ToString();
            Lecture latestLecture = null;

            //Get the latest lecture.
            cmd = new MySqlCommand("SELECT * FROM lecturetb WHERE Lecture_AttendanceDate = @today ORDER BY LectureID DESC;", connection);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@today", today);
            connection.Open();

            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                //Get latest lecture.
                latestLecture = new Lecture(reader["Lecture_Name"].ToString(), reader["Lecture_AttendanceDate"].ToString(), reader["Lecturer_StudentNumber"].ToString(), reader["Lecture_ModuleCode"].ToString());
            }

            connection.Close();
            reader.Close();

            if (latestLecture == null)
            {
                return 2;
            }

            //If there is a lecture.
            //Check if the student has attended the lecture.
            cmd = new MySqlCommand("SELECT * FROM attendancetb WHERE AttendanceDate = @today AND StudentNumber = @studentNum ORDER BY LectureID DESC;", connection);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@today", today);
            cmd.Parameters.AddWithValue("@studentNum", studentNum);
            connection.Open();

            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                //There is a student.
                return 0;
            }

            connection.Close();
            reader.Close();

            return 1;
        }
    }
}
