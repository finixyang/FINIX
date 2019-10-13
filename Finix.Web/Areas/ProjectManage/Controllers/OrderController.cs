using Finix.Application.Project;
using Finix.Application.SystemManage;
using Finix.Code;
using Finix.Domain.Entity.Project;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Finix.Web.Areas.ProjectManage.Controllers
{
    public class OrderController : ControllerBase
    {
        private OrderApp orderApp = new OrderApp();
        private ProductApp productApp = new ProductApp();
        private CustomerApp customerApp = new CustomerApp();

        public ActionResult IndexPage()
        {
            return View();
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTreeGridJson(string keyword)
        {
            var data = orderApp.GetTreeList();
            if (!string.IsNullOrEmpty(keyword))
            {
                data = data.TreeWhere(t => t.P_OrderCode.Contains(keyword));
            }
            var treeList = new List<TreeGridModel>();
            foreach (OrderProductModel item in data)
            {
                TreeGridModel treeModel = new TreeGridModel();
                bool hasChildren = item.F_ParentId == null ? true : false;
                treeModel.id = item.F_Id;
                treeModel.isLeaf = hasChildren;
                treeModel.parentId = item.F_ParentId;
                treeModel.expanded = hasChildren;
                treeModel.entityJson = item.ToJson();
                treeList.Add(treeModel);
            }
            var treeJson = treeList.TreeGridJson();
            return Content(data.ToJson());
        }

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
                rows = orderApp.GetList(pagination, keyword),
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
        public ActionResult CreateP()
        {
            return View();
        }
        public ActionResult FormP()
        {
            return View();
        }
        public ActionResult FormO()
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
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetOderProductFormJson(string keyValue)
        {
            var data = orderApp.GetOderProductFormJson(keyValue);
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetOrderFormJson(string keyValue)
        {
            var data = orderApp.GetOrderFormJson(keyValue);
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetOderProductJson(string keyValue)
        {
            var data = orderApp.GetForm(keyValue);
            var order = new OrderEntity();
            order.F_Id = data.F_Id;
            order.P_OrderCode = data.P_OrderCode;
            order.P_CustomerId = data.P_CustomerId;
            return Content(order.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(OrderEntity orderEntity, string keyValue)
        {
            orderApp.SubmitForm(orderEntity, keyValue);
            return Success("操作成功。");
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitFormP(OrderEntity orderEntity)
        {
            orderApp.SubmitFormP(orderEntity);
            return Success("操作成功。");
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitFormOP(OrderProductEditModel model)
        {
            orderApp.SubmitFormOP(model);
            return Success("操作成功。");
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitFormO(OrderEntity model)
        {
            orderApp.SubmitFormO(model);
            return Success("操作成功。");
        }

        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            orderApp.DeleteForm(keyValue);
            return Success("删除成功。");
        }

        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteOP(string keyValue, string orderproductid)
        {
            orderApp.DeleteOP(keyValue, orderproductid);
            return Success("删除成功。");
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetOrderFlowJson()
        {
            var itemdata = new ItemsDetailApp().GetList();
            List<object> list = new List<object>();
            foreach (var item in new ItemsApp().GetList())
            {
                var dataItemList = itemdata.FindAll(t => t.F_ItemId.Equals(item.F_Id) && item.F_EnCode == "OrderFlow" && t.F_EnabledMark == true);
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

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult IsHoldBack(string orderId)
        {
            var isHoldBack = orderApp.CheckOrderIsHold(orderId);
            return Content((new { isHoldBack = !isHoldBack }).ToJson());
        }
    }
}
