using Busines.MODEL;
using Busines.ModelRes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Busines.IDAO
{
   public interface DataGraph_IDal
    {
        IList HalfYear<T>();
        DataTable GetTaskData(int state);

        IList<TaskModel> GetTaskList(int state);

        Echarts_Bar_TotalRes GetTaskDataTotal();

        Echarts_Pie_MonthRes GetTaskMonthData();
        Echarts_Pie_MonthRes GetTaskMonthData(DateTime SLDate);

        Echarts_Receiveable_MonthRes GetReceivableData(int state);

        Echarts_Fee_ValueRes GetDepartmentFeeData();

        Echarts_MonthCheckRes GetMonthChecked();
    }
}
