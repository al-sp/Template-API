using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template_API.Data;

namespace Template_API.Contracts
{
    public interface IBookRepository : IRepositoryBase<Book>
    {
        public Task<string> GetImageFileName(int id);
    }
}
