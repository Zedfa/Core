using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;
namespace Core.Cmn
{
    /// <summary>
    /// Factory class to create a periodic Task to simulate a <see cref="System.Threading.Timer"/> using <see cref="Task">Tasks.</see>
    /// </summary>
    public class PeriodicTaskFactory
    {
        Action<PeriodicTaskFactory> _action;
        public Action<PeriodicTaskFactory> Action
        {
            get { return _action; }
            set { _action = value; }
        }

        TimeSpan _intervalTime;
        public TimeSpan IntervalTime
        {
            get { return _intervalTime; }
            set { _intervalTime = value; }
        }

        TimeSpan _delayTime;
        public TimeSpan DelayTime
        {
            get { return _delayTime; }
            set { _delayTime = value; }
        }

        TimeSpan _duration;
        public TimeSpan Duration
        {
            get { return _duration; }
            set { _duration = value; }
        }

        int _maxIterations;
        public int MaxIterations
        {
            get { return _maxIterations; }
            set { _maxIterations = value; }
        }

        bool _synchronous;

        public bool Synchronous
        {
            get { return _synchronous; }
            set { _synchronous = value; }
        }

        CancellationToken _cancelToken;

        public CancellationToken CancelToken
        {
            get { return _cancelToken; }
            set { _cancelToken = value; }
        }

        TaskCreationOptions _periodicTaskCreationOptions;
        public TaskCreationOptions PeriodicTaskCreationOptions
        {
            get { return _periodicTaskCreationOptions; }
            set { _periodicTaskCreationOptions = value; }
        }

        public PeriodicTaskFactory(Action<PeriodicTaskFactory> action,
                                        TimeSpan? intervalTime = null,
                                        TimeSpan? delayTime = null,
                                        TimeSpan? durationTime = null,
                                        int maxIterations = -1,
                                        bool synchronous = false,
                                        CancellationToken cancelToken = new CancellationToken(),
                                        TaskCreationOptions periodicTaskCreationOptions = TaskCreationOptions.None)
        {
            _action = action;
            if (!intervalTime.HasValue)
                _intervalTime = TimeSpan.MaxValue;
            else
                _intervalTime = intervalTime.Value;

            if (!delayTime.HasValue)
                _delayTime = TimeSpan.MinValue;
            else
                _delayTime = delayTime.Value;

            if (!durationTime.HasValue)
                _duration = TimeSpan.MaxValue;
            else
                _duration = durationTime.Value;

            _maxIterations = maxIterations;
            _synchronous = synchronous;
            _cancelToken = cancelToken;
            _periodicTaskCreationOptions = periodicTaskCreationOptions;
        }

        /// <summary>
        /// Starts the periodic task.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="intervalInMilliseconds">The interval in milliseconds.</param>
        /// <param name="delayInMilliseconds">The delay in milliseconds, i.e. how long it waits to kick off the timer.</param>
        /// <param name="duration">The duration.
        /// <example>If the duration is set to 10 seconds, the maximum time this task is allowed to run is 10 seconds.</example></param>
        /// <param name="maxIterations">The max iterations.</param>
        /// <param name="synchronous">if set to <c>true</c> executes each period in a blocking fashion and each periodic execution of the task
        /// is included in the total duration of the Task.</param>
        /// <param name="cancelToken">The cancel token.</param>
        /// <param name="periodicTaskCreationOptions"><see cref="TaskCreationOptions"/> used to create the task for executing the <see cref="Action"/>.</param>
        /// <returns>A <see cref="Task"/></returns>
        /// <remarks>
        /// Exceptions that occur in the <paramref name="action"/> need to be handled in the action itself. These exceptions will not be 
        /// bubbled up to the periodic task.
        /// </remarks>
        public Task Start()
        {
            Stopwatch stopWatch = new Stopwatch();
            Action wrapperAction = () =>
            {
                CheckIfCancelled(_cancelToken);
                _action(this);
            };

            Action mainAction = () =>
            {
                MainPeriodicTaskAction(wrapperAction, stopWatch);
            };

            return Task.Factory.StartNew(mainAction, _cancelToken, TaskCreationOptions.LongRunning, TaskScheduler.Current);
        }

        /// <summary>
        /// Mains the periodic task action.
        /// </summary>
        /// <param name="_intervalInMilliseconds">The interval in milliseconds.</param>
        /// <param name="_delayInMilliseconds">The delay in milliseconds.</param>
        /// <param name="_duration">The duration.</param>
        /// <param name="_maxIterations">The max iterations.</param>
        /// <param name="_cancelToken">The cancel token.</param>
        /// <param name="_stopWatch">The stop watch.</param>
        /// <param name="_synchronous">if set to <c>true</c> executes each period in a blocking fashion and each periodic execution of the task
        /// is included in the total duration of the Task.</param>
        /// <param name="_wrapperAction">The wrapper action.</param>
        /// <param name="_periodicTaskCreationOptions"><see cref="TaskCreationOptions"/> used to create a sub task for executing the <see cref="Action"/>.</param>
        private void MainPeriodicTaskAction(Action _wrapperAction, Stopwatch _stopWatch)
        {
            TaskCreationOptions subTaskCreationOptions = TaskCreationOptions.AttachedToParent | _periodicTaskCreationOptions;

            CheckIfCancelled(_cancelToken);

            if (_delayTime > TimeSpan.MinValue)
            {
                Task.Delay(_delayTime);
            }

            if (_maxIterations == 0) { return; }

            int iteration = 0;

            ////////////////////////////////////////////////////////////////////////////
            // using a ManualResetEventSlim as it is more efficient in small intervals.
            // In the case where longer intervals are used, it will automatically use 
            // a standard WaitHandle....
            // see http://msdn.microsoft.com/en-us/library/vstudio/5hbefs30(v=vs.100).aspx
            using (ManualResetEventSlim periodResetEvent = new ManualResetEventSlim(false))
            {
                ////////////////////////////////////////////////////////////
                // Main periodic logic. Basically loop through this block
                // executing the action
                while (true)
                {
                    CheckIfCancelled(_cancelToken);

                    Task subTask = Task.Factory.StartNew(_wrapperAction, _cancelToken, subTaskCreationOptions, TaskScheduler.Current);

                    if (_synchronous)
                    {
                        _stopWatch.Start();
                        try
                        {
                            subTask.Wait(_cancelToken);
                        }
                        catch { /* do not let an errant subtask to kill the periodic task...*/ }
                        _stopWatch.Stop();
                    }

                    // use the same Timeout setting as the System.Threading.Timer, infinite timeout will execute only one iteration.
                    if (_intervalTime == TimeSpan.MaxValue) { break; }

                    iteration++;

                    if (_maxIterations > 0 && iteration >= _maxIterations) { break; }

                    try
                    {
                        _stopWatch.Start();
                        periodResetEvent.Wait(_intervalTime, _cancelToken);
                        _stopWatch.Stop();
                    }
                    finally
                    {
                        periodResetEvent.Reset();
                    }

                    CheckIfCancelled(_cancelToken);

                    if (_duration > TimeSpan.MinValue && _stopWatch.ElapsedTicks >= _duration.Ticks)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Checks if cancelled.
        /// </summary>
        /// <param name="cancelToken">The cancel token.</param>
        private static void CheckIfCancelled(CancellationToken cancellationToken)
        {
            if (cancellationToken == null)
                throw new ArgumentNullException("cancellationToken");

            cancellationToken.ThrowIfCancellationRequested();
        }
    }
}
