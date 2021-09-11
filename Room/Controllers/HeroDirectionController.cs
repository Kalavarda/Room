using System;
using System.Windows;
using Room.Core.Models;

namespace Room.Controllers
{
    public class HeroDirectionController
    {
        private readonly Hero _hero;
        private readonly UIElement _uiElement;

        public HeroDirectionController(Hero hero, UIElement uiElement)
        {
            _hero = hero ?? throw new ArgumentNullException(nameof(hero));
            _uiElement = uiElement ?? throw new ArgumentNullException(nameof(uiElement));

            _uiElement.MouseMove += _uiElement_MouseMove;
        }

        private void _uiElement_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.Handled)
                return;

            var mousePos = e.GetPosition(_uiElement);
            var dx = (float)mousePos.X - _hero.Position.X;
            var dy = (float)mousePos.Y - _hero.Position.Y;
            _hero.LookDirection.Value = MathF.Atan2(dy, dx);

            e.Handled = true;
        }
    }
}
