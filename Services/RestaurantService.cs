using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Entities;
using WebApplication1.Exceptions;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public interface IRestaurantService
    {
        public RestaurantDto GetById(int id);

        public IEnumerable<RestaurantDto> GetAll();

        public int Create(CreateRestaurantDto dto);

        public void Delete(int id);

        public void  Edit(int id, EditRestaurantDto dto);
    }

    public class RestaurantService : IRestaurantService
    {

        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RestaurantService> _loger;

        public RestaurantService(RestaurantDbContext dbContext, IMapper mapper, ILogger<RestaurantService> loger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _loger = loger;
        }

        public RestaurantDto GetById(int id)
        {
            var restaurant = _dbContext.Restaurants
                .Include(r => r.Adress)
                .Include(r => r.Dishes)
                .FirstOrDefault(r => r.Id == id);

            if (restaurant is null)
            {
                throw new NotFoundException("Restaurant not found");
            }

            var restaurantDto = _mapper.Map<RestaurantDto>(restaurant);

            return restaurantDto;
        }

        public IEnumerable<RestaurantDto> GetAll()
        {
            
            var restaurants = _dbContext.Restaurants
               .Include(r => r.Adress)
               .Include(r => r.Dishes)
               .ToList();

            var restaurantsDtos = _mapper.Map<List<RestaurantDto>>(restaurants);

            return restaurantsDtos;
        }

        public int Create(CreateRestaurantDto dto)
        {
            var restaurant = _mapper.Map<Restaurant>(dto);
            _dbContext.Restaurants.Add(restaurant);
            _dbContext.SaveChanges();

            return restaurant.Id;
        }

        public void Edit(int id, EditRestaurantDto dto)
        {
            var restaurant = _dbContext.Restaurants
                .FirstOrDefault(r => r.Id == id);

            if (restaurant is null)
            {
                throw new NotFoundException("Restaurant not found");
            }

            restaurant.Name = dto.Name;
            restaurant.HasDelivery = dto.HasDelivery;
            restaurant.Description = dto.Description;

            //_dbContext.Restaurants.Add(restaurant);
            _dbContext.SaveChanges();

         
        }

        public void Delete(int id)
        {

            _loger.LogError($"Restaurant {id} not found");

            var restaurant = _dbContext.Restaurants
                .FirstOrDefault(r => r.Id == id);

            if(restaurant is null)
            {
                throw new NotFoundException("Restaurant not found");
               
            }


            _dbContext.Restaurants.Remove(restaurant);
            _dbContext.SaveChanges();
            

        }
    }
}
