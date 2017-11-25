using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using CheckINN.Image.Extensions;
using Tesseract;

namespace CheckINN.Image
{
    public class ImgProcessor
    {
        public Task<string> ImgToTxt(Bitmap img)
        {
            return Task.Run(() => {
                TesseractEngine engine = null;
                try
                {
                    using (engine = new TesseractEngine(@"./tessdata", "lit", EngineMode.TesseractOnly))
                    {
                        using (var receipt = engine.Process(img.Transform()))
                        {
                            return receipt.GetText();
                        }
                    }
                }
                catch (System.Exception)
                {
                    engine?.Dispose();
                    throw;
                }
            });
        }
    }
}
