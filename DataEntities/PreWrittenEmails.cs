using System;
using System.Collections.Generic;
using System.Text;

namespace DataEntities
{
    public static class PreWrittenEmails
    {
        public static string RegistrationEmail(string registrationGUID, string application)
        {
            string link = "https://midwestdevops.com/Registration/Email?UUID=";

            switch (application)
            {
                case "RPGParty":
                    link = "https://rpgparty.midwestdevops.com/Registration/Email?UUID=";
                    break;
            }

            string emailText = @"
                <html xmlns='http://www.w3.org/1999/xhtml'>
                <head>
                <title>Registration - Midwest DevOps, LLC</title>
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
                        <h1 style='text-align:center; color:white'; padding:10em; padding-top:2em;>Midwest DevOps, LLC</h1>
                        <div style='padding:7em; color:white; text-align:center'>
                            You have created an account at <a href='midwestdevops.com'>Midwest DevOps, LLC</a><br><br>
                            Click <a href='" + link + registrationGUID + @"'>here</a> to activate your email<br><br><br><br>
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
