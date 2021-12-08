﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryBookRenting.Contracts.Requests
{
    public class CreateUserRequest
    {
        [Required]
        public string UserName { get; set; }
    }
}
