using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Kalavarda.Primitives.Process;
using Kalavarda.Primitives.Skills;
using Kalavarda.Primitives.WPF.Skills;

namespace Room.Controllers
{
    public class SkillController: IDisposable
    {
        private readonly ISkilled _hero;
        private readonly UIElement _uiElement;
        private readonly IProcessor _processor;
        private readonly ISkillBinds _skillBinds;

        public SkillController(ISkilled hero, UIElement uiElement, IProcessor processor, ISkillBinds skillBinds)
        {
            _hero = hero ?? throw new ArgumentNullException(nameof(hero));
            _uiElement = uiElement ?? throw new ArgumentNullException(nameof(uiElement));
            _processor = processor ?? throw new ArgumentNullException(nameof(processor));
            _skillBinds = skillBinds ?? throw new ArgumentNullException(nameof(skillBinds));

            _uiElement.KeyDown += UiElement_KeyDown;
            _uiElement.MouseDown += UiElement_MouseDown;
        }

        private void UiElement_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Handled)
                return;

            var bind = _skillBinds.SkillBinds.FirstOrDefault(sb => sb.MouseButton == e.ChangedButton);
            if (bind != null)
                UseBind(e, bind);
        }

        private void UiElement_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Handled)
                return;

            var bind = _skillBinds.SkillBinds.FirstOrDefault(sb => sb.Key == e.Key);
            if (bind != null)
                UseBind(e, bind);
        }

        private void UseBind(RoutedEventArgs e, SkillBind bind)
        {
            var skill = _skillBinds.GetSkill(bind.SkillKey);
            if (skill != null)
            {
                e.Handled = true;
                var process = skill.Use(_hero);
                if (process != null)
                    _processor.Add(process);
            }
        }

        public void Dispose()
        {
            _uiElement.MouseDown -= UiElement_MouseDown;
            _uiElement.KeyDown -= UiElement_KeyDown;
        }
    }
}
