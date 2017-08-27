using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bot_Application.Message
{
    public class MessageResource
    {
        protected static string yesOrNo = $"是(Yes)\\否(No)";
        protected static string whichAccount = $"Which account want to Unlock?";
        protected static string tryAgain = "Try again message";

        protected static string helloCH = "您好，有什麼能幫得上忙的呢?";
        protected static string helloEN = "Hello, may I help you?";

        protected static string noneEN = $"Your enquiries are not covered here at the moment. Please contact Helpdesk Hotline (2993 2993) to get further advice and assistance. ";
        protected static string noneCH = $"你想問的問題現時未能在此回答，請聯絡ITSD Hotline (2993 2993)尋求進一步協助，謝謝你的查詢。";

        protected static string areYouSureCH = $"你的LAN ID Lock咗,現在是否要幫你unlock嗎?\n(是\\否)";
        protected static string areYouSureEN = $"We checked that your LAN ID is locked. Proceed to unlock?\n(Yes/No)";

        protected static string youAccountUnlockedEN = $"You LAN ID has been unlocked. Please try again in 3 minutes. Thank you for your enquiry.";
        protected static string youAccountUnlockedCH = $"你 LAN ID 已解鎖，請在三分鐘後再試，謝謝。";

        protected static string otherAccountEnquiriesEN = $"For other account enquiries, please contact Helpdesk Hotline (2993 2993) to get further advice and assistance. ";
        protected static string otherAccountEnquiriesCH = $"若是 其它account 問題，請聯絡ITSD Hotline (2993 2993)尋求進一步協助，謝謝你的查詢。";

        protected static string appreciateEN = $"You're welcomed!";
        protected static string appreciateCH = $"不用客氣! ";

        protected static string resetPasswordEN = $"Your password has been reset as XXXX. Please change your password after logging in.";
        protected static string resetPasswordCH = $"你的密碼已經被重置為XXXX。請於登入後盡快更改此密碼。 ";

        public static string ReturnMessage(string messageLocale, string messateType)
        {
            if (messageLocale == "en")
            {
                switch (messateType)
                {
                    case "none":
                        return noneEN;
                    case "hello":
                        return helloEN;
                    case "areYouSure":
                        return areYouSureEN;
                    case "youAccountedUnlocked":
                        return youAccountUnlockedEN;
                    case "otherAccountEnquiries":
                        return otherAccountEnquiriesEN;
                    case "YesOrNo":
                        return yesOrNo;
                    case "appreciate":
                        return appreciateEN;
                    case "resetPassword":
                        return resetPasswordEN;
                    default:
                        return noneEN;
                }
            }
            else
            {
                switch (messateType)
                {
                    case "none":
                        return noneCH;
                    case "hello":
                        return helloCH;
                    case "areYouSure":
                        return areYouSureCH;
                    case "youAccountedUnlocked":
                        return youAccountUnlockedCH;
                    case "otherAccountEnquiries":
                        return otherAccountEnquiriesCH;
                    case "YesOrNo":
                        return yesOrNo;
                    case "appreciate":
                        return appreciateCH;
                    case "resetPassword":
                        return resetPasswordCH;
                    default:
                        return noneCH;
                }
            }
            return "";
        }
    }
}