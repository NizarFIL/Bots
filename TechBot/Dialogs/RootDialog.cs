using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System.Net;
using System.Xml.Linq;
using System.Linq;
using Newtonsoft.Json;

namespace TechBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Hi! I'm Botty. I do not do much now, but I'll reply with a random number when you send me a message.");
            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            // get random number
            //Random rnd = new Random();
            //int number = rnd.Next(1000);

            //**************************

            string responseString = string.Empty;

            var query = activity.Text; ; //User Query
            var knowledgebaseId = "f7e8afe8-b641-4905-be2d-2b568537477b"; // Use knowledge base id created.
            var qnamakerSubscriptionKey = "8d2840bb903249b5a3d783db741d0bd2"; //Use subscription key assigned to you.

            //Build the URI
            Uri qnamakerUriBase = new Uri("https://westus.api.cognitive.microsoft.com/qnamaker/v1.0");
            var builder = new UriBuilder($"{qnamakerUriBase}/knowledgebases/{knowledgebaseId}/generateAnswer");

            //Add the question as part of the body
            var postBody = $"{{\"question\": \"{query}\"}}";

            //Send the POST request
            using (WebClient client = new WebClient())
            {
                //Set the encoding to UTF8
                client.Encoding = System.Text.Encoding.UTF8;

                //Add the subscription key header
                client.Headers.Add("Ocp-Apim-Subscription-Key", qnamakerSubscriptionKey);
                client.Headers.Add("Content-Type", "application/json");
                responseString = client.UploadString(builder.Uri, postBody);
                QnAMakerResult response;
                try
                {
                    response = JsonConvert.DeserializeObject<QnAMakerResult>(responseString);
                    string answer = response.Answer.ToString();
                }
                catch
                {
                    throw new Exception("Unable to deserialize QnA Maker response string.");
                }
                await context.PostAsync($"{answer}");
            }

            

            //**************************

            // return our reply to the user
            

            context.Wait(MessageReceivedAsync);
        }

        private class QnAMakerResult
        {
            /// <summary>
            /// The top answer found in the QnA Service.
            /// </summary>
            [JsonProperty(PropertyName = "answer")]
            public string Answer { get; set; }

            /// <summary>
            /// The score in range [0, 100] corresponding to the top answer found in the QnA    Service.
            /// </summary>
            [JsonProperty(PropertyName = "score")]
            public double Score { get; set; }
        }
    }
}