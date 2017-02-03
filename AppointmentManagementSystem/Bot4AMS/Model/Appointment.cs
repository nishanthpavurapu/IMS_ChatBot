using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bot4AMS.Model
{
    public class CreateDeleteAppointmentModel
    {
        public string primary_amsid { get; set; }
        public string secondary_amsid { get; set; }
        public string dateOfAppointment { get; set; }
        public string timeSlot { get; set; }
    }

    public class EditAppointmentModel
    {
        public string primary_amsid { get; set; }
        public string secondary_amsid { get; set; }
        public string dateOfAppointment { get; set; }
        public string timeSlot { get; set; }
        public string changedDateOfAppointment { get; set; }
        public string changedTimeSlot { get; set; }
    }
}