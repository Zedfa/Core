using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cmn
{
    public enum Language
    {
        Farsi = 1,
        Arabic = 2,
        English = 3
    }

    public interface IDependecyInjectionContract : IDisposable
    {
        void Init();

        TService GetService<TService>();
    }
    public class DiManager
    {
        public static IDependecyInjectionContract Current { get; set; }
    }


}
