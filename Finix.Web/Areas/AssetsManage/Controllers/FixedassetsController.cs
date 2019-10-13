using Finix.Application.Project;
using Finix.Code;
using Finix.Domain.Entity.Project;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Finix.Web.Areas.AssetsManage.Controllers
{
    public class FixedassetsController : ControllerBase
    {
        private FixedassetsApp fixedassetsApp = new FixedassetsApp();
        // GET: AssetsManage/Fixedassets
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyword)
        {
            var data = new
            {
                rows = fixedassetsApp.GetList(pagination, keyword),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }

        // GET: AssetsManage/Fixedassets/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(FixedassetsEntity fixedassetsEntity, string keyValue)
        {
            fixedassetsApp.SubmitForm(fixedassetsEntity, keyValue);
            return Success("操作成功。");
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetAssetsTypes()
        {
            var data = fixedassetsApp.GetAssetsTypes();
            List<object> list = new List<object>();
            foreach (var item in data)
            {
                list.Add(new { id = item.F_Id, text = item.F_ItemName });
            }
            return Content(list.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = fixedassetsApp.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            fixedassetsApp.DeleteForm(keyValue);
            return Success("删除成功。");
        }

    }
}
