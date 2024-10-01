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
    }
}
