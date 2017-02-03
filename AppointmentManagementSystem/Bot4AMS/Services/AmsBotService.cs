using AmsService;
using Bot4AMS.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace Bot4AMS.Services
{
    public class AmsBotService
    {
        //asyncTask<Appointment>
        public static async Task<string> CreateAppointment(string primaryamsid, string secondaryamsid, string dateofappointment, string timeslot)
        {
            string uri = $"http://localhost:57128/Service.svc/BookAppointment?primaryamsid={primaryamsid}&secondaryamsid={secondaryamsid}&dateofappointment={dateofappointment}&timeslot={timeslot}";

            using (var client = new WebClient())
            {
                var rawData = await client.DownloadStringTaskAsync(new Uri(uri));
                if (rawData.Contains("Success"))
                {
                    return "Appointment confirmed!";
                }
                else {
                    return "Appointment not confirmed!";
                }
            }
        }

        //asyncTask for Cancel Appointment
        public static async Task<string> CancelAppointment(string primaryamsid, string dateofappointment, string timeslot)
        {
            string uri = $"http://localhost:57128/Service.svc/CancelAppointment?primaryamsid={primaryamsid}&dateofappointment={dateofappointment}&timeslot={timeslot}";

            using (var client = new WebClient())
            {
                var rawData = await client.DownloadStringTaskAsync(new Uri(uri));
                if (rawData.Contains("Success"))
                {
                    return "Appointment Cancelled!";
                }
                else
                {
                    return "Appointment not found!";
                }
            }
        }

        //asyncTask for Cancel Appointment
        public static async Task<string> LoginUser(string primaryamsid, string password)
        {
            string uri = $"http://localhost:57128/Service.svc/LoginUser?primaryamsid={primaryamsid}&password={password}";

            using (var client = new WebClient())
            {
                var rawData = await client.DownloadStringTaskAsync(new Uri(uri));
                if (rawData.Contains("Success"))
                {
                    return "Login Success!";
                }
                else
                {
                    return "Login Failed!";
                }
            }
        }

        //asyncTask for Cancel Appointment
        public static async Task<string> UpdateAppointment(string primaryamsid, string dateofappointment, string timeslot,string newtimeslot)
        {
            string uri = $"http://localhost:57128/Service.svc/UpdateAppointment?primaryamsid={primaryamsid}&dateofappointment={dateofappointment}&oldtimeslot={timeslot}&newtimeslot={newtimeslot}";

            using (var client = new WebClient())
            {
                var rawData = await client.DownloadStringTaskAsync(new Uri(uri));
                if (rawData.Contains("Success"))
                {
                    return "Appointment Re-Scheduled!";
                }
                else
                {
                    return "Appointment not found!";
                }
            }
        }

        //asyncTask for View single appointment
        public static async Task<string> ViewAppointment(string primaryamsid, string dateofappointment, string timeslot)
        {
            string uri = $"http://localhost:57128/Service.svc/ViewUserAppointment?primaryamsid={primaryamsid}&dateofappointment={dateofappointment}&timeslot={timeslot}";

            using (var client = new WebClient())
            {
                string rawData = await client.DownloadStringTaskAsync(new Uri(uri));
                string JsonData = rawData.Replace(@"\", string.Empty);
                string final = JsonData.Trim().Substring(1, (JsonData.Length) - 2);
                List<Appointment> Appointments = JsonConvert.DeserializeObject<List<Appointment>>(final);

                string result="";
                if (Appointments.Count != 0)
                {
                    
                    foreach (Appointment appointment in Appointments)
                    {
                        result = "Your appointment on " + dateofappointment + " and " + timeslot + " is with " + appointment.secondaryamsid;
                    }
                }
                else
                {
                    result = "Sorry, No Appointment found!";
                }
                return result;
            }
        }

        //asyncTask for View multple appointment
        public static async Task<string> ViewAppointments(string primaryamsid, string dateofappointment)
        {
            string uri = $"http://localhost:57128/Service.svc/ViewUserAppointments?primaryamsid={primaryamsid}&dateofappointment={dateofappointment}";

            using (var client = new WebClient())
            {
                string rawData = await client.DownloadStringTaskAsync(new Uri(uri));
                string JsonData = rawData.Replace(@"\", string.Empty);
                string final = JsonData.Trim().Substring(1, (JsonData.Length) - 2);
                List<Appointment> Appointments = JsonConvert.DeserializeObject<List<Appointment>>(final);

                string result;
                if (Appointments.Count != 0)
                {
                    result = "Your Appointments for " + dateofappointment + " \n\n ................................................................. \n\n";
                    foreach (Appointment appointment in Appointments)
                    {
                        result = result + appointment.secondaryamsid + " - " + appointment.timeslot + " \n\n ";
                    }
                }
                else
                {
                    result = "Sorry, No Appointments found!";
                }
                return result;
            }
        }
    }
}