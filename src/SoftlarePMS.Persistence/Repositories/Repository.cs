using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SoftlarePMS.Persistence.Context;
using YourProject.Domain.Repositories;

namespace SoftlarePMS.Persistence.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly SoftlarePMSDbContext _context;
    private readonly DbSet<T> _dbSet;

    // Constructor to initialize the repository with the database context
    public Repository(SoftlarePMSDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    //Method to get an entity by its ID
    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    // Method to get all entities of type T
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    // Method to find entities based on a predicate
    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    // Method to add a new entity to the database
    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    // Method to update an existing entity in the database
    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    // Method to delete an entity from the database
    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }
}
