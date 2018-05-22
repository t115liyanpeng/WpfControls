using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace WpfControls.UserControls
{
    /// <summary>
    /// MarqueeText.xaml 的交互逻辑
    /// </summary>
    public partial class MarqueeText
    {


        private DoubleAnimation moveAnimation;

        private double time = 1;

        private double width = 0;

        public MarqueeText()
        {
            InitializeComponent();
            Body.Width = MaxTextWidth;
            this.Loaded += MarqueeText_Loaded;

        }

        protected void MarqueeText_Loaded(object sender, RoutedEventArgs e)
        {

            this.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                try
                {
                    #region<<滚动动画>>

                //获取字体大小
                double size = Block.FontSize;
                //Body.Height = size * 2;
                //double fontwidth = Block.Text.Length * size;
                fontwidth = GetFontSize(Block).Width;
                //Block.Width = fontwidth;
                if (MaxTextWidth < fontwidth)
                {
                    //将要移动的长度
                    double move = -(GetMoveLength(MaxTextWidth, fontwidth));
                    time = Math.Abs(move)/Speed;
                    moveAnimation = new DoubleAnimation
                    {
                        From = 0,
                        To = move,
                        Duration = TimeSpan.FromSeconds(time),
                        RepeatBehavior = RepeatBehavior.Forever,
                        DecelerationRatio = 0.9
                    };
                    Storyboard.SetTargetProperty(moveAnimation,
                        new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));
                    Storyboard.SetTarget(moveAnimation, Block);
                    //Storyboard story = new Storyboard();
                    story.Children.Add(moveAnimation);
                    if (fontwidth > MaxTextWidth)
                    {
                        story.Begin();
                    }
                }

                #endregion
                }
                catch (Exception ex)
                {
                    
                }
            }));


        }

        public static readonly DependencyProperty MaxTextWidthProperty = DependencyProperty.Register(
            "MaxTextWidth", typeof(int), typeof(MarqueeText), new PropertyMetadata(100));

        public int MaxTextWidth
        {
            get { return (int)GetValue(MaxTextWidthProperty); }
            set { SetValue(MaxTextWidthProperty, value); }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(MarqueeText), new PropertyMetadata(default(string)));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty SpeedProperty = DependencyProperty.Register(
            "Speed", typeof(double), typeof(MarqueeText), new PropertyMetadata(1.0));

        public double Speed
        {
            get { return (double)GetValue(SpeedProperty); }
            set { SetValue(SpeedProperty, value); }
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == MaxTextWidthProperty)
            {
                Body.Width = MaxTextWidth;

            }
            else if (e.Property == TextProperty)
            {

                Block.Text = Text;
                //width = GetFontSize(Block).Width - Block.ActualHeight;
            }
            //else if (e.Property == SpeedProperty)
            //{

            //    time = Math.Abs(width) / Speed;
            //}
        }

        private double GetMoveLength(double controllength, double stringlength)
        {

            if (controllength <= 0 || stringlength <= 0)
            {
                return 0;
            }
            if (stringlength > controllength)
            {
                double p = stringlength / controllength;

                if (p <= 1)
                {
                    return controllength;
                }


                return controllength * (p) - controllength;

            }
            else
            {
                return controllength;
            }
        }

        protected Size GetFontSize(TextBlock text)
        {
            var formattedText = new FormattedText(
            Block.Text,
            CultureInfo.CurrentUICulture,
            FlowDirection.LeftToRight,
            new Typeface(Block.FontFamily, Block.FontStyle, Block.FontWeight, Block.FontStretch),
            Block.FontSize,
            Brushes.Black);
            Size size = new Size(formattedText.Width, formattedText.Height);
            return size;
        }
    }
}
