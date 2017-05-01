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
    }
}
