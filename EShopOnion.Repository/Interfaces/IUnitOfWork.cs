using EShopOnion.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShopOnion.Repository.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<T> Repository<T>() where T : IBaseEntity;
    }

}
