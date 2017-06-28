namespace TechBot.Dialogs
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;
    using System.Net;
    using System.Xml.Linq;
    using System.Linq;

    [Serializable]
    public class MailboxQuotaDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            // await context.PostAsync("Ok, let's chack you current quota first.");
            await context.PostAsync("What is your A Number?");
            context.Wait(this.MessageReceivedAsync);

            return;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            // calculate something for us to return
            int length = (activity.Text ?? string.Empty).Length;

            await context.PostAsync($"Please wait while I check your mailbox details...");

            //Get Quota
            var client = new WebClient();
            var url = "http://k2server-k2dev2.fil.com:8888/SmartObjectServices/rest/TechHub/SmartObjects/ExecTHTB-Exchange/GetMailboxDetails?UserName=" + activity.Text;

            client.Headers[HttpRequestHeader.ContentType] = "application/xml";
            client.Headers[HttpRequestHeader.Authorization] = "Basic SU5UTFxzdmNLMlNldHVwSzJERVYyOmZ5S0pVNDgy"; //K2DEV2 setup account; 

            var responseString = client.DownloadString(url);

            var xDoc = XDocument.Parse(responseString);

            var quota = (from e in xDoc.Descendants("ProhibitSendQuota") select e.Value).SingleOrDefault();
            var size = (from e in xDoc.Descendants("ItemsSize") select e.Value).SingleOrDefault();
            var percentage = Convert.ToInt32((from e in xDoc.Descendants("MailboxUsedPercentage") select e.Value).SingleOrDefault());


            // return our reply to the user
            await context.PostAsync($"Your current mailbox size is {size}, and your quota is {quota}");

            if(percentage < 80)
            {
                await context.PostAsync($"You are using only {percentage}% of your quota and an increase is not required.");
            }
            else
            {
                PromptDialog.Confirm(context, this.OnOptionSelected, $"You are using only {percentage}% of your quota. Do you want to increas the quota?");
            }

        }

        private async Task OnOptionSelected(IDialogContext context, IAwaitable<bool> result)
        {
            bool isCorrect = await result;
            if (isCorrect)
            { await context.PostAsync($"Increasing quota..."); }
            else
            { await context.PostAsync($"Quota will not be increased"); }

        }
    }
}



