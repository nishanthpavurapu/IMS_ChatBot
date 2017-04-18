using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;



namespace amsBOT.Controllers
{
    [Serializable]
    public class MissingAttribDialog : IDialog
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> argument)
        {
            var activity = await argument as Activity;
            await context.PostAsync("fuck");
            context.Wait(MessageReceivedAsync);

        }
    }
}