using System;
using System.Collections.Generic;

namespace CristobalMunioz.Models;

public partial class Permiso
{
    public int Id { get; set; }

    public string Permiso1 { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<Person> People { get; set; } = new List<Person>();
}
