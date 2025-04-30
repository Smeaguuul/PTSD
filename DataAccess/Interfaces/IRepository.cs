using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface IRepository<T>
    {
        /// <summary>
        /// Adds an entity of T type to the Database.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>A task</returns>
        Task AddAsync(T entity);


        /// <summary>
        /// Removes and entity of T type that matches the paramenter given
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task RemoveAsync(T entity);

        /// <summary>
        /// Gives you the first Entity in the database that matches the given predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>Task of type T?. So it can return null if no matches are found. This includes if the list is empty. </returns>
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>>? predicate = null);

        /// <summary>
        /// Retrieves a collection of entities from the database based on optional filtering, ordering, and inclusion criteria.
        /// </summary>
        /// <param name="predicate">An optional expression to filter the entities. If provided, only entities that satisfy the predicate will be returned.</param>
        /// <param name="orderBy">An optional function to order the entities. If provided, the entities will be ordered according to the specified criteria.</param>
        /// <param name="include">An optional function to include related entities. If provided, the specified related entities will be included in the result.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of entities that match the specified criteria.</returns>
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, Func<IQueryable<T>, IQueryable<T>>? include = null);

        /// <summary>
        /// Retrieves a paginated collection of entities from the database based on optional filtering, ordering, and inclusion criteria.
        /// </summary>
        /// <param name="pageNumber">The page number to retrieve. Pagination is zero-based, so the first page is 0.</param>
        /// <param name="pageSize">The number of entities to include in each page.</param>
        /// <param name="predicate">An optional expression to filter the entities. If provided, only entities that satisfy the predicate will be returned.</param>
        /// <param name="orderBy">An optional function to order the entities. If provided, the entities will be ordered according to the specified criteria.</param>
        /// <param name="include">An optional function to include related entities. If provided, the specified related entities will be included in the result.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of entities that match the specified criteria and fall within the specified page.</returns>
        Task<IEnumerable<T>> GetAllAsync(int pageNumber, int pageSize, Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, Func<IQueryable<T>, IQueryable<T>>? include = null);
    }
}
