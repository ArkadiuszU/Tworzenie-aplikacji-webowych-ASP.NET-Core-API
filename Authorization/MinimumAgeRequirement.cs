using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Authorization
{
    public class MinimumAgeRequirement :IAuthorizationRequirement
    {
        public int MinAge { get;}
        public MinimumAgeRequirement(int age)
        {
            MinAge = age;
        }
    }
}
