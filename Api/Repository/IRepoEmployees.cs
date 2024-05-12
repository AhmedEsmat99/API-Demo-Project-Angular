using Api.Model;

namespace Api.Repository
{
    public interface IRepoEmployees
    {
        bool Delete(int id);
        List<Employee> GetAll();
        Employee GetByEmail(string email);
        Employee GetById(int id);
        Employee GetByName(string name);
        bool Insert(Employee entity);
        bool Update(Employee entity, int id);
    }
}