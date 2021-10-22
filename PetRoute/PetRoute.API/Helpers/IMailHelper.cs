using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PetRoute.commons.Models;

namespace PetRoute.API.Helpers
{
    public interface IMailHelper
    {
        Responses SendMail(string to, string subject, string body);
    }
}
