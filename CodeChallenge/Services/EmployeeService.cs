using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using CodeChallenge.Repositories;

namespace CodeChallenge.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(ILogger<EmployeeService> logger, IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        public Employee Create(Employee employee)
        {
            if(employee != null)
            {
                _employeeRepository.Add(employee);
                _employeeRepository.SaveAsync().Wait();
            }

            return employee;
        }

        public Employee GetById(string id)
        {
            if(!String.IsNullOrEmpty(id))
            {
                return _employeeRepository.GetById(id);
            }

            return null;
        }

        public Employee Replace(Employee originalEmployee, Employee newEmployee)
        {
            if(originalEmployee != null)
            {
                _employeeRepository.Remove(originalEmployee);
                if (newEmployee != null)
                {
                    // ensure the original has been removed, otherwise EF will complain another entity w/ same id already exists
                    _employeeRepository.SaveAsync().Wait();

                    _employeeRepository.Add(newEmployee);
                    // overwrite the new id with previous employee id
                    newEmployee.EmployeeId = originalEmployee.EmployeeId;
                }
                _employeeRepository.SaveAsync().Wait();
            }

            return newEmployee;
        }

        public ReportingStructure GetReportingStructure(string employeeId)
        {
            // make sure we get a valid parameter
            if (String.IsNullOrEmpty(employeeId))
            {
                return null;
            }
            
            // try and get the employee
            Employee employee = GetById(employeeId);
            if (employee == null)
            {
                return null;
            }

            // create the ReportingStructure
            ReportingStructure reportingStructure = new ReportingStructure(employee);
            if (!employee.DirectReports.Any())
            {
                return reportingStructure;
            }
            else 
            {
                CountReports(reportingStructure, employee);
                return reportingStructure;
            }
        }

        public void CountReports(ReportingStructure reportingStructure, Employee employee)
        {
            // count direct reports 
            if (employee.DirectReports != null)
            {
                reportingStructure.IncreaseReportsCountBy(employee.DirectReports.Count);
            }
            else 
            {
                return;
            }

            // go through each direct report recursivley
            foreach(var report in employee.DirectReports)
            {
                Employee currentEmployee = GetById(report.EmployeeId);
                CountReports(reportingStructure, currentEmployee);
            }
            
        }

        public Compensation GetCompensation(string employeeId)
        {
            if(!String.IsNullOrEmpty(employeeId))
            {
                return _employeeRepository.GetCompensationByEmployeeId(employeeId);
            }

            return null;
        }

        public Compensation CreateCompensation(Compensation comp)
        {  
            if(comp != null)
            {
                _employeeRepository.AddCompensation(comp);
                _employeeRepository.SaveAsync().Wait();
            }

            return comp;
        }
    }
}
