using Finix.Application.Project;
using Finix.Code;
using Finix.Domain.Entity.Project;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Finix.Web.Areas.ProjectManage.Controllers
{
    public class ProductController : ControllerBase
    {
        private ProductApp productApp = new ProductApp();

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
                rows = productApp.GetList(pagination, keyword),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTreeSelectJson()
        {
            var data = productApp.GetList();
            List<object> list = new List<object>();
            foreach (ProductEntity item in data)
            {
                list.Add(new { id = item.F_Id, text = item.P_ProductName + " ["+ item.P_DrawingNoOrModelNo + "]", draw = item.P_DrawingNoOrModelNo });
            }
            return Content(list.ToJson());
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
            var data = productApp.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(ProductEntity productEntity, string keyValue)
        {
            productApp.SubmitForm(productEntity, keyValue);
            return Success("操作成功。");
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult CheckProductInfo(string name, string no)
        {
           var data = productApp.CheckProductInfo(name, no);
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            productApp.DeleteForm(keyValue);
            return Success("删除成功。");
        }

        // GET: Order/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Order/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
