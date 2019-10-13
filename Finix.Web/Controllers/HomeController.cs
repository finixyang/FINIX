using Finix.Application.Project;
using Finix.Code;
using Finix.Domain.Entity.Project;
using System.Web.Mvc;

namespace Finix.Web.Controllers
{
    [HandlerLogin]
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {

            return View();
        }
        [HttpGet]
        public ActionResult Default()
        {
            return View();
        }
        [HttpGet]
        public ActionResult About()
        {
            return View();
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetOrdersDataJson(string beginTime, string endTime)
        {
            OrderApp orderApp = new OrderApp();
            var orderList = orderApp.GetAllOrderList(beginTime, endTime);
            return Content(orderList.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTaskByUser()
        {
            OrderFlowApp orderFlowApp = new OrderFlowApp();
            FinancialFlowApp financialFlowApp = new FinancialFlowApp();
            var data = new
            {
                financialTask = financialFlowApp.GetFinancialTaskByUser(),
                orderTask = orderFlowApp.GetOrderTaskByUser()
            };
            return Content(data.ToJson());
        }
    }
}
