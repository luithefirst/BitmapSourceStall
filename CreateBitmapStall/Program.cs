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
        class DataNode
        {
            public string Name;
            public List<DataNode> SubNodes;
        }

        static DataNode CreateDataTree(int level, int count, int maxDepth)
        {
            var subNodes = new List<DataNode>(count);

            // resursively generated nodes until maxDepth is reached
            if (level < maxDepth)
                for (int i = 0; i < count; i++)
                    subNodes.Add(CreateDataTree(level + 1, count, maxDepth));
            
            return new DataNode()
            {
                Name = new string('*', 5),
                SubNodes = subNodes
            };
        }

        static void CreateBitmapSources()
        {
            const int size = 64;

            for (int i = 0; i < 100; i++)
            {
                var sw = new Stopwatch();
                sw.Start();

                var bitData = new byte[size * size * 4];
                var bmp = BitmapSource.Create(size, size, 96, 96,
                    System.Windows.Media.PixelFormats.Bgra32, null, bitData, size * 4);

                Console.WriteLine("CreateBitmapSource: {0}ms", sw.ElapsedMilliseconds);
            }
        }

        [STAThread]
        static void Main(string[] args)
        {
            // show file dialog, necessary to cause stalls
            var fdlg = new Microsoft.Win32.OpenFileDialog();
            fdlg.ShowDialog();
            
            var appData = new HashSet<object>();

            // create lots of data and bitmap sources
            while (true)
            {
                Console.WriteLine("creating data...");
                appData.Add(CreateDataTree(0, 2, 21));

                // create bitmap sources
                CreateBitmapSources();
            }
        }
    }
}
