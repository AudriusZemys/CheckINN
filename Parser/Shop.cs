using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Parsing
{
    class Shop
    {
        public bool ShopName(string path, string pattern = "UAB")
        {
            //path = @"C:\Users\Audrius\checkText.txt"
            IEnumerable<string> result = File.ReadLines(path)
                                             .Where(l => l.Contains(pattern));
            if (result != null)
            {
                return true;
            }
            return false;
        }
    }
}
