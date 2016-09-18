using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Graphics.Canvas.Effects;
using Windows.UI.Composition;
using Windows.UI.Xaml.Hosting;

namespace Weather
{
    public sealed partial class MainPage : Page
    {
        GaussianBlurEffect blurEffect = new GaussianBlurEffect()
        {
            Name = "Blur",
            BorderMode = EffectBorderMode.Hard, // NB: default mode here isn't supported yet.
            BlurAmount = 10.0f,
            Source = new CompositionEffectSourceParameter("source")
        };

        #region gaussian blur

        private void SizeVisual(SpriteVisual effectVisual,Grid name)
        {
            if (effectVisual != null)
            {
                effectVisual.Size = new System.Numerics.Vector2(
                    (float)name.ActualWidth, (float)name.ActualHeight);
            }
        }

        /*
         * Gaussian blur effect
         */
        private void OnLoaded(object sender, RoutedEventArgs args)
        {
            Grid[] _list = { rightBottomBlur, LeftBottomBlur, splitBlur };
            for (int i = 0; i < _list.Length; i++)
            {
                BlurList.listOfGrid[i] = _list[i];
                //Windows.UI.Composition.Visual
                var gridVisual = ElementCompositionPreview.GetElementVisual(BlurList.listOfGrid[i]);
                var compositor = gridVisual.Compositor;
                BlurList.listOftVisual[i] = compositor.CreateSpriteVisual();

                SizeVisual(BlurList.listOftVisual[i], BlurList.listOfGrid[i]);

                var effectFactory = compositor.CreateEffectFactory(
                    blurEffect,
                    new string[] { "Blur.BlurAmount" }
                    );
                var effectBrush = effectFactory.CreateBrush();
                effectBrush.SetSourceParameter("source", compositor.CreateBackdropBrush());
                BlurList.listOftVisual[i].Brush = effectBrush;

                ElementCompositionPreview.SetElementChildVisual(BlurList.listOfGrid[i], BlurList.listOftVisual[i]);
            }


        }

        /*
         * leftBottomBlur and rightBottomBlur use same size changed event
         */
        private void OnGridSizeChanged(object sender, SizeChangedEventArgs e)
        {
            for (int i = 0; i < BlurList.listOfGrid.Length; i++)
            {
                SizeVisual(BlurList.listOftVisual[i], BlurList.listOfGrid[i]);
            }

        }

        #endregion

        public class BlurList
        {
            public static Grid[] listOfGrid = new Grid[3];
            public static SpriteVisual[] listOftVisual = new SpriteVisual[3];
        }
    }
}
