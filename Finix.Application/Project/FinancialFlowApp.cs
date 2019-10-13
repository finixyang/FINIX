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
    public class FinancialFlowApp
    {
        private IFinancialFlowRepository service = new FinancialFlowRepository();
        private IFinancialRepository FinancialService = new FinancialRepository();
        private IItemsDetailRepository itemsDetailService = new ItemsDetailRepository();
        private IItemsRepository itemsService = new ItemsRepository();
        private IUserRepository userService = new UserRepository();

        public Object GetFlowList(string keyword)
        {
            var userInfo = OperatorProvider.Provider.GetCurrent();
            //UserCode == "admin"
            var financialFlows = this.GetList(userInfo.UserCode == "admin" ? "" : userInfo.UserId, keyword);
            var financials = this.GetFinancialList(financialFlows.Select(x => x.P_FinancialId).ToList());
            var flowsItems = this.GetItemsDetailList(financialFlows.Select(x => x.P_FlowId).ToList());

            var arr = new List<Object>();
            foreach (var flow in financialFlows)
            {
                var financialList = financials.Where(t => t.F_Id == flow.P_FinancialId).ToList();
                if (financialList.Count > 0)
                {
                    foreach (var financial in financialList)
                    {
                        var flowsItem = flowsItems.Where(t => t.F_Id == flow.P_FlowId).ToList();
                        arr.Add(new
                        {
                            F_Id = flow.F_Id,
                            P_FinancialId = financial.F_Id,
                            P_Description = financial.P_Description,
                            P_TotalAmount = financial.P_TotalAmount,
                            P_FinancialType = financial.P_FinancialType,
                            F_CreatorUserId = flow.F_CreatorUserId,
                            F_CreatorTime = flow.F_CreatorTime,
                            P_FlowStatus = flowsItem[0].F_Id,
                            P_FlowItemName = flowsItem[0].F_ItemName,
                            P_SortCode = flowsItem[0].F_SortCode,
                            P_Executor = financial.F_LastModifyUserId,
                            P_FlowId = flow.P_FlowId,
                            P_FinancialStatus = flowsItem[0].F_Id,
                        });
                    }

                }
            }
            return arr;
        }

        public object GetFlowInfos(string keyValue)
        {
            var itemsDetail = itemsService.IQueryable().Where(t => t.F_EnCode == "FinancialFlow").ToList();
            var financialFlowIds = itemsDetail.Select(x => x.F_Id).ToList();
            var flowsItems = itemsDetailService.IQueryable().Where(t => financialFlowIds.Contains(t.F_ItemId)).OrderBy(t => t.F_SortCode).ToList();
            var financialFlows = service.IQueryable().Where(t => t.P_FinancialId == keyValue).OrderBy(t => t.F_CreatorTime).ToList();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            foreach (var item in flowsItems)
            {
                var financialInfo = financialFlows.Where(x => x.P_FlowId == item.F_Id).ToList();
                FinancialFlowEntity financial = new FinancialFlowEntity();
                if (financialInfo.Count > 0)
                {
                    financial = financialInfo[0];
                    dic.Add(item.F_SortCode.ToString(), new { id = item.F_Id, name = item.F_ItemName, status = financial.P_FlowStatus });
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
            var FinancialFlowInfo = itemsService.IQueryable().Where(t => t.F_EnCode == "FinancialFlow").ToList();
            var FinancialFlowIds = FinancialFlowInfo.Select(x => x.F_Id).ToList();
            var flowsItems = itemsDetailService.IQueryable().Where(t => FinancialFlowIds.Contains(t.F_ItemId)).OrderBy(t => t.F_CreatorTime).ToList();
            var nowFlow = flowsItems.Where(t => t.F_Id == executorId_now).ToList()[0];
            if (nowFlow.F_ItemCode == "FinancialPayOut")
                return nowFlow;
            //如果是外协订单需要特殊处理流程
            return flowsItems.Where(t => t.F_SortCode == (nowFlow.F_SortCode + 1)).ToList()[0];
        }

        public List<UserEntity> GetNextExecutor(string executorId_now)
        {
            var nextFlow = this.GetNextStatus(executorId_now);
            return userService.IQueryable().Where(t => t.F_DutyId == nextFlow.F_Executor && t.F_EnabledMark == true).OrderBy(t => t.F_CreatorTime).ToList();
        }

        public List<FinancialFlowEntity> GetList(string userId, string keyword)
        {
            if (userId == "")
                return service.IQueryable().Where(t => t.P_FlowId != null).OrderBy(t => t.F_CreatorTime).ToList();
            else
            {
                if (keyword == "1")
                    return service.IQueryable().Where(t => t.P_Executor == userId && t.P_FlowStatus == "1").OrderBy(t => t.F_CreatorTime).ToList();
                else
                    return service.IQueryable().Where(t => t.P_Executor == userId && t.P_FlowStatus == "0").OrderBy(t => t.F_LastModifyTime).ToList();
            }

        }

        public List<FinancialEntity> GetFinancialList(List<string> FinancialIds)
        {
            return FinancialService.IQueryable().Where(t => FinancialIds.Contains(t.F_Id)).OrderBy(t => t.F_CreatorTime).ToList();
        }

        public List<ItemsDetailEntity> GetItemsDetailList(List<string> itemsDetailIds)
        {
            return itemsDetailService.IQueryable().Where(t => itemsDetailIds.Contains(t.F_Id)).OrderBy(t => t.F_CreatorTime).ToList();
        }

        public ItemsDetailEntity CheckFinancialFlow(string flowId)
        {
            var itemsDetail = GetItemsDetailList(new List<string>()
            {
                flowId
            });
            return itemsDetail.Count > 0 ? itemsDetail[0] : null;
        }

        public FinancialFlowEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            service.DeleteForm(keyValue);
        }
        public void SubmitForm(FinancialFlowEntity financialFlowEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                financialFlowEntity.Modify(keyValue);
            }
            else
            {
                financialFlowEntity.Create();
            }
            service.SubmitForm(financialFlowEntity, keyValue);
        }

        public void SubmitForm(FinancialFlowModel financialFlowEntity_last, string keyValue, string flowId, string financialFlowId)
        {
            //1.拆分订单流程信息
            //a.订单信息 更新
            FinancialEntity financialEntity = new FinancialEntity();
            financialEntity.F_Id = keyValue;
            financialEntity.P_Description = financialFlowEntity_last.P_Description;
            financialEntity.P_TotalAmount = financialFlowEntity_last.P_TotalAmount;
            financialEntity.P_FinancialType = financialFlowEntity_last.P_FinancialType;
            financialEntity.Modify(keyValue);
            var financialResult = FinancialService.SubmitForm(financialEntity, financialEntity.F_Id);
            if (financialResult > 0)
            {
                //b.流程信息 更新原有流程 
                FinancialFlowEntity financialFlowEntity = new FinancialFlowEntity();
                financialFlowEntity.P_FlowStatus = "1";
                financialFlowEntity.Modify(financialFlowId);
                var financialFlowResult = service.SubmitForm(financialFlowEntity, financialFlowId);
                if (financialFlowResult > 0)
                {
                    var itemsDetail = itemsDetailService.IQueryable().Where(t => t.F_Id == flowId).SingleOrDefault();

                    //新建下一流程
                    if (financialFlowEntity_last.P_FinancialStatus_next != flowId)
                    {
                        financialFlowEntity = new FinancialFlowEntity();
                        financialFlowEntity.P_FinancialId = keyValue;
                        financialFlowEntity.P_FlowId = financialFlowEntity_last.P_FinancialStatus_next;
                        financialFlowEntity.P_FlowStatus = "0";
                        financialFlowEntity.P_Executor = financialFlowEntity_last.F_LastModifyUserId;
                        financialFlowEntity.Create();
                        financialFlowEntity.F_LastModifyUserId = financialFlowEntity.F_CreatorUserId;
                        financialFlowEntity.F_LastModifyTime = financialFlowEntity.F_CreatorTime;
                        service.SubmitForm(financialFlowEntity, null);
                    }

                }

            }

        }
        public void UpdateForm(FinancialFlowEntity financialFlowEntity)
        {
            service.Update(financialFlowEntity);
        }

        public int GetFinancialTaskByUser()
        {
            var userInfo = OperatorProvider.Provider.GetCurrent();
            return service.IQueryable().Where(f => f.P_FlowStatus == "0" && f.P_Executor == userInfo.UserId).ToList().Count;
        }
    }
}
