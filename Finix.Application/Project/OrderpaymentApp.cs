using Finix.Code;
using Finix.Domain.Entity.Project;
using Finix.Domain.IRepository.Project;
using Finix.Repository.Project;
using System.Collections.Generic;
using System.Linq;
using Finix.Domain.IRepository.SystemManage;
using Finix.Repository.SystemManage;
using Finix.Domain.Entity.SystemManage;
using System;

namespace Finix.Application.Project
{
    public class OrderpaymentApp
    {
        private IOrderpaymentRepository service = new OrderpaymentRepository();
        private IOrderRepository orderService = new OrderRepository();
        private IProductRepository productService = new ProductRepository();
        private ICustomerRepository customerService = new CustomerRepository();
        private IItemsDetailRepository itemsDetailService = new ItemsDetailRepository();
        private IOrderProductRepository orderProductService = new OrderProductRepository();

        public List<OrderPaymentModel> GetList(Pagination pagination, string keyword)
        {
            var orderpayments = service.IQueryable().Where(t => t.F_DeleteMark != true).OrderBy(t => t.P_OrderId).ToList();
            var orderIds = orderpayments.Select(x => x.P_OrderId).ToList();
            var orders = GetOrders();
            var opList = orderProductService.IQueryable().Where(x => x.F_CreatorTime != null).ToList();
            List<OrderPaymentModel> paymentsList = new List<OrderPaymentModel>();
            foreach (var item in orderpayments)
            {
                var payment = new OrderpaymentEntity();
                var checkOrderInfo = opList.Where(x => x.P_OrderId == item.P_OrderId).ToList();
                if (checkOrderInfo == null && checkOrderInfo.Count < 1)
                {
                    payment = GetOrderPaymentInfo(item);
                    UpdateForm(payment);
                }
                else
                {
                    payment = item;
                }
                var orderInfo = new OrderEntity();
                if (!string.IsNullOrEmpty(keyword))
                {
                    orderInfo = orders.Where(o => o.F_Id == payment.P_OrderId && o.P_OrderCode.Contains(keyword)).SingleOrDefault();
                }
                else
                {
                    orderInfo = orders.Where(o => o.F_Id == payment.P_OrderId).SingleOrDefault();
                }
                if (orderInfo != null && orderInfo.P_OrdreNumber > 0)
                {
                    var productInfo = productService.IQueryable().Where(t => t.F_Id == orderInfo.P_ProductId).SingleOrDefault();
                    var customerInfo = customerService.IQueryable().Where(t => t.F_Id == orderInfo.P_CustomerId).SingleOrDefault();
                    paymentsList.Add(new OrderPaymentModel()
                    {
                        F_Id = payment.F_Id,
                        P_OrderId = orderInfo.F_Id,
                        P_OrderCode = orderInfo.P_OrderCode,
                        P_OrdreNumber = orderInfo.P_OrdreNumber,
                        P_OrderPrice = orderInfo.P_OrderPrice,
                        P_OrderAmount = orderInfo.P_OrdreNumber * orderInfo.P_OrderPrice,
                        P_OrderDate = orderInfo.P_OrderDate,
                        P_OrderOwner = orderInfo.F_CreatorUserId,
                        P_OrderStatus = orderInfo.P_OrderStatus,
                        P_CustomerId = customerInfo.P_CustomerCode,
                        P_Amount = payment.P_Amount,
                        P_ProductId = productInfo.F_Id,
                        F_CreatorTime = payment.F_CreatorTime,
                        F_CreatorUserId = payment.F_CreatorUserId,
                        F_DeleteMark = payment.F_DeleteMark,
                        F_Description = payment.F_Description,
                        P_DeliveryDate = orderInfo.P_DeliveryDate
                    });
                }
            }


            //return service.FindList(expression, pagination);
            return paymentsList;
        }

        public ItemsDetailEntity GetOrderStatusByOrder(OrderEntity item)
        {
            return itemsDetailService.IQueryable().Where(x => x.F_Id == item.P_OrderStatus).SingleOrDefault();
        }

        public ProductEntity GetProductByOrder(OrderEntity item)
        {
            return productService.IQueryable().Where(x => x.F_Id == item.P_ProductId).SingleOrDefault();
        }

        public CustomerEntity GetCustomerByOrder(OrderEntity item)
        {
            return customerService.IQueryable().Where(x => x.F_Id == item.P_CustomerId).SingleOrDefault();
        }

        public List<OrderEntity> GetPaymentOrders()
        {
            var orderpayments = service.IQueryable().Where(t => t.F_DeleteMark != true).OrderBy(t => t.P_OrderId).ToList();
            var orderIds = orderpayments.Select(x => x.F_Id).ToList();
            var orderInfo = GetOrders();
            List<OrderEntity> orderList = new List<OrderEntity>();
            foreach (var order in orderInfo)
            {
                if (!orderIds.Contains(order.F_Id))
                {
                    orderList.Add(order);
                }
                else
                {
                    var paymentOrder = orderpayments.Where(o => o.F_Id == order.F_Id).SingleOrDefault();
                    var totalAmount = order.P_OrdreNumber * order.P_OrderPrice;
                    if (paymentOrder.P_Amount < totalAmount)
                    {
                        orderList.Add(order);
                    }
                }
            }
            return orderList;
        }

        public List<OrderEntity> GetOrders()
        {
            return orderService.IQueryable().Where(t => t.F_DeleteMark != true).OrderBy(t => t.F_CreatorTime).ToList();
        }

        public OrderpaymentEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void SubmitForm(OrderpaymentEntity orderpaymentEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                orderpaymentEntity.Modify(keyValue);
            }
            else
            {
                orderpaymentEntity = GetOrderPaymentInfo(orderpaymentEntity);
                orderpaymentEntity.Create();
            }
            service.SubmitForm(orderpaymentEntity, keyValue);
        }

        private OrderpaymentEntity GetOrderPaymentInfo(OrderpaymentEntity orderpaymentEntity)
        {
            var orderList = orderService.IQueryable().Where(x => x.F_CreatorTime != null).ToList();
            var orderInfo = orderList.Where(x => x.F_Id == orderpaymentEntity.P_OrderId).SingleOrDefault();
            var orderIds_Code = orderList.Where(x => x.P_OrderCode == orderInfo.P_OrderCode).Select(x=>x.F_Id).ToList();
            var orderProduct = orderProductService.IQueryable().Where(x=> orderIds_Code.Contains(x.P_OrderId)).SingleOrDefault();
            orderpaymentEntity.P_OrderId = orderProduct.P_OrderId;
            return orderpaymentEntity;
        }

        public void DeleteForm(string keyValue)
        {
            service.DeleteForm(keyValue);
        }
        public void UpdateForm(OrderpaymentEntity orderEntity)
        {
            service.Update(orderEntity);
        }
    }
}
