using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Zenos.Framework
{
    public interface IRegister
    {
        int Id { get; }
    }

    class Register : IRegister
    {
        public int Id { get; private set; }

        public Register(int id)
        {
            this.Id = id;
        }

        public override string ToString()
        {
            return string.Format("Register[{0}]", this.Id);
        }
    }
}
