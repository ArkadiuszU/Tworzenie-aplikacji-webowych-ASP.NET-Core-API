using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Authorization
{
    public class CreatedMultipleResourceRequirement : IAuthorizationRequirement
    {
        public CreatedMultipleResourceRequirement(int number)
        {
            MinNumber = number;
        }
        public int MinNumber { get; }
    }
}
