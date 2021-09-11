using System;
using Kalavarda.Primitives.Process;
using Room.Core.Models;

namespace Room.Processes
{
    public class HeroMoveProcess: IProcess
    {
        private readonly Hero _hero;
        private readonly Arena _arena;
        public event Action<IProcess> Completed;

        public HeroMoveProcess(Hero hero, Arena arena)
        {
            _hero = hero ?? throw new ArgumentNullException(nameof(hero));
            _arena = arena ?? throw new ArgumentNullException(nameof(arena));
        }

        public void Process(TimeSpan delta)
        {
            var speed = _hero.MoveSpeed.Value;
            var dt = (float)delta.TotalSeconds;
            var x = _hero.Position.X + dt * speed * MathF.Cos(_hero.MoveDirection.Value);
            var y = _hero.Position.Y + dt * speed * MathF.Sin(_hero.MoveDirection.Value);

            var w = _arena.Size.Width / 2;
            if (x < -w || x > w)
                return;
            var h = _arena.Size.Height / 2;
            if (y < -h || y > h)
                return;

            _hero.Position.Set(x, y);
        }

        public void Stop()
        {
        }
    }
}
