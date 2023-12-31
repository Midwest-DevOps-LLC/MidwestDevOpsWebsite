﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DataEntities
{
    public class User : BaseEntity
    {
        public int? UserID
        {
            get; set;
        }

        public string UUID
        {
            get; set;
        }

        public string Username
        {
            get; set;
        }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        public string Email
        {
            get; set;
        }

        public string Password
        {
            get; set;
        }

        public bool Activated
        {
            get; set;
        }
    }
}
