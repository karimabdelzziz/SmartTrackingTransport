using Infrastucture.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
	public interface IGenericRepository<T> where T : BaseEntity
	{
		Task<T> GetByIdAsync(int? id);
		Task<IReadOnlyList<T>> GetAllAsync();
		Task Add(T entity);
		void Update(T entity);
		void Delete(T entity);
		Task<IReadOnlyList<T>> FindAllAsync(Expression<Func<T, bool>> predicate);

	}
}