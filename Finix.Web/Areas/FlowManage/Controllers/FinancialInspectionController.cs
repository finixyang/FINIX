using Finix.Application.Project;
using System.Web.Mvc;

namespace Finix.Web.Areas.FlowManage.Controllers
{
    public class FinancialInspectionController : ControllerBase
    {
        private FinancialFlowApp financialFlowApp = new FinancialFlowApp();
        // GET: FlowManage/OrderInspection/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FlowManage/OrderInspection/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
