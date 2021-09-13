using System;
using System.Collections.Generic;
using System.Diagnostics;
using Kalavarda.Primitives.Utils;
using Kalavarda.Primitives.WPF;
using Room.Core.Abstract;

namespace Room.Controls
{
    public partial class LogControl
    {
        private readonly IList<string> _lines = new List<string>();
        private IGameItemsContainer _itemsContainer;

        public IGameItemsContainer ItemsContainer
        {
            get => _itemsContainer;
            set
            {
                if (_itemsContainer == value)
                    return;

                _itemsContainer = value;

                if (_itemsContainer != null)
                    _itemsContainer.Changed += _itemsContainer_Changed;
            }
        }

        private void _itemsContainer_Changed(IGameItemType type, long count)
        {
            if (count > 0)
                AddLine($"Вы получаете {count.ToStr()} [{type.Name}]");
            else
                AddLine($"Вы теряете {count.ToStr()} [{type.Name}]");
        }

        private void AddLine(string text)
        {
            this.Do(() =>
            {
                Debug.WriteLine(text);

                _lines.Add(text);
                while (_lines.Count > 5)
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
