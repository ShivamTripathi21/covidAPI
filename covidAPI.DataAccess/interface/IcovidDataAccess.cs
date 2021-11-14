using System;
using System.Threading.Tasks;
using covidAPI.model;

namespace covidAPI.DataAccess.@interface
{
    public interface IcovidDataAccess
    {
        Task<user> registerUser(user user);
    }
}
