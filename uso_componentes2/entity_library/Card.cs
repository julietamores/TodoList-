using System;

namespace entity_library
{
    public class Card
    {
        public virtual long Id { get; set; }

        public virtual string Description { get; set; }

        public virtual  DateTime? FinishDate { get; set; }

        public virtual entity_library.Sistema.Usuario User { get; set; }

        public virtual StatusCard StatusCard { get; set; }

        public virtual Category Category { get; set; }

        public virtual Priority Priority { get; set; }

    }
}
