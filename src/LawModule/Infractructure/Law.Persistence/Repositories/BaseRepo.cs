﻿using Law.Domain.Models;
using Law.Domain.Repositories;
using Law.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Utilities.ActionResponse;

namespace Law.Persistence.Repositories
{
    public class BaseRepo<T> : IBaseRepo<T> where T : ModelBase
    {
        private readonly LawDbContext _context;

        public BaseRepo(LawDbContext context)
        {
            _context = context;
        }

        public virtual async Task<ActionResult> Add(T entity)
        {
            _context.Add(entity);
            return await _context.SaveActionAsync();
        }
        public async Task<ActionResult> AddRange(IEnumerable<T> entities)
        {
            _context.AddRange(entities);
            return await _context.SaveActionAsync();
        }
        public virtual async Task<ActionResult<T>> AddAndReturn(T entity)
        {
            _context.Add(entity);
            return await _context.SaveActionAsync(entity);
        }

        public virtual async Task<ActionResult> Delete(Guid id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                _context.Remove(entity);
                return await _context.SaveActionAsync();
            }
            else
                return FailedAction("Record not found.");
        }

        public async Task<ActionResult<T>> DeleteAndReturn(Guid id)
        {
            var entity = await GetById(id);
            if (entity != null)
            {
                _context.Remove(entity);
                return await _context.SaveActionAsync(entity);
            }
            else
                return FailedAction<T>("Record not found.");
        }
        public virtual async Task<List<T>> FindByPredicate(Expression<Func<T, bool>> predicate, bool IsEagerLoad = false)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }

        public virtual async Task<T?> FindOneByPredicate(Expression<Func<T, bool>> predicate, bool IsEagerLoad = false)
        {
            return await _context.Set<T>().Where(predicate).FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public virtual async Task<T?> GetById(Guid id, bool IsEagerLoad = false)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<ActionResult> Update(T entity)
        {
            _context.Update(entity);
            return await _context.SaveActionAsync();
        }

        public async Task<ActionResult<T>> UpdateAndReturn(T entity)
        {
            _context.Update(entity);
            return await _context.SaveActionAsync(entity);
        }
        public ActionResult FailedAction(string message)
        {
            var res = new ActionResult();
            res.AddError(message);
            return res;
        }

        public ActionResult<U> FailedAction<U>(string message) where U : class
        {
            var res = new ActionResult<U>();
            res.AddError(message);
            return res;
        }


    }
}
