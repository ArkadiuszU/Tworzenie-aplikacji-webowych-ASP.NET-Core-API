using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Entities;

namespace WebApplication1
{
    public class RestaurantSeeder
    {
        RestaurantDbContext _dbContext;
        public RestaurantSeeder(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Seed()
        {

            if(_dbContext.Database.CanConnect())
            {
                if(!_dbContext.Restaurants.Any())
                {
                    var restaurants = GetRestaurants();
                    _dbContext.Restaurants.AddRange(restaurants);
                    _dbContext.SaveChanges();
                }

                if (!_dbContext.Roles.Any())
                {
                    var roles = GetRoles();
                    _dbContext.Roles.AddRange(roles);
                    _dbContext.SaveChanges();
                }

            }

        }

        private IEnumerable<Restaurant> GetRestaurants()
        {
            var restaurants = new List<Restaurant>()
            {
                new Restaurant()
                {
                    Name = "KFC",
                    Category = "Fast Food",
                    Description= "Descriptopn od KFC",
                    ContactEmail = "sample@kfc.com",
                    HasDelivery = true,
                    Dishes = new List<Dish>()
                    {
                        new Dish()
                        {
                            Name =  "Chicken Nugets",
                            Price = 5.12M,
                        },
                        new Dish()
                        {
                            Name =  "Grander",
                            Price = 8.5M,
                        }
                    },
                    Adress = new Adress()
                    {
                        City= "Warszawa",
                        Street = "Marszałkowska 51",
                        PostalCode= "99-999"
                    }
                },
                 new Restaurant()
                {
                    Name = "Mc Donald",
                    Category = "Fast Food",
                    Description= "Descriptopn od Mc Donald",
                    ContactEmail = "sample@mc.com",
                    HasDelivery = true,
                    Dishes = new List<Dish>()
                    {
                        new Dish()
                        {
                            Name =  "Chicken Nugets",
                            Price = 4.12M,
                        },
                        new Dish()
                        {
                            Name =  "Big Mac",
                            Price = 7.8M,
                        }
                    },
                    Adress = new Adress()
                    {
                        City= "Warszawa",
                        Street = "Długa 12",
                        PostalCode= "99-888"
                    }
                }
            };

            return restaurants;

        }

        private IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>()
            {
                new Role{ Name="User"},
                new Role{ Name="Admin"},
                new Role{ Name="Manager"}
            };

            return roles;
        }
    }
}
