using System;
using System.Collections.Generic;
using System.Linq;
using Kalavarda.Primitives.Process;
using Kalavarda.Primitives.Skills;
using Room.Core.Models;

namespace Room.Processes
{
    public class BossProcess : IProcess, IIncompatibleProcess
    {
        private readonly Boss _boss;
        private readonly Game _game;
        private readonly IProcessor _processor;

        private bool wait = true;

        public event Action<IProcess> Completed;

        public BossProcess(Boss boss, Game game, IProcessor processor)
        {
            _boss = boss ?? throw new ArgumentNullException(nameof(boss));
            _game = game ?? throw new ArgumentNullException(nameof(game));
            _processor = processor ?? throw new ArgumentNullException(nameof(processor));
        }

        public void Process(TimeSpan delta)
        {
            if (_game.Hero.IsDead)
            {
                Completed?.Invoke(this);
                return;
            }

            var distance = _boss.Position.DistanceTo(_game.Hero.Position);

            //// TODO: сделать нормально
            //if (distance > 5)
            //    wait = false;
            //if (wait)
            //    return;

            var skill = _boss.GetReadySkills()
                .Where(sk => sk.MaxDistance >= distance)
                .OrderByDescending(sk => sk.TimeLimiter.Interval)
                .FirstOrDefault();
            var skillProcess = skill?.Use(_boss);
            if (skillProcess != null)
                _processor.Add(skillProcess);
        }

        public void Stop()
        {
            Completed?.Invoke(this);
        }

        public IReadOnlyCollection<IProcess> GetIncompatibleProcesses(IReadOnlyCollection<IProcess> processes)
        {
            return processes.OfType<BossProcess>().ToArray();
        }
    }
}
