using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Parks.WebApp.Models.Entities;
using Parks.WebApp.Models.EntityBase;

namespace Parks.WebApp.Abstact
{
    public interface IEntityBaseRepository<T> where T : class, IEntityBase
    {
        IEnumerable<T> GetAllEntities();
        int Count();
        T GetSigleVehiecleItems(int id);
        T GetSingleVehiecleItems(Expression<Func<T, bool>> predicate);
        IEnumerable<T> FindVehiecleById(Expression<Func<T, bool>> predicate);
        void AddEntity(T entities);
        void UpdateEntity(T entities);
        void DeleteEntity(T entities);
        void DeleteWhereEntity(Expression<Func<T, bool>> predicate);
        void Commit();
        
    }
}
