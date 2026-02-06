using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OrderManagement.Core.Interfaces;
using OrderManagement.DataAccess.Context;

namespace OrderManagement.DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SystemDbContext _context;
        public UnitOfWork(SystemDbContext context) => _context = context;

        public async Task<int> CommitAsync() => await _context.SaveChangesAsync();
        public void Dispose() => _context.Dispose();
    }
}
