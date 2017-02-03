using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;



namespace Bot4AMS.Controllers
{
    [Serializable]
    public class MissingAttribDialog : IDialog
    {
        public string primary_amsid { get; set; }
        public string secondary_amsid { get; set; }
        public string dateOfAppointment { get; set; }
        public string timeSlot { get; set; }

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> argument)
        {
           var activity = await argument as Activity;
            var val = activity.Text;
           await context.PostAsync("Enter Missing time value");
            secondary_amsid = "test";
            
           context.Wait(MessageReceivedAsync);
            

        }
    }
}