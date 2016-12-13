using KunTaiServiceLibrary.valueObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml.Linq;

namespace KunTaiServiceLibrary.Tests
{
    [TestClass()]
    public class ZY_AssignPlanTests
    {
        [TestMethod()]
        public void addWR_STAT_ATest()
        {
            /*
            DataSet dataSetExcel = ExcelToDataSet.Load(@"\\Mac\Home\Desktop\用所选项目新建的文件夹\三支一扶成绩.xlsx");

            string insert = "INSERT INTO UserMessageTemp (ID,User_ID,MsgType,MsgTitle,MsgSender,MsgContent) VALUES (NewID(),(SELECT User_ID FROM Personinfo WHERE loginName='{0}'),3,'笔试通知','48f3889c-af8d-401f-ada2-c383031af92d','{1}');\r\n";

            System.Text.StringBuilder command = new System.Text.StringBuilder();
            foreach (DataRow row in dataSetExcel.Tables[0].Rows)
            {
                if(row["身份证号"].ToString() == "150204198810131818")
                {string te = string.Format(insert, row["身份证号"].ToString(), row["信息"].ToString());
                    
                }
                
                command.AppendFormat(insert, row["身份证号"].ToString(), row["信息"].ToString());
            }

            string temp = command.ToString();
            */
            string text = "<PUSHORDER><ID>'198e39d8-8b98-4d06-b1de-60c53dcd61e5','f0f448f2-33a7-4705-a115-ed9f9b0bb9ef'</ID></PUSHORDER>";

            string result = string.Empty;

            result = new ZY_PushOrder().downPushOrder(text);


        }
    }
}