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
                SqlCommand cmd = new SqlCommand("insert into Appointments values('" + primaryamsid + "','" + dateofappointment + "','" + secondaryamsid + "','" + timeslot + "')", con);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    return("Success");
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
            string cs = ConfigurationManager.ConnectionStrings["amsdbConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("delete from Appointments where primary_amsid = '" +  primaryamsid + "' and appointmentdate= '"+ dateofappointment + "' and slot = '" + timeslot + "'", con);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
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
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
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

        public string ViewUserAppointments(string primaryamsid, string dateofappointment)
        {

            List<Appointment> Appointments = new List<Appointment>();

            String cs = ConfigurationManager.ConnectionStrings["amsdbConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd_appointments = new SqlCommand("select * from Appointments where primary_amsid='" + primaryamsid + "' and appointmentdate='" + dateofappointment + "'", con);
                con.Open();
                SqlDataReader reader = cmd_appointments.ExecuteReader();

                while (reader.Read())
                {
                    Appointments.Add(new Appointment(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3)));         
                }
            }
           string AppointmentResultJson = JsonConvert.SerializeObject(Appointments);
           return AppointmentResultJson;
        }
    }
}
