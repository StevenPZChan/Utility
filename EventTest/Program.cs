using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventTest
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 10; i++)
            {
                Camera.ImageReceived += OneShot;
            }
            Camera.SnapShot();
            Console.ReadKey();
        }
        private static void OneShot(object sender, string e)
        {
            //Console.WriteLine(Camera.count.ToString());
            Console.WriteLine("----");
        }
    }

    public static class Camera
    {
        public static event EventHandler<string> ImageReceived;
        public static int count = 0;
        private static EventHandler<string> OnImageReceived = RaisedImageReceived;
        public static void SnapShot()
        {
            OnImageReceived.BeginInvoke(null, count++.ToString(), callback, count);
            //ImageReceived?.BeginInvoke(null, count++.ToString(), callback, count);
            ImageReceived?.Invoke(null, count++.ToString());
            Console.WriteLine("End.");
        }

        private static void callback(IAsyncResult ar)
        {
            Console.WriteLine("{0}\t{1}\t{2}", ar.AsyncState.ToString(), ar.CompletedSynchronously.ToString(),
                ar.IsCompleted.ToString());
        }

        private static void RaisedImageReceived(object sender, string e)
        {
            ImageReceived.Invoke(sender, e);
        }
    }
}
