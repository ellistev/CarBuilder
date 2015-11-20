using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarBuilder
{

    public class CircularException : Exception
    {
        public CircularException()
            : base("Circular exception: You have just caused an infinite loop.  Bad! \nCheck the input file for errors")
        {
        }
    }
}
