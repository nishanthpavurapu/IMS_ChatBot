using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;
using System.Configuration;
using System.Data.SqlClient;

namespace AmsService
{
    public class SendEmail
    {
        MailMessage mm;
        SmtpClient smtp;
        string mPrimaryEmail, mSecondaryEmail, mPrimaryUsername, mSecondaryUsername, mAppointmentInfo;

        public SendEmail(string primaryUsername, string secondaryUsername, string appointmentInfo)
        {
            mSecondaryUsername = secondaryUsername;
            mPrimaryUsername = primaryUsername;
            mAppointmentInfo = appointmentInfo;

            getUserEmails(mPrimaryUsername, mSecondaryUsername);

            mm = new MailMessage("bot4ams@gmail.com", mSecondaryEmail);
            mm.CC.Add(mPrimaryEmail);
            mm.IsBodyHtml = true;

            smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;

            NetworkCredential NetworkCred = new NetworkCredential("bot4ams@gmail.com", "testpassword");
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = NetworkCred;
            smtp.Port = 587;
        }

        public void getUserEmails(string primaryUsername, string secondaryUsername)
        {
            string cs = ConfigurationManager.ConnectionStrings["amsdbConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd_getUserEmails = new SqlCommand("select amsid,Email from Users where amsid IN('" + primaryUsername + "','" + secondaryUsername + "')", con);
                con.Open();

                SqlDataReader reader_primary = cmd_getUserEmails.ExecuteReader();

                if (reader_primary.HasRows)
                {
                    while (reader_primary.Read())
                    {
                        if (reader_primary["amsid"].ToString() == primaryUsername)
                        {
                            mPrimaryEmail = reader_primary["Email"].ToString();
                        }
                        else
                        {
                            mSecondaryEmail = reader_primary["Email"].ToString();
                        }
                    }
                }
                reader_primary.Close();
                con.Close();
            }
        }

        public void sendCreateConfirmation()
        {
            mm.Subject = "New AMS Appointment Confirmation!";
            mm.Body = "An AMS appointment has been successfully scheduled for <b>" + mPrimaryUsername + " </b> and <b>" + mSecondaryUsername + " </b>. Appointment time is <b>" + mAppointmentInfo + "</b>";
            smtp.Send(mm);
        }
        public void sendEditConfirmation()
        {
            mm.Subject = "AMS appointment re-scheduled!";
            mm.Body = "AMS Appointment with <b>" + mPrimaryUsername + " </b> and <b>" + mSecondaryUsername + " </b> has been successfully re-scheduled to <b>" + mAppointmentInfo + "</b>";
            smtp.Send(mm);
        }
        public void sendDeleteConfirmation()
        {
            mm.Subject = "AMS Appointment cancelled!";
            mm.Body = "AMS Appointment for <b>" + mPrimaryUsername + "</b> and <b>" + mSecondaryUsername + " </b> which was scheduled for <b>" + mAppointmentInfo + "</b> has been cancelled!";
            smtp.Send(mm);
        }
    }
}
