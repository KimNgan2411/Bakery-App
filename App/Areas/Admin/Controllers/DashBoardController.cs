using Bakery.Models;
using Bakery.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Bakery.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DashBoardController : Controller
    {
        private BakeryStoreDBEntities db = new BakeryStoreDBEntities();

        Tuple<int, string>[] ChartOpts = new Tuple<int, string>[] {
            new Tuple<int, string>(0, "Ngày này"),
            new Tuple<int, string>(1, "Tuần này"),
            new Tuple<int, string>(2, "Tháng này"),
            new Tuple<int, string>(3, "Năm này"),
        };

        // GET: Admin/DashBoard
        public ActionResult Index(DateTime? date, int chartOption = 0)
        {
            if (date == null) date = DateTime.Today;

            DateTime fromDay, toDay;
            getDates(date, chartOption, out fromDay, out toDay);
            var currentData = getData(fromDay, toDay);
            var previousData = getPreviousData(chartOption, fromDay, toDay);
            var chartData = getChartData(chartOption, fromDay, toDay);
            var products = db.sp_popular(fromDay, toDay, false, 10).ToList();
            var products2 = db.sp_popular(fromDay, toDay, true, 10).ToList();

            DashBoardVM model = new DashBoardVM(
                date.Value, fromDay, toDay,
                currentData,
                previousData,
                chartData,
                products,
                products2
            );
            ViewBag.chartOption = new SelectList(ChartOpts, "Item1", "Item2");
            return View(model);
        }

        //filter table by given date and option
        void getDates(DateTime? date, int chartOption, out DateTime fday, out DateTime tday) {
            //get start and end day by given date and option
            DateTime fromDay = date.Value.Date;
            DateTime toDay = date.Value.Date;
            switch (chartOption)
            {
                //monday to sunday
                case 1:
                    while (fromDay.DayOfWeek != DayOfWeek.Monday) fromDay = fromDay.AddDays(-1);
                    while (toDay.DayOfWeek != DayOfWeek.Sunday) toDay = toDay.AddDays(1);
                    break;
                //1 -> end of month
                case 2:
                    fromDay = new DateTime(fromDay.Year, fromDay.Month, 1);
                    toDay = fromDay.AddMonths(1).AddDays(-1);
                    break;
                //1 -> end of year
                case 3:
                    fromDay = new DateTime(fromDay.Year, 1, 1);
                    toDay = new DateTime(toDay.Year, 12, 31);
                    break;
                default: break;
            }

            fday = fromDay;
            tday = toDay;
        }

        //get total of given days
        DashBoardVM.Data getData(DateTime fromDay, DateTime toDay) {
            var hds = db.HoaDons.Where(x => x.NgayHD <= toDay && x.NgayHD >= fromDay);
            int noSales = hds.Count();
            int totalSales = hds.ToList().Sum(x => x.TongTien).GetValueOrDefault();

            var cthds = from hd in hds
                        from ct in db.CTHDs
                        where ct.MaHD == hd.MaHD
                        select ct;
            int noProducts = cthds.ToList().Sum(x => x.SoLuong).GetValueOrDefault();

            return new DashBoardVM.Data(noProducts, noSales, totalSales);
        }

        DashBoardVM.Data getPreviousData( int chartOption, DateTime fromDay, DateTime toDay)
        {
            switch (chartOption) {
                case 1:
                    fromDay = fromDay.AddDays(-7);
                    toDay = toDay.AddDays(-7);
                    break;
                case 2:
                    fromDay = fromDay.AddMonths(-1);
                    toDay = toDay.AddMonths(-1);
                    break;
                case 3:
                    fromDay = fromDay.AddYears(-1);
                    toDay = toDay.AddYears(-1);
                    break;
                default:
                    fromDay = fromDay.AddDays(-1);
                    toDay = toDay.AddDays(-1);
                    break;
            }

           return getData(fromDay, toDay);
        }

        List<DashBoardVM.Data> getChartData(int chartOption, DateTime fromDay, DateTime toDay) {
            List<DashBoardVM.Data> res = new List<DashBoardVM.Data>();

            switch (chartOption)
            {
                case 1:
                    for (DateTime i = fromDay; i <= toDay; i = i.AddDays(1)) {
                        var data = getData(i, i);
                        int t = (int)i.DayOfWeek + 1;
                        data.label = t == 1 ? "CN" : "T" + t;
                        res.Add(data);
                    }
                    break;
                case 2:
                    for (DateTime i = fromDay; i <= toDay; i = i.AddDays(1))
                    {
                        var data = getData(i, i);
                        data.label = i.Day.ToString();
                        res.Add(data);
                    }
                    break;
                case 3:
                    for (DateTime i = fromDay; i <= toDay; i = i.AddMonths(1))
                    {
                        var data = getData(i, i.AddMonths(1).AddDays(-1));
                        data.label = "Tháng " + i.Month;
                        res.Add(data);
                    }
                    break;
                default:  
                    break;
            }

            return res;
        }

    }
}