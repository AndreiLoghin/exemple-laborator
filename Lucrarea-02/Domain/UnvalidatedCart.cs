﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exemple.Domain
{
    public record UnvalidatedCart(string ProductID, string Quantity, string Address, string Price);
}
