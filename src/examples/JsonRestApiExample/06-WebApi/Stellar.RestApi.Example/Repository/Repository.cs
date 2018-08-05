using Stellar.RestApi.Example.Models;
using Stellar.RestApi.Example.Models.Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stellar.RestApi.Example
{
    public class Repository: IRepository
    {
        private readonly ConcurrentDictionary<Type, List<object>> _cache = new ConcurrentDictionary<Type, List<object>>();

        public Task<Guid> Add<T>(T obj) where T : IIdentifier
        {
            if (!_cache.ContainsKey(typeof(T)))
            {
                _cache[typeof(T)] = new List<object>();
            }

            obj.Id = Guid.NewGuid();
            _cache[typeof(T)].Add(obj);

            return Task.FromResult(obj.Id);
        }

        public void Attach<T>(T obj)
        {
            // do not need to do anything
        }

        public Task<T> Get<T>(Guid id) where T : class, IIdentifier
        {
            T result = null;
            if (_cache.ContainsKey(typeof(T)))
            {
                result = _cache[typeof(T)]
                    .OfType<T>()
                    .FirstOrDefault(n => n.Id == id);
            }

            return Task.FromResult(result);
        }

        public Task<List<T>> GetAll<T>(Func<T, bool> predicate) where T : new()
        {
            var result = new List<T>();

            if (_cache.ContainsKey(typeof(T)))
            {
                result = _cache[typeof(T)].OfType<T>().Where(predicate).ToList();
            }

            return Task.FromResult(result);
        }

        public Task<List<T>> GetAll<T>()
        {
            var result = new List<T>();

            if (_cache.ContainsKey(typeof(T)))
            {
                result = _cache[typeof(T)].OfType<T>().ToList();
            }

            return Task.FromResult(result);
        }

        public Task<IQueryable<T>> Query<T>()
        {
            var result = GetAll<T>().Result.AsQueryable();
            return Task.FromResult(result);
        }

        public Task<int> Remove<T>(Guid id) where T : IIdentifier
        {
            if (!_cache.ContainsKey(typeof(T)))
            {
                return Task.FromResult(0);
            }

            for (var i = 0; i < _cache[typeof(T)].Count; i++)
            {
                if (_cache[typeof(T)].OfType<T>().ToList()[i].Id == id)
                {
                    _cache[typeof(T)].RemoveAt(i);
                    return Task.FromResult(1);
                }
            }

            return Task.FromResult(0);
        }

        public Task<int> Save<T>(T obj) where T : IIdentifier
        {
            if (!_cache.ContainsKey(typeof(T)))
            {
                return Task.FromResult(0);
            }

            for (var i = 0; i < _cache[typeof(T)].Count; i++)
            {
                if (_cache[typeof(T)].OfType<T>().ToList()[i].Id == obj.Id)
                {
                    _cache[typeof(T)][i] = obj;
                    return Task.FromResult(1);
                }
            }

            return Task.FromResult(0);
        }

    }
}
