using System;

namespace KunTaiServiceLibrary
{
    public class MathUtils
    {
        #region 指定在范围内随机数字
        //http://www.cnblogs.com/jxsoft/archive/2011/03/15/1984509.html

        /// <summary>
        /// 随机生成唯一值的数组
        /// </summary>
        /// <param name="minValue">最小值范围</param>
        /// <param name="maxValue">最大值范围</param>
        /// <param name="count">生成的数量</param>
        /// <returns>返回指定范围内的唯一值的数组</returns>
        public static int[] Random(int minValue, int maxValue, uint count)
        {
            Random ra = new Random(unchecked((int)DateTime.Now.Ticks));
            int[] arrNum = new int[count];
            //给初始化的数字赋默认值
            for (int i = 0; i < count; i++)
            {
                arrNum[i] = -1;
            }

            int tmp = 0;
            for (int i = 0; i < count; i++)
            {
                tmp = ra.Next(minValue, maxValue); //随机取数
                arrNum[i] = getNum(arrNum, tmp, minValue, maxValue, ra); //取出值赋到数组中
            }

            return arrNum;
        }

        ///*
        private static int getNum(int[] arrNum, int tmp, int minValue, int maxValue, Random ra)
        {
        goToLabel:
            foreach (int item in arrNum)
            {
                if (item == -1)
                    continue;

                //验证当前值是否存在与数组内。
                if (item == tmp)
                {
                    tmp = ra.Next(minValue, maxValue); //重新随机获取。
                    goto goToLabel;//重新检查当前算出的数值，是否存在在数组内。
                }
            }
            return tmp;
        }
        //*/

        /*
        public int getNum(int[] arrNum, int tmp, int minValue, int maxValue, Random ra)
        {
            int n = 0;
            while (n <= arrNum.Length - 1)
            {
                if (arrNum[n] == tmp) //利用循环判断是否有重复
                {
                    tmp = ra.Next(minValue, maxValue); //重新随机获取。
                    getNum(arrNum, tmp, minValue, maxValue, ra);//递归:如果取出来的数字和已取得的数字有重复就重新随机获取。
                }
                n++;
            }
            return tmp;
        }
        //*/

        #endregion


        #region 获取一个随机值

        /// <summary>
        /// 获取一个随机值
        /// </summary>
        public static string RandomValue
        {
            get { return new Random(unchecked((int)DateTime.Now.Ticks)).Next().ToString(); }
        }

        #endregion

    }
}
