using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryBookRenting.Contracts
{
    public static class ApiRoutes
    {
        public const string Root = "api";

        public static class UserRoutes
        {
            public const string GetAll = Root + "/user";
            public const string GetById = Root + "/user/{userId:Guid}";
            public const string Create = Root + "/user";
            public const string Signin = Root + "/user/signin";
        }
    }
}
