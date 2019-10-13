using Finix.Application.Project;
using Finix.Application.SystemManage;
using Finix.Code;
using Finix.Domain.Entity.Project;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Finix.Web.Areas.ProjectManage.Controllers
{
    public class FinancialController : ControllerBase
    {
        private FinancialApp financialApp = new FinancialApp();

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
                rows = financialApp.GetList(pagination, keyword),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }

        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = financialApp.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(FinancialEntity financialEntity, string keyValue)
        {
            financialApp.SubmitForm(financialEntity, keyValue);
            return Success("操作成功。");
        }
        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            financialApp.DeleteForm(keyValue);
            return Success("删除成功。");
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFinancialFlowJson()
        {
            var itemdata = new ItemsDetailApp().GetList();
            List<object> list = new List<object>();
            foreach (var item in new ItemsApp().GetList())
            {
                var dataItemList = itemdata.FindAll(t => t.F_ItemId.Equals(item.F_Id) && item.F_EnCode == "FinancialFlow");
                if (dataItemList.Count > 0)
                {
                    foreach (var itemList in dataItemList)
                    {
                        list.Add(new { id = itemList.F_Id, text = itemList.F_ItemName });
                    }
                }
            }
            return Content(list.ToJson());
        }
    }
}
