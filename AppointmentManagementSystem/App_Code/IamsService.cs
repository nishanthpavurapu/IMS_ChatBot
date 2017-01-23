using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IamsService" in both code and config file together.
[ServiceContract]
public interface IamsService
{
    [OperationContract]    
    string getavailabletimings(string primaryamsid, string secondaryamsid ,string dateofappointment);

    [OperationContract]
    [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "/bookappointment")]
    string bookappointment();
    //string bookappointment(string primaryamsid, string secondaryamsid, string dateofappointment, string timeslot);
}
