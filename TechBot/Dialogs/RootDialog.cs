using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;

namespace TechBot.Dialogs
{
    [LuisModel("2bea3bc7-d2d3-4301-a620-faa223c361af", "d81d38f1310c4b60b72d72256518a80d")]
    [Serializable]
    public class RootDialog : LuisDialog<object>
    {
        //[LuisIntent("")]
        //[LuisIntent("None")]
        //public async Task None(IDialogContext context, LuisResult result)
        //{
        //    string message = $"Sorry, I did not understand '{result.Query}'. Type 'help' if you need assistance.";

        //    await context.PostAsync("Sorry I didn't get that..");

        //    context.Wait(this.MessageReceived);
        //}

        [LuisIntent("Mail.IncreaseQuota")]
        public async Task IncreaseQuota(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            var message = await activity;
            await context.PostAsync("You want to increase quota");

            //***************************

            //context.Call(new MailboxQuotaDialog(), ResumeAfterOptionDialog);

            //***************************
            context.Wait(this.MessageReceived);
        }

        [LuisIntent("Access.OnlineMeetingService")]
        public async Task AccessOnlineMeetingService(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            var message = await activity;
            await context.PostAsync("You want to access an online meeting");
            context.Call(new MailboxQuotaDialog(), ResumeAfterOptionDialog);
            context.Wait(this.MessageReceived);
        }

        private async Task ResumeAfterOptionDialog(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                var message = await result;
            }
            catch (Exception ex)
            {
                await context.PostAsync($"Failed with message: {ex.Message}");
            }
            finally
            {
                context.Wait(MessageReceived);
            }
        }
    }
}