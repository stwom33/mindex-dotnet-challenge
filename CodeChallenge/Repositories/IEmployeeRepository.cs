﻿using CodeChallenge.Models;
using System;
using System.Threading.Tasks;

namespace CodeChallenge.Repositories
{
    public interface IEmployeeRepository
    {
        Employee GetById(String id);
        Employee Add(Employee employee);
        Employee Remove(Employee employee);
        Compensation GetCompensationByEmployeeId(String id);
        Compensation AddCompensation(Compensation comp);
        Task SaveAsync();
    }
}