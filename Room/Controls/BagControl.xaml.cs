using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Kalavarda.Primitives.Abstract;
using Kalavarda.Primitives.WPF;
using Room.Core.Abstract;
using Room.Core.Models;

namespace Room.Controls
{
    public partial class BagControl
    {
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
                    _hero.Bag.Changed += Bag_Changed;
                    Bag_Changed(null, 0);
                }
            }
        }

        private void Bag_Changed(IHasName itemType, long count)
        {
            this.Do(() =>
            {
                _itemsControl.ItemsSource = _hero.Bag.AllTypes.OfType<IHasImage>();
            });
        }

        public IGameItemType SelectedItemType { get; private set; }

        public BagControl()
        {
            InitializeComponent();
        }

        private void OnEquipClick(object sender, RoutedEventArgs e)
        {
            if (SelectedItemType is IEquipmentItem equipItem)
                if (!_hero.Equipment.IsEquiped(equipItem))
                    if (_hero.Equipment.TryEquip(equipItem))
                        _hero.Bag.TryChangeCount(equipItem, -1);
        }

        private void OnContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            if (SelectedItemType is IEquipmentItem equipItem)
            {
                _miEquip.IsEnabled = true;
                _miEquip.IsChecked = _hero.Equipment.IsEquiped(equipItem);
            }
            else
                _miEquip.IsEnabled = false;
        }

        private void _itemsControl_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var point = e.GetPosition(_itemsControl);
            var frameworkElement = (FrameworkElement)System.Windows.Media.VisualTreeHelper.HitTest(this, point).VisualHit;
            while (!(frameworkElement is BagItemControl) && frameworkElement.Parent != null)
                frameworkElement = frameworkElement.Parent as FrameworkElement;
            SelectedItemType = frameworkElement is BagItemControl bagItemControl ? bagItemControl.ItemType : null;
        }
    }
}
