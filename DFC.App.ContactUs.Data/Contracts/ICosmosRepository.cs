using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.Data.Contracts
{
    public interface ICosmosRepository<T>
        where T : class, IDataModel
    {
        Task<bool> PingAsync();

        Task<T?> GetAsync(Expression<Func<T, bool>> where);

        Task<T?> GetAsync(string partitionKeyValue, Expression<Func<T, bool>> where);

        Task<IEnumerable<T>?> GetAllAsync();

        Task<IEnumerable<T>?> GetAllAsync(string partitionKeyValue);

        Task<HttpStatusCode> UpsertAsync(T model);

        Task<HttpStatusCode> DeleteAsync(Guid documentId);
    }
}