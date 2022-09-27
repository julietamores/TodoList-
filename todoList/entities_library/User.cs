using System;

namespace entities_library
{
    public class User
    {
        public virtual long Id { get; set; }

        public virtual string UserName{ get; set; }

        public virtual string Password { get; set; }

    }


}
