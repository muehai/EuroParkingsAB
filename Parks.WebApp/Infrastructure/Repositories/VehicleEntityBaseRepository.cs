using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Parks.WebApp.Abstact;
using Parks.WebApp.Data;
using Parks.WebApp.Models.Entities;
using Parks.WebApp.Models.EntityBase;

namespace Parks.WebApp.Repositories
{
    public class VehicleEntityBaseRepository<T> : IEntityBaseRepository<T> where T : class, IEntityBase, new()
    {
        private VehicleDbContext _context;
        #region Properties
        public VehicleEntityBaseRepository(VehicleDbContext context)
        {
            _context = context;

        }
        #endregion

        public virtual IEnumerable<T> GetAllEntities()
        {
            return _context.Set<T>().AsEnumerable();
        }

        public int Count()
        {
            return _context.Set<T>().Count();
        }
        public T GetSigleVehiecleItems(int id)
        {
            return _context.Set<T>().FirstOrDefault(v => v.Id == id);
        }
        public T GetSingleVehiecleItems(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().FirstOrDefault(predicate);
        }
        public IEnumerable<T> FindVehiecleById(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate);
        }
        public void AddEntity(T entities)
        {
            EntityEntry entityEntry = _context.Entry<T>(entities);
            _context.Set<T>().Add(entities);
        }

        public void UpdateEntity(T entities)
        {
            EntityEntry entityEntry = _context.Entry<T>(entities);
            entityEntry.State = EntityState.Modified;
        }

        public void DeleteEntity(T entities)
        {
            EntityEntry entityEntry = _context.Entry<T>(entities);
            entityEntry.State = EntityState.Deleted;
        }
        public void DeleteWhereEntity(Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> entities = _context.Set<T>().Where(predicate);

            foreach (var entity in entities)
            {
                _context.Entry<T>(entity).State = EntityState.Deleted;
            }
        }
        public void Commit()
        {
            _context.SaveChanges();
        }


    }
}
