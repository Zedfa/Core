using Core.Cmn.Monitoring;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Core.UnitTesting.Cmn
{
    [TestClass]
    public class Watch_tests
    {
        [TestMethod]
        public void stopWatch_test()
        {
            System.Diagnostics.Debug.WriteLine("start");
            using (var w = new Watch())
            {
                w.OnWatchStop += W_OnWatchStop;
                System.Diagnostics.Debug.WriteLine(w.Stopwatch.ElapsedMilliseconds);
                Task.Delay(100).Wait();
            }
            var ww = new WatchElapsed();
            using (new Watch(out ww))
            {
               // Task.Delay(1).Wait();
            }

            ww.ElapsedMilliseconds++;
        }

        private void W_OnWatchStop(object sender, WatchElapsed e)
        {
            System.Diagnostics.Debug.WriteLine(e.ElapsedMilliseconds);
        }
    }
}