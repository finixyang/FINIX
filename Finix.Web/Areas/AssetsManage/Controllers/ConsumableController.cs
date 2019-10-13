using Finix.Application.Project;
using Finix.Code;
using Finix.Domain.Entity.Project;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;

namespace Finix.Web.Areas.AssetsManage.Controllers
{
    public class ConsumableController : ControllerBase
    {
        private ConsumableApp consumableApp = new ConsumableApp();
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
                rows = consumableApp.GetList(pagination, keyword),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }

        public ActionResult IndexPage()
        {
            return View();
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTreeData(string keyword)
        {
            var dataList = consumableApp.GetList();

            var consList = new List<ConsumableList>();
            foreach (ConsumableEntity item in dataList.Where(x => x.F_ParentId.Length < 5).ToList())
            {
                var childList = dataList.Where(x => x.F_ParentId == item.F_Id).ToList();
                var conItem = new ConsumableList
                {
                    F_Id = item.F_Id,
                    F_ParentId = item.F_ParentId,
                    P_TypeId = item.P_TypeId,
                    F_CreatorTime = item.F_CreatorTime,
                    P_AssetToaltNum = childList.Sum(x => x.P_AssetNumber),
                    P_AssetAmount = childList.Sum(x => x.P_AssetAmount),
                    childList = childList
                };
                consList.Add(conItem);
            }
            return Content(consList.ToJson());
        }


        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTreeGridJson(string keyword)
        {
            var data = consumableApp.GetList();
            if (!string.IsNullOrEmpty(keyword))
            {
                data = data.TreeWhere(t => t.P_AssetName.Contains(keyword));
            }
            var treeList = new List<TreeGridModel>();
            foreach (ConsumableEntity item in data)
            {
                TreeGridModel treeModel = new TreeGridModel();
                //bool hasChildren = data.Count(t => t.F_ParentId == item.F_Id) == 0 ? false : true;
                bool hasChildren = data.Where(t => t.F_ParentId == item.F_Id).ToList().Count < 1 ? false : true;
                if (hasChildren)
                {
                    item.F_CreatorTime = null;
                    item.F_CreatorUserId = "";
                    item.P_Owner = "";
                    item.P_AssetModel = "";
                    item.P_PayType = "3";
                    item.P_AssetNumber = 0;
                    item.P_AssetPrice = 0;
                    item.P_RemNum = 0;
                    item.P_AssetAmount = 0;
                    //item.P_AssetName = "";
                }
                treeModel.id = item.F_Id;
                treeModel.isLeaf = hasChildren;
                treeModel.parentId = item.F_ParentId;
                treeModel.expanded = hasChildren;
                treeModel.entityJson = item.ToJson();
                treeList.Add(treeModel);
            }
            var treeJson = treeList.TreeGridJson();
            return Content(treeJson);
        }

        // GET: AssetsManage/Fixedassets/Create
        public ActionResult Create()
        {
            return View();
        }

        // GET: AssetsManage/Fixedassets/Create
        public ActionResult CreateP()
        {
            return View();
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(ConsumableEntity consumableEntity, string keyValue)
        {
            consumableApp.SubmitForm(consumableEntity, keyValue);
            return Success("操作成功。");
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitFormP(ConsumableEntity consumableEntity)
        {
            consumableApp.SubmitFormP(consumableEntity);
            return Success("操作成功。");
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetAssetsTypes()
        {
            var data = consumableApp.GetAssetsTypes();
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
            var data = consumableApp.GetForm(keyValue);
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJsonC(string keyValue)
        {
            var data = consumableApp.GetForm(keyValue);
            data.P_AssetName = "";
            data.P_AssetModel = "";
            data.P_AssetNumber = 0;
            data.P_AssetPrice = 0;
            return Content(data.ToJson());
        }
        
        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            consumableApp.DeleteForm(keyValue);
            return Success("删除成功。");
        }
    }
}
