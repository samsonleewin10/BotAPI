//using System;
//using System.Threading.Tasks;
//using Microsoft.Bot.Builder.Dialogs;
//using Microsoft.Bot.Connector;
//using System.Configuration;
//using System.Web;
//using System.Net;
//using System.IO;
//using Bot_Application.Models;
//using Newtonsoft.Json;
//using System.Web.Script.Serialization;
//using Bot_Application.Translator;
//using Bot_Application.Message;

//using QnAMakerDialog;

//namespace Bot_Application.Dialogs
//{
//    [Serializable]
//    public class TranslatorUnlockDialog : IDialog<object>
//    {
//        protected int count = 1;
//        protected string _userAccount = string.Empty;
//        protected string _userToken = string.Empty;
//        //protected string messageLocale = "en";
//        protected string greeting = string.Empty;

        

//        public async Task StartAsync(IDialogContext context)
//        {
//            context.Wait(MessageReceivedAsync);
//        }

//        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
//        {
//            var message = await argument;

//            _userToken = message.From.Id;

//            try
//            {
//                messageLocale = await TranslateMethod.TranslateDetectAsync(message.Text);
//                message.Text = await TranslateMethod.TranslateAsync(message.Text);
//            }
//            catch(Exception ex)
//            {
//                await context.PostAsync(ex.InnerException.ToString());
//                context.Wait(MessageReceivedAsync);
//            }
//            string strLuisKey = ConfigurationManager.AppSettings["LUISAPIKey"].ToString();
//            string strLuisAppId = ConfigurationManager.AppSettings["LUISAppId"].ToString();

//            string strMessage = HttpUtility.UrlEncode(message.Text);
//            string strLuisUrl = $"https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/{strLuisAppId}?subscription-key={strLuisKey}&timezoneOffset=0&verbose=true&q={strMessage.Trim()}";


//            // 收到文字訊息後，往LUIS送
//            WebRequest request = WebRequest.Create(strLuisUrl);
//            HttpWebResponse response1 = (HttpWebResponse)request.GetResponse();
//            Stream dataStream = response1.GetResponseStream();
//            StreamReader reader = new StreamReader(dataStream);
//            string json = reader.ReadToEnd();
//            CognitiveModels.LUISResult objLUISRes = JsonConvert.DeserializeObject<CognitiveModels.LUISResult>(json);

//            if (objLUISRes.intents.Count > 0)
//            {

//                string strIntent = objLUISRes.intents[0].intent;
//                float score = objLUISRes.intents[0].score;
//                if (score >= 0.6)
//                {
//                    //if (strIntent == "UnlockAccount")
//                    if (strIntent == "UnlockID")
//                    {
//                        PromptDialog.Text(
//                            context,
//                            AfterResetAsync,
//                            MessageResource.ReturnMessage(messageLocale,"areYouSure"),
//                            MessageResource.ReturnMessage(messageLocale, "YesOrNo"));
                         
//                    }
//                    //if (strIntent == "Hello")
//                    if (strIntent == "Greet")
//                    {
//                        await context.PostAsync(MessageResource.ReturnMessage(messageLocale, "hello"));
//                        context.Wait(MessageReceivedAsync);
//                    }
//                    //if(strIntent =="ChangePassword")
//                    //{
//                    //    var qnadialog = new ChangePwdQnADialog();
//                    //    var messageToForward = await message;
//                    //    await context.Forward(qnadialog, AfterQnADialog, messageToForward, CancellationToken.None);
//                    //}
//                    if (strIntent == "None")
//                    {
//                        await context.PostAsync(MessageResource.ReturnMessage(messageLocale, "none"));
//                        context.Wait(MessageReceivedAsync);
//                    }
//                }
//                else
//                {
//                    await context.PostAsync(MessageResource.ReturnMessage(messageLocale,"none"));
//                    context.Wait(MessageReceivedAsync);
//                }
//            }
//            else
//            {
//                await context.PostAsync(MessageResource.ReturnMessage(messageLocale, "none"));
//                context.Wait(MessageReceivedAsync);

//            }

//        }

        

//        public async Task AfterResetAsync(IDialogContext context, IAwaitable<string> argument)
//        {
//            var confirm = await argument;
//            confirm = await TranslateMethod.TranslateAsync(confirm.Replace("是","yes"));
//            if (confirm.ToLower() == "yes")
//            {
//                await context.PostAsync(MessageResource.ReturnMessage(messageLocale, "youAccountedUnlocked"));
//                context.Wait(MessageReceivedAsync);
//            }
//            else if (confirm.ToLower() == "no")
//            {
             
//               await context.PostAsync(MessageResource.ReturnMessage(messageLocale,"otherAccountEnquiries"));
//               context.Wait(MessageReceivedAsync);
//            }
//            else
//            {
//                PromptDialog.Text(
//                            context,
//                            AfterResetAsync,
//                            MessageResource.ReturnMessage(messageLocale, "YesOrNo"),
//                            MessageResource.ReturnMessage(messageLocale, "none"));
//            }
//        }

//        //private async Task AfterQnADialog(IDialogContext context, IAwaitable<bool> result)
//        //{
//        //    var answerFound = await result;

//        //    // we might want to send a message or take some action if no answer was found (false returned)
//        //    if (!answerFound)
//        //    {
//        //        await context.PostAsync("Sorry, I cannot understand your sentence just yet. Please contact Helpdesk Hotline (2993 2993) for further assistance.");
//        //        context.Wait(MessageReceived);
//        //    }
//        //}
//    }


//    //qnaMaker calls
//    //[Serializable]
//    //[QnAMakerService("e5bc4365d9dc4b8fa2d652058f78adc5", "58fe2af2-c12c-43d4-ad35-c739c23baf73")]
//    //public class ChangePwdQnADialog : QnAMakerDialog<bool>
//    //{
//    //    public override async Task NoMatchHandler(IDialogContext context, string originalQueryText)
//    //    {
//    //        context.Done(false);
//    //    }

//    //    public override async Task DefaultMatchHandler(IDialogContext context, string originalQueryText, QnAMakerResult result)
//    //    {
//    //        await context.PostAsync($"{result.Answer}");
//    //        context.Done(true);
//    //    }
//    //}

//    //[Serializable]
//    //[QnAMakerService("e5bc4365d9dc4b8fa2d652058f78adc5", "072995c6-272f-4d4e-b1a6-d759d7dda6ab")]
//    //public class OutlookQnADialog : QnAMakerDialog<bool>
//    //{
//    //    public override async Task NoMatchHandler(IDialogContext context, string originalQueryText)
//    //    {
//    //        context.Done(false);
//    //    }

//    //    public override async Task DefaultMatchHandler(IDialogContext context, string originalQueryText, QnAMakerResult result)
//    //    {
//    //        await context.PostAsync($"{result.Answer}");
//    //        context.Done(true);
//    //    }
//    //}

//    //[Serializable]
//    //[QnAMakerService("e5bc4365d9dc4b8fa2d652058f78adc5", "dd6f7872-cf51-4993-a592-2fd1025f2d51")]
//    //public class LanQnADialog : QnAMakerDialog<bool>
//    //{
//    //    public override async Task NoMatchHandler(IDialogContext context, string originalQueryText)
//    //    {
//    //        context.Done(false);
//    //    }

//    //    public override async Task DefaultMatchHandler(IDialogContext context, string originalQueryText, QnAMakerResult result)
//    //    {
//    //        await context.PostAsync($"{result.Answer}");
//    //        context.Done(true);
//    //    }
//    //}
//}
