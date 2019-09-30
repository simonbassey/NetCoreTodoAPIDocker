using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TuringECommerce.Core.Abstractions.Repositories;

namespace TodoDockerAPI.Data.Core.Repositories
{
    public abstract class BaseRepository<T> : IAsyncRepository<T> where T : class
    {
        protected readonly TodoDbContext _dbContext;
        protected BaseRepository()
        {
            //_dbContext = ServiceResolver.Resolve<TuringECommerceDbContext>();
            _dbContext = new TodoDbContext();
        }
        public virtual async Task<T> AddItemAsync(T tEntity)
        {
            using (var db = new TodoDbContext())
            {
                await db.Set<T>().AddAsync(tEntity);
                return (await db.SaveChangesAsync()) == 1 ? tEntity : null;
            }
        }

        public virtual async Task<T> GetItemAsync(params object[] key)
        {
            using (var db = new TodoDbContext())
            {
                return await db.Set<T>().FindAsync(key);
            }
        }

        public virtual async Task<IEnumerable<T>> GetItemsAsync()
        {
            using (var db = new TodoDbContext())
            {
                return await db.Set<T>().ToListAsync();
            }
        }
        public virtual async Task<IEnumerable<T>> GetItemsAsync(int page, int pageSize)
        {
            using (var db = new TodoDbContext())
            {
                return await db.Set<T>().Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            }
        }

        public virtual async Task<IEnumerable<T>> FilterAsync(Func<T, bool> criteria)
        {
            using (var db = new TodoDbContext())
            {
                return await Task.FromResult(db.Set<T>().Where(criteria).ToList());
            };

        }

        public virtual async Task<bool> RemoveItemAsync(params object[] itemKey)
        {
            using (var context = new TodoDbContext())
            {
                var item = await GetItemAsync(itemKey);
                if (item == null)
                    return false;
                context.Set<T>().Remove(item);
                return await context.SaveChangesAsync() == 1;
            }
        }

        public virtual async Task<IEnumerable<TEntity>> ExecuteQueryAsync<TEntity>(string procNameWithParamNames, KeyValuePair<string, object>[] inData, bool isEntitySet = false) where TEntity : class
        {
            try
            {
                using (var context = new TodoDbContext())
                {
                    var parameterString = string.Join(",", inData.Select(k => !(k.Value.GetType() != typeof(string)) ? $"'{k.Value}'" : k.Value).ToArray());
                    var query = $"call {procNameWithParamNames} ({parameterString})";
                    IEnumerable<TEntity> queryResult = isEntitySet ?
                        await context.Set<TEntity>().FromSql(query).ToListAsync() :
                        await context.Query<TEntity>().FromSql(query).ToListAsync();
                    return queryResult;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual async Task<T> UpdateItemAsync(T updatedItem, params object[] itemKey)
        {
            using (var db = new TodoDbContext())
            {
                var item = await GetItemAsync(itemKey);
                if (item == null)
                    throw new KeyNotFoundException($"Item with key {itemKey} not found");
                db.Entry<T>(updatedItem).State = EntityState.Modified; // .CurrentValues..SetValues(updatedItem);
                var updated = await db.SaveChangesAsync() == 1;
                return updated ? item : null;
            }
        }

        public T AddItem(T tEntity)
        {
            using (var db = new TodoDbContext())
            {
                db.Set<T>().AddAsync(tEntity);
                return db.SaveChanges() == 1 ? tEntity : null;
            }
        }

        public T GetItem(params object[] key)
        {
            using (var db = new TodoDbContext())
            {
                return db.Set<T>().Find(key);
            }
        }

        public IEnumerable<T> GetItems()
        {
            using (var db = new TodoDbContext())
            {
                return db.Set<T>().ToList();
            }
        }

        public IEnumerable<T> Filter(Func<T, bool> criteria)
        {
            using (var db = new TodoDbContext())
            {
                return db.Set<T>().Where(criteria).ToList();
            };
        }

        public bool RemoveItem(params object[] itemKey)
        {
            using (var context = new TodoDbContext())
            {
                var item = GetItem(itemKey);
                if (item == null)
                    return false;
                context.Set<T>().Remove(item);
                return context.SaveChanges() == 1;
            }
        }

        public T UpdateItem(T updatedItem, params object[] itemKey)
        {
            using (var db = new TodoDbContext())
            {
                var item = GetItem(itemKey);
                if (item == null)
                    throw new KeyNotFoundException($"Item with key {itemKey} not found");
                db.Entry<T>(item).CurrentValues.SetValues(updatedItem);
                return db.SaveChanges() == 1 ? item : null;
            }
        }

        public virtual async Task<int> RemoveItemsAsync(IEnumerable<T> list)
        {
            using (var context = new TodoDbContext())
            {
                context.Set<T>().RemoveRange(list);
                return await context.SaveChangesAsync();
            }
        }

        public async Task<int> AddItemsAsync(IList<T> entities)
        {
            using (var context = new TodoDbContext())
            {
                await context.Set<T>().AddRangeAsync(entities);
                return await context.SaveChangesAsync();
            }
        }

        public virtual async Task<long> GetCountAsync() => await _dbContext.Set<T>().LongCountAsync();

        public virtual async Task<T> GetFirstItemAsync() => await _dbContext.Set<T>().LastOrDefaultAsync();

        public virtual async Task<T> GetLastItemAsync() => await _dbContext.Set<T>().FirstOrDefaultAsync();

        public async Task<int> ExecuteNonQueryAsync(string procName, KeyValuePair<string, object>[] values)
        {
            try
            {
                using (var context = new TodoDbContext())
                {
                    var parameterString = string.Join(",", values.Select(k => !(k.Value.GetType() != typeof(string)) ? $"'{k.Value}'" : k.Value).ToArray());
                    var query = $"call {procName} ({parameterString})";
                    int queryResult = await context.Database.ExecuteSqlCommandAsync(query);
                    return queryResult;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
