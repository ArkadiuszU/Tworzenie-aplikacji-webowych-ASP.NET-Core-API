using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApplication1.Authorization
{
    public class MinimumAgeRequirementHendler : AuthorizationHandler<MinimumAgeRequirement>
    {
        private readonly ILogger<MinimumAgeRequirementHendler> _loger;

        public MinimumAgeRequirementHendler(ILogger<MinimumAgeRequirementHendler> loger)
        {
            _loger = loger;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
        {
            var dateofBirth = DateTime.Parse(context.User.FindFirst(c => c.Type == "DateOfBirth").Value);

            var userEmail = context.User.FindFirst(c => c.Type == ClaimTypes.Name).Value;

            _loger.LogInformation($"user : {userEmail} with date of birth: {dateofBirth}");

            if(dateofBirth.AddYears(requirement.MinAge) < DateTime.Today)
            {
                context.Succeed(requirement);
                _loger.LogInformation($"Authorization succedded");
            }
            else
            {
                _loger.LogInformation($"Authorization fault");
            }

            return Task.CompletedTask;

            }
    }
}
