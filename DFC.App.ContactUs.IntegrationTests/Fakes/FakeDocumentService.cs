using DFC.App.ContactUs.Data.Models;
using DFC.Compui.Cosmos.Contracts;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.IntegrationTests.Fakes
{
    public class FakeDocumentService : IDocumentService<StaticContentItemModel>
    {
        public Task<bool> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<StaticContentItemModel>?> GetAllAsync(string? partitionKeyValue = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<StaticContentItemModel>?> GetAsync(Expression<Func<StaticContentItemModel, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<StaticContentItemModel?> GetAsync(Expression<Func<StaticContentItemModel, bool>> where, string partitionKeyValue)
        {
            throw new NotImplementedException();
        }

        public Task<StaticContentItemModel?> GetByIdAsync(Guid id, string? partitionKey = null)
        {
            StaticContentItemModel staticContentItemModel = new StaticContentItemModel();
            return Task.FromResult(staticContentItemModel);
        }

        public Task<bool> PingAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> PurgeAsync()
        {
            throw new NotImplementedException();
        }

        public Task<HttpStatusCode> UpsertAsync(StaticContentItemModel model)
        {
            throw new NotImplementedException();
        }
    }
}
