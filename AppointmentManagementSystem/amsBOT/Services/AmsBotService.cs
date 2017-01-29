using amsBOT.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace amsBOT.Services
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
            return null;
        }
    }
}