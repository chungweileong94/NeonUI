using System;
using System.Collections.Generic;
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

namespace NeonUI.Views
{
    public sealed partial class HomePage : Page
    {
        public HomePage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Compositor compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            Visual headerVisual = ElementCompositionPreview.GetElementVisual(HeaderGrid);

            headerVisual.Offset = new System.Numerics.Vector3(0, -300f, 0);
            headerVisual.Opacity = 0;

            var headerOffsetAnimation = compositor.CreateVector3KeyFrameAnimation();
            headerOffsetAnimation.InsertKeyFrame(1f, new System.Numerics.Vector3(0, 0, 0));
            headerOffsetAnimation.Duration = TimeSpan.FromMilliseconds(1000);

            var headerFadeAnimation = compositor.CreateScalarKeyFrameAnimation();
            headerFadeAnimation.InsertKeyFrame(1f, 1);
            headerFadeAnimation.Duration = TimeSpan.FromMilliseconds(900);

            headerVisual.StartAnimation("Offset", headerOffsetAnimation);
            headerVisual.StartAnimation("Opacity", headerFadeAnimation);


            Visual contentVisual = ElementCompositionPreview.GetElementVisual(ContentGrid);

            contentVisual.Offset = new System.Numerics.Vector3(0, 300f, 0);
            contentVisual.Opacity = 0;

            var contentOffsetAnimation = compositor.CreateVector3KeyFrameAnimation();
            contentOffsetAnimation.InsertKeyFrame(1f, new System.Numerics.Vector3(0, 0, 0));
            contentOffsetAnimation.Duration = TimeSpan.FromMilliseconds(1000);

            var contentFadeAnimation = compositor.CreateScalarKeyFrameAnimation();
            contentFadeAnimation.InsertKeyFrame(1f, 1);
            contentFadeAnimation.Duration = TimeSpan.FromMilliseconds(900);

            contentVisual.StartAnimation("Offset", contentOffsetAnimation);
            contentVisual.StartAnimation("Opacity", contentFadeAnimation);
        }
    }
}
