using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsciiVideo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press any key to rendering...");
            Console.ReadKey(true);
            AsciiVideoManager.PlayVideoLoop("video4.mp4");
        }

    }
}
