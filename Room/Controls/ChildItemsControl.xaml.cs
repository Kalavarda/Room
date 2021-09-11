using Kalavarda.Primitives.WPF.Abstract;

namespace Room.Controls
{
    public partial class ChildItemsControl
    {
        private IChildUiElementFactory _uiElementFactory;

        public IChildUiElementFactory UiElementFactory
        {
            get => _uiElementFactory;
            set
            {
                if (_uiElementFactory == value)
                    return;

                _uiElementFactory = value;
            }
        }

        public ChildItemsControl()
        {
            InitializeComponent();
        }
    }
}
