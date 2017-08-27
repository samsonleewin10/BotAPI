using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Configuration;
using System.Web;
using System.Net;
using System.IO;
using Bot_Application.Models;
using Newtonsoft.Json;
using System.Web.Script.Serialization;

namespace Bot_Application.Dialogs
{
    [Serializable]
    public class UnlockAccountDialog : IDialog<object>
    {
        protected int count = 1;
        protected string _userAccount = string.Empty;
        protected string _userToken = string.Empty;



        protected string didntGetThat = $"Didn't get that!";
        protected string whichAccount = $"Which account want to Unlock?";
        protected string tryAgain = "Try again message";
        protected string hello = "您好，有什麼能幫得上忙的呢?";
        protected string icantknowthat = $"您在說什麼，我聽不懂";
        protected string none = $"無法識別的內容";
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

       
        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;

            _userToken = message.From.Id;

           

            string strLuisKey = ConfigurationManager.AppSettings["LUISAPIKey"].ToString();
            string strLuisAppId = ConfigurationManager.AppSettings["LUISAppId"].ToString();
            string strMessage = HttpUtility.UrlEncode(message.Text);
            //string strLuisUrl = $"https://api.projectoxford.ai/luis/v1/application?id={strLuisAppId}&subscription-key={strLuisKey}&q={strMessage}";
            string strLuisUrl = $"https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/{strLuisAppId}?subscription-key={strLuisKey}&timezoneOffset=0&verbose=true&q={strMessage}";


            // 收到文字訊息後，往LUIS送
            WebRequest request = WebRequest.Create(strLuisUrl);
            HttpWebResponse response1 = (HttpWebResponse)request.GetResponse();
            Stream dataStream = response1.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string json = reader.ReadToEnd();
            CognitiveModels.LUISResult objLUISRes = JsonConvert.DeserializeObject<CognitiveModels.LUISResult>(json);

            if (objLUISRes.intents.Count > 0)
            {

                string strIntent = objLUISRes.intents[0].intent;
                float score = objLUISRes.intents[0].score;
                if (score >= 0.8)
                {
                    if (strIntent == "UnlockAccount")
                    {
                       
                        //string userAccount = objLUISRes.entities.Find((x => x.type == "UserAccount")).entity;
                        var userAccount = objLUISRes.entities.Find((x => x.type == "Account"));
                        if (userAccount != null)
                        {
                            _userAccount = userAccount.entity;

                            string areYouSure = $"Are you sure to unlock this account: {_userAccount}\n(Yes\\No)";

                            PromptDialog.Confirm(
                               context,
                               AfterResetAsync,
                                areYouSure,
                                didntGetThat,
                               promptStyle: PromptStyle.None);
                        }
                        else
                        {

                            PromptDialog.Text(context, AfterUserNameAsync, whichAccount, tryAgain, 2);
                            // The following line shouldn't be here
                            //result.Entities[0].Entity = userSymbol;
                        }
                    }
                    if (strIntent == "Hello")
                    {
                        await context.PostAsync( hello);
                        context.Wait(MessageReceivedAsync);
                    }
                }
                else
                {
                    await context.PostAsync(icantknowthat);
                    context.Wait(MessageReceivedAsync);
                }
            }
            else
            {
                await context.PostAsync(none);
                context.Wait(MessageReceivedAsync);

            }
             
        }

        public async Task AfterResetAsync(IDialogContext context, IAwaitable<bool> argument)
        {
            var confirm = await argument;
            if (confirm)
            {
                
                await context.PostAsync($"This account({_userAccount}) unlocked.");
                _userAccount = string.Empty;
            }
            else
            {
                _userAccount = string.Empty;
                await context.PostAsync("Cancel");
            }
            context.Wait(MessageReceivedAsync);
        }
        public async Task AfterUserNameAsync(IDialogContext context, IAwaitable<string> argument)
        {
            var accountName = await argument;
            if (accountName != string.Empty)
            {
                _userAccount = accountName;
                string areYouSure = $"Are you sure to unlock this account: {_userAccount}\n(Yes\\No)";

                PromptDialog.Confirm(
                                context,
                                AfterResetAsync,
                                 areYouSure,
                                 didntGetThat,
                                promptStyle: PromptStyle.None);
            }
            else
                context.Wait(MessageReceivedAsync);
        }
    }
}