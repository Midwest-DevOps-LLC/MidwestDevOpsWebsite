using System;
using System.Collections.Generic;
using System.Text;

namespace MDO.RESTDataEntities.Standard
{
    public class ValidationModel
    {
        public string ErrorFullMessage { get; set; }
        public string SuccessFullMessage { get; set; }
        public string ErrorMessage { get; set; } = "Error";
        public string SuccessMessage { get; set; } = "Success";

        private List<ValidationMessage> _validationMessages = new List<ValidationMessage>();

        public List<ValidationMessage> validationMessages
        {
            get
            {
                if (_validationMessages != null)
                {
                    return _validationMessages;
                }
                else
                {
                    _validationMessages = new List<ValidationMessage>();

                    return _validationMessages;
                }
            }
        }

        public void Add(ValidationMessage v)
        {
            if (this.ValidationStatus == ValidationStatus.Success && v.validationStatus == ValidationStatus.Error)
            {
                this._validationStatus = ValidationStatus.Error;
            }

            validationMessages.Add(v);
        }

        internal ValidationStatus _validationStatus;

        public ValidationStatus ValidationStatus
        {
            get
            {
                return _validationStatus;
            }
            set
            {
                _validationStatus = value;
            }
        }

        public ValidationModel()
        {

        }

        //public ValidationModel(string errorMessage, string successMessage, string fullMessage)
        //{
        //    this.ErrorMessage = errorMessage;
        //    this.SuccessMessage = successMessage;
        //    this.FullMessage = fullMessage;
        //}
    }

    public class ValidationMessage
    {
        public string HTMLID
        {
            get; set;
        }

        public string Message
        {
            get; set;
        }

        public ValidationStatus validationStatus
        {
            get; set;
        }

        public ValidationMessage(string html, string message, ValidationStatus status)
        {
            this.HTMLID = html;
            this.Message = message;
            this.validationStatus = status;
        }

        public ValidationMessage()
        {

        }
    }

    public enum ValidationStatus
    {
        Success = 0,
        Error = 1
    }
}
