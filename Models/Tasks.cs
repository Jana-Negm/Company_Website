using System;
using System.Collections.Generic;

namespace Project1.Models;

public partial class Tasks
{
    public int TID { get; set; }

    public string TaskName { get; set; } = null!;

    public string? TaskStatus { get; set; } = null!;

    public string? Comments { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public DateTime? Deadline { get; set; }

    public DateTime AssignedDate { get; set; }

    public int? TaskPriority { get; set; }

    public int EID { get; set; }

    public virtual Employee? EIDNavigation { get; set; } = null!;

    
}
