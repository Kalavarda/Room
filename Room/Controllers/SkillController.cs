using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Kalavarda.Primitives.Process;
using Kalavarda.Primitives.Skills;

namespace Room.Controllers
{
    public class SkillController: IDisposable
    {
        private readonly ISkilled _hero;
        private readonly IInputElement _uiElement;
        private readonly IProcessor _processor;

        public SkillController(ISkilled hero, IInputElement uiElement, IProcessor processor)
        {
            _hero = hero ?? throw new ArgumentNullException(nameof(hero));
            _uiElement = uiElement ?? throw new ArgumentNullException(nameof(uiElement));
            _processor = processor ?? throw new ArgumentNullException(nameof(processor));

            _uiElement.KeyDown += UiElement_KeyDown;
        }

        private void UiElement_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Handled)
                return;

            switch (e.Key)
            {
                case Key.D1:
                    Use(0);
                    e.Handled = true;
                    break;

                case Key.D2:
                    Use(1);
                    e.Handled = true;
                    break;

                case Key.D3:
                    Use(2);
                    e.Handled = true;
                    break;
            }
        }

        private void Use(int skip)
        {
            var skill = _hero.Skills.Skip(skip).FirstOrDefault();
            var process = skill?.Use(_hero);
            if (process != null)
                _processor.Add(process);
        }

        public void Dispose()
        {
            _uiElement.KeyDown -= UiElement_KeyDown;
        }
    }
}
