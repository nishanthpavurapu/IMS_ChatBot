using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace AmsService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService
    {

        [OperationContract]
        [WebGet(RequestFormat =WebMessageFormat.Json,ResponseFormat =WebMessageFormat.Json,UriTemplate ="/GetData/{value}")]
        string GetData(string value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/BookAppointment?primaryamsid={primaryamsid}&secondaryamsid={secondaryamsid}&dateofappointment={dateofappointment}&timeslot={timeslot}")]
        string BookAppointment(string primaryamsid, string secondaryamsid, string dateofappointment, string timeslot);

        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/CancelAppointment?primaryamsid={primaryamsid}&dateofappointment={dateofappointment}&timeslot={timeslot}")]
        string CancelAppointment(string primaryamsid, string dateofappointment, string timeslot);

        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/UpdateAppointment?primaryamsid={primaryamsid}&secondaryamsid={secondaryamsid}&dateofappointment={dateofappointment}&oldtimeslot={oldtimeslot}&newtimeslot={newtimeslot}")]
        string UpdateAppointment(string primaryamsid, string secondaryamsid, string dateofappointment, string oldtimeslot, string newtimeslot);

        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/ViewUserAppointments?primaryamsid={primaryamsid}&dateofappointment={dateofappointment}&weekstartdate={weekstartdate}&weekenddate={weekenddate}&monthstartdate={monthstartdate}&monthenddate={monthenddate}")]
        string ViewUserAppointments(string primaryamsid, string dateofappointment, string weekstartdate, string weekenddate, string monthstartdate, string monthenddate);

        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/ViewUserAppointment?primaryamsid={primaryamsid}&dateofappointment={dateofappointment}&timeslot={timeslot}")]
        string ViewUserAppointment(string primaryamsid, string dateofappointment,string timeslot);

        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/LoginUser?primaryamsid={primaryamsid}&password={password}")]
        string LoginUser(string primaryamsid, string password);

        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/BookAppointmentJsonObj?primaryamsid={primaryamsid}&secondaryamsid={secondaryamsid}&dateofappointment={dateofappointment}&timeslot={timeslot}")]
        CompositeType BookAppointmentJsonObj(string primaryamsid, string secondaryamsid, string dateofappointment, string timeslot);
        // TODO: Add your service operations here
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";
        
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }

    [DataContract]
        public class Appointment
        {  
            [DataMember]
            public string primaryamsid
            {
                get;
                set;
            }
            [DataMember]
            public string secondaryamsid
            {
                get;
                set;
             }

            [DataMember]
            public string dateofappointment
            {
                get;
                set;
            }

            [DataMember]
            public string timeslot
            {
                get;
                set;
            }
            public Appointment(string primaryamsid, string secondaryamsid, string dateofappointment, string timeslot)
            {
                this.primaryamsid = primaryamsid;
                this.secondaryamsid = secondaryamsid;
                this.dateofappointment = dateofappointment;
                this.timeslot = timeslot;
            }
        }
    }
