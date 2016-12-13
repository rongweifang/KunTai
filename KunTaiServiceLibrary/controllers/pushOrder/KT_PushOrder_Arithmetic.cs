using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KunTaiServiceLibrary
{
    public class KT_PushOrder_Arithmetic
    {

        //指令时间
        public static decimal GetCommandTime(double MAXVALUE, double MINVALUE)
        {
            double aveTemp = (Convert.ToDouble(MAXVALUE) + Convert.ToDouble(MINVALUE)) / 2;
            //日运行时间
            double dayRunTime = (18 - aveTemp) * 24d / 28d;



            return Convert.ToDecimal(dayRunTime);
        }

        //指令煤
        public static decimal GetCommand_Coal(decimal CommandTime)
        {
            decimal result =Math.Round(CommandTime * 5.57m, 2);
            return result;
        }

        //指令水
        public static decimal GetCommand_Water(decimal CommandTime)
        {
            decimal result = Math.Round(CommandTime * 0.07m, 2);
            return result;
        }

        public static decimal GetCommand_Ele(decimal CommandTime)
        {
            decimal result = Math.Round(CommandTime * 375m, 2);
            return result;
        }

        public static decimal GetCommand_Alkali(decimal CommandTime)
        {
            decimal result = Math.Round(CommandTime * 0.02m, 2);
            return result;
        }

        public static decimal GetCommand_Salt(decimal CommandTime)
        {
            decimal result = Math.Round(CommandTime * 0.59m, 2);
            return result;
        }

        public static decimal GetCommand_Diesel(decimal CommandTime)
        {
            decimal result = Math.Round(CommandTime * 0.59m, 2);
            return result;
        }
    }
}
