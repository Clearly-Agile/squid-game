using NSubstitute;
using NSubstitute.Core;

using System.Collections.Generic;
using System.Linq;

namespace ClearlyAgile.Testing.Core.ExtensionMethods
{
    public static class IQueryableExtensions
    {
        /// <summary>
        /// Sets up the data using the supplied collection for an async return on a NSubstitute mock
        /// </summary>
        /// <typeparam name="TEntity">The type of the collection</typeparam>
        /// <param name="queryable">The queryable to extend</param>
        /// <param name="items">The collection of items that will be returned from the call</param>
        /// <returns>A NSubstitute ConfiguredCall with the data setup in the Return</returns>
        public static ConfiguredCall ReturnsAsync<TEntity>(this IQueryable<TEntity> queryable, ICollection<TEntity> items)
        {
            return queryable.Returns(new TestAsyncEnumerable<TEntity>(items));
        }

        /// <summary>
        /// Sets up the data using the supplied params for an async return on a NSubstitute mock
        /// </summary>
        /// <typeparam name="TEntity">The type of the collection</typeparam>
        /// <param name="queryable">The queryable to extend</param>
        /// <param name="items">One or more items of type TEntity to return in the Mock call</param>
        /// <returns>A NSubstitute ConfiguredCall with the data setup in the Return</returns>
        public static ConfiguredCall ReturnsAsync<TEntity>(this IQueryable<TEntity> queryable, params TEntity[] items)
        {
            return queryable.Returns(new TestAsyncEnumerable<TEntity>(items));
        }
    }
}
