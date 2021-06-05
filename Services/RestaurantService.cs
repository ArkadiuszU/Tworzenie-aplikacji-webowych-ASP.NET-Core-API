using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApplication1.Authorization;
using WebApplication1.Entities;
using WebApplication1.Expression;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public interface IRestaurantService
    {
        public RestaurantDto GetById(int id);

        public PageResult<RestaurantDto> GetAll(RestaurantQuerry querry);

        public int Create(CreateRestaurantDto dto);

        public void Delete(int id);

        public void  Edit(int id, EditRestaurantDto dto);
    }

    public class RestaurantService : IRestaurantService
    {

        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RestaurantService> _loger;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContextService;

        public RestaurantService(RestaurantDbContext dbContext, IMapper mapper, ILogger<RestaurantService> loger, IAuthorizationService authorizationService, IUserContextService userContextService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _loger = loger;
            _authorizationService = authorizationService;
           _userContextService = userContextService;
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

        public PageResult<RestaurantDto> GetAll(RestaurantQuerry querry)
        {
            var searchPhrase = querry.SearchPhrase;

            var baseQuerry = _dbContext.Restaurants
               .Include(r => r.Adress)
               .Include(r => r.Dishes)
               .Where(r => ((searchPhrase == null)) || (r.Name.ToLower().Contains(searchPhrase.ToLower())
                                                   || r.Description.ToLower().Contains(searchPhrase.ToLower())));

            if (!string.IsNullOrEmpty(querry.SortBy))
            {
                var columnSelectors = new Dictionary<string, Expression<Func<Restaurant, object>>>
                {
                    {nameof(Restaurant.Name), r => r.Name}, 
                    {nameof(Restaurant.Description), r => r.Description}, 
                    {nameof(Restaurant.Category), r => r.Category}
                };

                var selectedColumn = columnSelectors[querry.SortBy];

                if (querry.SortDirection == SortDirection.ASC)
                {
                    baseQuerry = baseQuerry.OrderBy(selectedColumn);
                }
                else
                {
                    baseQuerry =  baseQuerry.OrderByDescending(selectedColumn);
                }
                   
                    
            }


            var restaurants = baseQuerry.Skip(querry.PageSize * (querry.PageNumber-1))
               .Take(querry.PageSize)
               .ToList();


            var restaurantsDtos = _mapper.Map<List<RestaurantDto>>(restaurants);


            var result = new PageResult<RestaurantDto>(restaurantsDtos, baseQuerry.Count(), querry.PageSize, querry.PageNumber);

            return result;
        }

        public int Create(CreateRestaurantDto dto)
        {
            var restaurant = _mapper.Map<Restaurant>(dto);
            restaurant.CreatedById = _userContextService.GetUserId;
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

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, restaurant, new ResourceOperationRequirement(ResourceOperation.Update)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException("Authorization Fault");
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

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, restaurant, new ResourceOperationRequirement(ResourceOperation.Delete)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException("Authorization Fault");
            }


            _dbContext.Restaurants.Remove(restaurant);
            _dbContext.SaveChanges();
            

        }
    }
}
