using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace CheckINN.WebApi.Formatters
{
    public class SingleBitmapFormatter : BufferedMediaTypeFormatter
    {
        private readonly ILog _log;

        public SingleBitmapFormatter(ILog log) : this()
        {
            _log = log;
        }

        public override void WriteToStream(Type type, object value, Stream writeStream, HttpContent content)
        {
            var bitmap = value as Bitmap;
            bitmap?.Save(writeStream, ImageFormat.Bmp);
        }

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

        private SingleBitmapFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("image/bmp"));
        }

        public override bool CanReadType(Type type)
        {
            return true;
        }

        public override bool CanWriteType(Type type)
        {
            return type == typeof(Bitmap);
        }
    }
}