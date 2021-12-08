﻿using LibraryBookRenting.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryBookRenting.Services.Interfaces
{
    public interface IUserService
    {
        Task<AuthenticationResponse> SignupAsync(string userName, string password);
    }
}