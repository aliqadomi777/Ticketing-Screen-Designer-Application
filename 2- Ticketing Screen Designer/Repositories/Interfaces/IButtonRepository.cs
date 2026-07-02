using System;
using System.Collections.Generic;

namespace Ticketing_Screen_Designer.Interfaces
{
    public interface IButtonRepository<T> where T : class
    {
        T GetById(int id, string typeName);
    }
}