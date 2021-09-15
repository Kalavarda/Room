using System;
using System.Collections.Generic;
using System.Diagnostics;
using Kalavarda.Primitives.Utils;
using Kalavarda.Primitives.WPF;
using Room.Core.Abstract;
using Room.Core.Models;

namespace Room.Controls
{
    public partial class LogControl
    {
        private readonly IList<string> _lines = new List<string>();
        private Hero _hero;

        public Hero Hero
        {
            get => _hero;
            set
            {
                if (_hero == value)
                    return;

                _hero = value;

                if (_hero != null)
                {
                    _hero.Bag.Changed += ItemsContainer_Changed;
                    _hero.LevelChanged += Hero_LevelChanged;
                }
            }
        }

        private void Hero_LevelChanged()
        {
            AddLine($"Вы получаете уровень {_hero.Level}");
        }

        private void ItemsContainer_Changed(IGameItemType type, long count)
        {
            if (count > 0)
                AddLine($"Вы получаете {count.ToStr()} [{type.Name}]");
            else
                AddLine($"Вы теряете {(-count).ToStr()} [{type.Name}]");
        }

        private void AddLine(string text)
        {
            this.Do(() =>
            {
                Debug.WriteLine(text);

                _lines.Add(text);
                while (_lines.Count > 10)
                    _lines.RemoveAt(0);
                _tb.Text = string.Join(Environment.NewLine, _lines);
            });
        }

        public LogControl()
        {
            InitializeComponent();
        }
    }
}
