using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace API_TICKETS.Models;

public partial class User
{
    public int IdUser { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }
    [JsonIgnore]
    public virtual ICollection<Ticket> Tickets { get; } = new List<Ticket>();
}
