using System;
using System.Collections.Generic;

namespace DesafioPulseIT.Models;

public partial class Evento
{
    public int Id { get; set; }

    public string Titulo { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public DateTime Fecha { get; set; }

    public virtual ICollection<Invitado> Invitados { get; set; } = new List<Invitado>();
}
