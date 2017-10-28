using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cmn.Monitoring
{
    public class WatchElapsed : EventArgs
    {
        public long ElapsedMilliseconds { get; set; }
        public double TotalElapsedMilliseconds { get; set; }
    }
    public class Watch : IDisposable
    {
        public Stopwatch Stopwatch { get; protected set; }
        public event EventHandler<WatchElapsed> OnWatchStop;
        protected WatchElapsed WatchElapsed { get; set; }
        public Watch()
        {
            WatchElapsed = new Monitoring.WatchElapsed();
            Stopwatch = new Stopwatch();
            Stopwatch.Start();
        }
        public Watch(out WatchElapsed watchElapsed)
        {
            WatchElapsed = watchElapsed = new Monitoring.WatchElapsed();
            Stopwatch = new Stopwatch();
            Stopwatch.Start();
        }
        public void Dispose()
        {
            Stopwatch.Stop();
            WatchElapsed.ElapsedMilliseconds = Stopwatch.ElapsedMilliseconds;
            WatchElapsed.TotalElapsedMilliseconds = Stopwatch.Elapsed.TotalMilliseconds;
            OnWatchStop?.Invoke(this, WatchElapsed);
            OnWatchStop = null;
            Stopwatch = null;
        }
    }
}
