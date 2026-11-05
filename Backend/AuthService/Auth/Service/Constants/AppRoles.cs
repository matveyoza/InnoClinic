using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Constants
{
    public static class AppRoles
    {
        public const string Patient = "Patient";
        public const string Doctor = "Doctor";
        public const string Admin = "Admin";

        public static readonly string[] All = { Patient, Doctor, Admin };
    }
}