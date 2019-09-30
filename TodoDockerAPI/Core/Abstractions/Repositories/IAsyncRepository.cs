using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TuringECommerce.Core.Abstractions.Repositories
{
    public interface IAsyncRepository<T> where T : class
    {
        Task<T> AddItemAsync(T tEntity);
        Task<int> AddItemsAsync(IList<T> entities);
        Task<T> GetItemAsync(params object[] key);
        Task<bool> RemoveItemAsync(params object[] itemKey);
        Task<int> RemoveItemsAsync(IEnumerable<T> list);
        Task<T> UpdateItemAsync(T updatedItem, params object[] itemKey);
        Task<IEnumerable<T>> GetItemsAsync();
        Task<IEnumerable<T>> GetItemsAsync(int page, int pageSize);
        Task<IEnumerable<T>> FilterAsync(Func<T, bool> criteria);
        Task<IEnumerable<TEntity>> ExecuteQueryAsync<TEntity>(string procName, KeyValuePair<string, object>[] values, bool isEntitySet = false) where TEntity : class;
        Task<int> ExecuteNonQueryAsync(string procName, KeyValuePair<string, object>[] values);
        Task<long> GetCountAsync();
        Task<T> GetFirstItemAsync();
        Task<T> GetLastItemAsync();
    }
}
