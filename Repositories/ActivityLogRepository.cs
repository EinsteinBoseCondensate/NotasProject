using NotasProject.Models;
using NotasProject.Models.Config;
using NotasProject.Repoositories;

namespace NotasProject.Repositories
{
    public class ActivityLogRepository : GenericRepo<ActivityLogModel>, IActivityLogRepository
    {
        public ActivityLogRepository(ApplicationDbContext context) : base(context) { }
    }
}
