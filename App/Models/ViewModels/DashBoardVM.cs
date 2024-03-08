using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bakery.Models.ViewModels
{
    public class DashBoardVM
    {
        public DateTime date { get; set; }
        public DateTime fromDay { get; set; }
        public DateTime toDay { get; set; }
        public Data currentData { get; set; }
        public Data previousData { get; set; }
        public List<Data> chartData { get; set; }
        public List<sp_popular_Result> products {get; set; }
        public List<sp_popular_Result> products2 { get; set; }

        public DashBoardVM(){}

        public DashBoardVM(DateTime date, DateTime fromDay, DateTime toDay, Data currentData, Data previousData, List<Data> chartData, List<sp_popular_Result> products, List<sp_popular_Result> products2)
        {
            this.date = date;
            this.fromDay = fromDay;
            this.toDay = toDay;
            this.currentData = currentData;
            this.previousData = previousData;
            this.chartData = chartData;
            this.products = products;
            this.products2 = products2;
        }

        public class Data
        {
            public int noProducts { get; set; }
            public int noSales { get; set; }
            public int totalSales { get; set; }
            public string label { get; set; }

            public Data() { }

            public Data(int noProducts, int totalSales)
            {
                this.noProducts = noProducts;
                this.totalSales = totalSales;
            }
            public Data(int noProducts, int totalSales, string label)
            {
                this.noProducts = noProducts;
                this.totalSales = totalSales;
                this.label = label;
            }
            public Data(int noProducts, int noSales, int totalSales)
            {
                this.noProducts = noProducts;
                this.noSales = noSales;
                this.totalSales = totalSales;
            }
        }
    }
}