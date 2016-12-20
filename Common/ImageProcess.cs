using System;
using System.Collections.Generic;
using System.Web;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Plupload.Web.Common
{
    /// <summary>
    /// 提供图片缩略图和添加水印的一组方法。
    /// </summary>
    public class ImageProcess
    {
        /// <summary>
        /// 在图片上加入图片版权信息
        /// </summary>
        /// <param name="inputImage">输入图片</param>
        /// <param name="outputImage">输出图片</param>
        /// <param name="watermarkFile">水印图片</param>
        /// <param name="watermarkLocation">水印位置</param>
        /// <param name="bolFileName">是否删除输入图片</param>
        public void MakeWatermark(string inputImage, string outputImage, string watermarkFile, int watermarkLocation, bool bolFileName)
        {
            if (watermarkLocation > 0)
            {
                //开始
                Image oldImage = Image.FromFile(inputImage);
                Image watermarkImage = Image.FromFile(watermarkFile);
                Bitmap OutPut = new Bitmap(oldImage);
                Graphics GImage = Graphics.FromImage(OutPut);
                int IntX = 0;
                int IntY = 0;
                //在左上
                if (watermarkLocation == 1)
                {
                    IntX = 0;
                    IntY = 0;
                }
                //在正上
                else if (watermarkLocation == 2)
                {
                    IntX = (oldImage.Width - watermarkImage.Width) / 2;
                    IntY = 0;
                }
                //在右上
                else if (watermarkLocation == 3)
                {
                    IntX = oldImage.Width - watermarkImage.Width;
                    IntY = 0;
                }
                //在正左
                else if (watermarkLocation == 4)
                {
                    IntX = 0;
                    IntY = (oldImage.Height - watermarkImage.Height) / 2;
                }
                //在中间
                else if (watermarkLocation == 5)
                {
                    IntX = (oldImage.Width - watermarkImage.Width) / 2;
                    IntY = (oldImage.Height - watermarkImage.Height) / 2;
                }
                //在正右
                else if (watermarkLocation == 6)
                {
                    IntX = oldImage.Width - watermarkImage.Width;
                    IntY = (oldImage.Height - watermarkImage.Height) / 2;
                }
                // 在左下
                else if (watermarkLocation == 7)
                {
                    IntX = 0;
                    IntY = oldImage.Height - watermarkImage.Height;
                }
                //在正下
                else if (watermarkLocation == 8)
                {
                    IntX = (oldImage.Width - watermarkImage.Width) / 2;
                    IntY = oldImage.Height - watermarkImage.Height;
                }
                //在右下
                else if (watermarkLocation == 9)
                {
                    IntX = oldImage.Width - watermarkImage.Width;
                    IntY = oldImage.Height - watermarkImage.Height;
                }
                else
                {
                    IntX = oldImage.Width - watermarkImage.Width - 10;
                    IntY = oldImage.Height - watermarkImage.Height - 10;
                }
                // 画出水印的位置
                GImage.DrawImage(watermarkImage, IntX, IntY, watermarkImage.Width, watermarkImage.Height);
                string strExtend = inputImage.Substring(inputImage.LastIndexOf(".") + 1).ToLower();
                switch (strExtend)
                {
                    case "bmp":
                        OutPut.Save(outputImage, ImageFormat.Bmp);
                        break;
                    case "jpg":
                        OutPut.Save(outputImage, ImageFormat.Jpeg);
                        break;
                    case "gif":
                        OutPut.Save(outputImage, ImageFormat.Gif);
                        break;
                    case "icon":
                        OutPut.Save(outputImage, ImageFormat.Icon);
                        break;
                    case "png":
                        OutPut.Save(outputImage, ImageFormat.Png);
                        break;
                    case "tif":
                        OutPut.Save(outputImage, ImageFormat.Tiff);
                        break;
                    default:
                        OutPut.Save(outputImage, ImageFormat.Jpeg);
                        break;
                }
                GImage.Dispose();
                OutPut.Dispose();
                oldImage.Dispose();
                watermarkImage.Dispose();
                if (bolFileName) { File.Delete(inputImage); }
            }
        }

        /// <summary>
        /// 生成指定图片的缩略图并保存到流中。
        /// </summary>
        /// <param name="originalImage">指定的图片。</param>
        /// <param name="target_width">缩略图的宽度。</param>
        /// <param name="target_height">缩略图的高度。</param>
        /// <returns>返回生成的缩略图流。</returns>
        public static MemoryStream MakeThumbnail(Image originalImage, int target_width, int target_height)
        {
            System.Drawing.Bitmap final_image = null;
            System.Drawing.Graphics graphic = null;

            // Calculate the new width and height
            int width = originalImage.Width;
            int height = originalImage.Height;
            target_width = 100;
            target_height = 100;
            int new_width, new_height;

            float target_ratio = (float)target_width / (float)target_height;
            float image_ratio = (float)width / (float)height;

            if (target_ratio > image_ratio)
            {
                new_height = target_height;
                new_width = (int)Math.Floor(image_ratio * (float)target_height);
            }
            else
            {
                new_height = (int)Math.Floor((float)target_width / image_ratio);
                new_width = target_width;
            }

            new_width = new_width > target_width ? target_width : new_width;
            new_height = new_height > target_height ? target_height : new_height;

            final_image = new System.Drawing.Bitmap(target_width, target_height);
            graphic = System.Drawing.Graphics.FromImage(final_image);
            graphic.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.Black), new System.Drawing.Rectangle(0, 0, target_width, target_height));
            int paste_x = (target_width - new_width) / 2;
            int paste_y = (target_height - new_height) / 2;
            graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic; /* new way */
            //graphic.DrawImage(thumbnail_image, paste_x, paste_y, new_width, new_height);
            graphic.DrawImage(originalImage, paste_x, paste_y, new_width, new_height);
            MemoryStream ms = null;
            // Store the thumbnail in the session (Note: this is bad, it will take a lot of memory, but this is just a demo)
            ms = new MemoryStream();
            final_image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms;
        }

        /// <summary> 
        /// 生成指定图片的缩略图。 
        /// </summary> 
        /// <param name="originalImage">指定的图片。</param> 
        /// <param name="width">缩略图宽度。</param> 
        /// <param name="height">缩略图高度。</param> 
        /// <param name="mode">生成缩略图的方式。</param>     
        /// <returns>返回缩略图。</returns>
        public static Image MakeThumbnail(Image originalImage, int width, int height, ThumbnailMode mode)
        {
            int towidth = width;
            int toheight = height;
            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;
            switch (mode)
            {
                case ThumbnailMode.ByHeightAndWidth://指定高宽缩放（可能变形）                 
                    break;
                case ThumbnailMode.ByWidth://指定宽，高按比例                     
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case ThumbnailMode.ByHeight://指定高，宽按比例 
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case ThumbnailMode.CutByWidthAndHeight://指定高宽裁减（不变形）                 
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }
            //新建一个bmp图片 
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //新建一个画板 
            Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法 
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度 
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充 
            g.Clear(Color.White);

            //在指定位置并且按指定大小绘制原图片的指定部分 
            g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight),
               new Rectangle(x, y, ow, oh),
                GraphicsUnit.Pixel);
            return bitmap;
        }


        /// <summary> 
        /// 生成缩略图 
        /// </summary> 
        /// <param name="originalImagePath">源图路径（物理路径）</param> 
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param> 
        /// <param name="width">缩略图宽度</param> 
        /// <param name="height">缩略图高度</param> 
        /// <param name="mode">生成缩略图的方式</param>     
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, string mode)
        {
            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(originalImagePath);

            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (mode)
            {
                case "HW"://指定高宽缩放（可能变形）                 
                    break;
                case "W"://指定宽，高按比例                     
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H"://指定高，宽按比例 
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut"://指定高宽裁减（不变形）                 
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片 
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //新建一个画板 

            Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法 
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度 
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充 

            g.Clear(Color.White);

            //在指定位置并且按指定大小绘制原图片的指定部分 
            g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight),
                new Rectangle(x, y, ow, oh),
                GraphicsUnit.Pixel);

            try
            {
                //以jpg格式保存缩略图 

                bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }
        private static Size NewSize(int maxWidth, int maxHeight, int width, int height)
        {
            double w = 0.0;
            double h = 0.0;
            double sw = Convert.ToDouble(width);
            double sh = Convert.ToDouble(height); double mw = Convert.ToDouble(maxWidth);
            double mh = Convert.ToDouble(maxHeight); if (sw < mw && sh < mh)
            {
                w = sw;
                h = sh;
            }
            else if ((sw / sh) > (mw / mh))
            {
                w = maxWidth;
                h = (w * sh) / sw;
            }
            else
            {
                h = maxHeight;
                w = (h * sw) / sh;
            }
            return new Size(Convert.ToInt32(w), Convert.ToInt32(h));
        }

        public static void GenerateSmallImage(string oldFileName, string newFileName, int maxHeight, int maxWidth)
        {
            System.Drawing.Image img = System.Drawing.Image.FromFile(oldFileName);
            System.Drawing.Imaging.ImageFormat
            thisFormat = img.RawFormat;
            Size newSize = NewSize(maxWidth, maxHeight, img.Width, img.Height);
            Bitmap outBmp = new Bitmap(newSize.Width, newSize.Height);
            Graphics g = Graphics.FromImage(outBmp);
            // 设置画布的描绘质量  
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(img, new Rectangle(0, 0, newSize.Width, newSize.Height),
            0, 0, img.Width, img.Height, GraphicsUnit.Pixel);
            g.Dispose();
            // 以下代码为保存图片时,设置压缩质量  
            EncoderParameters encoderParams = new EncoderParameters();
            long[] quality = new long[1];
            quality[0] = 100;
            EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            encoderParams.Param[0] = encoderParam;
            //获得包含有关内置图像编码解码器的信息的ImageCodecInfo 对象.  
            ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo jpegICI = null;
            for (int x = 0; x < arrayICI.Length; x++)
            {
                if (arrayICI[x].FormatDescription.Equals("JPEG"))
                {
                    jpegICI = arrayICI[x];
                    //设置JPEG编码  
                    break;
                }
            }
            if (jpegICI != null)
            {
                outBmp.Save(newFileName, jpegICI, encoderParams);
            }
            else
            {
                outBmp.Save(newFileName,
                thisFormat);
            }
            img.Dispose();
            outBmp.Dispose();
        }
    }
}