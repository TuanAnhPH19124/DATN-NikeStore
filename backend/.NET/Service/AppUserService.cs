using Domain.DTOs;
using Domain.Entities;
using Domain.Models;
using Domain.Repositories;
using EntitiesDto;
using EntitiesDto.User;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Service.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
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

                var email_body = $"Please confirm your email Addr <a href=\"#URL#\">Click here </a>";

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
            return await _repositoryManger.AppUserRepository.AuthticationUserWithLogin(appUser.Account, appUser.Password);
        }

        public async Task<AppUser> GetByIdAsync(string id)
        {
            return await _repositoryManger.AppUserRepository.GetByIdAsync(id);
        }

        public async Task<AuthResult> Login(AppUserForLogin user)
        {
            var user_exist = await _repositoryManger.AppUserRepository.FindByEmailAsync(user.Account);

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

        public bool SendEmail(string emailBody, string toEmail)
        {
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("nikesneakerw@gmail.com", "kxsdwillelpulshc"),
                    EnableSsl = true,
                };
                var mailMessage = new MailMessage("your.email@yourprovider.com", toEmail, "Đặt lại mật khẩu", emailBody);
                smtpClient.Send(mailMessage);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<AppUser>> GetAllAppUserAsync(CancellationToken cancellationToken = default)
        {
            List<AppUser> appUserList = await _repositoryManger.AppUserRepository.GetAllAppUserAsync(cancellationToken);
            return appUserList;
        }

        public async Task<AppUser> GetByIdAppUserAsync(string id, CancellationToken cancellationToken = default)
        {
            AppUser appUser = await _repositoryManger.AppUserRepository.GetByIdAsync(id, cancellationToken);
            return appUser;
        }

        public async Task<AppUser> UpdateByIdAppUser(string id, AppUser appUser, CancellationToken cancellationToken = default)
        {
            var existingAppUser = await _repositoryManger.AppUserRepository.GetByIdAsync(id, cancellationToken);
            if (existingAppUser == null)
            {
                throw new Exception("User not found.");
            }
            else
            {
                existingAppUser.FullName = appUser.FullName;
                existingAppUser.PhoneNumber= appUser.PhoneNumber;
                existingAppUser.AvatarUrl= appUser.AvatarUrl;
                await _repositoryManger.AppUserRepository.UpdateAppUser(existingAppUser);
                return existingAppUser;
            }
        }

        public async Task<AppUser> UpdateByIdAppUserByAdmin(string id, AppUser appUser, CancellationToken cancellationToken = default)
        {
            var existingAppUser = await _repositoryManger.AppUserRepository.GetByIdAsync(id, cancellationToken);
            if (existingAppUser == null)
            {
                throw new Exception("User not found.");
            }
            else
            {
                existingAppUser.Status= appUser.Status;
                await _repositoryManger.AppUserRepository.UpdateAppUserbyAdmin(existingAppUser);
                return existingAppUser;
            }
        }

        public async Task<IdentityResult> ChangePasswordAsync(AppUser user, string currentPassword, string newPassword)
        {
            var isCorrectPassword = await _repositoryManger.AppUserRepository.CheckPassword(user, currentPassword);
            if (!isCorrectPassword)
            {
                // Trả về kết quả không thành công thay vì ném exception
                return IdentityResult.Failed(new IdentityError { Description = "Mật khẩu hiện tại không chính xác." });
            }

            // Thay đổi mật khẩu mới
            var result = await _repositoryManger.AppUserRepository.ChangePasswordAsync(user, currentPassword, newPassword);

            if (!result.Succeeded)
            {
                // Xử lý trường hợp thay đổi mật khẩu không thành công
                // Trả về kết quả không thành công hoặc thực hiện các xử lý khác
                return result;
            }

            // Trả về kết quả thành công
            return result;
        }


        private string GenerateRandomPassword(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+[]{}|;:,.<>?";
            var random = new Random();
            var newPassword = new string(Enumerable.Repeat(chars, length - 3) // Số ký tự - 3 (để để 3 ký tự đặc biệt, chữ hoa và chữ thường)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            // Thêm ít nhất 1 ký tự đặc biệt
            var specialChars = "!@#$%^&*()_+[]{}|;:,.<>?";
            newPassword += specialChars[random.Next(specialChars.Length)];

            // Thêm ít nhất 1 chữ viết hoa
            var upperCaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            newPassword += upperCaseChars[random.Next(upperCaseChars.Length)];

            // Trộn ngẫu nhiên lại để đảm bảo tính ngẫu nhiên của mật khẩu
            var shuffledPassword = new string(newPassword.ToCharArray().OrderBy(c => random.Next()).ToArray());

            return shuffledPassword;
        }
        
        public async Task<AuthResult> ForgotPassword(string email)
        {
            var user = await _repositoryManger.AppUserRepository.FindByEmailAsync(email);
            if (user == null)
            {
                return new AuthResult
                {
                    Result = false,
                    Error = new List<string> { "Không tìm thấy người dùng với địa chỉ email này." }
                };
            }

            var newPassword = GenerateRandomPassword(10);

            var token = await _repositoryManger.AppUserRepository.GeneratePasswordResetTokenAsync(user);
            var resetResult = await _repositoryManger.AppUserRepository.ResetPasswordAsync(user, token, newPassword);

            if (!resetResult.Succeeded)
            {
                return new AuthResult
                {
                    Result = false,
                    Error = new List<string> { "Không thể đặt lại mật khẩu." }
                };
            }

            var emailBody = $"Mật khẩu mới của bạn là: {newPassword} Vui lòng đăng nhập và thay đổi mật khẩu sau khi đăng nhập.";

            var isEmailSent =  SendEmail(emailBody, user.Email);
           

            if (!isEmailSent)
            {
                return new AuthResult
                {
                    Result = false,
                    Error = new List<string> { "Gửi email thất bại." }
                };
            }

            return new AuthResult
            {
                Result = true
            };
        }

        public async Task<AppUserPhoneDto> GetUserByPhoneNumber(string phoneNumber)
        {
            var user = await _repositoryManger.AppUserRepository.GetByPhoneAsync(phoneNumber);
            if (user == null)
                return null;
            return user;
        }
    }
}
