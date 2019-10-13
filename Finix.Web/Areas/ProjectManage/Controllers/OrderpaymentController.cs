using Finix.Application.Project;
using Finix.Code;
using Finix.Domain.Entity.Project;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Finix.Web.Areas.ProjectManage.Controllers
{
    public class OrderpaymentController : ControllerBase
    {
        private OrderpaymentApp orderpaymentApp = new OrderpaymentApp();
        // GET: ProjectManage/Orderpayment
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
                rows = orderpaymentApp.GetList(pagination, keyword),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }

        // GET: ProjectManage/Orderpayment/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetPaymentOrders()
        {
            var data = orderpaymentApp.GetPaymentOrders();
            List<object> list = new List<object>();
            foreach (var item in data)
            {
                var product = orderpaymentApp.GetProductByOrder(item);
                list.Add(new { id = item.F_Id, text = item.P_OrderCode, num = item.P_OrdreNumber, orderPrice = item.P_OrderPrice, deliveryDate = item.P_DeliveryDate, orderAmount = item.P_OrderPrice * item.P_OrdreNumber, orderStatus = orderpaymentApp.GetOrderStatusByOrder(item).F_ItemName, customerCode = orderpaymentApp .GetCustomerByOrder(item).P_CustomerCode, productName = product.P_ProductName, draw = product.P_DrawingNoOrModelNo });
            }
            return Content(list.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(OrderpaymentEntity orderpaymentEntity, string keyValue)
        {
            orderpaymentApp.SubmitForm(orderpaymentEntity, keyValue);
            return Success("操作成功。");
        }

        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            orderpaymentApp.DeleteForm(keyValue);
            return Success("删除成功。");
        }
    }
}
