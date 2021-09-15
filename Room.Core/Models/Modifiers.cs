using System;
using System.Collections.Generic;
using System.Linq;
using Room.Core.Abstract;

namespace Room.Core.Models
{
    public class Modifiers : IModifiers
    {
        public bool InvFrame { get; set; }

        public float Attack { get; set; }

        public float Defence { get; set; }

        public static Modifiers Combine(IReadOnlyCollection<IModifiers> modifs)
        {
            if (modifs == null) throw new ArgumentNullException(nameof(modifs));

            return new Modifiers
            {
                InvFrame = modifs.Any(m => m.InvFrame),
                Attack = modifs.Sum(m => m.Attack),
                Defence = modifs.Sum(m => m.Defence)
            };
        }
    }
}
