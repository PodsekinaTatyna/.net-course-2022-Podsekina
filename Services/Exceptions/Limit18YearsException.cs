﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Exceptions
{
    public class Limit18YearsException :Exception
    {
        public Limit18YearsException(string message) :base(message) { }
    }
}
