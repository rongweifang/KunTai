using System.Collections.Generic;

namespace KunTaiServiceLibrary.valueObjects
{
    public class AuthorityObject
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 父级编号
        /// </summary>
        public string PID { get; set; }

        /// <summary>
        /// 权限名称
        /// </summary>
        public string NAME { get; set; }

        /// <summary>
        /// 链接地址
        /// </summary>
        public string LINK { get; set; }

        /// <summary>
        /// 图片地址
        /// </summary>
        public string ICON { get; set; }

        /// <summary>
        /// 状态（是否勾选）
        /// </summary>
        public string STATE { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        public string LEVEL { get; set; }

        /// <summary>
        /// 子权限集合
        /// </summary>
        public List<AuthorityObject> CHILDREN { get; set; }

    }
}
