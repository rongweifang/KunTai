using System;
using System.Collections.Generic;
using System.Web;

namespace Plupload.Web.Common
{
    /// <summary>
    /// 生成缩略图方法。
    /// </summary>
    public enum ThumbnailMode
    {
        /// <summary>
        /// 指定高宽缩放（可能变形）。
        /// </summary>
        [EnumDescription(Description = "指定高宽缩放（可能变形）")]
        ByHeightAndWidth = 0,

        /// <summary>
        /// 指定高，宽按比例。
        /// </summary>
        [EnumDescription(Description = "指定宽，高按比例")]
        ByHeight = 1,

        /// <summary>
        /// 指定宽，高按比例。
        /// </summary>
        [EnumDescription(Description = "指定高，宽按比例")]
        ByWidth = 2,

        /// <summary>
        /// 指定高宽裁减（不变形）。
        /// </summary>
        [EnumDescription(Description = "指定高宽裁减（不变形）")]
        CutByWidthAndHeight = 3

    }
}