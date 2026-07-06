using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing_Screen_Designer.Interfaces
{
    public interface ITicketRepository<T> where T : class
    {
        bool Update(int id, T entity);


    }
}