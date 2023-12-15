using Booky.ADL.Data;
using Booky.ADL.Models;
using Booky.BL.Interface;
using Microsoft.EntityFrameworkCore;

namespace Booky.BL.Repository;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly BookyDbContext _context;
    public GenericRepository(BookyDbContext context)
    {
        _context = context;
    }
    public T GetById(int id)
    {
        return _context.Set<T>().Find(id);
    }

    // here we edit GetAll method to include category => mean get data from two table by using eager loading
    public IEnumerable<T> GetAll()
    {
        if(typeof(T) == typeof(Product))
        {
            return (IEnumerable<T>) _context.Products.Include(c => c.Category).ToList();
        }
        else
        {
            return _context.Set<T>().ToList();
            
        }
       
    }
    //==============================================================================================================

    public void Add(T entity)
    {
        _context.Set<T>().Add(entity);

    }

    public void Update(T entity)
    {
        _context.Set<T>().Update(entity);
    }

    public void Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
    }
}