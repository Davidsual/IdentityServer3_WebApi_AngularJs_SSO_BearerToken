using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdSrv.Custom
{
    public interface IMyCustomLogger
    {
        void Get();
    }
    public class MyCustomLogger : IMyCustomLogger
    {
        public void Get()
        {
            throw new NotImplementedException();
        }
    }
}
