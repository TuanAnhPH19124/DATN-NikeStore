using Domain.Entities;
using Domain.Models;
using Domain.Repositories;
using EntitiesDto;
using EntitiesDto.User;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Configuration;
using RestSharp;
using RestSharp.Authenticators;
using Service.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Service
{
    internal sealed class AppUserService : IAppUserService
    {
        private readonly IRepositoryManger _repositoryManger;
       

        public AppUserService(IRepositoryManger repositoryManger)
        {
            _repositoryManger = repositoryManger;
        }

        public async Task<IdentityResult> ConfirmEmailAsync(AppUser user, string code)
        {
            return await _repositoryManger.AppUserRepository.ConfirmEmailAsync(user, code);
        }

        public async Task<AuthResult> CreateAsync(AppUserForCreateDto appUserForCreationDto)
        {
            var user_exist = await _repositoryManger.AppUserRepository.FindByEmailAsync(appUserForCreationDto.Email);

            if (user_exist != null)
            {
                throw new Exception("Email already exist");
            }

            var new_user = appUserForCreationDto.Adapt<AppUser>();

            new_user.EmailConfirmed = false;

            var is_created = await _repositoryManger.AppUserRepository.Insert(new_user, appUserForCreationDto.Password);

            if (is_created.Succeeded)
            {

                var code = await _repositoryManger.AppUserRepository.GenerateEmailConfirmToken(new_user);

                var email_body = $"Please confirm your email address <a href=\"#URL#\">Click here </a>";

                var call_back_url = "https://localhost:44328/authentication/confrimemail/userid" + new_user.Id + "&" + code;

                var body = email_body.Replace("#URL#", call_back_url);

                //send email

                var result = SendEmail(body, new_user.Email);
                
                if (result)
                {
                    return new AuthResult()
                    {
                        Result = true
                    };
                }

                return new AuthResult()
                {
                    Result = false,
                    Error = new List<string>()
                    {
                        "fail"
                    }
                };

                // generate token
                //var token = _repositoryManger.AppUserRepository.GenerateJwtToken(new_user);
                //return new AuthResult()
                //{
                //    Result = true,
                //    Token = token
                //};



            }

            throw new Exception("Server Error");
        }

        public async Task<AppUser> GetauthenticationByGoogle(string email, CancellationToken cancellationToken = default)
        {
            return await _repositoryManger.AppUserRepository.AuthticationUserWithGoogle(email);
        }

        public async Task<AppUser> GetauthenticationByLogin(AppUserForLogin appUser, CancellationToken cancellationToken = default)
        {
            return await _repositoryManger.AppUserRepository.AuthticationUserWithLogin(appUser.Email, appUser.Password);
        }

        public async Task<AppUser> GetByIdAsync(string id)
        {
            return await _repositoryManger.AppUserRepository.GetByIdAsync(id);
        }

        public async Task<AuthResult> Login(AppUserForLogin user)
        {
            var user_exist = await _repositoryManger.AppUserRepository.FindByEmailAsync(user.Email);

            if (user_exist != null)
            {
                if (user_exist.EmailConfirmed)
                {
                    var is_correct = await _repositoryManger.AppUserRepository.CheckPassword(user_exist, user.Password);
                    if (is_correct)
                    {
                        var jwtToken = _repositoryManger.AppUserRepository.GenerateJwtToken(user_exist);
                        return new AuthResult()
                        {
                            Token = jwtToken,
                            Result = true
                        };
                    }
                    throw new Exception("Mật khẩu không chính xác.");
                }

                throw new Exception("Xac nhan email");
            }
            throw new Exception("Tài khoản không tồn tại");
        }

        public bool SendEmail(string body,string email)
        {
            RestClient client = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri("https://api.mailgun.net/v3"),
                Authenticator = new HttpBasicAuthenticator("api", "1433d69b025f2bbaac2b0b8639a08b59-5d9bd83c-dffd1e65")
            });

            RestRequest request = new RestRequest();
            request.AddParameter("domain", "sandbox3de508ca506941de806c80cc9e093f15.mailgun.org", ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";
            request.AddParameter("from", "Nike store <postmaster@sandbox3de508ca506941de806c80cc9e093f15.mailgun.org>");
            request.AddParameter("to", email);
            request.AddParameter("subject", "Email Verification");
            request.AddParameter("text", body);
            request.Method = Method.Post;

            var response = client.Execute(request);

            return response.IsSuccessful;
        }


    }
}
