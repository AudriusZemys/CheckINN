using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using log4net;

namespace CheckINN.WebApi.Formatters
{
    /// <summary>
    /// Reads http requests with image/bmp content-type and formats them for controllers to use.
    /// In layman's terms: turns http post content into a bitmap and turns a bitmap into http post content.
    /// </summary>
    public class SingleBitmapFormatter : BufferedMediaTypeFormatter
    {
        private readonly ILog _log;

        public SingleBitmapFormatter(ILog log) : this()
        {
            _log = log;
        }

        /// <summary>
        /// Turns bitmap into POST content
        /// </summary>
        /// <param name="type">The type it receives, for this class it's always Bitmap anyway, so it's unused</param>
        /// <param name="value">The object that comes from the controller, for this class it's always Bitmap, so it's just cast</param>
        /// <param name="writeStream">This is the stream of the POST request body, that's where data should end up</param>
        /// <param name="content">All other HTTP data we don't really need here</param>
        public override void WriteToStream(Type type, object value, Stream writeStream, HttpContent content)
        {
            var bitmap = value as Bitmap;
            bitmap?.Save(writeStream, ImageFormat.Bmp);
        }

        /// <summary>
        /// Turns a POST content into a bitmap
        /// </summary>
        /// <param name="type">The type that the controller expects to read</param>
        /// <param name="readStream">Where the raw data is at</param>
        /// <param name="content">Other HTTP data we don't really need here</param>
        /// <param name="formatterLogger">A log instance to log errors to</param>
        /// <returns></returns>
        public override object ReadFromStream(Type type, Stream readStream, HttpContent content,
            IFormatterLogger formatterLogger)
        {
            Image image = null;
            try
            {
                image = Image.FromStream(readStream, true, false);
            }
            catch (Exception ex)
            {
                _log.Debug(ex);
            }
            var bitmap = image as Bitmap;
            return bitmap;
        }


        /// <summary>
        /// Registers this formatter as supporting this content type
        /// </summary>
        private SingleBitmapFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("image/bmp"));
        }

        /// <summary>
        /// Checks if this formatter can format this request.
        /// Specifically, receive it.
        /// Always true, because the content negotiator will only send bmp images,
        /// because of the image/bmp header.
        /// </summary>
        /// <param name="type">The type of object the controller is trying to receive</param>
        /// <returns>Can this class format that type or not</returns>
        public override bool CanReadType(Type type)
        {
            return true;
        }

        /// <summary>
        /// Checks if this formatter can format this request.
        /// Specifically, send it off.
        /// </summary>
        /// <param name="type">The type of object the controller is trying to send</param>
        /// <returns>Can this class format that type or not</returns>
        public override bool CanWriteType(Type type)
        {
            return type == typeof(Bitmap);
        }
    }
}