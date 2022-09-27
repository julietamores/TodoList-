using System;

namespace entities_library
{
    public class Category
    {
        public virtual long Id { get; set; }

        public virtual string Description { get; set; }

        public virtual string Color { get; set; }
    }
}
