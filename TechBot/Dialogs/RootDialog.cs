using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System.Collections.Generic;

namespace TechBot.Dialogs
{
    [LuisModel("e7b5f21e-222a-41e3-aee9-07909a845e6a", "54b3a614d45047a5a53a46fe644be1b3")]
    [Serializable]
    public class RootDialog : LuisDialog<object>
    {


        [LuisIntent("None")]
        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("I'm sorry. I didn't understand you.");
            context.Wait(MessageReceived);
        }

        [LuisIntent("IncreaseQuota")]
        public async Task IncreaseQuota(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            var message = await activity;

            EntityRecommendation quotaEntity;

            if (result.TryFindEntity("MailboxQuota", out quotaEntity))
            {
                context.Call(new MailboxQuotaDialog(), ResumeAfterOptionDialog);
            }

        }


        [LuisIntent("Access.OnlineMeetingService")]
        public async Task AccessOnlineMeetingService(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            var message = await activity;
            var entities = new List<EntityRecommendation>(result.Entities);
            //DateTime start;
            //DateTime end;
            //foreach (var entity in result.Entities)
            //{
            //    if (entity.Type == "builtin.datetimeV2.daterange")
            //    {
            //        foreach (var vals in entity.Resolution.Values)
            //        {
            //            if (((Newtonsoft.Json.Linq.JArray)vals).First.SelectToken("type").ToString() == "daterange")
            //            {
            //                start = (DateTime)((Newtonsoft.Json.Linq.JArray)vals).First.SelectToken("start");
            //                end = (DateTime)((Newtonsoft.Json.Linq.JArray)vals).First.SelectToken("end");
            //                await context.PostAsync($"Start: {start} - End: {end}");
            //            }
            //        }
            //    }
            //}

            

            EntityRecommendation quotaEntity;

            if (result.TryFindEntity("OnlineMeetingService", out quotaEntity))
            {

                context.Call(new OnlineMeetingDialog(), ResumeAfterOptionDialog);
            }

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