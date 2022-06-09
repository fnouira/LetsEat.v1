using LetsEat.Models;

namespace LetsEat.DataAccess.Abstractions
{
    public interface ITableRepository
    {
         IReadOnlyCollection<TableDao> GetAvailableTables(DateTime date);
    }
}