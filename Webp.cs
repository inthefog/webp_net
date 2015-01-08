using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace inthefog.Webp
{
    /// <summary>
    /// Layer above unmanaged libwebp.dll
    /// </summary>
    public class Webp
    {
        /// <summary>
        /// Retrieve basic header information: width, height.
        /// This function will also validate the header and return 0 in case of formatting error.
        /// </summary>
        /// <param name="data">Pointer to WebP image data</param>
        /// <param name="data_size">This is the size of the memory block pointed to by data containing the image data</param>
        /// <param name="width">The range is limited currently from 1 to 16383</param>
        /// <param name="height">The range is limited currently from 1 to 16383</param>
        /// <returns>0 or error code in case of formatting error</returns>
        [DllImport("libwebp.dll")]
        private static extern int WebPGetInfo(byte[] data, int data_size, out int width, out int height);

        /// <summary>
        /// Decode the image directly into a pre-allocated buffer output_buffer.
        /// If this storage is not sufficient (or an error occurred), NULL is returned.
        /// Otherwise, output_buffer is returned, for convenience.
        /// </summary>
        /// <param name="data">Pointer to WebP image data</param>
        /// <param name="data_size">Size of the memory block pointed to by data containing the image data</param>
        /// <param name="output_buffer">Pre-allocated buffer output_buffer</param>
        /// <param name="output_buffer_size">The maximum storage available in this buffer. Expected to be at least output_stride * height</param>
        /// <param name="output_stride">Specifies the distance (in bytes) between scanlines</param>
        /// <returns>Pointer to output_buffer if succeeded, NULL otherwise</returns>
        [DllImport("libwebp.dll")]
        private static extern IntPtr WebPDecodeBGRAInto(byte[] data, int data_size, IntPtr output_buffer, int output_buffer_size, int output_stride);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Bitmap Decode(byte[] data)
        {
            try
            {
                //Validate formatting and determine size
                int width, height;
                if (WebPGetInfo(data, data.Length, out width, out height) == 0)
                    throw new Exception("Formatting error!");

                //Allocate output image
                Bitmap bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);

                //Lock for writing
                BitmapData bmpData = null;
                try
                {
                    bmpData = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

                    //Decode
                    return (WebPDecodeBGRAInto(data, data.Length, bmpData.Scan0, bmpData.Stride * bmpData.Height, bmpData.Stride) != bmpData.Scan0) ? null : bmp;
                }
                finally
                {
                    if (bmp != null && bmpData != null)
                        bmp.UnlockBits(bmpData);
                }
            }
            catch (Exception E)
            {
                return null;
            }
        }
    }
}
