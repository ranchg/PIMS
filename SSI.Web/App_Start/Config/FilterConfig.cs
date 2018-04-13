using System.Web.Mvc;

namespace SSI.Web
{
    public static class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HttpErrorAttribute());
        }
    }
}