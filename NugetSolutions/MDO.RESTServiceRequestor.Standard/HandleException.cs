using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace MDO.RESTServiceRequestor.Standard
{
    public static class HandleException
    {
        public static MDO.RESTDataEntities.Standard.APIResponse<T> Handle<T>(this Exception ex, MDO.RESTDataEntities.Standard.APIResponse<T> response)
        {
            if (ex.GetType() == typeof(WebException))
            {
                var webException = (WebException)ex;

                var s = webException.Status;

                if (webException.Status == WebExceptionStatus.UnknownError)
                {
                    response.AddError("Unable to connect to background service. Please try again later");
                }
                else
                {
                    MDO.RESTDataEntities.Standard.EndpointErrorResponse endpointError = null;

                    try //Possibly change this to have all api response's have an error dataentity
                    {
                        var resp = new StreamReader(webException.Response.GetResponseStream()).ReadToEnd();

                        endpointError = JsonConvert.DeserializeObject<MDO.RESTDataEntities.Standard.EndpointErrorResponse>(resp);

                        //endpointError = endpointErrorResponse.Data;
                        //endpointErrorResponse.Status = RESTDataEntities.Standard.APIResponse<RESTDataEntities.Standard.EndpointErrorResponse>.StatusEnum.Error;
                        //endpointErrorResponse.ValidationModel.ValidationStatus = RESTDataEntities.Standard.ValidationStatus.Error;
                    }
                    catch (Exception exx)
                    {

                    }

                    if (endpointError != null)
                    {
                        var customException = new MDO.RESTDataEntities.Standard.EndpointErrorException(endpointError);

                        throw customException;
                    }

                    //response.EndpointErrorResponse = endpointError;
                }
            }

            MDO.Utility.Standard.LogHandler.SaveException(ex);
            response.Status = MDO.RESTDataEntities.Standard.APIResponse<T>.StatusEnum.Error;
            response.ValidationModel.ValidationStatus = MDO.RESTDataEntities.Standard.ValidationStatus.Error;
            response.Error = ex.Message;

            return response;
        }
    }

    public class MDORestServiceException : Exception
    {
        public MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.EndpointErrorResponse> EndPointError { get; set; } = new RESTDataEntities.Standard.APIResponse<RESTDataEntities.Standard.EndpointErrorResponse>();
    
        public MDORestServiceException(MDO.RESTDataEntities.Standard.APIResponse<MDO.RESTDataEntities.Standard.EndpointErrorResponse> endPointError)
        {
            this.EndPointError = endPointError;
        }
    }
}
