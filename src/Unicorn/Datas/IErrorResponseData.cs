using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Unicorn.Datas
{
    public interface IErrorResponseData
    {
        void SetCode(int code);
        void SetMessage(string message);
    }
}
