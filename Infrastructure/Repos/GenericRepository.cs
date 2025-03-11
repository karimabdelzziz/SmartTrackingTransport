using Microsoft.EntityFrameworkCore;
using Infrastucture.DbContexts;
using Infrastructure.Interfaces;
using Infrastucture.Entities;



namespace Infrastructure.Repos
{
	public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
	{
		private readonly TransportContext transportContext;

		public GenericRepository(TransportContext transportContext)
		{
			this.transportContext = transportContext;
		}
		public async Task Add(T entity)

		   => await transportContext.Set<T>().AddAsync(entity);
		

		public void Delete(T entity)
			=> transportContext.Set<T>().Remove(entity);

		public async Task<IReadOnlyList<T>> GetAllAsync()
			=> await transportContext.Set<T>().ToListAsync();


		public async Task<T> GetByIdAsync(int? id)
			=> await transportContext.Set<T>().FindAsync(id);

		public void Update(T entity)
		    => transportContext.Set<T>().Update(entity);

	}
}
