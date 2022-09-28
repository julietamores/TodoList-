using System;

namespace entity_library.Sistema
{
    public class Usuario
    {
        public virtual long Id { get; set; }

        public virtual string NombreUsuario { get; set; }

        public virtual string Password { get; set; }

        public virtual string NombreCompleto { get; set; }
    }
}
