using NotasProject.Models;
using NotasProject.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotasProject.Services
{
    public static class LoggerService
    {
        public static void LogException(Exception ex)
        {
            using (ActivityLogRepository _logger = new ActivityLogRepository(new ApplicationDbContext()))
            {
                _logger.LogException(ex);
            }
        }
    }
}