using BGN.Domain.Entities;
using BGN.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGN.Infrastructure.Repositories
{
    internal sealed class MiscRepository : IMiscRepository
    {
        private readonly RepositoryDbContext _dbContext;

        public MiscRepository(RepositoryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<FoodOptions>> GetAllFoodOptionsAsync()
        {
            return await Task.FromResult(_dbContext.FoodOptions.AsNoTracking().ToList());
        }
    }
}
