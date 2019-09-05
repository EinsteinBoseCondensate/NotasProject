using NotasProject.Models;
using NotasProject.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotasProject.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepo;
        public UserService(UserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public ApplicationUser GetByEmail(string email)
        {
            return _userRepo.GetByEmail(email);
        }

        public void DetachUser(ApplicationUser user)
        {
            _userRepo.DetachUser(user);
        }

    }
}