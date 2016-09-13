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

        SpriteVisual effectVisual;
        SpriteVisual effectVisual2;

        private void SizeVisual()
        {
            if (this.effectVisual != null)
            {
                this.effectVisual.Size = new System.Numerics.Vector2(
                    (float)this.rightBottomBlur.ActualWidth, (float)this.rightBottomBlur.ActualHeight);
            }
            if (this.effectVisual2 != null)
            {
                this.effectVisual2.Size = new System.Numerics.Vector2(
                    (float)this.leftBottomBlur.ActualWidth, (float)this.leftBottomBlur.ActualHeight);
            }
        }
        /*
         * Gaussian blur effect
         */
        private void OnLoaded(object sender, RoutedEventArgs args)
        {

            //Windows.UI.Composition.Visual
            var gridVisual = ElementCompositionPreview.GetElementVisual(this.rightBottomBlur);
            var compositor = gridVisual.Compositor;
            effectVisual = compositor.CreateSpriteVisual();

            //Windows.UI.Composition.Visual
            var gridVisual2 = ElementCompositionPreview.GetElementVisual(this.leftBottomBlur);
            var compositor2 = gridVisual2.Compositor;
            effectVisual2 = compositor.CreateSpriteVisual();


            this.SizeVisual();


            var effectFactory = compositor.CreateEffectFactory(
                blurEffect,
                new string[] { "Blur.BlurAmount" }
                );
            var effectBrush = effectFactory.CreateBrush();
            effectBrush.SetSourceParameter("source", compositor.CreateBackdropBrush());
            effectVisual.Brush = effectBrush;


            var effectFactory2 = compositor2.CreateEffectFactory(
                blurEffect,
                new string[] { "Blur.BlurAmount" }
                );
            var effectBrush2 = effectFactory2.CreateBrush();
            effectBrush2.SetSourceParameter("source", compositor2.CreateBackdropBrush());
            effectVisual2.Brush = effectBrush2;

            ElementCompositionPreview.SetElementChildVisual(this.rightBottomBlur, this.effectVisual);
            ElementCompositionPreview.SetElementChildVisual(this.leftBottomBlur, this.effectVisual2);


        }
        /*
         * leftBottomBlur and rightBottomBlur use same size changed event
         */
        private void OnGridSizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.SizeVisual();
        }

        #endregion
    }
}
