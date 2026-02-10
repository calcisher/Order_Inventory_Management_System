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

        private readonly Dictionary<Type, object> _repositories = new();
        public UnitOfWork(SystemDbContext context) => _context = context;
        public IGenericRepository<T> GetRepository<T>() where T : class
        {
            var type = typeof(T);

            if (!_repositories.ContainsKey(type))
            {            
                var repositoryInstance = new GenericRepository<T>(_context);
                _repositories.Add(type, repositoryInstance);
            }

            return (IGenericRepository<T>)_repositories[type];
        }

        public async Task<int> CommitAsync() => await _context.SaveChangesAsync();
        public void Dispose() => _context.Dispose();
    }
}
