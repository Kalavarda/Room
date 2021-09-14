using System;
using System.Linq;
using Room.Core.Abstract;
using Room.Core.Models;

namespace Room.Controls
{
    public partial class ArenaSelector
    {
        private IArenaFactory _arenaFactory;

        public Arena SelectedArena
        {
            get
            {
                var wrapper = (ArenaWrapper) _listBox.SelectedItem;
                if (!wrapper.IsEnabled)
                    return null;
                return wrapper.Arena;
            }
        }

        public IArenaFactory ArenaFactory
        {
            get => _arenaFactory;
            set
            {
                if (_arenaFactory == value)
                    return;

                _arenaFactory = value;

                if (_arenaFactory != null)
                    RefreshArenas();
            }
        }

        public Hero Hero { get; set; }

        public void RefreshArenas()
        {
            var arenas = Enumerable.Range(1, Hero.MaxLevel)
                .Select(i => (ushort) i)
                .OrderByDescending(lvl => lvl)
                .Select(lvl => new ArenaWrapper(_arenaFactory.Create(lvl), Hero.Level + 1 >= lvl))
                .ToArray();
            _listBox.ItemsSource = arenas;
            _listBox.SelectedItem = arenas.Last();
            _listBox.ScrollIntoView(_listBox.SelectedItem);
        }

        public ArenaSelector()
        {
            InitializeComponent();
        }

        public class ArenaWrapper
        {
            public Arena Arena { get; }

            public bool IsEnabled { get; }

            public ArenaWrapper(Arena arena, bool isEnabled)
            {
                Arena = arena ?? throw new ArgumentNullException(nameof(arena));
                IsEnabled = isEnabled;
            }

            public override string ToString()
            {
                var s = "Уровень " + Arena.Boss.Level;
                if (!IsEnabled)
                    s += " (недоступно)";
                return s;
            }
        }
    }
}
