

namespace ULearn.Application.Interfaces;

public interface IParallelUnitOfWork
{
    /// <summary>
    /// Executes two queries in parallel using the same <see cref="IUnitOfWork"/> instance and returns their results as a tuple.
    /// </summary>
    /// <typeparam name="T1">The type of the result returned by the first query.</typeparam>
    /// <typeparam name="T2">The type of the result returned by the second query.</typeparam>
    /// <param name="query1">A function that takes an <see cref="IUnitOfWork"/> and returns a task producing the first result.</param>
    /// <param name="query2">A function that takes an <see cref="IUnitOfWork"/> and returns a task producing the second result.</param>
    /// <returns>A task that represents the asynchronous operation, containing a tuple of both results.</returns>
    Task<(T1, T2)> ParallelQueryAsync<T1, T2>(
        Func<IUnitOfWork, Task<T1>> query1,
        Func<IUnitOfWork, Task<T2>> query2);

    Task<(T1, T2, T3)> ParallelQueryAsync<T1, T2, T3>(
        Func<IUnitOfWork, Task<T1>> query1,
        Func<IUnitOfWork, Task<T2>> query2,
        Func<IUnitOfWork, Task<T3>> query3);

    Task<(T1, T2, T3,T4)> ParallelQueryAsync<T1, T2, T3, T4>(
        Func<IUnitOfWork, Task<T1>> query1,
        Func<IUnitOfWork, Task<T2>> query2,
        Func<IUnitOfWork, Task<T3>> query3,
        Func<IUnitOfWork, Task<T4>> query4);

}