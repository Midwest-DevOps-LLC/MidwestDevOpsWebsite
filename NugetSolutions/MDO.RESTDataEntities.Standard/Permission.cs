using System;
using System.Collections.Generic;
using System.Text;

namespace MDO.RESTDataEntities.Standard
{
    public class Permission
    {
        public int? ID { get; set; }
        public int? ParentPermissionID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Ordinal { get; set; }
        public bool UserAlwaysHas { get; set; }

        public List<Permission> Children { get; set; } = new List<Permission>();

        public static class PermissionLookup
        {
            public static class User
            {
                //public static int User = 1;
                //public static int Email = 2;
                //public static int FullName = 5;
                //public static int Password = 31;
                //public static int UserID = 39;

                public static int ViewAll = 38;
                public enum Email
                {
                    Update = 3,
                    Read = 4,
                    ReadAll = 34,
                    UpdateAll = 35
                }

                public enum FullName
                {
                    Update = 6,
                    Read = 7,
                    ReadAll = 36,
                    UpdateAll = 37
                }

                public enum Password
                {
                    Update = 32,
                    UpdateAll = 33
                }

                public enum UserID
                {
                    UpdateAll = 40
                }
            }

            public static class Site
            {
                public const int Create = 9;
                public const int ViewAll = 12;
                public enum User
                {
                    Add = 10,
                    Remove = 11
                }
            }

            public static class Employee
            {
                public const int View = 15;
                public const int GetEmployeeInformation = 16;
                public const int CreateUpdate = 17;
            }

            public static class ThirdParty
            {
                //public static int ThirdParty = 19;
                public static int GetAllByUser = 41;

                public enum Spotify
                {
                    Get = 21,
                    GetMe = 22,
                    CurrentlyPlaying = 23,
                    RecentlyPlayed = 24,
                    Search = 25,
                    AddTrack = 26
                }
            }

            public static class UserPermission
            {
                public const int CreateUpdate = 28;
                public const int ViewAll = 29;
                public const int Remove = 30;
            }
        }
    }
}
