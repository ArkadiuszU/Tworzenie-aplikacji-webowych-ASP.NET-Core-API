using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApplication1.Entities;

namespace WebApplication1.Authorization
{
    public class CreatedMultipleResourceRequirementHendler : AuthorizationHandler<CreatedMultipleResourceRequirement>
    {
        private readonly RestaurantDbContext _dbContext;

        public CreatedMultipleResourceRequirementHendler(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CreatedMultipleResourceRequirement requirement)
        {

            int? userId = (context.User.Claims.Count() == 0 )? null : (int?)int.Parse(context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);

            var counter = _dbContext.Restaurants.Count(r => r.CreatedById == userId);

            if(counter >= requirement.MinNumber)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        } 
    }
}
