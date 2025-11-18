
using ULearn.Application.Interfaces;

namespace ULearn.Infrastructure.Utils;

public static class UnitOfWorkExtensions
{
    public static async Task<TResult> ExecuteInTransactionAsync<T, TResult>(
        this T unitOfWork,
        Func<T, Task<TResult>> action) where T : IUnitOfWork
    {
        await unitOfWork.BeginTransactionAsync();

        try
        {
            var result = await action(unitOfWork);
            await unitOfWork.SaveChangesAsync();
            await unitOfWork.CommitTransactionAsync();
            return result;
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public static async Task ExecuteInTransactionAsync<T>(
        this T unitOfWork,
        Func<T, Task> action) where T : IUnitOfWork
    {
        await unitOfWork.ExecuteInTransactionAsync(async u =>
        {
            await action(u);
            return true;
        });
    }
}