using Finix.Application.Project;
using Finix.Code;
using System.Web.Mvc;

namespace Finix.Web.Areas.ReportManage.Controllers
{
    public class HighchartsController : Controller
    {
        #region 折线图
        /// <summary>
        /// 折线图
        /// </summary>
        /// <returns></returns>
        public ActionResult Sample1()
        {
            return View();
        }
        #endregion

        #region 曲线图
        /// <summary>
        /// 曲线图
        /// </summary>
        /// <returns></returns>
        public ActionResult Sample2()
        {
            return View();
        }
        #endregion

        #region 面积图
        /// <summary>
        /// 面积图
        /// </summary>
        /// <returns></returns>
        public ActionResult Sample3()
        {
            return View();
        }
        #endregion

        #region 条形图
        /// <summary>
        /// 条形图
        /// </summary>
        /// <returns></returns>
        public ActionResult Sample4()
        {
            return View();
        }
        #endregion

        #region 柱状图--财务汇总
        public ActionResult FinancialInfo()
        {
            return View();
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult FinancialSummary(string beginTime, string endTime)
        {
            FinancialApp financialApp = new FinancialApp();
            var data = new
            {
                financialData = financialApp.FinancialSummary(beginTime, endTime)
            };
            return Content(data.ToJson());
        }
        #endregion

        #region 柱状图--订单汇总
        /// <summary>
        /// 柱状图
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderSummary()
        {
            return View();
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetOrdersDataJson(string beginTime, string endTime)
        {
            OrderApp orderApp = new OrderApp();
            var orderList_date = orderApp.GetAllOrderDataByDate(beginTime, endTime);
            var orderList_cus_total = orderApp.GetAllOrderDataByCustomerTotal(beginTime, endTime);
            var orderList_cus = orderApp.GetAllOrderDataByCustomer(beginTime, endTime);
            var data = new
            {
                orderList_date = orderList_date,
                orderList_cus_total = orderList_cus_total,
                orderList_cus = orderList_cus
            };
            return Content(data.ToJson());
        }
        #endregion

        #region 饼图
        /// <summary>
        /// 饼图
        /// </summary>
        /// <returns></returns>
        public ActionResult Sample6()
        {
            return View();
        }
        #endregion

        #region 散点图
        /// <summary>
        /// 散点图
        /// </summary>
        /// <returns></returns>
        public ActionResult Sample7()
        {
            return View();
        }
        #endregion

        #region 气泡图
        /// <summary>
        /// 气泡图
        /// </summary>
        /// <returns></returns>
        public ActionResult Sample8()
        {
            return View();
        }
        #endregion

        #region 模拟时钟
        /// <summary>
        /// 模拟时钟
        /// </summary>
        /// <returns></returns>
        public ActionResult Sample9()
        {
            return View();
        }
        #endregion

        #region 仪表盘1
        /// <summary>
        /// 仪表盘1
        /// </summary>
        /// <returns></returns>
        public ActionResult Sample10()
        {
            return View();
        }
        #endregion

        #region 雷达图
        /// <summary>
        /// 雷达图
        /// </summary>
        /// <returns></returns>
        public ActionResult Sample11()
        {
            return View();
        }
        #endregion

        #region 蛛网图
        /// <summary>
        /// 蛛网图
        /// </summary>
        /// <returns></returns>
        public ActionResult Sample12()
        {
            return View();
        }
        #endregion

        #region 玫瑰图
        /// <summary>
        /// 玫瑰图
        /// </summary>
        /// <returns></returns>
        public ActionResult Sample13()
        {
            return View();
        }
        #endregion

        #region 漏斗图
        /// <summary>
        /// 漏斗图
        /// </summary>
        /// <returns></returns>
        public ActionResult Sample14()
        {
            return View();
        }
        #endregion

        #region 蜡烛图
        /// <summary>
        /// 蜡烛图
        /// </summary>
        /// <returns></returns>
        public ActionResult Sample15()
        {
            return View();
        }
        #endregion

        #region 流程图
        /// <summary>
        /// 流程图
        /// </summary>
        /// <returns></returns>
        public ActionResult Sample16()
        {
            return View();
        }
        #endregion
    }
}
