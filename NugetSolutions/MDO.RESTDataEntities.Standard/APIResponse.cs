using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MDO.RESTDataEntities.Standard
{
    public class APIResponse<T>
    {
        public T Data { get; set; }
        //public EndpointErrorResponse EndpointErrorResponse { get; set; } this is going to get thrown as a custom execption now
        private StatusEnum _Status { get; set; } = StatusEnum.None;
        public StatusEnum Status {
            get
            {
                if (ValidationModel.ValidationStatus == ValidationStatus.Error)
                    return StatusEnum.Error;

                return _Status;
            }
            set
            {
                _Status = value;
            }
        }
        public string Error { get; set; }
        public bool? IsSessionValid { get; set; }
        public ValidationModel ValidationModel { get; set; } = new ValidationModel();

        public enum StatusEnum
        {
            None,
            Complete,
            Error
        }

        /// <summary>
        /// Will add the error message also to the validation model
        /// </summary>
        public void AddError(string errorMessage)
        {
            this.Error = errorMessage;
            this.ValidationModel.ErrorFullMessage = errorMessage;
            this.Status = StatusEnum.Error;
            this.ValidationModel.ValidationStatus = ValidationStatus.Error;
        }

        public APIResponse(object dataObject, string errorMessage, string successMessage, string errorFullMessage, string successFullMessage)
        {
            this.Data = (T)dataObject;
            this.ValidationModel.ErrorMessage = errorMessage;
            this.ValidationModel.SuccessMessage = successMessage;
            this.ValidationModel.ErrorFullMessage = errorFullMessage;
            this.ValidationModel.SuccessFullMessage = successFullMessage;
        }

        public APIResponse(string errorMessage, string successMessage, string errorFullMessage, string successFullMessage)
        {
            this.ValidationModel.ErrorMessage = errorMessage;
            this.ValidationModel.SuccessMessage = successMessage;
            this.ValidationModel.ErrorFullMessage = errorFullMessage;
            this.ValidationModel.SuccessFullMessage = successFullMessage;
        }

        public APIResponse(string errorMessage, string successMessage)
        {
            this.ValidationModel.ErrorMessage = errorMessage;
            this.ValidationModel.SuccessMessage = successMessage;
        }

        public APIResponse(string fullErrorMessage)
        {
            this.ValidationModel.ErrorFullMessage = fullErrorMessage;
        }

        public APIResponse()
        {

        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
