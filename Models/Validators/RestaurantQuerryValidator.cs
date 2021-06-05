using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models.Validators
{
    public class RestaurantQuerryValidator : AbstractValidator<RestaurantQuerry>
    {
        private int[] allowPagsizes = new[] { 5, 10, 15 };
        private string[] allowedSortByColumnsNames = {nameof(RestaurantDto.Name), nameof(RestaurantDto.Description),
                                                        nameof(RestaurantDto.Category)};
        public RestaurantQuerryValidator()
        {
            RuleFor(x => x.PageNumber).NotEmpty().GreaterThanOrEqualTo(1);
            RuleFor(x => x.PageSize).Custom((value, context) =>
                {
                    if (!allowPagsizes.Contains(value))
                        {
                            context.AddFailure("PageSize", $"PageSize must in [{string.Join(",", allowPagsizes)}]");
                        }
                });
            RuleFor(x => x.SortBy).Custom((value, context) =>
            {
                if ( (!(value is null)) && !allowedSortByColumnsNames.Contains(value))
                {
                    context.AddFailure("SortBy", $"SortBy is optional or must in[{string.Join(",", allowedSortByColumnsNames)}]");
                }
            });

        }
    }
}
