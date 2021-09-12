using System;
using Room.Core.Abstract;
using Room.Core.Models;

namespace Room.Core.Impl
{
    public class FinesSource: IFinesSource
    {
        private readonly Hero _hero;

        public FinesSource(Hero hero)
        {
            _hero = hero ?? throw new ArgumentNullException(nameof(hero));
        }

        public void Fine()
        {
        }
    }
}
