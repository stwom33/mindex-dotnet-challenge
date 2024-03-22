using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CodeChallenge.Models
{
    public class Compensation
    {
        [Key]
        public String Employee {get;set;}
        public float Salary {get;set;}
        public DateTime EffectiveDate {get;set;}
        
    }
}