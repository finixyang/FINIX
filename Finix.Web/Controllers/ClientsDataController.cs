using Finix.Application.Project;
using Finix.Application.SystemManage;
using Finix.Code;
using Finix.Domain.Entity.Project;
using Finix.Domain.Entity.SystemManage;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System;

namespace Finix.Web.Controllers
{
    [HandlerLogin]
    public class ClientsDataController : ControllerBase
    {
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetClientsDataJson()
        {
            var data = new
            {
                dataItems = this.GetDataItemList(),
                organize = this.GetOrganizeList(),
                role = this.GetRoleList(),
                duty = this.GetDutyList(),
                authorizeMenu = this.GetMenuList(),
                authorizeButton = this.GetMenuButtonList(),
            };
            return Content(data.ToJson());
        }
        private object GetDataItemList()
        {
            var itemdata = new ItemsDetailApp().GetList();
            Dictionary<string, object> dictionaryItem = new Dictionary<string, object>();
            foreach (var item in new ItemsApp().GetList())
            {
                var dataItemList = itemdata.FindAll(t => t.F_ItemId.Equals(item.F_Id));
                Dictionary<string, string> dictionaryItemList = new Dictionary<string, string>();
                foreach (var itemList in dataItemList)
                {
                    dictionaryItemList.Add(itemList.F_ItemCode, itemList.F_ItemName);
                }
                if (dictionaryItemList.Count > 0)
                {
                    dictionaryItem.Add(item.F_EnCode, dictionaryItemList);
                }                
            }
            return dictionaryItem;
        }
        private object GetOrganizeList()
        {
            OrganizeApp organizeApp = new OrganizeApp();
            var data = organizeApp.GetList();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            foreach (OrganizeEntity item in data)
            {
                var fieldItem = new
                {
                    encode = item.F_EnCode,
                    fullname = item.F_FullName
                };
                dictionary.Add(item.F_Id, fieldItem);
            }
            return dictionary;
        }
        private object GetRoleList()
        {
            RoleApp roleApp = new RoleApp();
            var data = roleApp.GetList();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            foreach (RoleEntity item in data)
            {
                var fieldItem = new
                {
                    encode = item.F_EnCode,
                    fullname = item.F_FullName
                };
                dictionary.Add(item.F_Id, fieldItem);
            }
            return dictionary;
        }
        private object GetDutyList()
        {
            DutyApp dutyApp = new DutyApp();
            var data = dutyApp.GetList();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            foreach (RoleEntity item in data)
            {
                var fieldItem = new
                {
                    encode = item.F_EnCode,
                    fullname = item.F_FullName
                };
                dictionary.Add(item.F_Id, fieldItem);
            }
            return dictionary;
        }
        private object GetMenuList()
        {
            var roleId = OperatorProvider.Provider.GetCurrent().RoleId;
            return ToMenuJson(new RoleAuthorizeApp().GetMenuList(roleId), "0");
        }
        private string ToMenuJson(List<ModuleEntity> data, string parentId)
        {
            StringBuilder sbJson = new StringBuilder();
            sbJson.Append("[");
            List<ModuleEntity> entitys = data.FindAll(t => t.F_ParentId == parentId && t.F_EnabledMark == true);
            if (entitys.Count > 0)
            {
                foreach (var item in entitys)
                {
                    string strJson = item.ToJson();
                    strJson = strJson.Insert(strJson.Length - 1, ",\"ChildNodes\":" + ToMenuJson(data, item.F_Id) + "");
                    sbJson.Append(strJson + ",");
                }
                sbJson = sbJson.Remove(sbJson.Length - 1, 1);
            }
            sbJson.Append("]");
            return sbJson.ToString();
        }
        private object GetMenuButtonList()
        {
            var roleId = OperatorProvider.Provider.GetCurrent().RoleId;
            var data = new RoleAuthorizeApp().GetButtonList(roleId);
            var dataModuleId = data.Distinct(new ExtList<ModuleButtonEntity>("F_ModuleId"));
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            foreach (ModuleButtonEntity item in dataModuleId)
            {
                var buttonList = data.Where(t => t.F_ModuleId.Equals(item.F_ModuleId));
                dictionary.Add(item.F_ModuleId, buttonList);
            }
            return dictionary;
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetOrderRelationDataJson()
        {
            var data = new
            {
                user = this.GetUserList(),
                customerItems = this.GetCustomerList(),
                productItems = this.GetProductList(),
                orderFlows = this.GetOrderFlow(),
                financialFlows = this.GetFinancialFlow(),
            };
            return Content(data.ToJson());
        }

        private object GetFinancialFlow()
        {
            return GetItemsDetailInfo("FinancialFlow");
        }

        private object GetCustomerList()
        {
            CustomerApp customerApp = new CustomerApp();
            var data = customerApp.GetList();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            foreach (CustomerEntity item in data)
            {
                var fieldItem = new
                {
                    customerName = item.P_CustomerName,
                    customerCode = item.P_CustomerCode,
                    customerLevel = item.P_CustomerLevel
                };
                dictionary.Add(item.F_Id, fieldItem);
            }
            return dictionary;
        }

        private object GetProductList()
        {
            ProductApp productApp = new ProductApp();
            var data = productApp.GetList();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            foreach (ProductEntity item in data)
            {
                var fieldItem = new
                {
                    productName = item.P_ProductName,
                    productNo = item.P_DrawingNoOrModelNo,
                    productInfo = item.P_ProductName + " [" + item.P_DrawingNoOrModelNo + "]"
                };
                dictionary.Add(item.F_Id, fieldItem);
            }
            return dictionary;
        }
        private object GetUserList()
        {
            UserApp userApp = new UserApp();
            var data = userApp.GetGroupUserJson();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            foreach (UserEntity item in data)
            {
                var fieldItem = new
                {
                    userName = item.F_RealName,
                };
                dictionary.Add(item.F_Id, fieldItem);
            }
            return dictionary;
        }

        private object GetOrderFlow()
        {
            return GetItemsDetailInfo("OrderFlow");
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetAssetsDataJson()
        {
            var data = new
            {
                asset = this.GetAssetList(),
                consumable = this.GetConsumableList(),
                benefits = this.GetBenefitsList()
            };
            return Content(data.ToJson());
        }
        private object GetAssetList()
        {
            return GetItemsDetailInfo("fixedAssetTypes");
        }

        private object GetConsumableList()
        {
            return GetItemsDetailInfo("consumableTypes");
        }

        private object GetBenefitsList()
        {
            return GetItemsDetailInfo("benefitsTypes");
        }

        private object GetItemsDetailInfo(string enCode)
        {
            var itemdata = new ItemsDetailApp().GetList();
            Dictionary<string, object> dictionaryItem = new Dictionary<string, object>();
            foreach (var item in new ItemsApp().GetList())
            {
                var dataItemList = itemdata.FindAll(t => t.F_ItemId.Equals(item.F_Id) && item.F_EnCode == enCode);
                Dictionary<string, string> dictionaryItemList = new Dictionary<string, string>();
                foreach (var itemList in dataItemList)
                {
                    var fieldItem = new
                    {
                        itemName = itemList.F_ItemName,
                    };
                    dictionaryItem.Add(itemList.F_Id, fieldItem);
                }
            }
            return dictionaryItem;
        }
    }
}
