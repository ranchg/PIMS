using System.Web.Mvc;

namespace SSI.Web.Areas.EquipmentManage
{
    public class EquipmentManageAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "EquipmentManage";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "EquipmentManage_default",
                "EquipmentManage/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}