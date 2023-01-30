using Accord.Video.FFMPEG;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsciiVideo
{
    public static class AsciiVideoManager
    {
        public static void PlayVideo(string videofile)
        {
            (Bitmap[] LoadedFrames, VideoFileReader reader) = GetFrames(videofile);

            for (int i = 0; i < LoadedFrames.Length; i++)
            {
                int deltatime = WriteFrame(LoadedFrames[i]);
                System.Threading.Thread.Sleep(deltatime < 30 ? 30 - deltatime : 0);
            }
        }
        public static void PlayVideoLoop(string videofile)
        {
            (Bitmap[] LoadedFrames, VideoFileReader reader) = GetFrames(videofile);

            int i = 0;
            while(true)
            {
                int deltatime = WriteFrame(LoadedFrames[i]);
                System.Threading.Thread.Sleep(deltatime < 30 ? 30 - deltatime : 0);
                i = i < LoadedFrames.Length - 1 ? i + 1 : 0;
            }
        }
        static (Bitmap[], VideoFileReader) GetFrames(string videofile)
        {
            var reader = new VideoFileReader();
            reader.Open(videofile);

            int totalFrames = (int)reader.FrameCount;
            Bitmap[] bitmapArray = new Bitmap[totalFrames];

            for (int i = 0; i < totalFrames; i++)
            {
                Console.SetCursorPosition(0, 0);
                Console.Write('|');
                for(int j = 0; j < 20; j++)
                {
                    int index = (int)(((float)i / (float)totalFrames) * (float)20);
                    if(j < index)
                    {
                        Console.Write('█');
                    }
                    else
                    {
                        Console.Write(' ');
                    }
                }
                Console.Write('|');
                Console.WriteLine($" Rendering(x {Console.WindowWidth - 1}, y {Console.WindowHeight - 2})...");
                bitmapArray[i] = resizeFrame(reader.ReadVideoFrame(), new Size(Console.WindowWidth - 1, Console.WindowHeight - 2));
            }
            reader.Close();

            return (bitmapArray, reader);
        }
        static int WriteFrame(Bitmap frame)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Console.SetCursorPosition(0, 0);

            char[] chars = { '#', '#', '@', '%', '=', '+', '*', ':', '-', ' ', ' ' };

            StringBuilder sb;
            sb = new StringBuilder();

            for (int h = 0; h < frame.Height; h++)
            {
                for (int w = 0; w < frame.Width; w++)
                {
                    Color cl = ((Bitmap)frame).GetPixel(w, h);
                    int gray = (cl.R + cl.G + cl.B) / 3;
                    int index = (gray * (chars.Length - 1)) / 255;

                    sb.Append(chars[index]);
                }
                sb.Append('\n');
            }
            Console.WriteLine(sb.ToString());
            stopwatch.Stop();
            return (int)stopwatch.ElapsedMilliseconds;
        }
        static Bitmap resizeFrame(Bitmap frameToResize, Size size)
        {
            Bitmap b = new Bitmap(size.Width, size.Height);
            Graphics g = Graphics.FromImage((System.Drawing.Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(frameToResize, 0, 0, size.Width, size.Height);
            g.Dispose();
            return (Bitmap)b;
        }
    }
}
