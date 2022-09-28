using System;

namespace entities_library
{
    public class Card
    {
        public virtual long Id { get; set; }

        public virtual string Description { get; set; }

        public virtual  DateTime? FinishDate { get; set; }

        public virtual User User { get; set; }

        public virtual StatusCard StatusCard { get; set; }

        public virtual Category Category { get; set; }

        public virtual Priority Priority { get; set; }

    }
}
