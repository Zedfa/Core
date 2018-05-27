using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Cmn.SharedMemory
{
    public class SharedMemoryBase<T> where T : class
    {
        private int _int32Size = Marshal.SizeOf(typeof(int));

        private static object sync = new object();

        private PeriodicTaskFactory _stateTimer;

        private static ConcurrentQueue<T> _entityListToWrite = new ConcurrentQueue<T>();
        private static Mutex _lockKeyData;
        private static Mutex LockKeyData
        {
            get

            {
                if (!Mutex.TryOpenExisting($"SM_Data_LOCK", out _lockKeyData))
                    _lockKeyData = new Mutex(false, $"SM_Data_LOCK");
                return _lockKeyData;
            }
            set { _lockKeyData = value; }
        }

        private string Name { get; set; }
        private long Size { get; set; }

        private MemoryMappedFile MemoryMappedFile { get; set; }
        private MemoryMappedViewAccessor Accessor { get; set; }



        private static long _pointer;
        private static long Pointer
        {
            get
            {
                lock (sync)
                {
                    return _pointer;
                }
            }
            set
            {
                lock (sync)
                {
                    _pointer = value;
                }
            }
        }


        long _lastPosition;
        private long LastPosition
        {

            get
            {
                lock (sync)
                {
                    Accessor.Read(0, out _lastPosition);
                    return _lastPosition;
                }
            }
            set
            {
                lock (sync)
                {
                    _lastPosition = value;
                    Accessor.Write(0, _lastPosition);
                }
            }
        }

        long _nextStart;
        private long NextStart
        {

            get
            {
                lock (sync)
                {
                    Accessor.Read(8, out _nextStart);
                    return _nextStart;
                }
            }
            set
            {
                lock (sync)
                {
                    if (value >= Size)
                    {
                        value = 0;
                    }
                    _nextStart = value;
                    Accessor.Write(8, _nextStart);
                }
            }
        }

        long _getterRoundCount;
        private long GetterRoundCount
        {

            get
            {
                lock (sync)
                {
                    Accessor.Read(16, out _getterRoundCount);
                    return _getterRoundCount;
                }
            }
            set
            {
                lock (sync)
                {
                    _getterRoundCount = value;
                    Accessor.Write(16, _getterRoundCount);
                }
            }
        }

        long _setterRoundCount;
        private long SetterRoundCount
        {

            get
            {
                lock (sync)
                {
                    Accessor.Read(24, out _setterRoundCount);
                    return _setterRoundCount;
                }
            }
            set
            {
                lock (sync)
                {
                    _setterRoundCount = value;
                    Accessor.Write(24, _setterRoundCount);
                }
            }
        }

        public SharedMemoryBase(string name, long size)
        {
            Name = name;
            Size = size;

        }


        public bool Open()
        {
            // Create lock

            var LockKey = new Mutex(true, $"SM_{Name}_LOCK");
            LockKey.WaitOne();
            try
            {
                // Create named MMF
                try
                {
                    MemoryMappedFile = MemoryMappedFile.OpenExisting(Name);

                }
                catch
                {
                    MemoryMappedFile = MemoryMappedFile.CreateOrOpen(Name, Size);
                }
                // Create accessors to MMF
                Accessor = MemoryMappedFile.CreateViewAccessor(0, Size,
                                     MemoryMappedFileAccess.ReadWrite);



                //create space for length of MMF
                if (LastPosition <= 0)
                {
                    LastPosition = 32;
                    NextStart = 0;
                }

                Pointer = 32;



            }
            catch
            {
                return false;
            }

            //LockKey.ReleaseMutex();
            LockKey.Close();
            return true;
        }

        public virtual void Close()
        {
            Accessor.Dispose();

            MemoryMappedFile.Dispose();

            LockKeyData.Dispose();
        }


        public List<T> Data
        {
            get
            {
                // var LockKey = new Mutex(true, $"SM_{Guid.NewGuid()}_LOCK");
                //LockKeyData.WaitOne();

                lock (sync)
                {
                    LockKeyData.WaitOne();

                    var _data = ReadSequences(Accessor);

                    LockKeyData.ReleaseMutex();
                    return _data;
                }

            }

            set
            {
                //var LockKey = new Mutex(true, $"SM_{Guid.NewGuid()}_LOCK");
                //  LockKey.WaitOne();
                // LockKeyData.WaitOne();

                lock (sync)
                {
                    WriteSequencesPeriodically(Accessor, value);
                }
                // LockKeyData.ReleaseMutex();
                // LockKey.Close();

            }
        }



        private List<T> ReadSequences(MemoryMappedViewAccessor accessor)
        {
            var entityList = new List<T>();

          
            // when rollover was accurred
            if (GetterRoundCount < SetterRoundCount)
            {
                if (SetterRoundCount - GetterRoundCount == 1)
                {
                    if (Pointer < NextStart)
                    {
                        Pointer = NextStart;
                    }

                    else if (Pointer > NextStart)
                    {
                        Pointer = 32;

                    }
                    GetterRoundCount = SetterRoundCount;

                }

                // writer wrote more than one round 
                else if (SetterRoundCount - GetterRoundCount > 1)
                {
                    Pointer = NextStart;

                    GetterRoundCount = SetterRoundCount = 0;


                }


            }
           //when rollover happend we need to read from next satrt point instead of pointer
            if (NextStart > 0 && GetterRoundCount == SetterRoundCount) {
                Pointer = NextStart;
            }

            while (Pointer < LastPosition)
            {
                entityList.Add(ReadSequence(accessor, Pointer));
            }

            return entityList;
        }

        public List<T> ReadAllSequences()
        {
            var traceList = new List<T>();

            for (long i = 32; i < LastPosition;)
            {
                traceList.Add(ReadSequence(Accessor, i));
                i = Pointer;
            }

            return traceList;
        }

        private T ReadSequence(MemoryMappedViewAccessor accessor, long from)
        {
            T seq;
            // Get size of trace
            int objectSize = accessor.ReadInt32(from);

            from += _int32Size;

            // Read from memory mapped file.
            byte[] data = new byte[objectSize];

            accessor.ReadArray<byte>(from, data, 0, objectSize);

            from += objectSize;

            seq = Core.Serialization.BinaryConverter.Deserialize<T>(data);
            Pointer = from;

            return seq;
        }

        private void WriteSequencesPeriodically(MemoryMappedViewAccessor accessor, List<T> sequences)
        {


            foreach (var sequence in sequences)
            {
                _entityListToWrite.Enqueue(sequence);
            }

            if (_stateTimer != null)
            {
                _stateTimer.CancelToken = new CancellationToken(true);
            }
            _stateTimer = new PeriodicTaskFactory((task) =>
                    {
                        lock (sync)
                        {
                            // var isOwnedMutex =  LockKeyData.WaitOne();
                            LockKeyData.WaitOne();
                            WriteSequences(accessor);
                            //  if (isOwnedMutex)
                            LockKeyData.ReleaseMutex();
                        }
                    },
                    new TimeSpan(0, 0, 2),
                    new TimeSpan(0, 0, 2));
            _stateTimer.Start();



        }
        private void WriteSequences(MemoryMappedViewAccessor accessor)
        {
            // LockKeyData.WaitOne();

            T sequence;
            while (_entityListToWrite.TryDequeue(out sequence))
            {
                lock (sync)
                {
                    LastPosition = WriteSequence(Accessor, sequence, LastPosition);
                }
            }
            //LockKeyData.Close();
        }

        private long WriteSequence(MemoryMappedViewAccessor accessor, T seq, long position)
        {

            byte[] data;

            int seqSize;

            if (seq != null)
            {
                data = Core.Serialization.BinaryConverter.Serialize(seq);

                // var b = typeof(T) == typeof(Core.Cmn.Trace.Source) && (seq as Core.Cmn.Trace.Source).Message.Any(c => "".Contains(c));

                seqSize = data.Length;

                //reset last position when MMF is Full
                if (position + seqSize + _int32Size > Size)
                {
                    position = 32;

                    SetterRoundCount += 1;

                    return WriteInMemoryMappedFile(accessor, position, seqSize, data);

                }

            }
            else
            {
                data = new byte[Size];
                seqSize = data.Length;
                position = Size - 16;
            }
            // Write to memory mapped file.

            return WriteInMemoryMappedFile(accessor, position, seqSize, data);


        }
        private void CalculateNextStart(MemoryMappedViewAccessor accessor, int seqSize,long writerPosition)
        {
            long nextStartpoint = 32,
                       nextBlockSize = 0;


            //do
            //{

            //    var prevBlockSize = accessor.ReadInt32(nextStartpoint);
            //    nextStartpoint += prevBlockSize + _int32Size;

            //} while (nextStartpoint > writerPosition);

            while (nextBlockSize < seqSize)
            {

                var prevBlockSize = accessor.ReadInt32(nextStartpoint);

                nextStartpoint += prevBlockSize + _int32Size;

                nextBlockSize += prevBlockSize + _int32Size;


            }
            //if (nextStartpoint >= Size) {
            //    nextStartpoint = 32;
            //}
            NextStart = nextStartpoint;
            

        }

        private long WriteInMemoryMappedFile(MemoryMappedViewAccessor accessor, long position, int seqSize, byte[] data)
        {
           
                if (NextStart > 0)
                {
                    CalculateNextStart(accessor, seqSize,position);

                }

                accessor.Write<Int32>(position, ref seqSize);

                position += _int32Size;

                accessor.WriteArray<byte>(position, data, 0, data.Length);

                position += data.Length;

                return position;
           

        }
        public void OverWriteSequences(List<T> sequences)
        {
            var LockKey = new Mutex(true, $"SM_{Guid.NewGuid()}_LOCK");

            LockKey.WaitOne();

            lock (LockKey)
            {
                LastPosition = 8;

                if (sequences.Count > 0)
                {
                    foreach (var sequence in sequences)
                    {
                        LastPosition = WriteSequence(Accessor, sequence, LastPosition);
                    }
                }
                else
                {
                    LastPosition = WriteSequence(Accessor, null, LastPosition);
                }

            }
            LockKey.Close();
        }
    }
}
