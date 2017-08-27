using Bot_Application.Message;
using Bot_Application.Translator;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using QnAMakerDialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;


namespace Bot_Application.Dialogs
{
    enum YesNoBank { Yes, No };

    [LuisModel("126cf1e6-0351-4774-884f-c06db0cc7805", "1559b69fda3144f68db0e0f614b10af4")]
    [Serializable]
    public class MainDialog : LuisDialog<Object>
    {

        DateTime thisDate = DateTime.UtcNow.AddHours(8);

        [LuisIntent("UnlockID")]
        public async Task UnlockID(IDialogContext context, LuisResult result)
        {
            if (result.TopScoringIntent.Score >= 0.6)
            {
                PromptDialog.Text(
                    context,
                    AfterResetAsync,
                    MessageResource.ReturnMessage(TranslateMethod.messageLocale, "areYouSure"),
                    MessageResource.ReturnMessage(TranslateMethod.messageLocale, "YesOrNo"));
            }
            else
            {
                await None(context, result);
            }
        }

        [LuisIntent("ResetPassword")]
        public async Task ResetPassword(IDialogContext context, LuisResult result)
        {
            if (result.TopScoringIntent.Score >= 0.6)
            {
                await context.PostAsync(MessageResource.ReturnMessage(TranslateMethod.messageLocale, "resetPassword"));
                context.Wait(MessageReceived);
            }
            else
            {
                await None(context, result);
            }

        }

        public async Task AfterResetAsync(IDialogContext context, IAwaitable<string> argument)
        {
            var confirm = await argument;
            //confirm = await TranslateMethod.TranslateAsync(confirm.Replace("是", "yes"));
            //if (confirm.ToLower() == "yes")
            if (Regex.Match(confirm.ToLower(), ".*是.*").Success || Regex.Match(confirm.ToLower(), ".*係.*").Success || Regex.Match(confirm.ToLower(), ".*好.*").Success ||
                Regex.Match(confirm.ToLower(), ".*yes.*").Success || Regex.Match(confirm.ToLower(), ".*sure.*").Success)
            {
                await context.PostAsync(MessageResource.ReturnMessage(TranslateMethod.messageLocale, "youAccountedUnlocked"));
                context.Wait(MessageReceived);
            }
            else if (Regex.Match(confirm.ToLower(), ".*否.*").Success || Regex.Match(confirm.ToLower(), ".*唔係.*").Success ||
                     Regex.Match(confirm.ToLower(), ".*no.*").Success)
            {

                await context.PostAsync(MessageResource.ReturnMessage(TranslateMethod.messageLocale, "otherAccountEnquiries"));
                context.Wait(MessageReceived);
            }
            else
            {
                PromptDialog.Text(
                            context,
                            AfterResetAsync,
                            MessageResource.ReturnMessage(TranslateMethod.messageLocale, "YesOrNo"),
                            MessageResource.ReturnMessage(TranslateMethod.messageLocale, "none"));
            }
        }

        [LuisIntent("Greet")]
        public async Task Greet(IDialogContext context, LuisResult result)
        {
            if (result.TopScoringIntent.Score >= 0.6)
            {
                await context.PostAsync(MessageResource.ReturnMessage(TranslateMethod.messageLocale, "hello"));
                context.Wait(MessageReceived);
            }
            else
            {
                await None(context, result);
            }
        }

        [LuisIntent("Appreciate")]
        public async Task Appreciate(IDialogContext context, LuisResult result)
        {
            if (result.TopScoringIntent.Score >= 0.6)
            {
                await context.PostAsync(MessageResource.ReturnMessage(TranslateMethod.messageLocale, "appreciate"));
                context.Wait(MessageReceived);
            }
            else
            {
                await None(context, result);
            }
        }

        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync(MessageResource.ReturnMessage(TranslateMethod.messageLocale, "none")
                                    + Environment.NewLine + "----------------------------------------------------"
                                    + "Transalted Message: " + result.Query.ToString() + " | Intent: " + result.TopScoringIntent.Intent + " | Score: " + result.TopScoringIntent.Score);
            context.Wait(MessageReceived);
        }


        //Below are sections for calling QnA maker API

        private async Task AfterQnADialog(IDialogContext context, IAwaitable<bool> result)
        {
            var answerFound = await result;

            // we might want to send a message or take some action if no answer was found (false returned)
            if (!answerFound)
            {
                await context.PostAsync("Sorry, I cannot understand your sentence just yet. Please contact Helpdesk Hotline (2993 2993) for further assistance.");
                context.Wait(MessageReceived);
            }
        }

        [LuisIntent("ChangePassword")]
        public async Task ChangePassword(IDialogContext context, IAwaitable<IMessageActivity> message, LuisResult result)
        {
            var qnadialog = new ChangePwdQnADialog();
            var messageToForward = await message;
            await context.Forward(qnadialog, AfterQnADialog, messageToForward, CancellationToken.None);
        }

        [Serializable]
        [QnAMakerService("e5bc4365d9dc4b8fa2d652058f78adc5", "58fe2af2-c12c-43d4-ad35-c739c23baf73")]
        public class ChangePwdQnADialog : QnAMakerDialog<bool>
        {
            public override async Task NoMatchHandler(IDialogContext context, string originalQueryText)
            {
                context.Done(false);
            }

            public override async Task DefaultMatchHandler(IDialogContext context, string originalQueryText, QnAMakerResult result)
            {
                await context.PostAsync($"{result.Answer}");
                context.Done(true);
            }
        }



        [LuisIntent("OutlookData")]
        public async Task OutlookData(IDialogContext context, IAwaitable<IMessageActivity> message, LuisResult result)
        {
            var qnadialog = new OutlookQnADialog();
            var messageToForward = await message;
            await context.Forward(qnadialog, AfterQnADialog, messageToForward, CancellationToken.None);
        }

        [Serializable]
        [QnAMakerService("e5bc4365d9dc4b8fa2d652058f78adc5", "072995c6-272f-4d4e-b1a6-d759d7dda6ab")]
        public class OutlookQnADialog : QnAMakerDialog<bool>
        {
            public override async Task NoMatchHandler(IDialogContext context, string originalQueryText)
            {
                context.Done(false);
            }

            public override async Task DefaultMatchHandler(IDialogContext context, string originalQueryText, QnAMakerResult result)
            {
                await context.PostAsync($"{result.Answer}");
                context.Done(true);
            }
        }

        [LuisIntent("idFormat")]
        public async Task LANIDformat(IDialogContext context, IAwaitable<IMessageActivity> message, LuisResult result)
        {
            var qnadialog = new LanQnADialog();
            var messageToForward = await message;
            await context.Forward(qnadialog, AfterQnADialog, messageToForward, CancellationToken.None);
        }

        [Serializable]
        [QnAMakerService("e5bc4365d9dc4b8fa2d652058f78adc5", "dd6f7872-cf51-4993-a592-2fd1025f2d51")]
        public class LanQnADialog : QnAMakerDialog<bool>
        {
            public override async Task NoMatchHandler(IDialogContext context, string originalQueryText)
            {
                context.Done(false);
            }

            public override async Task DefaultMatchHandler(IDialogContext context, string originalQueryText, QnAMakerResult result)
            {
                await context.PostAsync($"{result.Answer}");
                context.Done(true);
            }
        }
    }
}