using System;
using System.Collections.Generic;
using System.Text;

namespace MiniBank.InterfaceTypes
{
    public interface IRepository<K, T>
    {
        int Count { get; }
        void Add(T e);
        void Remove(T e);
        T GetById(K id);
        List<T> GetAll();
    }
}
