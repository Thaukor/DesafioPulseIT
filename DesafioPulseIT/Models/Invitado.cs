using System;
using System.Collections.Generic;

namespace DesafioPulseIT.Models;

public partial class Invitado
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public bool Asiste { get; set; }

    public bool Respondio { get; set; }

    public int EventoId { get; set; }

    public virtual Evento Evento { get; set; } = null!;
}
