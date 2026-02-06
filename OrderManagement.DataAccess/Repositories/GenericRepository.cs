using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using OrderManagement.Core.Interfaces;
using OrderManagement.DataAccess.Context;

namespace OrderManagement.DataAccess.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly SystemDbContext _context;
        private readonly DbSet<T> _dbSet;
        public GenericRepository(SystemDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        } 

        public async Task<IEnumerable<T>> GetAllAsync() => await _context.Set<T>().ToListAsync();
        public async Task AddAsync(T entity) => await _context.Set<T>().AddAsync(entity);
        public async Task<T> GetByIdAsync(int id) => await _dbSet.FindAsync(id);
        public void Update(T entity) =>_dbSet.Update(entity);
        public void Delete(T entity) => _dbSet.Remove(entity);
    }
}
