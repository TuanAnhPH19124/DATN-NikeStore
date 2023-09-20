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
                if (!await _roleManager.RoleExistsAsync("Employee"))
                    await _roleManager.CreateAsync(new IdentityRole("Employee"));
                if (!await _roleManager.RoleExistsAsync("User"))
                    await _roleManager.CreateAsync(new IdentityRole("User"));
                #endregion

                #region UserManager
                if (!await _userManager.Users.AnyAsync())
                {
                    var admin = new AppUser() { Email = "admin@m.com", UserName = "admin",FullName="admin" };
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
                // if (!await _dbContext.Tags.AnyAsync())
                // {
                //     if (Directory.Exists(filePath))
                //     {
                //         var file = Path.Combine(filePath, "MOCK_TAG.json");
                //         if (File.Exists(file))
                //         {
                //             var jsonContent = File.ReadAllText(file);
                //             var data = JsonConvert.DeserializeObject<List<Tag>>(jsonContent);
                //             using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                //             {
                //                 try
                //                 {
                //                     await _dbContext.Tags.AddRangeAsync(data);
                //                     await _dbContext.SaveChangesAsync();
                //                     await transaction.CommitAsync();
                //                     Console.WriteLine("Sccessfully inserted mock data to table Tags");
                //                 }
                //                 catch (System.Exception)
                //                 {
                //                     await transaction.RollbackAsync();
                //                     throw;
                //                 }
                //             }
                //         }
                //         else
                //         {
                //             Console.WriteLine($"Error: Could not find {file}");
                //         }
                //     }
                //     else
                //     {
                //         Console.WriteLine($"Error: {filePath} is not exists.");
                //     }
                // }
                // Console.WriteLine("Skipping seed data to table Tags..");
                #endregion

                #region Color
                if (!await _dbContext.Colors.AnyAsync())
                {
                 if (Directory.Exists(filePath))
                 {
                   var file = Path.Combine(filePath, "MOCK_COLOR.json");
                   if (File.Exists(file))
                   {
                     var jsonContent = File.ReadAllText(file);
                     var data = JsonConvert.DeserializeObject<List<Color>>(jsonContent);
                     using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                     {
                       try
                       {
                         await _dbContext.Colors.AddRangeAsync(data);
                         await _dbContext.SaveChangesAsync();
                         await transaction.CommitAsync();
                         Console.WriteLine("Sccessfully inserted mock data to table color");
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
                Console.WriteLine("Skipping seed data to table color..");
                #endregion

                #region Material
                if (!await _dbContext.Materials.AnyAsync())
                {
                 if (Directory.Exists(filePath))
                 {
                   var file = Path.Combine(filePath, "MOCK_MATERIAL.json");
                   if (File.Exists(file))
                   {
                     var jsonContent = File.ReadAllText(file);
                     var data = JsonConvert.DeserializeObject<List<Material>>(jsonContent);
                     using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                     {
                       try
                       {
                         await _dbContext.Materials.AddRangeAsync(data);
                         await _dbContext.SaveChangesAsync();
                         await transaction.CommitAsync();
                         Console.WriteLine("Sccessfully inserted mock data to table material");
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
                Console.WriteLine("Skipping seed data to table material..");
                #endregion

                #region Size
                if (!await _dbContext.Sizes.AnyAsync())
                {
                 if (Directory.Exists(filePath))
                 {
                   var file = Path.Combine(filePath, "MOCK_SIZE.json");
                   if (File.Exists(file))
                   {
                     var jsonContent = File.ReadAllText(file);
                     var data = JsonConvert.DeserializeObject<List<Size>>(jsonContent);
                     using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                     {
                       try
                       {
                         await _dbContext.Sizes.AddRangeAsync(data);
                         await _dbContext.SaveChangesAsync();
                         await transaction.CommitAsync();
                         Console.WriteLine("Sccessfully inserted mock data to table Size");
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
                Console.WriteLine("Skipping seed data to table Size..");
                #endregion

                #region Sole
                if (!await _dbContext.Soles.AnyAsync())
                {
                 if (Directory.Exists(filePath))
                 {
                   var file = Path.Combine(filePath, "MOCK_SOLE.json");
                   if (File.Exists(file))
                   {
                     var jsonContent = File.ReadAllText(file);
                     var data = JsonConvert.DeserializeObject<List<Sole>>(jsonContent);
                     using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                     {
                       try
                       {
                         await _dbContext.Soles.AddRangeAsync(data);
                         await _dbContext.SaveChangesAsync();
                         await transaction.CommitAsync();
                         Console.WriteLine("Sccessfully inserted mock data to table Sole");
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
                Console.WriteLine("Skipping seed data to table Sole..");
                #endregion

                #region Product
                if (!await _dbContext.Products.AnyAsync())
                {
                 if (Directory.Exists(filePath))
                 {
                   var file = Path.Combine(filePath, "MOCK_PRODUCT_V2.json");
                   if (File.Exists(file))
                   {
                     var jsonContent = File.ReadAllText(file);
                     var data = JsonConvert.DeserializeObject<List<Product>>(jsonContent);
                     using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                     {
                       try
                       {
                         await _dbContext.Products.AddRangeAsync(data);
                         await _dbContext.SaveChangesAsync();
                         await transaction.CommitAsync();
                         Console.WriteLine("Sccessfully inserted mock data to table Product");
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
                Console.WriteLine("Skipping seed data to table Products..");
                #endregion

                

                #region ProductCategory
                if (!await _dbContext.CategoryProducts.AnyAsync())
                {
                 if (Directory.Exists(filePath))
                 {
                   var file = Path.Combine(filePath, "MOCK_PRODUCT_CATEGORY.json");
                   if (File.Exists(file))
                   {
                     var jsonContent = File.ReadAllText(file);
                     var data = JsonConvert.DeserializeObject<List<CategoryProduct>>(jsonContent);
                     using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                     {
                       try
                       {
                         await _dbContext.CategoryProducts.AddRangeAsync(data);
                         await _dbContext.SaveChangesAsync();
                         await transaction.CommitAsync();
                         Console.WriteLine("Sccessfully inserted mock data to table ProductCategory");
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
                Console.WriteLine("Skipping seed data to table ProductsCategory..");
                #endregion

                #region Stock
                if (!await _dbContext.Stocks.AnyAsync())
                {
                 if (Directory.Exists(filePath))
                 {
                   var file = Path.Combine(filePath, "MOCK_STOCK.json");
                   if (File.Exists(file))
                   {
                     var jsonContent = File.ReadAllText(file);
                     var data = JsonConvert.DeserializeObject<List<Stock>>(jsonContent);
                     using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                     {
                       try
                       {
                         await _dbContext.Stocks.AddRangeAsync(data);
                         await _dbContext.SaveChangesAsync();
                         await transaction.CommitAsync();
                         Console.WriteLine("Sccessfully inserted mock data to table Stock");
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
                Console.WriteLine("Skipping seed data to table Stock..");
                #endregion


                #region Product_image
                if (!await _dbContext.ProductImages.AnyAsync())
                {
                 if (Directory.Exists(filePath))
                 {
                   var file = Path.Combine(filePath, "MOCK_PRODUCT_IMAGE.json");
                   if (File.Exists(file))
                   {
                     var jsonContent = File.ReadAllText(file);
                     var data = JsonConvert.DeserializeObject<List<ProductImage>>(jsonContent);
                     using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                     {
                       try
                       {
                         await _dbContext.ProductImages.AddRangeAsync(data);
                         await _dbContext.SaveChangesAsync();
                         await transaction.CommitAsync();
                         Console.WriteLine("Sccessfully inserted mock data to table ProductImages");
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
                Console.WriteLine("Skipping seed data to table ProductsImages..");
                #endregion
            }
        }
    }
}