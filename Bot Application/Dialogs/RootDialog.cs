//using Microsoft.Bot.Builder.Dialogs;
//using Microsoft.Bot.Builder.Luis;
//using Microsoft.Bot.Builder.Luis.Models;
//using Microsoft.Bot.Connector;
//using QnAMakerDialog;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Web;

//namespace Bot_Application.Dialogs
//{
//    enum YesNoBank { Yes, No };

//    [LuisModel("126cf1e6-0351-4774-884f-c06db0cc7805", "70205f37a07e436aab45b9a339e00513")]
//    [Serializable]

//    public class RootDialog : LuisDialog<Object>
//    {
//        DateTime thisDate = DateTime.UtcNow.AddHours(8);

//        [LuisIntent("UnlockID")]
//        public async Task UnlockID(IDialogContext context, LuisResult result)
//        {
//            if (result.TopScoringIntent.Score < 0.6)
//            {
//                //do something
//            }

//            var YN = (IEnumerable<YesNoBank>)Enum.GetValues(typeof(YesNoBank));

//            var GreetingText = string.Empty;

//            if (thisDate.Hour > 17)
//            {
//                GreetingText = "Good evening! ";
//            }
//            else if (thisDate.Hour > 11)
//            {
//                GreetingText = "Good afternoon! ";
//            }
//            else
//            {
//                GreetingText = "Good morning! ";
//            }

//            PromptDialog.Choice(context,
//                        idConfirmPrompt, YN,
//                        GreetingText + "Has your LAN/Domain ID locked yet ?");
//        }

//        private async Task idConfirmPrompt(IDialogContext context, IAwaitable<YesNoBank> YN)
//        {
//            var message = string.Empty;
//            bool unlockID = false;

//            switch (await YN)
//            {
//                case YesNoBank.Yes:
//                    message = "You LAN ID has been unlocked and please try again. Thank you for your enquiry.";
//                    unlockID = true;
//                    break;
//                default:
//                    message = "For other account enquiries, please contact Helpdesk Hotline(2993 2993) to get further advice and assistance.";
//                    break;
//            }

//            if (unlockID == true)
//            {
//                await context.PostAsync("Please wait while we are unlocking your LAN ID. It might take 30 seconds to 1 minute.");
//                Thread.Sleep(3000);
//            }

//            await context.PostAsync(message);
//            context.Wait(MessageReceived);
//        }


//        [LuisIntent("ResetPassword")]
//        public async Task ResetPassword(IDialogContext context, LuisResult result)
//        {
//            var GreetingText = string.Empty;

//            if (thisDate.Hour > 17)
//            {
//                GreetingText = "Good evening! ";
//            }
//            else if (thisDate.Hour > 11)
//            {
//                GreetingText = "Good afternoon! ";
//            }
//            else
//            {
//                GreetingText = "Good morning! ";
//            }

//            await context.PostAsync(GreetingText + "If you have forgotten your login password, please contact Helpdesk Hotline (2993 2993) for assistance.");
//            context.Wait(MessageReceived);
//        }

//        [LuisIntent("Greet")]
//        public async Task Greet(IDialogContext context, LuisResult result)
//        {
//            var GreetingText = string.Empty;

//            if (thisDate.Hour > 17)
//            {
//                GreetingText = "Good evening! ";
//            }
//            else if (thisDate.Hour > 11)
//            {
//                GreetingText = "Good afternoon! ";
//            }
//            else
//            {
//                GreetingText = "Good morning! ";
//            }

//            await context.PostAsync(GreetingText + "I can help you with unlocking your LAN ID account, reset password, and some general questions about password setting :)");
//            context.Wait(MessageReceived);
//        }

//        [LuisIntent("Appreciate")]
//        public async Task Appreciate(IDialogContext context, LuisResult result)
//        {
//            await context.PostAsync("You're welcomed :)");
//            context.Wait(MessageReceived);
//        }

//        [LuisIntent("")]
//        public async Task None(IDialogContext context, LuisResult result)
//        {
//            await context.PostAsync("Sorry, I cannot understand your sentence just yet. Please contact Helpdesk Hotline (2993 2993) for further assistance.");
//            context.Wait(MessageReceived);
//        }


//        //Below are sections for calling QnA maker API

//        private async Task AfterQnADialog(IDialogContext context, IAwaitable<bool> result)
//        {
//            var answerFound = await result;

//            // we might want to send a message or take some action if no answer was found (false returned)
//            if (!answerFound)
//            {
//                await context.PostAsync("Sorry, I cannot understand your sentence just yet. Please contact Helpdesk Hotline (2993 2993) for further assistance.");
//                context.Wait(MessageReceived);
//            }
//        }

//        [LuisIntent("ChangePassword")]
//        public async Task ChangePassword(IDialogContext context, IAwaitable<IMessageActivity> message, LuisResult result)
//        {
//            var qnadialog = new ChangePwdQnADialog();
//            var messageToForward = await message;
//            await context.Forward(qnadialog, AfterQnADialog, messageToForward, CancellationToken.None);
//        }

//        [Serializable]
//        [QnAMakerService("e5bc4365d9dc4b8fa2d652058f78adc5", "58fe2af2-c12c-43d4-ad35-c739c23baf73")]
//        public class ChangePwdQnADialog : QnAMakerDialog<bool>
//        {
//            public override async Task NoMatchHandler(IDialogContext context, string originalQueryText)
//            {
//                context.Done(false);
//            }

//            public override async Task DefaultMatchHandler(IDialogContext context, string originalQueryText, QnAMakerResult result)
//            {
//                await context.PostAsync($"{result.Answer}");
//                context.Done(true);
//            }
//        }



//        [LuisIntent("OutlookData")]
//        public async Task OutlookData(IDialogContext context, IAwaitable<IMessageActivity> message, LuisResult result)
//        {
//            var qnadialog = new OutlookQnADialog();
//            var messageToForward = await message;
//            await context.Forward(qnadialog, AfterQnADialog, messageToForward, CancellationToken.None);
//        }

//        [Serializable]
//        [QnAMakerService("e5bc4365d9dc4b8fa2d652058f78adc5", "072995c6-272f-4d4e-b1a6-d759d7dda6ab")]
//        public class OutlookQnADialog : QnAMakerDialog<bool>
//        {
//            public override async Task NoMatchHandler(IDialogContext context, string originalQueryText)
//            {
//                context.Done(false);
//            }

//            public override async Task DefaultMatchHandler(IDialogContext context, string originalQueryText, QnAMakerResult result)
//            {
//                await context.PostAsync($"{result.Answer}");
//                context.Done(true);
//            }
//        }

//        [LuisIntent("idFormat")]
//        public async Task LANIDformat(IDialogContext context, IAwaitable<IMessageActivity> message, LuisResult result)
//        {
//            var qnadialog = new LanQnADialog();
//            var messageToForward = await message;
//            await context.Forward(qnadialog, AfterQnADialog, messageToForward, CancellationToken.None);
//        }

//        [Serializable]
//        [QnAMakerService("e5bc4365d9dc4b8fa2d652058f78adc5", "dd6f7872-cf51-4993-a592-2fd1025f2d51")]
//        public class LanQnADialog : QnAMakerDialog<bool>
//        {
//            public override async Task NoMatchHandler(IDialogContext context, string originalQueryText)
//            {
//                context.Done(false);
//            }

//            public override async Task DefaultMatchHandler(IDialogContext context, string originalQueryText, QnAMakerResult result)
//            {
//                await context.PostAsync($"{result.Answer}");
//                context.Done(true);
//            }
//        }
//    }
//}