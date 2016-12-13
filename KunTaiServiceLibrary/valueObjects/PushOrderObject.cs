using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace KunTaiServiceLibrary.valueObjects
{
    public class PushOrderObject
    {
        public string NUM { get; set; }

        public string ID { get; set; }

        public string PUSHID { get; set; }

        public string NAME { get; set; }//PUSHID对应的姓名

        public string PUSHDATETIME { get; set; }

        public string RUNDATETIME { get; set; }

        public string FILEURL { get; set; }

        public string LOCALFILENAME { get; set; }

        public string NOTE { get; set; }

        public List<PushOrderReceiveObject> RECEIVEITEM { get; set; }

        public string RECEIVEEMPLOYEE { get; set; }

        public PushOrderObject()
        {

        }


        public PushOrderObject(XElement xml)
        {
            if (xml != null)
            {
                this.NUM = xml.Element("NUM") == null ? string.Empty : xml.Element("NUM").Value;
                this.ID = xml.Element("ID") == null ? string.Empty : xml.Element("ID").Value;
                this.PUSHID = xml.Element("PUSHID") == null ? string.Empty : xml.Element("PUSHID").Value;
                this.RUNDATETIME = xml.Element("RUNDATETIME") == null ? string.Empty : xml.Element("RUNDATETIME").Value;
                this.FILEURL = xml.Element("FILEURL") == null ? string.Empty : xml.Element("FILEURL").Value;
                this.LOCALFILENAME = xml.Element("LOCALFILENAME") == null ? string.Empty : xml.Element("LOCALFILENAME").Value;
                this.NOTE = xml.Element("NOTE") == null ? string.Empty : xml.Element("NOTE").Value;

                if (xml.Element("RECEIVEID") != null)
                {
                    RECEIVEITEM = new List<PushOrderReceiveObject>();
                    foreach (string item in xml.Element("RECEIVEID").Value.Split(','))
                    {
                        RECEIVEITEM.Add(new PushOrderReceiveObject(this.ID, item));
                    }
                }

                this.PUSHDATETIME = string.Empty;
                this.RECEIVEEMPLOYEE = string.Empty;
                this.NAME = string.Empty;
            }
        }


        public PushOrderObject(DataRow dataRow)
        {
            if (dataRow != null)
            {
                this.NUM = dataRow.Table.Columns.Contains("NUM") ? dataRow["NUM"].ToString() : string.Empty;
                this.ID = dataRow.Table.Columns.Contains("ID") ? dataRow["ID"].ToString() : string.Empty;
                this.PUSHID = dataRow.Table.Columns.Contains("PUSHID") ? dataRow["PUSHID"].ToString() : string.Empty;
                this.NAME = dataRow.Table.Columns.Contains("NAME") ? dataRow["NAME"].ToString() : string.Empty;
                this.RUNDATETIME = dataRow.Table.Columns.Contains("RUNDATETIME") ? dataRow["RUNDATETIME"].ToString() : string.Empty;
                this.PUSHDATETIME = dataRow.Table.Columns.Contains("PUSHDATETIME") ? dataRow["PUSHDATETIME"].ToString() : string.Empty;
                this.FILEURL = dataRow.Table.Columns.Contains("FILEURL") ? dataRow["FILEURL"].ToString() : string.Empty;
                this.LOCALFILENAME = dataRow.Table.Columns.Contains("LOCALFILENAME") ? dataRow["LOCALFILENAME"].ToString() : string.Empty;
                this.NOTE = dataRow.Table.Columns.Contains("NOTE") ? dataRow["NOTE"].ToString() : string.Empty;
                RECEIVEITEM = null;
                this.RECEIVEEMPLOYEE = dataRow.Table.Columns.Contains("RECEIVEEMPLOYEE") ? dataRow["RECEIVEEMPLOYEE"].ToString() : string.Empty;
            }
        }

    }

}
