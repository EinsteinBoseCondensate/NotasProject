using NotasProject.Repositories;
using NotasProject.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NotasProject.Models.Config
{
    public class NotasErrorAttribute : HandleErrorAttribute
    {
        public NotasErrorAttribute()
        {
        }
        public override async void OnException(ExceptionContext filterContext)
        {
            await Task.Run(() => { LoggerService.LogException(filterContext.Exception); });

            base.OnException(filterContext);
        }
    }
}