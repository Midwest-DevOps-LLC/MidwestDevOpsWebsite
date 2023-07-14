using System;
using System.Collections.Generic;
using System.Text;

namespace MDO.EmailHandler.Standard
{
    public static class PreWrittenEmails //TODO have this pull the url from mdo sites table
    {
        public static string RegistrationEmail(string serviceURL, string registrationGUID, string applicationGUID, string applicationName, string applicationURL)
        {

                if (string.IsNullOrEmpty(serviceURL))
                    MDO.Utility.Standard.LogHandler.SaveLog(new Utility.Standard.LogHandler.Log() { text = "serviceURL is empty", time = DateTime.UtcNow });

                if (string.IsNullOrEmpty(registrationGUID))
                    MDO.Utility.Standard.LogHandler.SaveLog(new Utility.Standard.LogHandler.Log() { text = "registrationGUID is empty", time = DateTime.UtcNow });

                if (string.IsNullOrEmpty(applicationGUID))
                    MDO.Utility.Standard.LogHandler.SaveLog(new Utility.Standard.LogHandler.Log() { text = "applicationGUID is empty", time = DateTime.UtcNow });

                if (string.IsNullOrEmpty(applicationName))
                    MDO.Utility.Standard.LogHandler.SaveLog(new Utility.Standard.LogHandler.Log() { text = "applicationName is empty", time = DateTime.UtcNow });

                if (string.IsNullOrEmpty(applicationURL))
                    MDO.Utility.Standard.LogHandler.SaveLog(new Utility.Standard.LogHandler.Log() { text = "applicationURL is empty", time = DateTime.UtcNow });


                int lastSlash = serviceURL.LastIndexOf('/');
                serviceURL = (lastSlash > -1) ? serviceURL.Substring(0, lastSlash) : serviceURL;

                string link = serviceURL + "/api/Registration/Verify?GUID=" + registrationGUID + "&Application=" + applicationGUID;

                //switch (application)
                //{
                //    case "RPGParty":
                //        link = "https://rpgparty.midwestdevops.com/Registration/Email?UUID=";
                //        break;
                //}

                string emailText = @"
                <html xmlns='http://www.w3.org/1999/xhtml'>
                <head>
                <title>Registration - " + applicationName + @"</title>
                <meta http-equiv='Content-Type' content='text/html; charset=UTF-8' />
                <meta http-equiv='X-UA-Compatible' content='IE=edge' />
                <meta name = 'viewport' content='width=device-width, initial-scale=1.0 ' />
                <style>
                    body {
                        background-color: #1FA89C;
                        bgcolor: #1FA89C;
                    }
                </style>
                </head>
                <body style='background-color: #1FA89C; bgcolor: #1FA89C;' bgcolor='#1FA89C; border-radius:.25em;'>
                    <div style='background-color: #1FA89C; bgcolor: #1FA89C;' bgcolor='#1FA89C'>
                        <h1 style='text-align:center; color:white'; padding:10em; padding-top:2em;>" + applicationName + @"</h1>
                        <div style='padding:7em; color:white; text-align:center'>
                            You have created an account at <a href='" + applicationURL + @"'>" + applicationName + @"</a><br><br>
                            Click <a href='" + link + @"'>here</a> to activate your email<br><br><br><br>
                            <p style='color:lightgrey'>If you didn't create this account, ignore this email</p>
                        </div>
                    </div>
                </body>
                </html>
            ";

                return emailText;
            
        }
    }
}
