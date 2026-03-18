using System.Data;

namespace ProyectoArqSoft.FactoryProducts
{
    public interface IRepository<T> // equivalente coceptualmente al ICRUD
    {
        int Insert(T t);
        int Update(T t);
        int Delete(T t);
        DataTable GetAll();
        DataTable GetAll(string filtro);
        T? GetById(int id);

    }
}
