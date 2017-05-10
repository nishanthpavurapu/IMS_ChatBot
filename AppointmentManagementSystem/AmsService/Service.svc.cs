using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using Newtonsoft.Json;

namespace AmsService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service : IService
    {
        public string BookAppointment(string primaryamsid, string secondaryamsid, string dateofappointment, string timeslot)
        {
            string cs = ConfigurationManager.ConnectionStrings["amsdbConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("insert into Appointments values('" + primaryamsid + "','" + dateofappointment + "','" + secondaryamsid + "','" + timeslot + "','agenda')", con);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    //Sending Confirmation Email
                    SendEmail obj = new SendEmail(primaryamsid, secondaryamsid, dateofappointment + ", " + timeslot);
                    obj.sendCreateConfirmation();
                    return ("Success");
                }
                catch (SqlException ex)
                {
                    if (ex.ErrorCode == 2627)
                    {
                        return("Failed");
                    }
                }
            }
            return null;
        }

        public string CancelAppointment(string primaryamsid, string dateofappointment, string timeslot)
        {
            string secondaryamsid="";

            string cs = ConfigurationManager.ConnectionStrings["amsdbConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("delete from Appointments where primary_amsid = '" + primaryamsid + "' and appointmentdate= '" + dateofappointment + "' and slot = '" + timeslot + "'", con);

                SqlCommand cmd_getsecondayuser = new SqlCommand("select secondary_amsid from Appointments where primary_amsid = '" +  primaryamsid + "' and appointmentdate= '"+ dateofappointment + "' and slot = '" + timeslot + "'", con);
                
                try
                {
                    con.Open();

                    //Getting the secondary user name
                    SqlDataReader reader = cmd_getsecondayuser.ExecuteReader();
                    while (reader.Read())
                    {
                        secondaryamsid = reader.GetString(0);
                    }
                    reader.Close();

                    cmd.ExecuteNonQuery();
                    con.Close();

                    //Sending Confirmation Email
                    SendEmail obj = new SendEmail(primaryamsid, secondaryamsid, dateofappointment + ", " + timeslot);
                    obj.sendDeleteConfirmation();

                    return "Success";
                }
                catch (SqlException ex)
                {
                    if (ex.ErrorCode == 2627)
                    {
                        return "Failed";
                    }
                }
            }
            return null;
        }

        public CompositeType BookAppointmentJsonObj(string primaryamsid, string secondaryamsid, string dateofappointment, string timeslot)
        {
            CompositeType ct = new CompositeType();
            ct.BoolValue = true;
            ct.StringValue = "Nishanth";
            return ct;
        }



        public string GetData(string value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        public string UpdateAppointment(string primaryamsid, string secondaryamsid, string dateofappointment, string oldtimeslot, string newtimeslot)
        {
            string cs = ConfigurationManager.ConnectionStrings["amsdbConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("update Appointments set slot='" + newtimeslot + "' where primary_amsid='" + primaryamsid + "' and appointmentdate='" + dateofappointment + "' and slot='" + oldtimeslot + "'", con);
                if (secondaryamsid == "" || secondaryamsid == null)
                {
                    SqlCommand cmd_getsecondayuser = new SqlCommand("select secondary_amsid from Appointments where primary_amsid = '" + primaryamsid + "' and appointmentdate= '" + dateofappointment + "' and slot = '" + oldtimeslot + "'", con);
                    con.Open();
                    // Getting the secondary user name
                    SqlDataReader reader = cmd_getsecondayuser.ExecuteReader();
                    while (reader.Read())
                    {
                        secondaryamsid = reader.GetString(0);
                    }
                    reader.Close();
                    con.Close();
                }
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    //Sending Confirmation Email
                    SendEmail obj = new SendEmail(primaryamsid, secondaryamsid, dateofappointment + ", " + newtimeslot);
                    obj.sendEditConfirmation();

                    return "Success";
                }
                catch (SqlException ex)
                {
                    if (ex.ErrorCode == 2627)
                    {
                        return "Failed";
                    }
                }
            }
            return null;
        }

        public string ViewUserAppointments(string primaryamsid, string dateofappointment, string weekstartdate, string weekenddate, string monthstartdate, string monthenddate)
        {
            string userCondition = "(primary_amsid='" + primaryamsid + "' or secondary_amsid='" + primaryamsid + "')";
            string dateCondition = "and (cast(appointmentdate as date) = '" + dateofappointment + "')";
            string dateRangeCondition = "and (cast(appointmentdate as date) between '"+ weekstartdate + "' and '" + weekenddate + "')";
            string monthRangeCondition = "and (cast(appointmentdate as date) between '" + monthstartdate + "' and '" + monthenddate + "')"; 

            if ((weekenddate == null || weekenddate == "none") || (weekstartdate == null || weekstartdate == "none"))
            {
                dateRangeCondition = "";
            }
            if ( dateofappointment == null || dateofappointment == "none")
            {
                dateCondition = "";
            }
            if ((monthenddate == null || monthenddate == "none") || (monthstartdate == null || monthstartdate == ""))
            {
                monthRangeCondition = "";
            }

            List<Appointment> Appointments = new List<Appointment>();

            String cs = ConfigurationManager.ConnectionStrings["amsdbConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd_appointments = new SqlCommand("select * from Appointments where "+ userCondition +  dateCondition + dateRangeCondition + monthRangeCondition, con);
                con.Open();
                SqlDataReader reader = cmd_appointments.ExecuteReader();

                while (reader.Read())
                {
                    Appointments.Add(new Appointment(reader.GetString(0), reader.GetString(2), reader.GetString(1), reader.GetString(3)));         
                }
            }
           string AppointmentResultJson = JsonConvert.SerializeObject(Appointments);
           return AppointmentResultJson;
        }

        public string ViewUserAppointment(string primaryamsid, string dateofappointment,string timeslot)
        {

            List<Appointment> Appointments = new List<Appointment>();

            String cs = ConfigurationManager.ConnectionStrings["amsdbConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd_appointments = new SqlCommand("select * from Appointments where primary_amsid='" + primaryamsid + "' and appointmentdate='" + dateofappointment + "' and slot='"+timeslot+"' ", con);
                con.Open();
                SqlDataReader reader = cmd_appointments.ExecuteReader();

                while (reader.Read())
                {
                    Appointments.Add(new Appointment(reader.GetString(0), reader.GetString(2), reader.GetString(1), reader.GetString(3)));
                }
            }
            string AppointmentResultJson = JsonConvert.SerializeObject(Appointments);
            return AppointmentResultJson;
        }

        public string LoginUser(string primaryamsid, string password)
        {
            String cs = ConfigurationManager.ConnectionStrings["amsdbConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd_login = new SqlCommand("select amsid from Users where amsid='" + primaryamsid + "' and password='" + password + "'", con);
                con.Open();
                SqlDataReader reader = cmd_login.ExecuteReader();

                if (reader.Read())
                {
                    return "Success";
                }
                else
                {
                    return "Fail";
                }
            }
        }
    }
}
