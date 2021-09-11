using System;
using System.Windows;
using System.Windows.Input;
using Kalavarda.Primitives;
using Kalavarda.Primitives.Geometry;
using Room.Core.Models;

namespace Room.Controllers
{
    public class HeroMoveController
    {
        private readonly Hero _hero;
        private readonly Mode _mode;
        private bool _upPressed;
        private bool _downPressed;
        private bool _leftPressed;
        private bool _rightPressed;

        public HeroMoveController(Hero hero, IInputElement uiElement, Mode mode)
        {
            _hero = hero ?? throw new ArgumentNullException(nameof(hero));
            _mode = mode;

            uiElement.KeyDown += UiElement_KeyDown;
            uiElement.KeyUp += UiElement_KeyUp;
        }

        private void UiElement_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Handled)
                return;

            switch (e.Key)
            {
                case Key.W:
                    _upPressed = true;
                    _downPressed = false;
                    e.Handled = true;
                    break;
                case Key.S:
                    _downPressed = true;
                    _upPressed = false;
                    e.Handled = true;
                    break;
                case Key.A:
                    _leftPressed = true;
                    _rightPressed = false;
                    e.Handled = true;
                    break;
                case Key.D:
                    _rightPressed = true;
                    _leftPressed = false;
                    e.Handled = true;
                    break;
            }

            ProcessKeys();
        }

        private void UiElement_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Handled)
                return;

            switch (e.Key)
            {
                case Key.W:
                    _upPressed = false;
                    e.Handled = true;
                    break;
                case Key.S:
                    _downPressed = false;
                    e.Handled = true;
                    break;
                case Key.A:
                    _leftPressed = false;
                    e.Handled = true;
                    break;
                case Key.D:
                    _rightPressed = false;
                    e.Handled = true;
                    break;
            }

            ProcessKeys();
        }

        private void ProcessKeys()
        {
            switch (_mode)
            {
                case Mode.Simple:
                    ProcessSimple(_hero.MoveDirection, _hero.MoveSpeed);
                    break;

                case Mode.ByLook:
                    ProcessByLook(_hero.MoveDirection, _hero.MoveSpeed);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        private void ProcessSimple(AngleF direction, RangeF speed)
        {
            if (_upPressed)
            {
                if (_leftPressed)
                    direction.ValueInDegrees = -135;
                else if (_rightPressed)
                    direction.ValueInDegrees = -45;
                else
                    direction.ValueInDegrees = -90;
            }
            else if (_downPressed)
            {
                if (_leftPressed)
                    direction.ValueInDegrees = 135;
                else if (_rightPressed)
                    direction.ValueInDegrees = 45;
                else
                    direction.ValueInDegrees = 90;
            }
            else if (_leftPressed)
                direction.ValueInDegrees = 180;
            else if (_rightPressed)
                direction.ValueInDegrees = 0;

            if (_upPressed || _downPressed || _leftPressed || _rightPressed)
                speed.SetMax();
            else
                speed.SetMin();
        }

        private void ProcessByLook(AngleF direction, RangeF speed)
        {
            if (_upPressed)
            {
                if (_leftPressed)
                    direction.ValueInDegrees = _hero.LookDirection.ValueInDegrees - 45;
                else if (_rightPressed)
                    direction.ValueInDegrees = _hero.LookDirection.ValueInDegrees + 45;
                else
                    direction.Value = _hero.LookDirection.Value;
            }
            else if (_downPressed)
            {
                if (_leftPressed)
                    direction.ValueInDegrees = _hero.LookDirection.ValueInDegrees - 135;
                else if (_rightPressed)
                    direction.ValueInDegrees = _hero.LookDirection.ValueInDegrees + 135;
                else
                    direction.ValueInDegrees = _hero.LookDirection.ValueInDegrees + 180;
            }
            else if (_leftPressed)
                direction.ValueInDegrees = _hero.LookDirection.ValueInDegrees - 90;
            else if (_rightPressed)
                direction.ValueInDegrees = _hero.LookDirection.ValueInDegrees + 90;

            if (_upPressed || _downPressed || _leftPressed || _rightPressed)
                speed.SetMax();
            else
                speed.SetMin();
        }

        public enum Mode
        {
            Simple,
            ByLook
        }
    }
}
