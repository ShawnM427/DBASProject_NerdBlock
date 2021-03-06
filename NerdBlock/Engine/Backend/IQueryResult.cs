﻿using System.Data;

namespace NerdBlock.Engine.Backend
{
    /// <summary>
    /// Represents the results of a query, and provides access to it's resulting data. This
    /// lets us implement our database using various backends
    /// </summary>
    public interface IQueryResult
    {
        /// <summary>
        /// Gets the data row at the current pointer
        /// </summary>
        DataRow Row { get; }

        /// <summary>
        /// Gets the number of results from this query
        /// </summary>
        int NumRows { get; }

        /// <summary>
        /// Gets the index of the current row in the data results
        /// </summary>
        int CurrentRow { get; }

        /// <summary>
        /// Gets whether another row is available with MoveNext()
        /// </summary>
        /// <returns>True if a row is available, false if otherwise</returns>
        bool HasRow { get; }

        /// <summary>
        /// Gets the query result as a dataset
        /// </summary>
        DataSet Source { get; }

        /// <summary>
        /// Moves to the next row in the result set
        /// </summary>
        /// <returns>True if we moved to the next element, false if otherwise</returns>
        bool MoveNext();

        /// <summary>
        /// Moves to the previous row in the result set
        /// </summary>
        /// <returns>True if we moved to the previous element, false if otherwise</returns>
        bool MovePrev();
    }
}
