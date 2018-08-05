using Stellar.RestApi.Example.Models;
using Stellar.RestApi.Example.Models.Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stellar.RestApi.Example
{
    public interface IRepository
    {
        Task<Guid> Add<T>(T obj) where T : IIdentifier;

        void Attach<T>(T obj);

        Task<T> Get<T>(Guid id) where T : class, IIdentifier;

        Task<List<T>> GetAll<T>(Func<T, bool> predicate) where T : new();

        Task<List<T>> GetAll<T>();

        Task<IQueryable<T>> Query<T>();

        Task<int> Remove<T>(Guid id) where T : IIdentifier;

        Task<int> Save<T>(T obj) where T : IIdentifier;
    }
}
