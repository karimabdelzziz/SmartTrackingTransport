﻿using Microsoft.EntityFrameworkCore;
using Infrastucture.DbContexts;
using Infrastructure.Interfaces;
using Infrastucture.Entities;
using System.Linq.Expressions;



namespace Infrastructure.Repos
{
	public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
	{
		protected readonly TransportContext _transportContext;

		public GenericRepository(TransportContext transportContext)
		{
			_transportContext = transportContext;
		}
		public async Task Add(T entity)

		   => await _transportContext.Set<T>().AddAsync(entity);
		

		public void Delete(T entity)
			=> _transportContext.Set<T>().Remove(entity);

		public async Task<IReadOnlyList<T>> GetAllAsync()
			=> await _transportContext.Set<T>().ToListAsync();


		public async Task<T> GetByIdAsync(int? id)
			=> await _transportContext.Set<T>().FindAsync(id);

		public void Update(T entity)
		    => _transportContext.Set<T>().Update(entity);
		public async Task<IReadOnlyList<T>> FindAllAsync(Expression<Func<T, bool>> predicate)
		{
			// Using the predicate to filter the entities
			return await _transportContext.Set<T>().Where(predicate).ToListAsync();
		}

	}

}
