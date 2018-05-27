using Core.Cmn.Attributes;
using Core.Cmn.SharedMemory;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Core.Service
{
    [Injectable(InterfaceType = typeof(ISharedMemoryService), DomainName = "Core")]
    public class SharedMemoryService : ISharedMemoryService
    {
        public void Dispose<T>(string sharedMemoryName) where T : class
        {
            var smList = Core.Cmn.AppBase.SharedMemoryList;
            var config = smList.Find(item => item.Name.ToLower().Equals(sharedMemoryName));
            if (config != null)
            {
                var sharedMemory = new SharedMemoryBase<T>(config.Name, config.Size);
                
                //access MemoryMappedFile &  MemoryMappedViewAccessor
                sharedMemory.Open();
                
                //dispose MemoryMappedFile &  MemoryMappedViewAccessor
                sharedMemory.Close();

                //delete from MemoryMappedConfig

                smList.Remove(config);
                Core.Cmn.AppBase.SharedMemoryList = smList;
            }
            else
                throw new Exception($"there isn't any sharedMemory with \"{sharedMemoryName}\" Name.please check the name ");

        }
    }
}
