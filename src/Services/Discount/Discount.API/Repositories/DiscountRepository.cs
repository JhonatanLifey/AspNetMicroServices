using Dapper;
using Discount.API.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.API.Repositories
{
    public class DiscountRepository: IDiscountRepository
    {
        private readonly IConfiguration _configuration;

        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Coupon> GetDiscount(string productName) 
        {
            using var connection = new NpgsqlConnection
                (_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>("SELECT * FROM coupon WHERE ProductName = @ProductName", 
                new { ProductNAme = productName });

            if (coupon == null) 
                return new Coupon
                { ProductName = "No Discount", Amount = 0, Description = "No Discount Desc" };

                return coupon;
            
        }

        public async Task<bool> CreateDiscount(Coupon coupon)
        {

            using var connection = new NpgsqlConnection
                (_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            

            var effect =
                await connection.ExecuteAsync
                    ("INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)",
                    new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount });

            if (effect == 0)
                return false;

            return true;

        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {

            using var connection = new NpgsqlConnection
                (_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var effect =
                await connection.ExecuteAsync
                ("UPDATE Coupon Set ProductName = @ProductName, Description = @Description, Amount = @Amount WHERE Id = @Id", 
                new { ProductName = coupon.ProductName, Description = coupon.Description, Amount=coupon.Amount , Id = coupon.Id});

            if (effect == 0)
                return false;

            return true;
        }

        public async Task<bool> DeleteDiscount(string productName)
        {

            using var connection = new NpgsqlConnection
                (_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var effect =
                await connection.ExecuteAsync
                ("DELETE FROM Coupon WHERE ProductName = @ProductName",
                new { ProductName = productName });

            if (effect == 0)
                return false;

            return true;
        }

    }
}
