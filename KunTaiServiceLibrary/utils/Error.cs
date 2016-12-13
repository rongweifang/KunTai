
namespace KunTaiServiceLibrary
{
    public class Error
    {

        /// <summary>
        /// 方法接收的参数不能为空。
        /// </summary>
        public const string XML_IS_NULL = "方法接收的参数不能为空。";

        /// <summary>
        /// 参数不是有效的XML格式字符串。
        /// </summary>
        public const string XML_FORMAT_ERROR = "参数不是有效的字符串。";

        /// <summary>
        /// 查询数据失败。数据表的数量为0。
        /// </summary>
        public const string RESULT_ERROR = "查询数据失败。数据表的数量为0。";

        /// <summary>
        /// 存在相同的记录。
        /// </summary>
        public const string EXISTS_ERROR = "系统中已经存在相同的记录。";

    }
}
