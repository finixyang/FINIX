using Finix.Code;
using Finix.Domain.Entity.Project;
using Finix.Domain.Entity.SystemManage;
using Finix.Domain.IRepository.Project;
using Finix.Domain.IRepository.SystemManage;
using Finix.Repository.Project;
using Finix.Repository.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Finix.Application.Project
{
    public class OrderFlowApp
    {
        private IOrderFlowRepository service = new OrderFlowRepository();
        private IOrderRepository orderService = new OrderRepository();
        private IOrderInspectionRepository orderInspectionService = new OrderInspectionRepository();
        private IProductRepository productService = new ProductRepository();
        private IItemsDetailRepository itemsDetailService = new ItemsDetailRepository();
        private IItemsRepository itemsService = new ItemsRepository();
        private IUserRepository userService = new UserRepository();

        public object GetRoleFlowList(string keyword)
        {
            var userInfo = OperatorProvider.Provider.GetCurrent();
            var orderFlows = this.GetRoleInfoList(userInfo.RoleId, keyword);

            return GetOrderFlowInfo(orderFlows);
        }

        public Object GetFlowList(string keyword)
        {
            var userInfo = OperatorProvider.Provider.GetCurrent();
            //UserCode == "admin"
            //var orderFlows = this.GetList(userInfo.UserCode == "admin" ? "" : userInfo.UserId, keyword);
            var orderFlows = this.GetList(userInfo.UserId, keyword);

            return GetOrderFlowInfo(orderFlows);
        }

        public Object GetOrderFlowInfo(List<OrderFlowEntity> orderFlows)
        {
            var orders = this.GetOrderList(orderFlows.Select(x => x.P_OrderId).ToList());
            var products = this.GetProductList(orders.Select(x => x.P_ProductId).ToList());
            var flowsItems = this.GetItemsDetailList(orderFlows.Select(x => x.P_FlowId).ToList());

            var arr = new List<Object>();
            foreach (var flow in orderFlows)
            {
                var orderList = orders.Where(t => t.F_Id == flow.P_OrderId).ToList();
                if (orderList.Count > 0)
                {
                    foreach (var order in orderList)
                    {
                        var product = products.Where(t => t.F_Id == order.P_ProductId).ToList();
                        var flowsItem = flowsItems.Where(t => t.F_Id == flow.P_FlowId).ToList();
                        var inspectionInfo = this.GetOrderInspectionList(order.F_Id);
                        arr.Add(new
                        {
                            F_Id = flow.F_Id,
                            P_OrderId = order.F_Id,
                            F_CreatorUserId = order.F_CreatorUserId,
                            F_CreatorTime = flow.F_CreatorTime,
                            P_FlowStatus = flowsItem[0].F_Id,
                            P_FlowItemName = flowsItem[0].F_ItemName,
                            P_SortCode = flowsItem[0].F_SortCode,
                            P_Executor = order.P_OrderOwner,
                            P_FlowId = flow.P_FlowId,
                            P_OrderCode = order.P_OrderCode,
                            P_ProductId = product[0].F_Id,
                            P_OrderDate = order.F_CreatorTime,
                            P_DeliveryDate = order.P_DeliveryDate,
                            P_OrdreNumber = order.P_OrdreNumber,
                            P_OrderOwner = flow.F_LastModifyUserId,
                            P_OrderStatus = flowsItem[0].F_Id,
                            P_CustomerId = order.P_CustomerId,
                            P_IsOutsourcing = order.P_IsOutsourcing,
                            P_AppearanceIinspection = inspectionInfo.P_AppearanceIinspection,
                            P_DimensionalIinspection = inspectionInfo.P_DimensionalIinspection,
                            P_AmountOfIinspection = inspectionInfo.P_AmountOfIinspection,
                            P_InspectionProportion = inspectionInfo.P_InspectionProportion,
                            P_InspectionResult = inspectionInfo.P_InspectionResult,
                            P_FlowCount = flow.P_FlowCount
                        });
                    }

                }
            }
            return arr;
        }

        public object GetFlowInfos(string keyValue)
        {
            var itemsDetail = itemsService.IQueryable().Where(t => t.F_EnCode == "OrderFlow").ToList();
            var orderFlowIds = itemsDetail.Select(x => x.F_Id).ToList();
            var flowsItems = itemsDetailService.IQueryable().Where(t => orderFlowIds.Contains(t.F_ItemId)).OrderBy(t => t.F_SortCode).ToList();
            var orderFlows = service.IQueryable().Where(t => t.P_OrderId == keyValue).OrderBy(t => t.F_CreatorTime).ToList();
            var users = userService.IQueryable().Where(u => u.F_DeleteMark != true).ToList();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            foreach (var item in flowsItems)
            {
                var orderInfo = orderFlows.Where(x => x.P_FlowId == item.F_Id).ToList();
                OrderFlowEntity order = new OrderFlowEntity();
                if (orderInfo.Count > 0)
                {
                    order = orderInfo[0];
                    var user = users.Where(u => u.F_Id == order.F_CreatorUserId).SingleOrDefault();
                    dic.Add(item.F_SortCode.ToString(), new { id = item.F_Id, name = item.F_ItemName, status = order.P_FlowStatus, orderCount = order.P_FlowCount, creator = user.F_RealName });
                }
                else
                {
                    dic.Add(item.F_SortCode.ToString(), new { id = item.F_Id, name = item.F_ItemName, status = 2 });
                }
            }
            return dic;
        }

        public ItemsDetailEntity GetNextStatus(string executorId_now)
        {
            var orderFlowInfo = itemsService.IQueryable().Where(t => t.F_EnCode == "OrderFlow").ToList();
            var orderFlowIds = orderFlowInfo.Select(x => x.F_Id).ToList();
            var flowsItems = itemsDetailService.IQueryable().Where(t => orderFlowIds.Contains(t.F_ItemId) && t.F_EnabledMark == true).OrderBy(t => t.F_CreatorTime).ToList();
            var nowFlow = flowsItems.Where(t => t.F_Id == executorId_now).ToList()[0];
            if (nowFlow.F_ItemCode == "OutboundOrder")
                return nowFlow;
            //如果是外协订单需要特殊处理流程
            //return flowsItems.Where(t => t.F_SortCode == (nowFlow.F_SortCode + 1)).ToList()[0];
            return GetNextFlow(flowsItems, nowFlow.F_SortCode);
        }

        public ItemsDetailEntity GetNextFlow(List<ItemsDetailEntity> flowsItems, int? sortCode)
        {
            ItemsDetailEntity flowItem = new ItemsDetailEntity();
            var maxCode = flowsItems.Max(x => x.F_SortCode);
            for (int? i = sortCode + 1; i < maxCode; i++)
            {
                var resultList = flowsItems.Where(t => t.F_SortCode == i).ToList();
                if (resultList.Count > 0) {
                    return resultList[0];
                }
            }
            return null;
        }

        public List<UserEntity> GetNextExecutor(string executorId_now)
        {
            var nextFlow = this.GetNextStatus(executorId_now);
            return userService.IQueryable().Where(t => t.F_DutyId == nextFlow.F_Executor && t.F_EnabledMark == true).OrderBy(t => t.F_CreatorTime).ToList();
        }

        public List<OrderFlowEntity> GetList(string userId, string keyword)
        {
            //if (userId == "")
            //    return service.IQueryable().Where(t => t.P_FlowId != null).OrderBy(t => t.F_CreatorTime).ToList();
            //else
            //{
            //    if(keyword == "1")
            //        return service.IQueryable().Where(t => t.P_Executor == userId && t.P_FlowStatus == "1").OrderBy(t => t.F_CreatorTime).ToList();
            //    else
            //        return service.IQueryable().Where(t => t.P_Executor == userId && t.P_FlowStatus == "0").OrderBy(t => t.F_LastModifyTime).ToList();
            //}
            if (keyword == "1")
                return service.IQueryable().Where(t => t.P_Executor == userId && t.P_FlowStatus == "1").OrderBy(t => t.F_CreatorTime).ToList();
            else
                return service.IQueryable().Where(t => t.P_Executor == userId && t.P_FlowStatus == "0").OrderBy(t => t.F_LastModifyTime).ToList();

        }

        public List<OrderFlowEntity> GetRoleInfoList(string roleId, string keyword)
        {
            //var users = userService.IQueryable().Where(u => u.F_RoleId == roleId).ToList();
            //var userIds = users.Select(u => u.F_Id).ToList();
            if (keyword == "1")
                return service.IQueryable().Where(t => t.P_FlowStatus == "1").OrderBy(t => t.F_CreatorTime).ToList();
            else
                return service.IQueryable().Where(t => t.P_FlowStatus == "0").OrderBy(t => t.F_LastModifyTime).ToList();
        }

        public List<OrderEntity> GetOrderList(List<string> orderIds)
        {
            return orderService.IQueryable().Where(t => orderIds.Contains(t.F_Id)).OrderBy(t => t.F_CreatorTime).ToList();
        }

        public List<ProductEntity> GetProductList(List<string> productIds)
        {
            return productService.IQueryable().Where(t => productIds.Contains(t.F_Id)).OrderBy(t => t.F_CreatorTime).ToList();
        }

        public List<ItemsDetailEntity> GetItemsDetailList(List<string> itemsDetailIds)
        {
            return itemsDetailService.IQueryable().Where(t => itemsDetailIds.Contains(t.F_Id)).OrderBy(t => t.F_CreatorTime).ToList();
        }

        public OrderInspectionEntity GetOrderInspectionList(string orderId)
        {
            var list = orderInspectionService.IQueryable().Where(t => t.P_OrderId == orderId).OrderBy(t => t.F_CreatorTime).ToList();
            if (list.Count > 0)
                return list[0];
            return new OrderInspectionEntity();
        }

        public ItemsDetailEntity CheckOrderFlow(string flowId)
        {
            var itemsDetail = GetItemsDetailList(new List<string>()
            {
                flowId
            });
            return itemsDetail.Count > 0 ? itemsDetail[0] : null;
        }

        public OrderFlowEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            service.DeleteForm(keyValue);
        }
        public void SubmitForm(OrderFlowEntity orderFlowEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                orderFlowEntity.Modify(keyValue);
            }
            else
            {
                orderFlowEntity.Create();
            }
            service.SubmitForm(orderFlowEntity, keyValue);
        }

        public void SubmitForm(OrderFlowModel orderFlowinfo, string keyValue, string flowId, string orderFlowId)
        {
            //1.拆分订单流程信息
            //a.订单信息 更新
            OrderEntity orderEntity = new OrderEntity();
            orderEntity.F_Id = keyValue;
            orderEntity.P_CustomerId = orderFlowinfo.P_CustomerId;
            orderEntity.P_OrderCode = orderFlowinfo.P_OrderCode;
            orderEntity.P_ProductId = orderFlowinfo.P_ProductId;
            orderEntity.P_OrderDate = orderFlowinfo.P_OrderDate;
            orderEntity.P_DeliveryDate = orderFlowinfo.P_OrderDate;
            orderEntity.P_OrdreNumber = orderFlowinfo.P_OrdreNumber;
            orderEntity.P_OrderOwner = orderFlowinfo.P_OrderOwner_next;
            orderEntity.P_OrderStatus = orderFlowinfo.P_OrderStatus_next;
            orderEntity.Modify(orderEntity.F_Id);
            var orderResult = orderService.SubmitForm(orderEntity, orderEntity.F_Id);
            if (orderResult > 0)
            {
                //b.流程信息 更新原有流程 
                OrderFlowEntity orderFlowEntity = new OrderFlowEntity();
                orderFlowEntity.P_FlowStatus = "1";
                orderFlowEntity.Modify(orderFlowId);
                var orderFlowResult = service.SubmitForm(orderFlowEntity, orderFlowId);
                if (orderFlowResult > 0)
                {
                    var itemsDetail = itemsDetailService.IQueryable().Where(t => t.F_Id == flowId).SingleOrDefault();
                    //创建检验信息
                    if (itemsDetail.F_ItemCode == "CheckOfFirst" || itemsDetail.F_ItemCode == "CheckOfOrder")
                    {
                        OrderInspectionEntity orderInspectionEntity = new OrderInspectionEntity();
                        orderInspectionEntity.P_OrderId = keyValue;
                        orderInspectionEntity.P_ProductId = orderFlowinfo.P_ProductId;
                        orderInspectionEntity.P_AmountOfIinspection = orderFlowinfo.P_AmountOfIinspection;
                        orderInspectionEntity.P_AppearanceIinspection = orderFlowinfo.P_AppearanceIinspection;
                        orderInspectionEntity.P_DimensionalIinspection = orderFlowinfo.P_DimensionalIinspection;
                        orderInspectionEntity.P_InspectionProportion = orderFlowinfo.P_InspectionProportion;
                        orderInspectionEntity.P_InspectionResult = orderFlowinfo.P_InspectionResult;
                        orderInspectionEntity.P_FlowId = orderFlowEntity.F_Id;
                        orderInspectionEntity.Create();
                        orderInspectionEntity.F_LastModifyUserId = orderInspectionEntity.F_CreatorUserId;
                        orderInspectionEntity.F_LastModifyTime = orderInspectionEntity.F_CreatorTime;
                        orderInspectionService.SubmitForm(orderInspectionEntity, null);
                    }
                    //订单入库 -- 更新产品库存
                    if (itemsDetail.F_ItemCode == "OrderToStorage")
                    {
                        var productInfo = productService.IQueryable().Where(t => t.F_Id == orderEntity.P_ProductId).SingleOrDefault();
                        ProductEntity productEntity = new ProductEntity();
                        productEntity.P_InventoryQuantity = productInfo.P_InventoryQuantity + orderEntity.P_OrdreNumber;
                        productEntity.Modify(productInfo.F_Id);
                        productService.SubmitForm(productEntity, productInfo.F_Id);
                    }
                    //订单出库 -- 更新产品库存
                    if (itemsDetail.F_ItemCode == "OutboundOrder")
                    {
                        var productInfo = productService.IQueryable().Where(t => t.F_Id == orderEntity.P_ProductId).SingleOrDefault();
                        ProductEntity productEntity = new ProductEntity();
                        productEntity.P_InventoryQuantity = productInfo.P_InventoryQuantity - orderEntity.P_OrdreNumber;
                        productEntity.Modify(productInfo.F_Id);
                        productService.SubmitForm(productEntity, productInfo.F_Id);
                    }

                    //新建下一流程
                    if (orderFlowinfo.P_OrderStatus_next != flowId)
                    {
                        orderFlowEntity = new OrderFlowEntity();
                        orderFlowEntity.P_OrderId = keyValue;
                        orderFlowEntity.P_FlowId = orderFlowinfo.P_OrderStatus_next;
                        orderFlowEntity.P_FlowStatus = "0";
                        orderFlowEntity.P_Executor = orderFlowinfo.P_OrderOwner_next;
                        orderFlowEntity.Create();
                        orderFlowEntity.F_LastModifyUserId = orderFlowEntity.F_CreatorUserId;
                        orderFlowEntity.F_LastModifyTime = orderFlowEntity.F_CreatorTime;
                        orderFlowEntity.P_FlowCount = orderFlowinfo.P_FlowCount;
                        service.SubmitForm(orderFlowEntity, null);
                    }
                    
                }
                
            }
            
        }
        public void UpdateForm(OrderFlowEntity orderFlowEntity)
        {
            service.Update(orderFlowEntity);
        }

        public bool CheckOrderIsHold(string orderId)
        {
            var userInfo = OperatorProvider.Provider.GetCurrent();
            var orderInfo = orderService.IQueryable().Where(o => o.F_Id == orderId).SingleOrDefault();
            var orderFlow = service.IQueryable().Where(f => f.P_OrderId == orderId && f.P_FlowId == orderInfo.P_OrderStatus).SingleOrDefault();
            return userInfo.UserId == orderFlow.F_LastModifyUserId;
        }

        public int GetOrderTaskByUser()
        {
            var userInfo = OperatorProvider.Provider.GetCurrent();
            return service.IQueryable().Where(f => f.P_FlowStatus == "0" && f.P_Executor == userInfo.UserId).ToList().Count;
        }
    }
}
