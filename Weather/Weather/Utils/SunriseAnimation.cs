using System;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Weather
{
    public sealed partial class MainPage : Page
    {
        private void sunAnimatePath()
        {
            double radius = 100;
            double timeStep = 0.01;
            double angle = 180;
            Point center = new Point(radius, radius);
            //One or more DiscreteObjectKeyFrame object elements that define the key frames for the animation
            var progressAnimation = new ObjectAnimationUsingKeyFrames();
            Storyboard.SetTarget(progressAnimation, progressPath);
            Storyboard.SetTargetProperty(progressAnimation, "(Path.Data)");

            //need 300 points so multiplier should be 100
            double multiplier = 3;
            Point initialPoint = new Point(Math.Cos(Math.PI * (60 / (multiplier * angle) - 1)) * radius + center.X, Math.Sin(Math.PI * (60 / (multiplier * angle) - 1)) * radius + center.Y);

            for (int i = 60; i <= angle * multiplier - 60; i++)
            {
                var discreteObjectKeyFrame = new DiscreteObjectKeyFrame();
                discreteObjectKeyFrame.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(i * timeStep));

                // create points for each ArcSegment
                Point firstArcPoint = initialPoint;
                Point secondArcPoint = new Point()
                {
                    //Parameter equation for Ellipse 
                    X = Math.Cos(Math.PI * (i / (multiplier * angle) - 1)) * radius + center.X,
                    Y = Math.Sin(Math.PI * (i / (multiplier * angle) - 1)) * radius + center.Y
                };

                string dataValue = "m {0},{1} A {2},{2} 0 0 1 {3},{4} A {2},{2} 0 0 1 {5},{6}";
                discreteObjectKeyFrame.Value = string.Format(dataValue, initialPoint.X, initialPoint.Y, radius, firstArcPoint.X, firstArcPoint.Y, secondArcPoint.X, secondArcPoint.Y);
                progressAnimation.KeyFrames.Add(discreteObjectKeyFrame);
            }
            var storyboard = new Storyboard();
            storyboard.Children.Add(progressAnimation);
            storyboard.Begin();
        }
    }
}
