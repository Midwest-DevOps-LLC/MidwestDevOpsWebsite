using System;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //var r = MDO.Utility.Standard.EmailHandler.SendEmail(MDO.Utility.Standard.EmailHandler.EmailAddresses.noreply, "1998markmueller@gmail.com", "f", "subject", "body", true);
            var r = MDO.EmailHandler.Standard.EmailHandler.SendEmail("haley@midwestdevops.com", MDO.EmailHandler.Standard.EmailHandler.EmailAddresses.noreply, "1998haleymueller@gmail.com", "oqaruhxanwfecrwb", "subject", "body", true);


            var rr = 1;
        }
    }
}
