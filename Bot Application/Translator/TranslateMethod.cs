using System;
using System.Net;
using System.IO;
using System.Runtime.Serialization;
using System.Web;

namespace Bot_Application.Translator
{
    public class TranslateMethod
    {
        public static string messageLocale;

        public static async System.Threading.Tasks.Task<string> TranslateAsync(string text)
        {

            var authTokenSource = new AzureAuthToken("f25293922a254f9cb117a7d1bd4ac4d9");
            string authToken;
            authToken = await authTokenSource.GetAccessTokenAsync();
            return Run(authToken,text);

        }
        public static async System.Threading.Tasks.Task<string> TranslateDetectAsync(string text)
        {

            var authTokenSource = new AzureAuthToken("f25293922a254f9cb117a7d1bd4ac4d9");
            string authToken;
            authToken = await authTokenSource.GetAccessTokenAsync();
            return Detect(authToken, text);

        }
        public static string Detect(string authToken, string text)
        {
            string uri = "https://api.microsofttranslator.com/v2/Http.svc/Detect?text=" + text;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
            httpWebRequest.Headers.Add("Authorization", authToken);
            using (WebResponse response = httpWebRequest.GetResponse())
            using (Stream stream = response.GetResponseStream())
            {
                System.Runtime.Serialization.DataContractSerializer dcs = new System.Runtime.Serialization.DataContractSerializer(Type.GetType("System.String"));
                return (string)dcs.ReadObject(stream);
                 
            }
        }
        public static string Run(string authToken, string text)
        {

            //string from = Detect(authToken, text);
            string from = "YUE";
            string to = "en";
            string uri = "https://api.microsofttranslator.com/v2/Http.svc/Translate?text=" + HttpUtility.UrlEncode(text) + "&from=" + from + "&to=" + to;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
            httpWebRequest.Headers.Add("Authorization", authToken);
            using (WebResponse response = httpWebRequest.GetResponse())
            using (Stream stream = response.GetResponseStream())
            {
                DataContractSerializer dcs = new DataContractSerializer(Type.GetType("System.String"));
                return  (string)dcs.ReadObject(stream);
                 
            }
 
        }
    }
}