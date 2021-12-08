using LibraryBookRenting.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryBookRenting.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected void AddErrorsFromModelState(ref ErrorModel error)
        {
            error.ErrorMessages.AddRange(ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            return;
            foreach (string errorMessage in ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage))
            {

            };
        }
    }
}
