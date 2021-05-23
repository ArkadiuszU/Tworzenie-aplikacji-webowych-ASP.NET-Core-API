using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Entities;
using WebApplication1.Models;

namespace WebApplication1
{
    public class RestaurantMappingProfile:Profile
    {
        public RestaurantMappingProfile()
        {
            CreateMap<Restaurant, RestaurantDto>()
                .ForMember(m => m.City, c => c.MapFrom(s => s.Adress.City))
                .ForMember(m => m.Street, c => c.MapFrom(s => s.Adress.Street))
                .ForMember(m => m.PostCode, c => c.MapFrom(s => s.Adress.PostalCode));

            CreateMap<Dish, DishDto>();


            CreateMap<CreateRestaurantDto, Restaurant>()
                .ForMember(r => r.Adress, c => c.MapFrom(s => new Adress() { City = s.City, PostalCode = s.PostCode, Street = s.Street }));


             
        }
      
       
    }
}
