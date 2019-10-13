using Finix.Application.Project;
using Finix.Application.SystemManage;
using Finix.Code;
using Finix.Domain.Entity.Project;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Finix.Web.Areas.AssetsManage.Controllers
{
    public class RecipientController : ControllerBase
    {
        private RecipientApp recipientApp = new RecipientApp();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyword)
        {
            var recipientInfo = recipientApp.GetList(pagination, keyword);
            var data = new
            {
                rows = recipientApp.SetRecipientInfo(recipientInfo),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue, string flag)
        {
            var data = recipientApp.GetRecipientInfo(keyValue, flag);
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(RecipientModel recipientModel)
        {
            if (recipientApp.CheckNum(recipientModel))
            {
                recipientApp.SubmitForm(recipientModel);
                return Success("操作成功。");
            }
            return Error("库存数量不足，请及时采购！");
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetRecipientOwner()
        {
            List<object> list = new List<object>();
            var data = new UserApp().GetGroupUserJson();
            foreach (var item in data)
            {
                list.Add(new { id = item.F_Id, text = item.F_RealName });
            }
            return Content(list.ToJson());
        }

        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            recipientApp.DeleteForm(keyValue);
            return Success("删除成功。");
        }
    }
}