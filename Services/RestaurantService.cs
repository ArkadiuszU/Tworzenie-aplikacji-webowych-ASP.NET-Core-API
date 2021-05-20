﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Entities;
using WebApplication1.Models;

namespace WebApplication1.Services
{


    public interface IRestaurantService
    {
        public RestaurantDto GetById(int id);

        public IEnumerable<RestaurantDto> GetAll();

        public int Create(CreateRestaurantDto dto);

        public bool Delete(int id);

        public bool Edit(int id, EditRestaurantDto dto);
    }

    public class RestaurantService : IRestaurantService
    {

        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _mapper;

        public RestaurantService(RestaurantDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public RestaurantDto GetById(int id)
        {
            var restaurant = _dbContext.Restaurants
                .Include(r => r.Adress)
                .Include(r => r.Dishes)
                .FirstOrDefault(r => r.Id == id);

            if (restaurant is null)
            {
                return null;
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

        public bool Edit(int id, EditRestaurantDto dto)
        {
            var restaurant = _dbContext.Restaurants
                .FirstOrDefault(r => r.Id == id);

            if (restaurant is null)
            {
                return false;
            }

            restaurant.Name = dto.Name;
            restaurant.HasDelivery = dto.HasDelivery;
            restaurant.Description = dto.Description;

            //_dbContext.Restaurants.Add(restaurant);
            _dbContext.SaveChanges();

            return true;
        }

        public bool Delete(int id)
        {
            var restaurant = _dbContext.Restaurants
                .FirstOrDefault(r => r.Id == id);

            if(restaurant is null)
            {
                return false;
            }


            _dbContext.Restaurants.Remove(restaurant);
            _dbContext.SaveChanges();
            return true;

        }
    }
}
