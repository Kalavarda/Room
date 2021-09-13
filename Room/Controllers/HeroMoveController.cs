using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Kalavarda.Primitives.Geometry;
using Room.Core.Models;

namespace Room.Controllers
{
    public class HeroMoveController: IDisposable
    {
        private readonly Hero _hero;
        private readonly IInputElement _uiElement;
        private readonly Mode _mode;
        private bool _upPressed;
        private bool _downPressed;
        private bool _leftPressed;
        private bool _rightPressed;
        private readonly DispatcherTimer _timer = new DispatcherTimer();

        public HeroMoveController(Hero hero, IInputElement uiElement, Mode mode)
        {
            _hero = hero ?? throw new ArgumentNullException(nameof(hero));
            _uiElement = uiElement;
            _mode = mode;

            uiElement.KeyDown += UiElement_KeyDown;
            uiElement.KeyUp += UiElement_KeyUp;

            _timer.Tick += (sender, e) => ProcessKeys();
            _timer.Start();
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
                    ProcessSimple(_hero.MoveDirection);
                    break;

                case Mode.ByLook:
                    ProcessByLook(_hero.MoveDirection);
                    break;

                default:
                    throw new NotImplementedException();
            }

            SetSpeed();
        }

        private void SetSpeed()
        {
            if (_hero.IsAlive && (_upPressed || _downPressed || _leftPressed || _rightPressed))
                _hero.MoveSpeed.SetMax();
            else
                _hero.MoveSpeed.SetMin();
        }

        private void ProcessSimple(AngleF direction)
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
        }

        private void ProcessByLook(AngleF direction)
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
        }

        public enum Mode
        {
            Simple,
            ByLook
        }

        public void Dispose()
        {
            _timer.Stop();
            _uiElement.KeyDown -= UiElement_KeyDown;
            _uiElement.KeyUp -= UiElement_KeyUp;
        }
    }
}
