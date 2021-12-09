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
            public const string Create = Root + "/user";
            public const string Signin = Root + "/user/signin";
            public const string RentBook = Root + "/user/book";
            public const string GetBook = Root + "/user/book";
            public const string GetBookExpireInWeek = Root + "/user/book-expire-in-week";
            public const string GetBookExpired = Root + "/user/book-expired";
            public const string GetBookNotExpired = Root + "/user/book-not-expired";
        }

        public static class BookRoutes
        {
            public const string GetAll = Root + "/book";
        }
    }
}
