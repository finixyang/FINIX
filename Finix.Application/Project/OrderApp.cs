using Finix.Code;
using Finix.Domain.Entity.Project;
using System.Collections.Generic;
using Finix.Domain.IRepository.Project;
using Finix.Repository.Project;
using System.Linq;
using Finix.Domain.IRepository.SystemManage;
using Finix.Repository.SystemManage;
using Finix.Domain.Entity.SystemManage;
using System;
using Newtonsoft.Json;

namespace Finix.Application.Project
{
    public class OrderApp
    {
        private IOrderRepository service = new OrderRepository();
        private IOrderFlowRepository flowService = new OrderFlowRepository();
        private IItemsDetailRepository itemsDetailService = new ItemsDetailRepository();
        private IItemsRepository itemsService = new ItemsRepository();
        private IOrderpaymentRepository orderPaymentService = new OrderpaymentRepository();

        private ICustomerRepository customerService = new CustomerRepository();

        private IOrderProductRepository orderProductService = new OrderProductRepository();
        private IProductRepository productService = new ProductRepository();
        private IOrderInspectionRepository orderInspectionServer = new OrderInspectionRepository();

        public List<OrderEntity> GetList(Pagination pagination, string keyword)
        {
            var expression = ExtLinq.True<OrderEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.P_OrderCode.Contains(keyword));
            }
            expression = expression.And(t => t.F_DeleteMark != true);
            this.SplitOrders();
            return service.FindList(expression, pagination);
        }

        private void SplitOrders()
        {
            var opList = orderProductService.IQueryable().Where(o => o.F_CreatorUserId != "").ToList();
            var orderIds = opList.Select(x => x.P_OrderId).ToList();
            var orderList = service.IQueryable().Where(x => x.F_DeleteMark != true && !orderIds.Contains(x.F_Id)).OrderByDescending(x => x.F_CreatorTime).ToList();
            var containtOrder = service.IQueryable().Where(x => x.F_DeleteMark != true && orderIds.Contains(x.F_Id)).OrderByDescending(x => x.F_CreatorTime).ToList();
            if (containtOrder.Count > 0)
            {
                foreach (var order in orderList)
                {
                    var id = "";
                    var orderInfo = containtOrder.Where(x => x.P_ProductId == order.P_ProductId && x.P_OrderCode == order.P_OrderCode).ToList();
                    if (orderInfo == null)
                    {
                        id = order.F_Id;
                        var orderProductInfo = orderProductService.IQueryable().Where(o => o.P_OrderId == id && o.P_ProductId == order.P_ProductId).ToList();
                        if (orderProductInfo.Count < 1)
                            this.CreateOrderProduct(order, id);
                    }
                }
            }
            else
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                foreach (var order in orderList)
                {
                    var id = "";
                    if (dic.Keys.Contains(order.P_OrderCode))
                    {
                        dic.TryGetValue(order.P_OrderCode, out id);
                        var flowInfos = flowService.IQueryable().Where(x => x.P_OrderId == order.F_Id).ToList();
                        //删除流程信息
                        foreach (var flow in flowInfos)
                        {
                            flowService.DeleteForm(flow.F_Id);
                        }
                    }
                    else
                    {
                        id = order.F_Id;
                        dic.Add(order.P_OrderCode, id);
                    }
                    var orderProductInfo = orderProductService.IQueryable().Where(o => o.P_OrderId == id && o.P_ProductId == order.P_ProductId).ToList();
                    if (orderProductInfo.Count < 1)
                        this.CreateOrderProduct(order, id);
                }
            }
        }

        public OrderEntity GetOrderFormJson(string keyValue)
        {
            return service.IQueryable().Where(o => o.F_Id == keyValue).SingleOrDefault();
        }

        public void SubmitFormOP(OrderProductEditModel model)
        {
            var orderProduct = new OrderProductEntity();
            orderProduct.F_Id = model.F_Id;
            orderProduct.P_OrderId = model.P_OrderId;
            orderProduct.P_ProductId = model.P_ProductId;
            orderProduct.P_OrderPrice = model.P_OrderPrice;
            orderProduct.P_OrderUnit = model.P_OrderUnit;
            orderProduct.P_OrdreNumber = model.P_OrdreNumber;
            orderProduct.P_TotalAmount = model.P_OrderPrice * model.P_OrdreNumber;
            orderProductService.SubmitForm(orderProduct, model.F_Id);
        }

        public void SubmitFormO(OrderEntity model)
        {
            var order = new OrderEntity();
            model.F_Id = model.F_Id;
            order.P_DeliveryDate = model.P_DeliveryDate;
            order.P_IsOutsourcing = model.P_IsOutsourcing;
            service.SubmitForm(order, model.F_Id);
        }

        public OrderProductEditModel GetOderProductFormJson(string keyValue)
        {
            OrderProductEditModel opNodel = new OrderProductEditModel();
            var orderProductInfo = orderProductService.IQueryable().Where(o => o.F_Id == keyValue).SingleOrDefault();
            var orderInfo = service.IQueryable().Where(o => o.F_Id == orderProductInfo.P_OrderId).SingleOrDefault();
            var productInfo = productService.IQueryable().Where(o => o.F_Id == orderProductInfo.P_ProductId).SingleOrDefault();
            opNodel.F_Id = orderProductInfo.F_Id;
            opNodel.P_OrderId = orderProductInfo.P_OrderId;
            opNodel.P_OrderCode = orderInfo.P_OrderCode;
            opNodel.P_CustomerId = orderInfo.P_CustomerId;
            opNodel.P_ProductId = orderProductInfo.P_ProductId;
            opNodel.P_DrawingNoOrModelNo = productInfo.P_DrawingNoOrModelNo;
            opNodel.P_OrdreNumber = orderProductInfo.P_OrdreNumber;
            opNodel.P_OrderPrice = orderProductInfo.P_OrderPrice;
            opNodel.P_OrderUnit = orderProductInfo.P_OrderUnit;
            opNodel.P_TotalAmount = orderProductInfo.P_TotalAmount;
            opNodel.F_CreatorTime = orderProductInfo.F_CreatorTime;
            opNodel.F_CreatorUserId = orderProductInfo.F_CreatorUserId;
            return opNodel;
        }

        public List<OrderProductModel> GetTreeList()
        {
            var orderAndProductList = orderProductService.IQueryable().Where(t => t.F_CreatorUserId != "").ToList();
            var orderIds = orderAndProductList.Select(x => x.P_OrderId).ToList();
            var orderList = service.IQueryable().Where(t => t.F_DeleteMark != true && orderIds.Contains(t.F_Id)).OrderByDescending(t => t.F_CreatorTime).ToList();
            var productIds = orderAndProductList.Select(p => p.P_ProductId).ToList();
            var productList = productService.IQueryable().Where(t => productIds.Contains(t.F_Id)).ToList();
            var orderInspectionIds = orderAndProductList.Select(x => x.F_Id).ToList();
            var orderInspectionList = orderInspectionServer.IQueryable().Where(x => orderInspectionIds.Contains(x.F_OPId)).ToList();
            //r orderFlow_last = this.GetLastFlowInfo();
            var resultOrderList = new List<OrderProductModel>();
            foreach (var order in orderList)
            {
                var orderPaymentWarning = this.GetOrderPaymentWarning(order);
                OrderProductModel model = this.SetModel(order);
                var orderAndProducts = orderAndProductList.Where(x => x.P_OrderId == order.F_Id).ToList();
                var dic = new Dictionary<string, List<object>>();
                foreach (var item in orderAndProducts)
                {
                    var product = productList.Where(p => p.F_Id == item.P_ProductId).SingleOrDefault();
                    var orderInspection = orderInspectionList.Where(x => x.F_OPId == item.F_Id && x.P_FlowId == order.P_OrderStatus).SingleOrDefault();
                    if (orderInspection == null)
                        orderInspection = new OrderInspectionEntity();
                    var productInfo = new List<Object>();
                    var orderInspectionInfo = new List<Object>();
                    var orderProductInfo = new List<Object>();
                    dic.TryGetValue("product", out productInfo);
                    if (productInfo == null)
                    {
                        productInfo = new List<Object>();
                        productInfo.Add(product);
                        dic.Add("product", productInfo);
                    }
                    else
                    {
                        productInfo.Add(product);
                    }
                    dic.TryGetValue("orderInspection", out orderInspectionInfo);
                    if (orderInspectionInfo == null)
                    {
                        orderInspectionInfo = new List<Object>();
                        orderInspectionInfo.Add(orderInspection);
                        dic.Add("orderInspection", orderInspectionInfo);
                    }
                    else
                    {
                        orderInspectionInfo.Add(orderInspection);
                    }
                    dic.TryGetValue("orderProduct", out orderProductInfo);
                    if (orderProductInfo == null)
                    {
                        orderProductInfo = new List<Object>();
                        orderProductInfo.Add(item);
                        dic.Add("orderProduct", orderProductInfo);
                    }
                    else
                    {
                        orderProductInfo.Add(item);
                    }
                }
                model.OrderProductList = dic;
                resultOrderList.Add(model);
            }
            
            return resultOrderList;
        }

        private Object GetOrderPaymentWarning(OrderEntity order)
        {
            var data = new
            {
                id = "",
                pricing = 0
            };
            return data;
        }

        public void DeleteOP(string keyValue, string orderproductid)
        {
            orderProductService.DeleteForm(orderproductid);
        }

        private OrderProductModel SetModel(OrderEntity order)
        {
            var model = new OrderProductModel();
            model.F_Id = order.F_Id;
            model.P_CustomerId = order.P_CustomerId;
            model.P_OrderCode = order.P_OrderCode;
            model.P_OrderDate = order.P_OrderDate;
            model.P_DeliveryDate = order.P_DeliveryDate;
            model.P_OrderOwner = order.P_OrderOwner;
            model.P_OrderStatus = order.P_OrderStatus;
            model.F_CreatorUserId = order.F_CreatorUserId;
            model.F_CreatorTime = order.F_CreatorTime;
            model.F_DeleteMark = order.F_DeleteMark;
            model.F_DeleteUserId = order.F_DeleteUserId;
            model.F_DeleteTime = order.F_DeleteTime;
            model.F_LastModifyUserId = order.F_LastModifyUserId;
            model.F_LastModifyTime = order.F_LastModifyTime;
            model.P_IsOutsourcing = order.P_IsOutsourcing;
            model.OrderProductList = new Dictionary<string, List<object>>();
            return model;
        }

        public OrderEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            service.DeleteForm(keyValue);
        }
        public void SubmitForm(OrderEntity orderEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                orderEntity.Modify(keyValue);
                //var orderProductInfo = orderProductservice.IQueryable().Where(o => o.P_OrderId == orderEntity.F_Id && o.P_ProductId == orderEntity.P_ProductId).ToList();
                //if (orderProductInfo.Count < 1)
                //    this.CreateOrderProduct(orderEntity, "");
            }
            else
            {
                orderEntity.Create();
                orderEntity.P_OrderDate = orderEntity.F_CreatorTime;
                //orderEntity.P_OrderStatus = "1";
                //this.CreateOrderProduct(orderEntity, "");
            }
            orderEntity.P_TotalAmount = orderEntity.P_OrderPrice * orderEntity.P_OrdreNumber;
            var result = service.SubmitForm(orderEntity, keyValue);
            if (result > 0 && string.IsNullOrEmpty(keyValue))
            {
                this.SubmitFormP(orderEntity);
                service.CreateFlow(orderEntity);
            }
        }
        public void SubmitFormP(OrderEntity orderEntity)
        {
            var orderProduct = new OrderProductEntity();
            orderProduct.P_OrderId = orderEntity.F_Id;
            orderProduct.P_ProductId = orderEntity.P_ProductId;
            orderProduct.P_OrderPrice = orderEntity.P_OrderPrice;
            orderProduct.P_OrderUnit = orderEntity.P_OrderUnit;
            orderProduct.P_OrdreNumber = orderEntity.P_OrdreNumber;
            orderProduct.P_TotalAmount = orderEntity.P_OrderPrice * orderEntity.P_OrdreNumber;
            orderProduct.Create();
            orderProductService.SubmitForm(orderProduct, "");
        }

        private void CreateOrderProduct(OrderEntity orderEntity, string keyValue)
        {
            var orderProduct = new OrderProductEntity();
            orderProduct.P_OrderId = keyValue;
            orderProduct.P_ProductId = orderEntity.P_ProductId;
            orderProduct.P_OrdreNumber = orderEntity.P_OrdreNumber;
            orderProduct.P_OrderPrice = orderEntity.P_OrderPrice;
            orderProduct.P_OrderUnit = orderEntity.P_OrderUnit;
            orderProduct.P_TotalAmount = orderEntity.P_OrdreNumber * orderEntity.P_OrderPrice;
            orderProduct.Create();
            orderProductService.SubmitForm(orderProduct, "");

            var orderInspection = orderInspectionServer.IQueryable().Where(o => o.P_OrderId == keyValue && o.P_ProductId == orderEntity.P_ProductId).ToList();
            if (orderInspection.Count > 0)
            {
                foreach (var item in orderInspection)
                {
                    item.F_OPId = orderProduct.F_Id;
                    item.Modify(item.F_Id);
                    orderInspectionServer.SubmitForm(item, item.F_Id);
                }
            }
        }

        public void UpdateForm(OrderEntity orderEntity)
        {
            service.Update(orderEntity);
        }

        ItemsDetailEntity lsstFlowInfo = null;
        List<OrderFlowEntity> flowInfo = null;

        /// <summary>
        /// 按照交付日期过滤订单
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public Object GetAllOrderList(string beginTime, string endTime)
        {
            if (beginTime == "" && endTime == "")
            {
                DateTime now = DateTime.Now;
                beginTime = now.Year + "-01-01";
                endTime = now.Year + "-12-31";
            }
            DateTime begin = Convert.ToDateTime(beginTime);
            DateTime end = Convert.ToDateTime(endTime);
            List<int> allOrders = new List<int>();
            List<int> outboundOrders = new List<int>();
            var labels = GetChartLabel(begin, end);
            for (int i = 0; i < labels.Count; i++)
            {
                allOrders.Add(0);
                outboundOrders.Add(0);
            }
            List<OrderProductEntity> orderProducts = new List<OrderProductEntity>();
            var orders = GetOrders(begin, end, out orderProducts);
            lsstFlowInfo = lsstFlowInfo == null ? this.GetLastFlowInfo() : lsstFlowInfo;
            flowInfo = flowInfo == null ? this.GetFlowInfo(lsstFlowInfo) : flowInfo;
            foreach (var order in orders)
            {
                var deliveryDate =  Convert.ToDateTime(order.P_DeliveryDate);
                for (int labelIndex = 0; labelIndex < labels.Count; labelIndex++)
                {
                    var monthNum = labels[labelIndex].Replace("月", "");
                    if (deliveryDate.Month == Convert.ToInt32(monthNum))
                    {
                        allOrders[labelIndex] += 1;
                        if ( flowInfo.Where(o => o.P_OrderId == order.F_Id).ToList().Count > 0)
                        {
                            outboundOrders[labelIndex] += 1;
                        }
                    }
                }
            }
            var resultdata = new
            {
                orderCount = orders.Count,
                outboundOrder = this.GetOutboundOrderList(begin, end).Count,
                labels = labels,
                allOrders = allOrders,
                outboundOrders = outboundOrders
            };
            return resultdata;
        }

        public bool CheckOrderIsHold(string orderId)
        {
            var userInfo = OperatorProvider.Provider.GetCurrent();
            var orderInfo = service.IQueryable().Where(o => o.F_Id == orderId).SingleOrDefault();
            return userInfo.UserId == orderInfo.F_CreatorUserId;
        }

        public object GetAllOrderDataByDate(string beginTime, string endTime)
        {
            if (beginTime == "" && endTime == "")
            {
                DateTime now = DateTime.Now;
                beginTime = now.Year + "-01-01";
                endTime = now.Year + "-12-31";
            }
            DateTime begin = Convert.ToDateTime(beginTime);
            DateTime end = Convert.ToDateTime(endTime);
            List<int> allOrders = new List<int>();
            List<int> outboundOrders = new List<int>();
            List<decimal> ordersAmount = new List<decimal>();
            List<decimal> orderReceivable = new List<decimal>();
            List<decimal> orderArrears = new List<decimal>();
            var labels = GetChartLabel(begin, end);
            for (int i = 0; i < labels.Count; i++)
            {
                allOrders.Add(0);
                outboundOrders.Add(0);
                ordersAmount.Add(0);
                orderReceivable.Add(0);
                orderArrears.Add(0);
            }
            List<OrderProductEntity> orderProducts = new List<OrderProductEntity>();
            var orders = GetOrders(begin, end, out orderProducts);
            lsstFlowInfo = lsstFlowInfo == null ? this.GetLastFlowInfo() : lsstFlowInfo;
            flowInfo = flowInfo == null ? this.GetFlowInfo(lsstFlowInfo) : flowInfo;
            List<OrderpaymentEntity> orderPayInfo = this.GetPaymentInfo(orders);
            foreach (var order in orders)
            {
                var orderProduct = orderProducts.Where(x => x.P_OrderId == order.F_Id).ToList();
                var deliveryDate = Convert.ToDateTime(order.P_DeliveryDate);
                for (int labelIndex = 0; labelIndex < labels.Count; labelIndex++)
                {
                    var monthNum = labels[labelIndex].Replace("月", "");
                    if (deliveryDate.Month == Convert.ToInt32(monthNum))
                    {
                        allOrders[labelIndex] += 1;
                        var amount = orderProduct.Sum( x => x.P_TotalAmount);
                        decimal amount_sig = 0;
                        ordersAmount[labelIndex] += amount;
                        if (flowInfo.Where(o => o.P_OrderId == order.F_Id).ToList().Count > 0)
                        {
                            amount_sig = orderPayInfo.Where(x => x.P_OrderId == order.F_Id).Sum(m => m.P_Amount);
                            outboundOrders[labelIndex] += 1;
                        }
                        orderReceivable[labelIndex] += amount_sig;
                        var diff = amount - amount_sig;
                        orderArrears[labelIndex] += diff;
                    }
                }
            }
            //var resultdata = new Dictionary<string, List<Object>>();
            var dataList = new List<Object>();
            var orderTotal = new {
                        name = "订单总数",
                        data = allOrders
            };
            dataList.Add(orderTotal);
            var orderOut = new
            {
                name = "交付订单",
                data = outboundOrders
            };
            dataList.Add(orderOut);
            var orderAmounts = new
            {
                name = "订单金额",
                data = ordersAmount
            };
            dataList.Add(orderAmounts);
            var receivable = new
            {
                name = "订单回款",
                data = orderReceivable
            };
            dataList.Add(receivable);
            var arrears = new
            {
                name = "订单欠款",
                data = orderArrears
            };
            dataList.Add(arrears);
            return new
            {
                labels = labels,
                data = dataList
            };
        }

        public object GetAllOrderDataByCustomerTotal(string beginTime, string endTime)
        {
            if (beginTime == "" && endTime == "")
            {
                DateTime now = DateTime.Now;
                beginTime = now.Year + "-01-01";
                endTime = now.Year + "-12-31";
            }
            DateTime begin = Convert.ToDateTime(beginTime);
            DateTime end = Convert.ToDateTime(endTime);
            var labels = this.GetChartLabel(begin, end);
            List<decimal> ordersAmount = this.GetDisplayAmount(labels);
            List<OrderProductEntity> orderProducts = new List<OrderProductEntity>();
            var orders = GetOrders(begin, end, out orderProducts);
            lsstFlowInfo = lsstFlowInfo == null ? this.GetLastFlowInfo() : lsstFlowInfo;
            flowInfo = flowInfo == null ? this.GetFlowInfo(lsstFlowInfo) : flowInfo;
            List<CustomerEntity> orderCustomerInfo = this.GetCustomerInfo(orders);
            Dictionary<string, List<decimal>> customerDic = new Dictionary<string, List<decimal>>();
            foreach (var customer in orderCustomerInfo)
            {
                var customerOrders = orders.Where(x => x.P_CustomerId == customer.F_Id).ToList();
                ordersAmount = this.GetDisplayAmount(labels);
                if (customerDic.Keys.Contains(customer.F_Id))
                {
                    customerDic.TryGetValue(customer.F_Id, out ordersAmount);
                    foreach (var order in customerOrders)
                    {
                        var orderProduct = orderProducts.Where(x => x.P_OrderId == order.F_Id).ToList();
                        var createDate = Convert.ToDateTime(order.F_CreatorTime);
                        for (int labelIndex = 0; labelIndex < labels.Count; labelIndex++)
                        {
                            var monthNum = labels[labelIndex].Replace("月", "");
                            if (createDate.Month == Convert.ToInt32(monthNum))
                            {
                                var amount = orderProduct.Sum(x => x.P_TotalAmount);
                                ordersAmount[labelIndex] += amount;
                            }
                        }
                    }
                }
                else
                {
                    foreach (var order in customerOrders)
                    {
                        var orderProduct = orderProducts.Where(x => x.P_OrderId == order.F_Id).ToList();
                        var createDate = Convert.ToDateTime(order.F_CreatorTime);
                        for (int labelIndex = 0; labelIndex < labels.Count; labelIndex++)
                        {
                            var monthNum = labels[labelIndex].Replace("月", "");
                            if (createDate.Month == Convert.ToInt32(monthNum))
                            {
                                var amount = orderProduct.Sum(x => x.P_TotalAmount);
                                ordersAmount[labelIndex] += amount;
                            }
                        }
                    }
                    customerDic.Add(customer.F_Id, ordersAmount);
                }

            }
            var dataList = new List<Object>();
            foreach (var customer in customerDic.Keys)
            {
                var customerInfo = orderCustomerInfo.Where(c => c.F_Id == customer).SingleOrDefault();
                var orderTotal = new
                {
                    name = customerInfo.P_CustomerName,
                    data = customerDic[customer]
                };
                dataList.Add(orderTotal);
            }
            return new
            {
                labels = labels,
                data = dataList
            };
        }

        public object GetAllOrderDataByCustomer(string beginTime, string endTime)
        {
            if (beginTime == "" && endTime == "")
            {
                DateTime now = DateTime.Now;
                beginTime = now.Year + "-01-01";
                endTime = now.Year + "-12-31";
            }
            DateTime begin = Convert.ToDateTime(beginTime);
            DateTime end = Convert.ToDateTime(endTime);
            List<OrderProductEntity> orderProducts = new List<OrderProductEntity>();
            var orders = GetOrders(begin, end, out orderProducts);
            lsstFlowInfo = lsstFlowInfo == null ? this.GetLastFlowInfo() : lsstFlowInfo;
            flowInfo = flowInfo == null ? this.GetFlowInfo(lsstFlowInfo) : flowInfo;
            List<CustomerEntity> orderCustomerInfo = this.GetCustomerInfo(orders);
            var labels = this.GetChartLabelByCus(orderCustomerInfo);
            var newLabels = this.ConverLabel(labels, orderCustomerInfo);
            Dictionary<string, List<decimal>> customerDic = new Dictionary<string, List<decimal>>();
            List<int> allOrders = new List<int>();
            List<int> outboundOrders = new List<int>();
            List<decimal> ordersAmount = new List<decimal>();
            List<decimal> orderReceivable = new List<decimal>();
            List<decimal> orderArrears = new List<decimal>();
            for (int i = 0; i < labels.Count; i++)
            {
                allOrders.Add(0);
                outboundOrders.Add(0);
                ordersAmount.Add(0);
                orderReceivable.Add(0);
                orderArrears.Add(0);
            }
            List<OrderpaymentEntity> orderPayInfo = this.GetPaymentInfo(orders);
            foreach (var order in orders)
            {
                var orderProduct = orderProducts.Where(x => x.P_OrderId == order.F_Id).ToList();
                var deliveryDate = Convert.ToDateTime(order.P_DeliveryDate);
                for (int labelIndex = 0; labelIndex < labels.Count; labelIndex++)
                {
                    if (labels[labelIndex] == order.P_CustomerId)
                    {
                        allOrders[labelIndex] += 1;
                        var amount = orderProduct.Sum(x => x.P_TotalAmount);
                        ordersAmount[labelIndex] += amount;
                        decimal amount_sig = 0;
                        if (flowInfo.Where(o => o.P_OrderId == order.F_Id).ToList().Count > 0)
                        {
                            amount_sig = orderPayInfo.Where(x => x.P_OrderId == order.F_Id).Sum(m => m.P_Amount);
                            outboundOrders[labelIndex] += 1;
                        }
                        orderReceivable[labelIndex] += amount_sig;
                        var diff = amount - amount_sig;
                        orderArrears[labelIndex] += diff;
                    }
                }
            }
            var dataList = new List<Object>();
            var orderTotal = new
            {
                name = "订单总数",
                data = allOrders
            };
            dataList.Add(orderTotal);
            var orderOut = new
            {
                name = "交付订单",
                data = outboundOrders
            };
            dataList.Add(orderOut);
            var orderAmounts = new
            {
                name = "订单金额",
                data = ordersAmount
            };
            dataList.Add(orderAmounts);
            var receivable = new
            {
                name = "订单回款",
                data = orderReceivable
            };
            dataList.Add(receivable);
            var arrears = new
            {
                name = "订单欠款",
                data = orderArrears
            };
            dataList.Add(arrears);
            return new
            {
                labels = newLabels,
                data = dataList
            };
        }

        private List<string> ConverLabel(List<string> labels, List<CustomerEntity> customers)
        {
            List<string> newLabels = new List<string>();
            for (int i = 0; i < labels.Count; i++)
            {
                newLabels.Add(customers.Where(c => c.F_Id == labels[i]).SingleOrDefault().P_CustomerName);
            }
            return newLabels;
        }

        private List<string> GetChartLabelByCus(List<CustomerEntity> orderCustomerInfo)
        {
            List<string> labels = new List<string>();
            foreach (var cus in orderCustomerInfo)
            {
                labels.Add(cus.F_Id);
            }
            return labels;
        }

        private List<decimal> GetDisplayAmount(List<string> labels)
        {
            List<decimal> ordersAmount = new List<decimal>();
            for (int i = 0; i < labels.Count; i++)
            {
                ordersAmount.Add(0);
            }
            return ordersAmount;
        }

        public List<CustomerEntity> GetCustomerInfo(List<OrderEntity> orders)
        {
            var customerIds = orders.Select(o => o.P_CustomerId).ToList();
            return customerService.IQueryable().Where(t => t.F_DeleteMark != true && customerIds.Contains(t.F_Id)).OrderBy(t => t.F_CreatorTime).ToList();
        }

        public List<OrderpaymentEntity> GetPaymentInfo(List<OrderEntity> orders)
        {
            var orderIds = orders.Select(o => o.F_Id).ToList();
            return orderPaymentService.IQueryable().Where(t => t.F_DeleteMark != true && orderIds.Contains(t.P_OrderId)).OrderBy(t => t.F_CreatorTime).ToList();
        }

        public List<OrderEntity> GetOrders(DateTime begin, DateTime end, out List<OrderProductEntity> orderProducts)
        {
            orderProducts = orderProductService.IQueryable().Where(t => t.F_CreatorTime != null).ToList();
            var orderIds = orderProducts.Select(x => x.P_OrderId).ToList();
            return service.IQueryable().Where(t => t.F_DeleteMark != true && t.P_DeliveryDate <= end && t.P_DeliveryDate >= begin && orderIds.Contains(t.F_Id)).OrderBy(t => t.F_CreatorTime).ToList();
        }
        public List<OrderEntity> GetOutboundOrderList(DateTime begin, DateTime end)
        {
            lsstFlowInfo = lsstFlowInfo == null ? this.GetLastFlowInfo() : lsstFlowInfo;
            flowInfo = flowInfo == null ? this.GetFlowInfo(lsstFlowInfo) : flowInfo;
            var orderIds = flowInfo.Select(t => t.P_OrderId).ToList();
            return service.IQueryable().Where(t => t.F_DeleteMark != true && orderIds.Contains(t.F_Id) && t.P_OrderStatus == lsstFlowInfo.F_Id && t.P_DeliveryDate <= end && t.P_DeliveryDate >= begin).OrderBy(t => t.F_CreatorTime).ToList();
        }

        public List<OrderFlowEntity> GetFlowInfo(ItemsDetailEntity lsstFlowInfo)
        {
            return flowService.IQueryable().Where(t => t.P_FlowId == lsstFlowInfo.F_Id && t.P_FlowStatus == "1").ToList();
        }

        public ItemsDetailEntity GetLastFlowInfo()
        {
            return itemsDetailService.IQueryable().Where(t => t.F_ItemCode == "OutboundOrder").SingleOrDefault();
        }

        private ItemsEntity GetItemsInfo()
        {
            return itemsService.IQueryable().Where(t => t.F_EnCode == "OrderFlow").SingleOrDefault();
        }

        private List<string> GetChartLabel(DateTime begin, DateTime end)
        {
            List<string> labels = new List<string>();
            for (DateTime dt = begin; dt <= end; dt = dt.AddMonths(1))
            {
                labels.Add(dt.Month + "月");
            }
            return labels;
        }
    }
}
