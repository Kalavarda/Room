using System.Windows.Media.Imaging;
using Kalavarda.Primitives.Abstract;
using Room.Core.Abstract;

namespace Room.Controls
{
    public partial class BagItemControl
    {
        private IGameItemType _itemType;

        public IGameItemType ItemType
        {
            get => _itemType;
            private set
            {
                if (_itemType == value)
                    return;

                _itemType = value;

                if (_itemType == value)
                {
                    if (_itemType is IHasImage hasImage)
                    {
                        _image.Source = new BitmapImage(hasImage.ImageUri);
                    }
                }
            }
        }

        public BagItemControl()
        {
            InitializeComponent();

            DataContextChanged += BagItemControl_DataContextChanged;
        }

        private void BagItemControl_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            ItemType = DataContext as IGameItemType;
        }
    }
}
