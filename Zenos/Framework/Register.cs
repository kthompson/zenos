using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Zenos.Framework
{
    class Register : IRegister
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        public Register(int id)
        {
            this.Id = id;
            this.Name = "reg" + id;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
