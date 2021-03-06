using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Kalavarda.Primitives.Abstract;
using Kalavarda.Primitives.Skills;
using Kalavarda.Primitives.WPF;
using Kalavarda.Primitives.WPF.Abstract;
using Kalavarda.Primitives.WPF.Controllers;

namespace Room.Controllers
{
    public class ChildItemsController
    {
        private readonly Panel _panel;
        private readonly IChildUiElementFactory _uiElementFactory;
        private readonly IDictionary<IChildItem, UIElement> _uiElements = new ConcurrentDictionary<IChildItem, UIElement>();
        private readonly IDictionary<IChildItem, PositionController> _positionController = new ConcurrentDictionary<IChildItem, PositionController>();

        public ChildItemsController(IChildItemsContainer childItemsContainer, IChildUiElementFactory uiElementFactory, Panel panel)
        {
            _uiElementFactory = uiElementFactory ?? throw new ArgumentNullException(nameof(uiElementFactory));
            _panel = panel ?? throw new ArgumentNullException(nameof(panel));

            childItemsContainer.ChildItemAdded += ChildItemsOwner_ChildItemAdded;
            childItemsContainer.ChildItemRemoved += ChildItemsOwner_ChildItemRemoved;
        }

        private void ChildItemsOwner_ChildItemAdded(IChildItemsContainer itemsContainer, IChildItem childItem)
        {
            _panel.Do(() =>
            {
                var uiElement = _uiElementFactory.Create(childItem);
                _uiElements.Add(childItem, uiElement);
                _panel.Children.Add(uiElement);

                if (childItem is IHasBounds hasBounds)
                    if (uiElement is FrameworkElement fe)
                    {
                        var posController = new PositionController(fe, hasBounds.Bounds);
                        _positionController.Add(childItem, posController);
                    }
            });
        }

        private void ChildItemsOwner_ChildItemRemoved(IChildItemsContainer itemsContainer, IChildItem childItem)
        {
            _panel.Do(() =>
            {
                var uiElement = _uiElements[childItem];
                _uiElements.Remove(childItem);
                _panel.Children.Remove(uiElement);

                var posController = _positionController[childItem];
                _positionController.Remove(childItem);
                posController.Dispose();
            });
        }
    }
}
