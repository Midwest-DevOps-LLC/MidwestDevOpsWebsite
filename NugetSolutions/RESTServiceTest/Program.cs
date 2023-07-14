using System;

namespace RESTServiceTest
{
    class Program
    {
        static void Main(string[] args)
        {
            MDO.RESTServiceRequestor.Standard.StatusRequest statusRequest = new MDO.RESTServiceRequestor.Standard.StatusRequest("https://localhost:44350/", "825d53c9-df35-451f-b668-76b467e5f54b");

            try
            {
                var r = statusRequest.Status();
            }
            catch (MDO.RESTDataEntities.Standard.EndpointErrorException eeex)
            {

            }
            catch (Exception ex)
            {
                
            }

            Console.ReadLine();
        }
    }
}
