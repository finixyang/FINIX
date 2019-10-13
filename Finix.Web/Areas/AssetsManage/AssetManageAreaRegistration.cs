﻿using System.Web.Mvc;

namespace Finix.Web.Areas.AssetManage
{
    public class AssetsManageAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "AssetsManage";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                this.AreaName + "_Default",
                this.AreaName + "/{controller}/{action}/{id}",
                new { area = this.AreaName, controller = "Home", action = "Index", id = UrlParameter.Optional },
                new string[] { "Finix.Web.Areas." + this.AreaName + ".Controllers" }
            );
        }
    }
}
