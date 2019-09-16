using NotasProject.Models;
using NotasProject.Repoositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace NotasProject.Repositories
{
    public class UserRepository : GenericRepo<ApplicationUser>
    {

        public UserRepository(ApplicationDbContext context) : base(context) { }

        public ApplicationUser GetByEmail(string email)
        {
            return this.BuildQuery().FirstOrDefault(user => user.Email == email);
        }
        public ApplicationUser GetByName(string email)
        {
            return this.BuildQuery().FirstOrDefault(user => user.UserName == email);
        }
        public void DetachUser(ApplicationUser user)
        {
            _context.Entry(user).State = EntityState.Detached;
        }
    }
}