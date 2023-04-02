using Domain.Models;
using Infrasructure.Data;
using Microsoft.EntityFrameworkCore;
using Moq;
using Service.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class UrlShortenedRepositoryTests
    {
        // In fact of in project there are no complex bussines logic 
        // i made this stupid simple tests

        Mock<UrlDbContext> contextMock = new Mock<UrlDbContext>();

        [Theory]
        [InlineData("")]
        [InlineData("sdsad")]
        [InlineData("http:/wwww")]
        [InlineData("github.")]
        public async Task Add_InvalidUrl_ShouldReturnFalseAsync(string url)
        {
            //Arrange 
            var options = new DbContextOptionsBuilder<UrlDbContext>()
                .UseInMemoryDatabase(databaseName: "UrlDb")
                .Options;

            using (var context = new UrlDbContext(options))
            {
                ShortenedUrlRepository repository = new ShortenedUrlRepository(context);

                var guid = Guid.NewGuid();

                //Act
                var result = await repository.AddShortenedUrlAsync(url, guid);

                //Assert
                Assert.False(result);
            }
        }

        [Theory]
        [InlineData("http://localhost:4200/home")]
        [InlineData("https://djinni.co/")]
        public async Task Generate_ShortenedUrl_ShouldReturnTrue(string url)
        {
            //Arrange 
            var options = new DbContextOptionsBuilder<UrlDbContext>()
                .UseInMemoryDatabase(databaseName: "UrlDb")
                .Options;

            using (var context = new UrlDbContext(options))
            {
                ShortenedUrlRepository repository = new ShortenedUrlRepository(context);

                var guid = Guid.NewGuid();
                var userId = Guid.NewGuid();

                ShortenedUrl shortened = new()
                {
                    Id = guid,
                    LongUrl = url,
                    UserId = userId,
                    DateOfCreation = default
                };
                await repository.AddShortenedUrlAsync(url, userId);

                //Act
                var result = await repository.DoesLongUrlExists(url);

                //Assert
                Assert.True(result);
            }
        }
        [Theory]
        [InlineData("http://localhost:4200/home")]
        [InlineData("https://djinni.co/")]
        public async Task DoesExist_ShortenedUrl_ShouldReturnFalse(string url)
        {
            //Arrange 
            var options = new DbContextOptionsBuilder<UrlDbContext>()
                .UseInMemoryDatabase(databaseName: "UrlDb")
                .Options;

            using (var context = new UrlDbContext(options))
            {
                ShortenedUrlRepository repository = new ShortenedUrlRepository(context);

                var guid = Guid.NewGuid();
                var userId = Guid.NewGuid();

                string existingUrl = url + "/123";

                ShortenedUrl shortened = new()
                {
                    Id = guid,
                    LongUrl = existingUrl,
                    UserId = userId,
                    DateOfCreation = default
                };
                await repository.AddShortenedUrlAsync(url, userId);

                //Act
                var result = await repository.DoesLongUrlExists(url);

                //Assert
                Assert.True(result);
            }
        }
    }
}
