using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGN.Domain.Repositories
{
    public interface IRepositoryManager
    {
        IPersonRepository PersonRepository { get; }
        IGameNightRepository GameNightRepository { get; }
        IGameRepository GameRepository { get; }
        IMiscRepository MiscRepository { get; }
    }
}
