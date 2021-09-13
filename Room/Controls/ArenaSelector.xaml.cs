using System.Linq;
using Room.Core.Abstract;
using Room.Core.Models;

namespace Room.Controls
{
    public partial class ArenaSelector
    {
        private IArenaFactory _arenaFactory;

        public Arena SelectedArena => (Arena)_listBox.SelectedItem;

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

        public void RefreshArenas()
        {
            var arenas = Enumerable.Range(1, 50)
                .Select(i => (ushort) i)
                .OrderByDescending(lvl => lvl)
                .Select(lvl => _arenaFactory.Create(lvl))
                .ToArray();
            _listBox.ItemsSource = arenas;
            _listBox.SelectedItem = arenas.Last();
            _listBox.ScrollIntoView(_listBox.SelectedItem);
        }

        public ArenaSelector()
        {
            InitializeComponent();
        }
    }
}
