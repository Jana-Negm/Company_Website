using System;
using System.Collections.Generic;

namespace Project1.Models;

public partial class Department
{
    public int DID { get; set; }

    public string DeptName { get; set; } = null!;

    public int? MID { get; set; }
}
