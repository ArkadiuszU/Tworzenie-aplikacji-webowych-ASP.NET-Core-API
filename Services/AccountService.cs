﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Entities;
using WebApplication1.Expression;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public interface IAccountService
    {
         void RegisterUser(RegisterUserDto dto);
        string GenerateJwt(LoginDto dto);
    }
    public class AccountService : IAccountService
    {
        private RestaurantDbContext _dbContext;
        private IPasswordHasher<User> _passwordHasher;
        private AuthenticationSettings _authenticationSettings;

        public AccountService(RestaurantDbContext dbContext, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
        }

        public string GenerateJwt(LoginDto dto)
        {
            var user = _dbContext.Users.Include(user=>user.Role).FirstOrDefault(u => u.Email == dto.Email);
            if(user is null)
            {
                throw new BadRequestException("Invalid Username or password");
            }

           var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);

            if(result == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Invalid Username or password");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, "" + user.Email),
                new Claim(ClaimTypes.Role, user.Role.Name),
                new Claim("DateOfBirth", user.DateOfBirth.Value.ToString("yyy-MM-dd")),
            };

            if (!String.IsNullOrEmpty(user.Nationality))
            {
                new Claim("Nationality", user.Nationality);
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer, _authenticationSettings.JwtIssuer, claims, expires: expires, signingCredentials: cred);

            var tokenHendler = new JwtSecurityTokenHandler();


            return tokenHendler.WriteToken(token);

        }


        public void RegisterUser(RegisterUserDto dto)
        {

            var newUser = new User
            {
                Email = dto.Email,
                DateOfBirth = dto.DateOfBirth,
                Nationality = dto.Nationality,
                RoleId = dto.RoleId
            };

            var hashedPassword = _passwordHasher.HashPassword(newUser, dto.Password);

            newUser.PasswordHash = hashedPassword;

            _dbContext.Users.Add(newUser);
            _dbContext.SaveChanges();
        }



    }
}
