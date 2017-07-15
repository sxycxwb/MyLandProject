using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spring.Model
{
    /// <summary>
    /// 坐标实体
    /// </summary>
    public class CoordinatesModel
    {
        /// <summary>
        /// 序号
        /// </summary>
        public string OrderNum { get; set; }

        /// <summary>
        /// 预编点号
        /// </summary>
        public string SerialNumber { get; set; }

        /// <summary>
        /// 界址点编号
        /// </summary>
        public string BoundaryPointNum { get; set; }

        /// <summary>
        /// X坐标
        /// </summary>
        public string X { get; set; }

        /// <summary>
        /// Y坐标
        /// </summary>
        public string Y { get; set; }

        /// <summary>
        /// X'检查坐标
        /// </summary>
        public string cX { get; set; }

        /// <summary>
        /// Y'检查坐标
        /// </summary>
        public string cY { get; set; }

        /// <summary>
        /// ∆X坐标差值
        /// </summary>
        public string difX { get; set; }

        /// <summary>
        /// ∆Y坐标差值
        /// </summary>
        public string difY { get; set; }

        /// <summary>
        /// ∆L两点差值
        /// </summary>
        public string difL { get; set; }

        /// <summary>
        /// ∆L2两点差值平方
        /// </summary>
        public string difSquareL { get; set; }
    }
}
