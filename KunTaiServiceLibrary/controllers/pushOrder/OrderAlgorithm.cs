using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KunTaiServiceLibrary.controllers.pushOrder
{
    public class OrderAlgorithm
    {
        private double _maxValue;
        private double _minValue;
        private decimal _Area;
        private int _Tonnage = 40;//吨位
        private int _Target ;//
        private int _BoilerCount = 3;//锅炉数量
        private decimal _Power ;//功率
        private decimal _PatchPower;//补水泵功率
        private decimal _PatchEfficiency;//补水泵效率
        private decimal _Scontent = 0.0006m;//煤中硫含量
        private decimal _Efficiency = 0.85m;//循环泵效率
        private decimal _Calorie = 5133m;//热量大卡
        private decimal _Ratio = 1;

        private const decimal _GJ = 277777.78m;//吉焦
        private const decimal _DK = 238900m;//大卡
        private decimal _CYCLEFLOW=1m;//循环泵流量
        #region
        public double MaxValue
        {
            get
            {
                return _maxValue;
            }

            set
            {
                _maxValue = value;
            }
        }

        public double MinValue
        {
            get
            {
                return _minValue;
            }

            set
            {
                _minValue = value;
            }
        }

        public decimal Area
        {
            get
            {
                return _Area;
            }

            set
            {
                _Area = value;
            }
        }

        public int Tonnage
        {
            get
            {
                return _Tonnage;
            }

            set
            {
                _Tonnage = value;
            }
        }

        public int BoilerCount
        {
            get
            {
                return _BoilerCount;
            }

            set
            {
                _BoilerCount = value;
            }
        }

        public decimal Power
        {
            get
            {
                return _Power;
            }

            set
            {
                _Power = value;
            }
        }

        public decimal Efficiency
        {
            get
            {
                return _Efficiency;
            }

            set
            {
                _Efficiency = value;
            }
        }

        public decimal CYCLEFLOW
        {
            get
            {
                return _CYCLEFLOW;
            }

            set
            {
                _CYCLEFLOW = value;
            }
        }

        public decimal Ratio
        {
            get
            {
                return _Ratio;
            }

            set
            {
                _Ratio = value;
            }
        }

        public int Target
        {
            get
            {
                return _Target;
            }

            set
            {
                _Target = value;
            }
        }

        public decimal PatchPower
        {
            get
            {
                return _PatchPower;
            }

            set
            {
                _PatchPower = value;
            }
        }

        public decimal PatchEfficiency
        {
            get
            {
                return _PatchEfficiency;
            }

            set
            {
                _PatchEfficiency = value;
            }
        }
        #endregion
        //最大热负荷
        private decimal GetLoad()
        {

            return Target * _Area * 24;
        }
        //供热日热负荷
        private decimal GetLoadDay()
        {
            return Math.Round(GetLoad() * (18 - (decimal)_AveTemp()) / (18 + 21),2);
        }
        //平均温度
        private decimal _AveTemp()
        {
            return (decimal)(Convert.ToDouble(_maxValue) + Convert.ToDouble(_minValue)) / 2;
        }

        private decimal GetHeadGJ()
        {
            return Math.Round(GetLoadDay() / _GJ,2);
        }
        //总热量
        private decimal GetHeatDK()
        {
            return Math.Round(GetHeadGJ() * _DK,2);
        }
        //总煤量KG
        private decimal _CoalTotal()
        {
            return Math.Round(GetHeatDK() / _Calorie,2);
        }

        //指令耗煤量T
        public decimal GetCoalTotal()
        {
            return Math.Round(_CoalTotal() / 1000,2);
        }
        //指令运行时间
        public decimal GetRunDate()
        {
            return Math.Round(_CoalTotal() / 133 / _Tonnage,2);
        }
        //耗电量
        public decimal GetEle()
        {
            return Math.Round(_BoilerCount * GetRunDate() * _Power * _Efficiency + 1.2m*7,2);
        }
        //耗水量（一网水量）
        public decimal GetWaterTotal()
        {
            return Math.Round(GetHeadGJ() /84,2);
        }
        //耗碱量
        public decimal GetAlkali()
        {
            return Math.Round(GetCoalTotal() * _Scontent  / 0.19M,2);
        }
        //耗盐量
        public decimal GetSalt()
        {
            return Math.Round(GetWaterTotal() * 0.41m,2);
        }

        //循环泵指令时间
        public decimal GetPumpDate(decimal _WaterTotal)
        {
            return _WaterTotal / _CYCLEFLOW / 0.002m;
        }

        //换热站
        //二网水
        public decimal GetAllWater2()
        {
            return 100 * GetHeadGJ() / 4.2m;
        }
        public decimal GetWater2()
        {
            //_Target * _Area * (18 - _AveTemp()) / (18 + 21) / 277777.78 * 100 / 4.2 * 0.003;
            return Math.Round(GetAllWater2()*0.003m, 2);
        }
        //换热站指令时间
        public decimal GetStationRunDate()
        {
            return Math.Round(GetAllWater2() / _CYCLEFLOW / _Efficiency,2);
        }
        //换热站耗电量
        public decimal GetStationEle()
        {
            return Math.Round(GetStationRunDate() * (_Power * _Efficiency + _PatchPower * _PatchEfficiency), 2);
        }
    }
}
