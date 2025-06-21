using System;
using Etorium.ObjectPoolSystem;
using UnityEngine;

namespace Etorium.Interfaces
{
    public interface IObjectPoolItem
    {
        void SetObjectPool<T>(ObjectPool pool, T comp) where T : Component;

        void Release();
    }
}