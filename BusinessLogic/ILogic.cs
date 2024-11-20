using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public interface ILogic<IParam, IResult>
    {
        Task<IResult>? Execute(IParam param);
    }
}