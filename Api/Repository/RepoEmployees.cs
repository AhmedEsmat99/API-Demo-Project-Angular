using Api.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Api.Repository
{
    public class RepoEmployees : IRepoEmployees
    {
        Context db;
        public RepoEmployees(Context context)
        {
            this.db = context;
        }
        public List<Employee> GetAll()
        {
            return db.Employees.ToList();
        }
        public Employee GetById(int id)
        {
            return db.Employees.Find(id);
        }
        public Employee GetByName(string name)
        {
            return db.Employees.FirstOrDefault(e => e.Name == name);
        }
        public Employee GetByEmail(string email)
        {
            return db.Employees.FirstOrDefault(e => e.Email == email);
        }
        public bool Insert(Employee entity)
        {
            try
            {
                db.Employees.Add(entity);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false; 
            }
        }
        public bool Update(Employee entity, int id)
        {
            try 
            { 
                Employee OldData = GetById(id);
                OldData.Name = entity.Name;
                OldData.Resume = entity.Resume;
                OldData.Salary = entity.Salary;
                OldData.Address = entity.Address;
                OldData.Phone = entity.Phone;
                OldData.Gender = entity.Gender;
                OldData.Age = entity.Age;
                OldData.Email = entity.Email;
                db.SaveChanges();
                return true;
            }catch 
            {
                return false;
            }
        }
        public bool Delete(int id)
        {
            try
            {
                db.Employees.Remove(GetById(id));
                db.SaveChanges();
                return true;
            }
            catch { return false; }
        }
    }
}
