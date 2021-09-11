using System;
using Kalavarda.Primitives.Process;
using Room.Core.Models;

namespace Room.Processes
{
    public class HeroMoveProcess: IProcess
    {
        private readonly Hero _hero;
        public event Action<IProcess> Completed;

        public HeroMoveProcess(Hero hero)
        {
            _hero = hero ?? throw new ArgumentNullException(nameof(hero));
        }

        public void Process(TimeSpan delta)
        {
            var speed = _hero.MoveSpeed.Value;
            if (speed > 0)
            {
                var dt = (float)delta.TotalSeconds;
                var x = _hero.Position.X + dt * speed * MathF.Cos(_hero.MoveDirection.Value);
                var y = _hero.Position.Y + dt * speed * MathF.Sin(_hero.MoveDirection.Value);
                _hero.Position.Set(x, y);
            }
        }

        public void Stop()
        {
        }
    }
}
