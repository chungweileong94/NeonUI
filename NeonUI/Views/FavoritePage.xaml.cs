using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using NeonUI.Extendsions;
using System.Numerics;

namespace NeonUI.Views
{
    public sealed partial class FavoritePage : Page
    {
        public ObservableCollection<string> ItemCollection { get; set; }

        private Compositor compositor;

        public FavoritePage()
        {
            this.InitializeComponent();

            ItemCollection = new ObservableCollection<string>();

            for (int i = 0; i < 20; i++)
            {
                ItemCollection.Add($"Item {i}");
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            Visual headerVisual = ElementCompositionPreview.GetElementVisual(HeaderGrid);

            headerVisual.Offset = new Vector3(0, -300f, 0);
            headerVisual.Opacity = 0;

            var headerOffsetAnimation = compositor.CreateVector3KeyFrameAnimation();
            headerOffsetAnimation.InsertKeyFrame(1f, new Vector3(0, 0, 0));
            headerOffsetAnimation.Duration = TimeSpan.FromMilliseconds(1000);

            var headerFadeAnimation = compositor.CreateScalarKeyFrameAnimation();
            headerFadeAnimation.InsertKeyFrame(1f, 1);
            headerFadeAnimation.Duration = TimeSpan.FromMilliseconds(900);

            headerVisual.StartAnimation("Offset", headerOffsetAnimation);
            headerVisual.StartAnimation("Opacity", headerFadeAnimation);
        }

        private void ItemListView_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            if (!args.InRecycleQueue)
            {
                args.ItemContainer.Loaded += ItemContainer_Loaded;
            }

            args.Handled = true;
        }

        private void ItemContainer_Loaded(object sender, RoutedEventArgs e)
        {
            var listViewItem = sender as ListViewItem;
            var itemsPanel = ItemListView.ItemsPanelRoot as ItemsStackPanel;
            var index = ItemListView.IndexFromContainer(listViewItem);

            if (index >= itemsPanel.FirstVisibleIndex && index <= itemsPanel.LastVisibleIndex)
            {
                Grid grid = listViewItem.ContentTemplateRoot.FindVisualChild<Grid>();
                float width = (float)grid.RenderSize.Width;
                float height = (float)grid.RenderSize.Height;

                Visual visual = ElementCompositionPreview.GetElementVisual(grid);
                visual.Opacity = 0;
                visual.CenterPoint = new Vector3(width / 2, height / 2, 0);
                visual.Scale = new Vector3(.05f, .05f, 0);
                visual.Offset = new Vector3(0, 300f, 0);

                var fadeAnimation = compositor.CreateScalarKeyFrameAnimation();
                fadeAnimation.Duration = TimeSpan.FromMilliseconds(500);
                fadeAnimation.DelayTime = TimeSpan.FromMilliseconds(index * 100);
                fadeAnimation.InsertKeyFrame(0, 0);
                fadeAnimation.InsertKeyFrame(1f, 1f);
                visual.StartAnimation(nameof(visual.Opacity), fadeAnimation);

                var scaleAnimation = compositor.CreateVector3KeyFrameAnimation();
                scaleAnimation.Duration = TimeSpan.FromMilliseconds(600);
                scaleAnimation.DelayTime = TimeSpan.FromMilliseconds(100 * index);
                scaleAnimation.InsertKeyFrame(1f, new Vector3(.05f, .05f, 0));
                scaleAnimation.InsertKeyFrame(1f, new Vector3(1f, 1f, 0));
                visual.StartAnimation(nameof(visual.Scale), scaleAnimation);

                var offsetAnimation = compositor.CreateVector3KeyFrameAnimation();
                offsetAnimation.Duration = TimeSpan.FromMilliseconds(600);
                offsetAnimation.DelayTime = TimeSpan.FromMilliseconds(100 * index);
                offsetAnimation.InsertKeyFrame(1f, new Vector3(0, 300f, 0));
                offsetAnimation.InsertKeyFrame(1f, new Vector3(0, 0, 0));
                visual.StartAnimation(nameof(visual.Offset), offsetAnimation);
            }

            listViewItem.Loaded -= ItemContainer_Loaded;
        }
    }
}
