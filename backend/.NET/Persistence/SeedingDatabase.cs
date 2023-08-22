using Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Persistence
{
    public class SeedingDatabase
    {
        public static async Task Start(IApplicationBuilder applicationBuilder)
        {
            using (var CreateScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var _dbContext = CreateScope.ServiceProvider.GetRequiredService<AppDbContext>();
                var _userManager = CreateScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                var _roleManager = CreateScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                string currentDirectory = Directory.GetCurrentDirectory();
                string filePath = Path.Combine(currentDirectory, "mockData");

                #region RoleManager
                if (!await _roleManager.RoleExistsAsync("Admin"))
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                if (!await _roleManager.RoleExistsAsync("User"))
                    await _roleManager.CreateAsync(new IdentityRole("User"));
                #endregion

                #region UserManager
                if (!await _userManager.Users.AnyAsync())
                {
                    var admin = new AppUser() { Email = "admin@m.com", UserName = "admin" };
                    await _userManager.CreateAsync(admin, "A012292@a");
                    await _userManager.AddToRoleAsync(admin, "Admin");
                    Console.WriteLine("Sccessfully created new a new 'Admin' user");
                }
                #endregion

                #region Category
                if (!await _dbContext.Categories.AnyAsync())
                {
                    if (Directory.Exists(filePath))
                    {
                        var file = Path.Combine(filePath, "MOCK_CATEGORY.json");
                        if (File.Exists(file))
                        {
                            var jsonContent = File.ReadAllText(file);
                            var data = JsonConvert.DeserializeObject<List<Category>>(jsonContent);
                            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                            {
                                try
                                {
                                    await _dbContext.Categories.AddRangeAsync(data);
                                    await _dbContext.SaveChangesAsync();
                                    await transaction.CommitAsync();
                                    Console.WriteLine("Sccessfully inserted mock data to table Tags");
                                }
                                catch (System.Exception)
                                {
                                    await transaction.RollbackAsync();
                                    throw;
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Error: Could not find {file}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Error: {filePath} is not exists.");
                    }
                }
                Console.WriteLine("Skipping seed data to table Categories..");
                #endregion

                #region Tag
                if (!await _dbContext.Tags.AnyAsync())
                {
                    if (Directory.Exists(filePath))
                    {
                        var file = Path.Combine(filePath, "MOCK_TAG.json");
                        if (File.Exists(file))
                        {
                            var jsonContent = File.ReadAllText(file);
                            var data = JsonConvert.DeserializeObject<List<Tag>>(jsonContent);
                            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                            {
                                try
                                {
                                    await _dbContext.Tags.AddRangeAsync(data);
                                    await _dbContext.SaveChangesAsync();
                                    await transaction.CommitAsync();
                                    Console.WriteLine("Sccessfully inserted mock data to table Tags");
                                }
                                catch (System.Exception)
                                {
                                    await transaction.RollbackAsync();
                                    throw;
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Error: Could not find {file}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Error: {filePath} is not exists.");
                    }
                }
                Console.WriteLine("Skipping seed data to table Tags..");
                #endregion

                #region Product
                //if (!await _dbContext.Products.AnyAsync())
                //{
                //  if (Directory.Exists(filePath))
                //  {
                //    var file = Path.Combine(filePath, "MOCK_PRODUCT.json");
                //    if (File.Exists(file))
                //    {
                //      var jsonContent = File.ReadAllText(file);
                //      var data = JsonConvert.DeserializeObject<List<Product>>(jsonContent);
                //      using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                //      {
                //        try
                //        {
                //          await _dbContext.Products.AddRangeAsync(data);
                //          await _dbContext.SaveChangesAsync();
                //          await transaction.CommitAsync();
                //          Console.WriteLine("Sccessfully inserted mock data to table Product");
                //        }
                //        catch (System.Exception)
                //        {
                //          await transaction.RollbackAsync();
                //          throw;
                //        }
                //      }
                //    }
                //    else
                //    {
                //      Console.WriteLine($"Error: Could not find {file}");
                //    }
                //  }
                //  else
                //  {
                //    Console.WriteLine($"Error: {filePath} is not exists.");
                //  }
                //}
                //Console.WriteLine("Skipping seed data to table Products..");
                #endregion

            }
        }
    }
}