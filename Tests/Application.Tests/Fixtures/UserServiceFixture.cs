﻿using Application.Services;
using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Entities;
using Core.Enums;
using Core.Interfaces.Repositories;
using Core.Models;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;
using System.Security.Principal;

namespace Application.Tests.Fixtures
{
    public class UserServiceFixture
    {
        public UserServiceFixture()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            MockUserRepository = fixture.Freeze<Mock<IUserRepository>>();
            MockLogger = fixture.Freeze<Mock<ILogger<UserService>>>();

            MockUserService = new(
                MockUserRepository.Object,
                MockLogger.Object);

            Id = 1;
            Name = "Name";
            Bytes = new byte[0];
            User = GetUser();
            PaginatedList = GetPaginatedList();
            IIdentity = GetClaimsIdentity();
            SufficientClaims = GetSufficientClaims();
            InsufficientClaims = GetInsufficientClaims();
        }

        public UserService MockUserService { get; }
        public Mock<IUserRepository> MockUserRepository { get; }
        public Mock<ILogger<UserService>> MockLogger { get; }

        public int Id { get; }
        public string Name { get; }
        public byte[] Bytes { get; }
        public User User { get; }
        public PaginatedList<User> PaginatedList { get; }
        public IIdentity IIdentity { get; }
        public IEnumerable<Claim> SufficientClaims { get; }
        public IEnumerable<Claim> InsufficientClaims { get; }

        private User GetUser()
        {
            return new()
            {
                Id = Id,
                Username = Name,
                Role = UserRole.User,
                PasswordHash = Bytes,
                PasswordSalt = Bytes
            };
        }

        private List<User> GetUsers()
        {
            return new()
            {
                User,
                User
            };
        }

        private PaginatedList<User> GetPaginatedList()
        {
            return new(GetUsers(), 6, 1, 5);
        }

        private IIdentity GetClaimsIdentity()
        {
            return new ClaimsIdentity(Name, Name, Name);
        }

        private IEnumerable<Claim> GetInsufficientClaims()
        {
            return new List<Claim>()
            {
                new Claim(ClaimTypes.Name, Name!),
                new Claim(ClaimTypes.Role, Name!)
            };
        }

        private IEnumerable<Claim> GetSufficientClaims()
        {
            return new List<Claim>()
            {
                new Claim(ClaimTypes.Name, string.Empty),
                new Claim(ClaimTypes.Role, UserRole.Admin.ToString())
            };
        }
    }
}
