using Application.Services;
using AutoFixture.AutoMoq;
using AutoFixture;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Security.Cryptography;
using System.Text;
using Core.Entities;
using Core.Enums;

namespace Application.Tests.Fixtures
{
    public class PasswordServiceFixture
    {
        public PasswordServiceFixture()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            MockConfiguration = fixture.Freeze<Mock<IConfiguration>>();
            MockConfigurationSection = fixture.Freeze<Mock<IConfigurationSection>>();

            MockPasswordService = new(
                MockConfiguration.Object);

            StringPlaceholder = "SymmetricSecurityKey";
            Bytes = new byte[0];
            PasswordHash = GetPasswordHash();
            User = GetPlayer();
        }

        public PasswordService MockPasswordService { get; }
        public Mock<IConfiguration> MockConfiguration { get; }
        public Mock<IConfigurationSection> MockConfigurationSection { get; }

        public string? StringPlaceholder { get; }
        public byte[] Bytes { get; }
        public byte[] PasswordHash { get; }
        public User User { get; }

        private byte[] GetPasswordHash()
        {
            using (HMACSHA512 hmac = new(Bytes))
            {
                var passwordAsByteArray = Encoding.UTF8.GetBytes(StringPlaceholder!);
                return hmac.ComputeHash(passwordAsByteArray);
            }
        }

        private User GetPlayer()
        {
            return new User()
            {
                Id = 1,
                Username = StringPlaceholder,
                Role = UserRole.User,
                PasswordHash = Bytes,
                PasswordSalt = Bytes
            };
        }
    }
}
