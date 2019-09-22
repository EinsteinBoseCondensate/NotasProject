using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NotasProject.Models;
using NotasProject.Models.Config;
using NotasProject.Properties;
using NotasProject.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace NotasProject.Services
{
    public class UserService
    {
        private readonly UserRepository UserRepo;
        public UserService(UserRepository userRepo)
        {
            UserRepo = userRepo;
        }
        public ApplicationUser GetByEmail(string email)
        {
            return UserRepo.GetByEmail(email);
        }

        public void DetachUser(ApplicationUser user)
        {
            UserRepo.DetachUser(user);
        }
        public ApplicationUser GetUserById(string userId)
        {
            return UserRepo.GetById(userId);
        }
        public ConfirmationState IsConfirmationOk(string userId, string code, bool isEmailConfirmation = true)
        {
            try
            {   
                if(userId == null || code == null)
                {
                    return ConfirmationState.DataKO;
                }
                ApplicationUser user = GetUserById(userId);
                var checker = isEmailConfirmation ? user == null || user.EmailConfirmed : user == null;
                if (checker)
                {
                    UserRepo.Dispose();
                    return ConfirmationState.DataKO;
                }
                var separator = new string[] { Resources.KeyCacheSeparator };
                var splitted = code.Split(separator, StringSplitOptions.None);
                var decryptedSplittedData = CryptoService.AES_Decrypt(splitted[0], Convert.FromBase64String(splitted[1])).Split(separator, StringSplitOptions.None);
                if (DateTime.Parse(decryptedSplittedData[1]) < DateTime.Now)
                {
                    UserRepo.Dispose();
                    return ConfirmationState.Outdated;
                }
                else if(decryptedSplittedData[0] != userId)
                {
                    UserRepo.Dispose();
                    return ConfirmationState.DataKO;
                }
                user.EmailConfirmed = true;
                var result = UserRepo.TrySaveChanges() == PersistedState.OK ? ConfirmationState.OK : ConfirmationState.ConnectionProblem;
                UserRepo.Dispose();
                return result;
            }
            catch (Exception e)
            {
                UserRepo.LogException(e);
                UserRepo.Dispose();
                return ConfirmationState.DataKO;
            }            
        }

    }


}