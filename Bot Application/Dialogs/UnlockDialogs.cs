using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Connector;

namespace Bot_Application.Dialogs
{
    [LuisModel("126803f4-ec56-472f-86fd-ade9c2caef2f", "ecd8014916a24c709fbb93c3fdc1520f")]
    [Serializable]
    public class UnlockDialogs : LuisDialog<object>
    {
        #region None
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string message = $"Sorry, I did not understand '{result.Query}'. Type 'help' if you need assistance.";
            await context.PostAsync(message);
            context.Done(true);
        }
        #endregion

        #region Help
        [LuisIntent("Help")]
        public async Task Help(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Hi! Try asking me things like 'Please unlock my account'");

            context.Wait(this.MessageReceived);
        }
        #endregion

        #region Hello
        [LuisIntent("Hello")]
        public async Task Hello(IDialogContext context, LuisResult result)
        {
            string message = "Hello CT, May I help you?";
            await context.PostAsync(message);
            context.Done(true);
        }
        #endregion

        #region Introduction
        [LuisIntent("Introduction")]
        public async Task HowAreYou(IDialogContext context, LuisResult result)
        {
            string message = "Fine. May I help you?";
            await context.PostAsync(message);
            context.Done(true);
        }
        #endregion

        #region UnlockAccount
        [LuisIntent("UnlockAccount")]
        public async Task UnlockAccount(IDialogContext context, LuisResult result)
        {

            string greeting = string.Empty;
            //04:01 - 12:00 Good Morning
            //12:01 - 18:00 Good Afternoon 
            //18:01 - 04:00 Good Evening
             

            if (DateTime.Now.Hour < 12)
            {
                greeting = "Good Morning";
                  
            }
            else if (DateTime.Now.Hour < 17)
            {
                greeting = "Good Afternoon";
                
            }
            else
            {
                greeting = "Good Evening";
                
            }




            if (result.TopScoringIntent.Score >= 0.8)
            {
                PromptDialog.Confirm(context, UnlockUserAccount, $"{greeting} You LAN ID has been unlocked and please try  again. Thank you for your enquiry.");
            }
            else
            {
                await None(context, result);
            }
            //string message = "UnlockAccount.";
            //await context.PostAsync(message);
            //context.Done(true);
        }

        public async Task UnlockUserAccount(IDialogContext context, IAwaitable<bool> argument)
        {
            var confirm = await argument;
            if (confirm)
            {
                
                await context.PostAsync($"You LAN ID has been unlocked and please try  again. Thank you for your enquiry.");
                context.Done(true);
            }
            else
            {
                 
                await context.PostAsync("For other account enquiries, please contact Helpdesk Hotline (2993 2993) to get further advice and assistance.");
                context.Done(true);
            }
             
        }

        #endregion
    }
}