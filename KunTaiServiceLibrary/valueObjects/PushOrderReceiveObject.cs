namespace KunTaiServiceLibrary.valueObjects
{

    public class PushOrderReceiveObject
    {
        /// <summary>
        /// 数据库编号
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 所属PushOrder编号
        /// </summary>
        public string POID { get; set; }

        /// <summary>
        /// 接收人编号
        /// </summary>
        public string RECEIVEID { get; set; }


        public PushOrderReceiveObject()
        {

        }

        /// <summary>
        /// PushOrder对应的接收人对象
        /// </summary>
        /// <param name="POID">所属PushOrder编号</param>
        /// <param name="RECEIVEID">接收人编号</param>
        public PushOrderReceiveObject(string POID, string RECEIVEID)
        {
            this.POID = POID;
            this.RECEIVEID = RECEIVEID;
        }
    }
}
