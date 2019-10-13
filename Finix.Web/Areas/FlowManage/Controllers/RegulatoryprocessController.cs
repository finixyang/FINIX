using Finix.Application.Project;
using Finix.Application.SystemManage;
using Finix.Code;
using Finix.Domain.Entity.Project;
using Finix.Domain.Entity.SystemManage;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Finix.Web.Areas.FlowManage.Controllers
{
    public class RegulatoryprocessController : ControllerBase
    {
        private ProductApp productApp = new ProductApp();
        private CustomerApp customerApp = new CustomerApp();
        private OrderFlowApp orderFlowApp = new OrderFlowApp();
        private OrderApp orderApp = new OrderApp();
        private UserApp userApp = new UserApp();

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyword = "0")
        {
            var obj = orderFlowApp.GetRoleFlowList(keyword);
            var data = new
            {
                rows = obj,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFlowInfos(string keyValue)
        {
            var obj = orderFlowApp.GetFlowInfos(keyValue);
            return Content(obj.ToJson());
        }

        // GET: FlowManage/OrderFlow/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = orderApp.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(OrderFlowModel orderFlowInfo, string keyValue, string flowId, string orderFlowId)
        {
            orderFlowApp.SubmitForm(orderFlowInfo, keyValue, flowId, orderFlowId);
            return Success("操作成功。");
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGroupUserJson(string keyValue)
        {
            var data = orderFlowApp.GetNextExecutor(keyValue);
            List<object> list = new List<object>();
            foreach (UserEntity item in data)
            {
                list.Add(new { id = item.F_Id, text = item.F_RealName });
            }
            return Content(list.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetNextOrderFlowJson(string keyValue)
        {
            var itemsDetail = orderFlowApp.GetNextStatus(keyValue);
            List<object> list = new List<object>();
            list.Add(new { id = itemsDetail.F_Id, text = itemsDetail.F_ItemName });
            return Content(list.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult CheckOrderFlow(string flowId)
        {
            var itemsDetail = orderFlowApp.CheckOrderFlow(flowId);
            if (itemsDetail == null)
            {
                return Content((new { result = false }).ToJson());
            }
            var result = new { result = itemsDetail.F_ItemCode == "CheckOfOrder" || itemsDetail.F_ItemCode == "CheckOfFirst" };
            return Content(result.ToJson());
        }
    }
}