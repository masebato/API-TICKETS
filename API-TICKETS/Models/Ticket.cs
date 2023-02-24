using System;
using System.Collections.Generic;

namespace API_TICKETS.Models;

public partial class Ticket
{
    public int IdTicket { get; set; }

    public string? Description { get; set; }

    public DateTime? DateCreate { get; set; }

    public DateTime? DateUpdate { get; set; }

    public string? Status { get; set; }

    public int? IdUser { get; set; }

    public virtual User? IdUserNavigation { get; set; }
}
