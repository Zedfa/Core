using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Cmn.Monitoring
{
    public class ResourceMonitoring
    {

        private Task CpuMonitorTask { get; set; }
        private CancellationTokenSource _cancellation;

        public ResourceMonitoring()
        {
            _cancellation = new CancellationTokenSource();
            CpuMonitorTask = new Task(() =>
            {
                while (!_cancellation.IsCancellationRequested)
                {
                    try
                    {

                        var task = GetCpuUsageIndex();
                        task.Wait();
                        CpuUsagePercentage = task.Result;

                    }
                    catch
                    { }

                }
            }, _cancellation.Token);

        }


        public void StartMonitoring()
        {
#if !DEBUG
            CpuMonitorTask.Start();
#endif
        }

        public void StopMonitoring()
        {
            _cancellation.Cancel();
        }

        public int CalcProcessorAffinityCount()
        {
            long affinity = (long)System.Diagnostics.Process.GetCurrentProcess().ProcessorAffinity;
            int processorCount = 0;
            long binaryAfinty = 1;
            for (int i = 1; i <= Environment.ProcessorCount; i++)
            {
                if ((affinity & binaryAfinty) == binaryAfinty)
                    processorCount++;
                binaryAfinty = binaryAfinty * 2;
            }

            return processorCount;
        }

        public double CpuUsagePercentage { get; private set; }

        private async Task<double> GetCpuUsageIndex()
        {
            var firstTime = System.Diagnostics.Process.GetCurrentProcess().TotalProcessorTime.TotalMilliseconds;
            await Task.Delay(1000, _cancellation.Token);
            var secondTime = System.Diagnostics.Process.GetCurrentProcess().TotalProcessorTime.TotalMilliseconds;
            var result = (secondTime - firstTime) / (CalcProcessorAffinityCount() * 10);
            return result;
        }
    }
}
