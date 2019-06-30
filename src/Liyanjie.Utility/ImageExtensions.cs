#if NET45 || NETSTANDARD2_0
using System;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace System.Drawing
{
    /// <summary>
    /// 
    /// </summary>
    public static class ImageExtensions
    {
        /// <summary>
        /// 将图片转码为base64字符串
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static string Encode(this Image image)
        {
            using (var stream = new MemoryStream())
            {
                image.Save(stream, image.RawFormat);
                var bytes = new byte[stream.Length];
                stream.Position = 0;
                stream.Read(bytes, 0, (int)stream.Length);
                return Convert.ToBase64String(bytes);
            }
        }

        /// <summary>
        /// 在图片上绘制字符。
        /// 默认字体包含："Verdana", "Tahoma", "Arial", "Helvetica Neue", "Helvetica", "Sans - Serif"。
        /// 实际包含字体以参数指定及程序所在服务器安装字体及以上字体共有列表为准。
        /// </summary>
        /// <param name="image">底图</param>
        /// <param name="characters">字符</param>
        /// <param name="fontSize">字号</param>
        /// <param name="fontFamilies">字体</param>
        /// <returns></returns>
        public static void DrawCharacters(this Image image, string characters, int fontSize, params string[] fontFamilies)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));
            if (characters == null)
                throw new ArgumentNullException(nameof(characters));
            if (fontSize < 1)
                throw new ArgumentException($"参数{nameof(fontSize)}必须大于0", nameof(characters));

            if (fontFamilies == null || fontFamilies.Length == 0)
                fontFamilies = new string[] { "Verdana", "Tahoma", "Arial", "Helvetica Neue", "Helvetica", "Sans - Serif" };

            var charWidth = (float)(image.Width - 10) / characters.Length;//字符所占宽度
            var fontStyles = new[] { FontStyle.Bold, FontStyle.Italic, FontStyle.Regular };

            using (var graphics = GetGraphics(image))
            {
                graphics.Clear(GetRandomBackgroudColor());//设置图片背景

                var random = new Random();

                //绘制干扰线
                for (int i = 0; i < 64; i++)
                {
                    var x = random.Next(image.Width);
                    var y = random.Next(image.Height);
                    using (var pen = new Pen(GetRandomBackgroudColor()))
                    {
                        graphics.DrawLine(pen, x, y, x + random.Next(30) * (random.Next(3) - 1), y + random.Next(30) * (random.Next(3) - 1));
                    }
                }
                //绘制字符
                for (int i = 0; i < characters.Length; i++)
                {
                    var @char = characters[i];
                    using (var pen = new Pen(GetRandomFontColor()))
                    using (var font = new Font(fontFamilies[random.Next(fontFamilies.Length)], fontSize, fontStyles[random.Next(fontStyles.Length)]))
                    {
                        graphics.DrawString(@char.ToString(), font, pen.Brush, (5 + charWidth * i), random.Next(2, image.Height - fontSize - 2));
                    }
                }
            }
        }

        /// <summary>
        /// 绘制水印
        /// </summary>
        /// <param name="image"></param>
        /// <param name="watermark">水印图片</param>
        /// <param name="width">水印宽度</param>
        /// <param name="height">水印高度</param>
        /// <param name="startX">水印左上角水平位置</param>
        /// <param name="startY">水印左上角垂直位置</param>
        /// <param name="opacity">透明度，0~1</param>
        /// <returns></returns>
        public static void DrawWatermark(this Image image, Image watermark, int startX, int startY, int width, int height, float opacity = 0.5F)
        {
            DrawWatermark(image, watermark, new Point(startX, startY), new Size(width, height), opacity);
        }

        /// <summary>
        /// 绘制水印
        /// </summary>
        /// <param name="image"></param>
        /// <param name="watermark">水印图片</param>
        /// <param name="start">水印左上角坐标</param>
        /// <param name="size">水印大小</param>
        /// <param name="opacity">透明度，0~1</param>
        /// <returns></returns>
        public static void DrawWatermark(this Image image, Image watermark, Point start, Size size, float opacity = 0.5F)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));

            var newColorMatrix = new[]
            {
                new float[] { 1, 0, 0, 0, 0 },
                new float[] { 0, 1, 0, 0, 0 },
                new float[] { 0, 0, 1, 0, 0 },
                new float[] { 0, 0, 0, opacity > 1 ? 1 : opacity < 0 ? 0 : opacity, 0 },
                new float[] { 0, 0, 0, 0, 1 }
            };
            var colorMatrix = new ColorMatrix(newColorMatrix);
            var imageAttributes = new ImageAttributes();
            imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            using (var graphics = GetGraphics(image))
            {
                graphics.DrawImage(watermark, new Rectangle(start, size), 0, 0, watermark.Width, watermark.Height, GraphicsUnit.Pixel, imageAttributes);
            }
        }

        /// <summary>
        /// 清除整个图像并以指定颜色填充
        /// </summary>
        /// <param name="image"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static void Clear(this Image image, Color color)
        {
            using (var graphics = GetGraphics(image))
            {
                graphics.Clear(color);
            }
        }

        /// <summary>
        /// 裁剪图片
        /// </summary>
        /// <param name="image">源图片</param>
        /// <param name="start">裁剪开始坐标</param>
        /// <param name="end">裁剪结束坐标</param>
        /// <returns></returns>
        public static Image Crop(this Image image, Point start, Point end)
        {
            return Crop(image, start.X, start.Y, end.X - start.X, end.Y - start.Y);
        }

        /// <summary>
        /// 裁剪图片
        /// </summary>
        /// <param name="image">源图片</param>
        /// <param name="start">裁剪开始坐标</param>
        /// <param name="size">裁剪尺寸</param>
        /// <returns></returns>
        public static Image Crop(this Image image, Point start, Size size)
        {
            return Crop(image, start.X, start.Y, size.Width, size.Height);
        }

        /// <summary>
        /// 裁剪图片
        /// </summary>
        /// <param name="image">源图片</param>
        /// <param name="start">裁剪开始坐标</param>
        /// <param name="width">裁剪宽度</param>
        /// <param name="height">裁剪高度</param>
        /// <returns></returns>
        public static Image Crop(this Image image, Point start, int width, int height)
        {
            return Crop(image, start.X, start.Y, width, height);
        }

        /// <summary>
        /// 裁剪图片
        /// </summary>
        /// <param name="image">源图片</param>
        /// <param name="startX">裁剪开始 X 坐标</param>
        /// <param name="startY">裁剪开始 Y 坐标</param>
        /// <param name="width">裁剪宽度</param>
        /// <param name="height">裁剪高度</param>
        /// <returns></returns>
        public static Image Crop(this Image image, int startX, int startY, int width, int height)
        {
            if (startX >= image.Width || startY >= image.Height || width <= 0 || height <= 0)
                return image;

            if (startX < 0)
                startX = 0;
            if (startY < 0)
                startY = 0;
            if (width > image.Width - startX)
                width = image.Width - startX;
            if (height > image.Height - startY)
                height = image.Height - startY;

            return Crop(image, new Rectangle(startX, startY, width, height));
        }

        /// <summary>
        /// 裁剪图片
        /// </summary>
        /// <param name="image"></param>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        public static Image Crop(this Image image, Rectangle rectangle)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));

            return new Bitmap(image).Clone(rectangle, image.PixelFormat);
        }

        /// <summary>
        /// 调整大小
        /// </summary>
        /// <param name="image"></param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        /// <param name="zoom">等比缩放</param>
        /// <returns></returns>
        public static Image Resize(this Image image, int? width, int? height, bool zoom = true)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));

            width = width == 0 ? null : width;
            height = height == 0 ? null : height;

            if (width == null && height == null)
                return image;

            int
                w = image.Width,
                h = image.Height;

            if (width.HasValue && height.HasValue)
            {
                double
                    wW = (double)image.Width / width.Value,
                    hH = (double)image.Height / height.Value;
                if (zoom)
                {
                    if (wW > hH)
                    {
                        w = width.Value;
                        h = (int)(image.Height / wW);
                    }
                    else
                    {
                        w = (int)(image.Width / hH);
                        h = height.Value;
                    }
                }
                else
                {
                    w = width.Value;
                    h = height.Value;
                }
            }
            else if (width.HasValue)
            {
                w = width.Value;
                h = zoom ? (int)(image.Height / ((double)image.Width / width.Value)) : image.Height;
            }
            else if (height.HasValue)
            {
                w = zoom ? (int)(image.Width / ((double)image.Height / height.Value)) : image.Width;
                h = height.Value;
            }

            return image.GetThumbnailImage(w, h, () => false, IntPtr.Zero);
        }

        /// <summary>
        /// 组合多张图片
        /// </summary>
        /// <param name="image">底图</param>
        /// <param name="images">图片集合</param>
        /// <returns></returns>
        public static void Combine(this Image image, params (Point Point, Size Size, Image Image, bool Zoom)[] images)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));
            if (images == null)
                throw new ArgumentNullException(nameof(images));

            if (images.Length == 0)
                return;

            using (var graphics = GetGraphics(image))
            {
                foreach (var item in images)
                {
                    var tmp = item.Zoom
                        ? Resize(item.Image, item.Size.Width, item.Size.Height, item.Zoom)
                        : item.Image;
                    graphics.DrawImage(tmp, item.Point);
                }
            }
        }

        /// <summary>
        /// 拼接图片
        /// </summary>
        /// <param name="image1"></param>
        /// <param name="image2"></param>
        /// <param name="direction">true=水平方向，false=垂直方向</param>
        /// <returns></returns>
        public static Image Concat(this Image image1, Image image2, bool direction = false)
        {
            if (image1 == null)
                throw new ArgumentNullException(nameof(image1));
            if (image2 == null)
                return image1;

            var width = direction ? image1.Width + image2.Width : Math.Max(image1.Width, image2.Width);

            if (direction)
            {
                var image = new Bitmap(image1.Width + image2.Width, Math.Max(image1.Height, image2.Height));
                using (var graphics = GetGraphics(image))
                {
                    graphics.DrawImage(image1, 0, 0);
                    graphics.DrawImage(image2, image.Width, 0);
                }

                return image;
            }
            else
            {
                var image = new Bitmap(Math.Max(image1.Width, image2.Width), image1.Height + image2.Height);
                using (var graphics = GetGraphics(image))
                {
                    graphics.DrawImage(image1, 0, 0);
                    graphics.DrawImage(image2, 0, image.Height);
                }

                return image;
            }
        }

        /// <summary>
        /// 压缩存储
        /// </summary>
        /// <param name="image"></param>
        /// <param name="path"></param>
        /// <param name="quality">质量，0~100</param>
        public static void CompressSave(this Image image, string path, long quality)
        {
            if (quality < 0)
                quality = 0;
            if (quality > 100)
                quality = 100;

            var imageCodecInfo = ImageCodecInfo.GetImageEncoders().FirstOrDefault(_ => _.FormatID == GetFormat(Path.GetExtension(path)).Guid);
            if (imageCodecInfo != null)
                image.Save(path, imageCodecInfo, new EncoderParameters
                {
                    Param = new[] { new EncoderParameter(Encoder.Quality, quality) }
                });
            else
                image.Save(path, image.RawFormat);
        }

        static Graphics GetGraphics(Image image)
        {
            var graphics = Graphics.FromImage(image);
            graphics.InterpolationMode = InterpolationMode.High;//设置高质量插值法
            graphics.SmoothingMode = SmoothingMode.HighQuality;//设置高质量,低速度呈现平滑程度
            return graphics;
        }

        /// <summary>
        /// 随机的背景色
        /// </summary>
        static Color GetRandomBackgroudColor()
        {
            var random = new Random();
            return Color.FromArgb(random.Next(150) + 100, random.Next(150) + 100, random.Next(150) + 100);
        }

        /// <summary>
        /// 随机的字体色
        /// </summary>
        static Color GetRandomFontColor()
        {
            var random = new Random();
            return Color.FromArgb(random.Next(150), random.Next(150), random.Next(150));
        }

        static ImageFormat GetFormat(string extension)
        {
            switch (extension.ToLower())
            {
                case ".bmp":
                    return ImageFormat.Bmp;
                case ".emf":
                    return ImageFormat.Emf;
                case ".gif":
                    return ImageFormat.Gif;
                case ".ico":
                case ".icon":
                    return ImageFormat.Icon;
                case ".jpg":
                case ".jpeg":
                    return ImageFormat.Jpeg;
                case ".png":
                    return ImageFormat.Png;
                case ".tif":
                case ".tiff":
                    return ImageFormat.Tiff;
                case ".wmf":
                    return ImageFormat.Wmf;
                default:
                    return ImageFormat.Jpeg;
            }
        }
    }
}
#endif
