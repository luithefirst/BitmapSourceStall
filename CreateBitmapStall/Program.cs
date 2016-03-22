using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace CreateBitmapStall
{
    class Program
    {
        class GarbageNode
        {
            public string Name;
            public List<GarbageNode> SubNodes;
        }

        static GarbageNode CreateGarbageNode(int level, int count, int maxDepth)
        {
            var subNodes = new List<GarbageNode>(count);

            // resursively generated nodes until maxDepth is reached
            if (level < maxDepth)
                for (int i = 0; i < count; i++)
                    subNodes.Add(CreateGarbageNode(level + 1, count, maxDepth));
            
            return new GarbageNode()
            {
                Name = new string('*', 5),
                SubNodes = subNodes
            };
        }

        static void CreateBitmapSources()
        {
            for (int i = 0; i < 100; i++)
            {
                var sw = new Stopwatch();
                sw.Start();

                var bitData = new byte[64 * 64 * 4];
                var bmp = BitmapSource.Create(
                    (int)64, (int)64, 96, 96,
                    System.Windows.Media.PixelFormats.Bgra32, null, bitData, 64 * 4);

                Console.WriteLine("CreateBitmapSource: {0}ms", sw.ElapsedMilliseconds);
            }
        }

        [STAThread]
        static void Main(string[] args)
        {
            //// show file dialog
            var fdlg = new Microsoft.Win32.OpenFileDialog();
            fdlg.ShowDialog();
            
            var garbage = new HashSet<object>();

            while (true)
            {
                // create garbage
                Console.WriteLine("creating garbage...");
                var g = CreateGarbageNode(0, 2, 21);
                garbage.Add(g);

                // create bitmaps sources
                CreateBitmapSources();
            }
        }
    }
}
