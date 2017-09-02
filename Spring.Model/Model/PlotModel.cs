using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spring.Model
{
    /// <summary>
    /// 地块实体
    /// </summary>
    public class PlotModel
    {
        /// <summary>
        /// 地块名称
        /// </summary>
        public string PlotName { get; set; }

        /// <summary>
        /// 承包方代码
        /// </summary>
        public string CbfCode { get; set; }

        /// <summary>
        /// 承包方代表名称
        /// </summary>
        public string CbfDBName { get; set; }

        /// <summary>
        /// 地块代码
        /// </summary>
        public string PlotCode { get; set; }

        /// <summary>
        /// 地块面积P
        /// </summary>
        public string PlotArea { get; set; }

        

        /// <summary>
        /// 检查地块面积P'
        /// </summary>
        public string PlotCheckArea { get; set; }

        /// <summary>
        /// 面积差
        /// </summary>
        public string DifArea { get; set; }

        /// <summary>
        /// 误差百分比
        /// </summary>
        public string PercentageError { get; set; }

        /// <summary>
        /// 坐标列表
        /// </summary>
        public List<CoordinatesModel> CoordinateList { get; set; }

        /// <summary>
        /// 界址点中误差
        /// </summary>
        public string PlotM { get; set; }

        /// <summary>
        /// 是否生成PDF
        /// </summary>
        public bool IsGenerate { get; set; }

    }
}
