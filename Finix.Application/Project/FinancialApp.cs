using Finix.Code;
using Finix.Domain.Entity.Project;
using System.Collections.Generic;
using Finix.Domain.IRepository.Project;
using Finix.Repository.Project;
using System;
using System.Linq;

namespace Finix.Application.Project
{
    public class FinancialApp
    {
        private IFinancialRepository service = new FinancialRepository();
        private IFinancialFlowRepository flowService = new FinancialFlowRepository();

        public List<FinancialEntity> GetList(Pagination pagination, string keyword)
        {
            var expression = ExtLinq.True<FinancialEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.P_Description.Contains(keyword));
            }
            expression = expression.And(t => t.F_DeleteMark != true);
            return service.FindList(expression, pagination);
        }
        public FinancialEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            service.DeleteForm(keyValue);
        }
        public void SubmitForm(FinancialEntity financialEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                financialEntity.Modify(keyValue);
            }
            else
            {
                financialEntity.Create();
            }
            var result = service.SubmitForm(financialEntity, keyValue);
            if (result > 0 && string.IsNullOrEmpty(keyValue))
            {
                service.CreateFlow(financialEntity);
            }
        }

        public void UpdateForm(FinancialEntity financialEntity)
        {
            service.Update(financialEntity);
        }

        public object FinancialSummary(string beginTime, string endTime)
        {
            if (beginTime == "" && endTime == "")
            {
                DateTime now = DateTime.Now;
                beginTime = now.Year + "-01-01";
                endTime = now.Year + "-12-31";
            }
            DateTime begin = Convert.ToDateTime(beginTime);
            DateTime end = Convert.ToDateTime(endTime);
            List<decimal> orderSummary = new List<decimal>();//订单回款
            List<decimal> fixedAssetsSummary = new List<decimal>();//固定资产采购
            List<decimal> fixedAssetsRecipient = new List<decimal>();//固定资产领用
            List<decimal> benefitsSummary = new List<decimal>();//福利采购
            List<decimal> consumableSummary = new List<decimal>();//消耗品采购
            List<decimal> consumableRecipient = new List<decimal>();//消耗品领用
            List<decimal> reimbursementIn = new List<decimal>();//报账入账
            List<decimal> reimbursementOut = new List<decimal>();//报账出账
            var labels = GetChartLabel(begin, end);
            for (int i = 0; i < labels.Count; i++)
            {
                orderSummary.Add(0);
                fixedAssetsSummary.Add(0);
                fixedAssetsRecipient.Add(0);
                benefitsSummary.Add(0);
                consumableSummary.Add(0);
                consumableRecipient.Add(0);
                reimbursementIn.Add(0);
                reimbursementOut.Add(0);
            }

            var recipientList = new List<RecipientEntity>();
            IRecipientRepository recipientServer = new RecipientRepository();
            recipientList = recipientServer.IQueryable().Where(x => x.F_CreatorTime != null).ToList();

            var dataList = new List<Object>();
            IOrderpaymentRepository paymentService = new OrderpaymentRepository();
            var orderAmountInfo = paymentService.IQueryable().Where(t => t.F_CreatorTime <= end && t.F_CreatorTime >= begin).OrderBy(t => t.F_CreatorTime).ToList();
            foreach (var order in orderAmountInfo)
            {
                var createDate = Convert.ToDateTime(order.F_CreatorTime);
                for (int labelIndex = 0; labelIndex < labels.Count; labelIndex++)
                {
                    var monthNum = labels[labelIndex].Replace("月", "");
                    if (createDate.Month == Convert.ToInt32(monthNum))
                    {
                        orderSummary[labelIndex] += order.P_Amount;
                    }
                }
            }
            var orderSum = new
            {
                name = "订单回款",
                data = orderSummary
            };
            dataList.Add(orderSum);

            IFixedassetsRepository fixedassetsService = new FixedassetsRepository();
            var fixedAssetsInfo = fixedassetsService.IQueryable().Where(t => t.F_CreatorTime <= end && t.F_CreatorTime >= begin).OrderBy(t => t.F_CreatorTime).ToList();
            foreach (var fixedAssets in fixedAssetsInfo)
            {
                var createDate = Convert.ToDateTime(fixedAssets.F_CreatorTime);
                for (int labelIndex = 0; labelIndex < labels.Count; labelIndex++)
                {
                    var monthNum = labels[labelIndex].Replace("月", "");
                    if (createDate.Month == Convert.ToInt32(monthNum))
                    {
                        fixedAssetsSummary[labelIndex] += fixedAssets.P_AssetAmount;
                    }
                }
            }
            var fixedAssetsSum = new
            {
                name = "固定资产采购",
                data = fixedAssetsSummary
            };
            dataList.Add(fixedAssetsSum);

            var fixedAssetsIds = fixedAssetsInfo.Select(x => x.F_Id).ToList();
            var fixedAssetsRecipientList = recipientList.Where(x => fixedAssetsIds.Contains(x.P_ItemId)).ToList();
            foreach (var fixedAssetsR in fixedAssetsRecipientList)
            { var fixedAssets = fixedAssetsInfo.Where(x => x.F_Id == fixedAssetsR.P_ItemId).SingleOrDefault();
                var createDate = Convert.ToDateTime(fixedAssetsR.F_CreatorTime);
                for (int labelIndex = 0; labelIndex < labels.Count; labelIndex++)
                {
                    var monthNum = labels[labelIndex].Replace("月", "");
                    if (createDate.Month == Convert.ToInt32(monthNum))
                    {
                        var amount = fixedAssetsR.P_RecipientNum * fixedAssets.P_AssetPrice;
                        fixedAssetsRecipient[labelIndex] += amount;
                    }
                }
            }
            var fixedAssetsRSum = new
            {
                name = "固定资产领用",
                data = fixedAssetsRecipient
            };
            dataList.Add(fixedAssetsRSum);

            IBenefitsRepository benefitsService = new BenefitsRepository();
            var benefitsInfo = benefitsService.IQueryable().Where(t => t.F_CreatorTime <= end && t.F_CreatorTime >= begin).OrderBy(t => t.F_CreatorTime).ToList();
            foreach (var benefits in benefitsInfo)
            {
                var createDate = Convert.ToDateTime(benefits.F_CreatorTime);
                for (int labelIndex = 0; labelIndex < labels.Count; labelIndex++)
                {
                    var monthNum = labels[labelIndex].Replace("月", "");
                    if (createDate.Month == Convert.ToInt32(monthNum))
                    {
                        benefitsSummary[labelIndex] += benefits.P_AssetAmount;
                    }
                }
            }
            var benefitsSum = new
            {
                name = "福利采购",
                data = benefitsSummary
            };
            dataList.Add(benefitsSum);

            IConsumableRepository consumableService = new ConsumableRepository();
            var consumableInfo = consumableService.IQueryable().Where(t => t.F_CreatorTime <= end && t.F_CreatorTime >= begin).OrderBy(t => t.F_CreatorTime).ToList();
            foreach (var consumable in consumableInfo)
            {
                var createDate = Convert.ToDateTime(consumable.F_CreatorTime);
                for (int labelIndex = 0; labelIndex < labels.Count; labelIndex++)
                {
                    var monthNum = labels[labelIndex].Replace("月", "");
                    if (createDate.Month == Convert.ToInt32(monthNum))
                    {
                        consumableSummary[labelIndex] += consumable.P_AssetAmount;
                    }
                }
            }
            var consumableSum = new
            {
                name = "消耗品采购",
                data = consumableSummary
            };
            dataList.Add(consumableSum);

            var consumableIds = consumableInfo.Select(x => x.F_Id).ToList();
            var consumableRecipientList = recipientList.Where(x => consumableIds.Contains(x.P_ItemId)).ToList();
            foreach (var consumableR in consumableRecipientList)
            {
                var consumable = consumableInfo.Where(x => x.F_Id == consumableR.P_ItemId).SingleOrDefault();
                var createDate = Convert.ToDateTime(consumableR.F_CreatorTime);
                for (int labelIndex = 0; labelIndex < labels.Count; labelIndex++)
                {
                    var monthNum = labels[labelIndex].Replace("月", "");
                    if (createDate.Month == Convert.ToInt32(monthNum))
                    {
                        var amount = consumableR.P_RecipientNum * consumable.P_AssetPrice;
                        consumableRecipient[labelIndex] += amount;
                    }
                }
            }
            var consumableRSum = new
            {
                name = "消耗品领用",
                data = consumableRecipient
            };
            dataList.Add(consumableRSum);

            IFinancialRepository financialService = new FinancialRepository();
            var reimbursementInfo = financialService.IQueryable().Where(t => t.F_CreatorTime <= end && t.F_CreatorTime >= begin).OrderBy(t => t.F_CreatorTime).ToList();
            foreach (var reimbursement in reimbursementInfo)
            {
                var createDate = Convert.ToDateTime(reimbursement.F_CreatorTime);
                for (int labelIndex = 0; labelIndex < labels.Count; labelIndex++)
                {
                    var monthNum = labels[labelIndex].Replace("月", "");
                    if (createDate.Month == Convert.ToInt32(monthNum))
                    {
                        if (reimbursement.P_FinancialType == "B")
                        {
                            reimbursementOut[labelIndex] += reimbursement.P_TotalAmount;
                        }
                        if (reimbursement.P_FinancialType == "H")
                        {
                            reimbursementIn[labelIndex] += reimbursement.P_TotalAmount;
                        }
                    }
                }
            }
            var reimbursementInSum = new
            {
                name = "报账入账",
                data = reimbursementIn
            };
            dataList.Add(reimbursementInSum);
            var reimbursementOutSum = new
            {
                name = "报账出账",
                data = reimbursementOut
            };
            dataList.Add(reimbursementOutSum);
            return new
            {
                labels = labels,
                data = dataList
            };
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
