using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace UtilityCode
{
    public static class MathCode
    {
        /// <summary>
        /// 多边形面积计算(不论凸凹)
        /// </summary>
        /// <param name="count">坐标点数</param>
        /// <param name="X">坐标的横坐标集合</param>
        /// <param name="Y">坐标的纵坐标集合</param>
        /// <param name="style"></param>
        /// <returns></returns>
        public static double AoArea(int count, double[] X, double[] Y, string style = "亩")
        {
            double area;
            if (count < 3)
                return 0;
            area = Y[0] * (X[count - 1] - X[1]);
            for (int i = 1; i < count; i++)
                area += Y[i] * (X[(i - 1)] - X[(i + 1) % count]);
            if (style == "亩")
                return area / 2 * 0.0015;
            return area / 2;
        }

        /// <summary>
        /// 获得一定范围内的随机小数
        /// </summary>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        /// <param name="Len"></param>
        /// <returns></returns>
        public static double GetRandomNumber(double minimum, double maximum, int len)   //Len小数点保留位数
        {
            //Thread.Sleep(15);
            Random random = new Random(GetRandomSeed());
            return Math.Round(random.NextDouble() * (maximum - minimum) + minimum, len);
        }

        static int GetRandomSeed()
        {
            byte[] bytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }
    }
}
