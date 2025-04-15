using System;
using System.Collections.Generic;

namespace Project1.Models;

public partial class Employee
{
    public int EID { get; set; }

    public string? UserName { get; set; } 

    public string? UserPassword { get; set; } 

    public string? FirstName { get; set; } 

    public string? LastName { get; set; } = null!;

    public bool? Administrator { get; set; }

    public int? MID { get; set; }

    public string? ConfirmPassword { get; set; }

    public string? UserRole { get; set; }

    public bool? IsManager { get; set; }

    public virtual ICollection<Employee> InverseMidNavigation { get; set; } = new List<Employee>();

    public virtual Employee? MIDNavigation { get; set; }

    public virtual ICollection<Tasks> Tasks { get; set; } = new List<Tasks>();
}
