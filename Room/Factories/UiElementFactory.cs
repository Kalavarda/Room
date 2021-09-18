using System.Windows;
using Kalavarda.Primitives.Skills;
using Kalavarda.Primitives.WPF.Abstract;
using Room.Controls;
using Room.Core.Skills;

namespace Room.Factories
{
    public class UiElementFactory: IChildUiElementFactory
    {
        public UIElement Create(IChildItem childItem)
        {
            if (childItem is Fireball fireball)
                return new FireballControl(fireball);

            if (childItem is RoundArea area)
                return new AreaControl(area);

            if (childItem is Bomb bomb)
                return new BombControl(bomb);

            throw new System.NotImplementedException();
        }
    }
}
