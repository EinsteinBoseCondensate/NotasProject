using NotasProject.Models.Config;
using NotasProject.Repositories;
using System.Web;
using System.Web.Mvc;

namespace NotasProject
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new NotasErrorAttribute());
        }
    }
}
