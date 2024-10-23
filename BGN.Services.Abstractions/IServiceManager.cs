using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGN.Services.Abstractions
{
    public interface IServiceManager
    {
        IPersonService PersonService { get; }
        IGameNightService GameNightService { get; }
        IGameService GameService { get; }
        IMiscService MiscService { get; }
    }
}
