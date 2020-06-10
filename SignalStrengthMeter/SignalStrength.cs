using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace WindowWidgets
{

    public enum SignalStrengthLayout
    {
        LeftToRight = 0,
        RightToLeft = 1,
        BottomToTop = 2,
        TopToBottom = 3
    }

    public enum SignalStrengthBackgroundStyle
    {
        Normal,
        Transparent
    }

    [ToolboxItemFilter("Silic0re Controls"), ToolboxBitmap("signalimg.bmp")]
    public partial class SignalStrength : UserControl
    {
        #region Fields

        // Bar Properties
        private int numberOfBars = 5;
        private int barSpacing = 2;
        private int smallBarHeight = 10;
        private int barStepSize = 20;

        //Bar Colors
        private Color goodSignalColor = Color.Green;
        private Color poorSignalColor = Color.Yellow;
        private Color weakSignalColor = Color.Red;
        private Color noSignalColor = Color.White;
        private Color emptyBarColor = Color.Gray;
        private Color centerGradientColor = Color.WhiteSmoke;
        private Color xColor = Color.Red;
        private bool useSolidBars = false;

        //Ranges
        private float goodSignalThreshold = 0.8f;
        private float poorSignalThreshold = 0.5f;
        private float weakSignalThreshold = 0.2f;
        private float noSignalThreshold = 0.0f;

        //Layout/Style
        private SignalStrengthLayout barLayout;
        private SignalStrengthBackgroundStyle backgroundStyle;
        private bool drawXIfNoSignal = true;
        private float xPenWidth = 1.5f;

        //Data
        private float value = 0.0f;
        private float minValue = 0.0f;
        private float maxValue = 1.0f;

        #endregion

        #region Properties

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Bindable(BindableSupport.Yes),
        Category("Appearance"), Description("Draw X over bar graph if no signal is seen, only works if"
            + " BackgroundStyle is set to Normal")]
        public bool XIfNoSignal
        {
            get { return drawXIfNoSignal; }
            set { drawXIfNoSignal = value; this.Invalidate(); }
        }

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Bindable(BindableSupport.Yes),
        Category("Appearance"), Description("The color of the X that is drawn if no signal value")]
        public Color XColor
        {
            get { return xColor; }
            set { xColor = value; this.Invalidate(); }
        }

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Bindable(BindableSupport.Yes),
        Category("Appearance"), Description("Width of the X lines if no signal value")]
        public float XWidth
        {
            get { return xPenWidth; }
            set { xPenWidth = value; this.Invalidate(); }
        }

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Bindable(BindableSupport.Yes),
        Category("Appearance"), Description("Style of the background, if transparent the region is changed"
        + " to allow for true transparency")]
        public SignalStrengthBackgroundStyle BackgroundStyle
        {
            get { return backgroundStyle; }
            set { backgroundStyle = value; CalculateRegion(); this.Invalidate(); }
        }

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Bindable(BindableSupport.Yes),
        Category("Appearance"), Description("True to use a solid fill on the bars, otherwise a gradient fill is used")]
        public bool UseSolidBars
        {
            get { return useSolidBars; }
            set { useSolidBars = value; this.Invalidate(); }
        }

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Bindable(BindableSupport.Yes),
        Category("Appearance"), Description("Layout orientation of the signal bars")]
        public SignalStrengthLayout BarLayout
        {
            get { return barLayout; }
            set { barLayout = value; this.Invalidate(); }
        }

        [Browsable(true), EditorBrowsable( EditorBrowsableState.Always), Bindable( BindableSupport.Yes), 
        Category("Appearance"), Description("Number of bars in the signal monitor")]
        public int NumberOfBars
        {
            get { return numberOfBars; }
            set { numberOfBars = value; this.Invalidate(); }
        }

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Bindable(BindableSupport.Yes), 
        Category("Appearance"), Description("Spacing in pixels between the bars")]
        public int BarSpacing
        {
            get { return barSpacing; }
            set { barSpacing = value; this.Invalidate(); }
        }

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Bindable(BindableSupport.Yes), 
        Category("Appearance"), Description("Height of the smallest bar")]
        public int SmallBarHeight
        {
            get { return smallBarHeight; }
            set { smallBarHeight = value; }
        }

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Bindable(BindableSupport.Yes),
        Category("Appearance"), Description("Number of pixels to add to the length of the bars")]
        public int BarStepSize
        {
            get { return barStepSize; }
            set { barStepSize = value; this.Invalidate(); }
        }

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Bindable(BindableSupport.Yes),
        Category("Appearance"), Description("Color used when the bar is considered empty")]
        public Color GoodSignalColor
        {
            get { return goodSignalColor; }
            set { goodSignalColor = value; this.Invalidate(); }
        }

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Bindable(BindableSupport.Yes),
        Category("Appearance"), Description("Color of the bars when the bar is empty")]
        public Color EmptyBarColor
        {
            get { return emptyBarColor; }
            set { emptyBarColor = value; this.Invalidate(); }
        }

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Bindable(BindableSupport.Yes),
        Category("Appearance"), Description("Color of the bars when the signal is at or above PoorSignalThreshold but"
            + " below GoodSignalThreshold")]
        public Color PoorSignalColor
        {
            get { return poorSignalColor; }
            set { poorSignalColor = value; this.Invalidate(); }
        }

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Bindable(BindableSupport.Yes),
        Category("Appearance"), Description("Color of the bars when the signal is at or above WeakSignalThreshold but"
            + " below PoorSignalThreshold")]
        public Color WeakSignalColor
        {
            get { return weakSignalColor; }
            set { weakSignalColor = value; this.Invalidate(); }
        }

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Bindable(BindableSupport.Yes),
        Category("Appearance"), Description("Color of the bars when the signal is at or below NoSignalThreshold")]
        public Color NoSignalColor
        {
            get { return noSignalColor; }
            set { noSignalColor = value; this.Invalidate(); }
        }

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Bindable(BindableSupport.Yes),
        Category("Appearance"), Description("Color of the center of the bars in the gradient fill")]
        public Color CenterGradientColor
        {
            get { return centerGradientColor; }
            set { centerGradientColor = value; this.Invalidate(); }
        }

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Bindable(BindableSupport.Yes),
        Category("Signal Thresholds"), Description("At or above this level the signal is drawn using the" 
            + " GoodSignalColor")]
        public float GoodSignalThreshold
        {
            get { return goodSignalThreshold; }
            set { goodSignalThreshold = value; this.Invalidate(); }
        }

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Bindable(BindableSupport.Yes),
        Category("Signal Thresholds"), Description("At or above this value, but below GoodSignalThreshold"
            + " the signal is drawn using the PoorSignalColor")]
        public float PoorSignalThreshold
        {
            get { return poorSignalThreshold; }
            set { poorSignalThreshold = value; this.Invalidate(); }
        }

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Bindable(BindableSupport.Yes),
        Category("Signal Thresholds"), Description("At or above this value, but below PoorSignalThreshold"
            + " the signal is drawn using the WeakSignalColor")]
        public float WeakSignalThreshold
        {
            get { return weakSignalThreshold; }
            set { weakSignalThreshold = value; this.Invalidate(); }
        }

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Bindable(BindableSupport.Yes),
        Category("Signal Thresholds"), Description("At or below this value the signal is drawn using the"
            + " NoSignalColor")]
        public float NoSignalThreshold
        {
            get { return noSignalThreshold; }
            set { noSignalThreshold = value; this.Invalidate(); }
        }

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Bindable(BindableSupport.Yes),
        Category("Data"), Description("Value of the signal, used to determine how many bars to fill" 
            + " and what color to use")]
        public float Value
        {
            get { return value; }
            set { this.value = value; this.Invalidate(); }
        }

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Bindable(BindableSupport.Yes),
        Category("Data"), Description("The maximum value of the signal input")]
        public float MaximumValue
        {
            get { return maxValue; }
            set { this.maxValue = value; this.Invalidate(); }
        }

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Bindable(BindableSupport.Yes),
        Category("Data"), Description("The minimum value of the signal input")]
        public float MinimumValue
        {
            get { return minValue; }
            set { this.minValue = value; this.Invalidate(); }
        }

        #endregion

        public SignalStrength()
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.Selectable, false);
            SetStyle(ControlStyles.UserPaint, true);
        }

        private void CalculateRegion()
        {
            if (backgroundStyle == SignalStrengthBackgroundStyle.Normal)
            {
                GraphicsPath gp = new GraphicsPath();
                gp.AddRectangle(ClientRectangle);
                this.Region = new Region(gp);
            }
            else
            {
                GraphicsPath myGP = new GraphicsPath();

                float startX = 0.0f;
                float startY = 0.0f;
                float stepX = 0.0f;
                float stepY = 0.0f;
                float barWidth = 0.0f;
                float barHeight = 0.0f;
                RectangleF barRect;
                float calcBarStep = 0.0f;
                float barHeightStep = 0.0f;

                if (barLayout == SignalStrengthLayout.LeftToRight)
                {
                    calcBarStep = (this.Height - smallBarHeight) / numberOfBars;
                    startX = barSpacing / 2.0f;
                    barWidth = this.Width / numberOfBars - barSpacing;
                    if (barWidth <= 0)
                        barWidth = 1;
                    startY = this.Height - smallBarHeight;
                    stepX = barWidth + barSpacing;
                    stepY = -calcBarStep;
                    barHeight = smallBarHeight;
                    barHeightStep = calcBarStep;
                }
                else if (barLayout == SignalStrengthLayout.RightToLeft)
                {
                    calcBarStep = (this.Height - smallBarHeight) / numberOfBars;

                    barWidth = this.Width / numberOfBars - barSpacing;
                    startX = this.Width - (barSpacing / 2.0f) - barWidth;
                    if (barWidth <= 0)
                        barWidth = 1;
                    startY = this.Height - smallBarHeight;
                    stepX = -(barWidth + barSpacing);
                    stepY = -calcBarStep;
                    barHeight = smallBarHeight;
                    barHeightStep = calcBarStep;
                }
                else if (barLayout == SignalStrengthLayout.TopToBottom)
                {
                    calcBarStep = (this.Width - smallBarHeight) / numberOfBars;
                    barWidth = this.Height / numberOfBars - barSpacing;
                    if (barWidth <= 0)
                        barWidth = 0;

                    startX = 0;
                    startY = 0;
                    stepX = 0;
                    stepY = barWidth + barSpacing;
                    barHeight = smallBarHeight;
                    barHeightStep = calcBarStep;
                }
                else
                {
                    calcBarStep = (this.Width - smallBarHeight) / numberOfBars;
                    barWidth = this.Height / numberOfBars - barSpacing;
                    if (barWidth <= 0)
                        barWidth = 0;

                    startX = 0;
                    startY = this.Height - barWidth;
                    stepX = 0;
                    stepY = -(barWidth + barSpacing);
                    barHeight = smallBarHeight;
                    barHeightStep = calcBarStep;
                }

                for (int i = 0; i < numberOfBars; i++)
                {
                    if (barLayout == SignalStrengthLayout.LeftToRight || barLayout == SignalStrengthLayout.RightToLeft)
                    {
                        barRect = new RectangleF(startX, startY, barWidth, barHeight);
                    }
                    else
                        barRect = new RectangleF(startX, startY, barHeight, barWidth);

                    myGP.AddRectangle(barRect);

                    startX += stepX;
                    startY += stepY;
                    barHeight += calcBarStep;
                }

                this.Region = new Region(myGP);
            }
        }

        #region Hidden Properties

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public new Image BackgroundImage
        {
            get { return base.BackgroundImage; }
            set { base.BackgroundImage = value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public new ImageLayout BackgroundImageLayout
        {
            get { return base.BackgroundImageLayout; }
            set { base.BackgroundImageLayout = value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public new Font Font
        {
            get { return base.Font; }
            set { base.Font = value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public new Color ForeColor
        {
            get { return base.ForeColor; }
            set { base.ForeColor = value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public new RightToLeft RightToLeft
        {
            get { return base.RightToLeft; }
            set { base.RightToLeft = value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public new bool UseWaitCursor
        {
            get { return base.UseWaitCursor; }
            set { UseWaitCursor = value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public new bool AllowDrop
        {
            get { return base.AllowDrop; }
            set { base.AllowDrop = value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public new AutoScaleMode AutoScaleMode
        {
            get { return base.AutoScaleMode; }
            set { base.AutoScaleMode = value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public new bool AutoScroll
        {
            get { return base.AutoScroll; }
            set { base.AutoScroll = value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public new Size AutoScrollMargin
        {
            get { return base.AutoScrollMargin; }
            set { base.AutoScrollMargin = value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public new Size AutoScrollMinSize
        {
            get { return base.AutoScrollMinSize; }
            set { base.AutoScrollMinSize = value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public new Point AutoScrollOffset
        {
            get { return base.AutoScrollOffset; }
            set { base.AutoScrollOffset = value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public new Point AutoScrollPosition
        {
            get { return base.AutoScrollPosition; }
            set { base.AutoScrollPosition = value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public new bool AutoSize
        {
            get { return base.AutoSize; }
            set { base.AutoSize = value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public new AutoSizeMode AutoSizeMode
        {
            get { return base.AutoSizeMode; }
            set { base.AutoSizeMode = value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public new ImeMode ImeMode
        {
            get { return base.ImeMode; }
            set { base.ImeMode = value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public new bool TabStop
        {
            get { return base.TabStop; }
            set { base.TabStop = value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public new int TabIndex
        {
            get { return base.TabIndex; }
            set { base.TabIndex = value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public new AutoValidate AutoValidate
        {
            get { return base.AutoValidate; }
            set { base.AutoValidate = value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public new bool CausesValidation
        {
            get { return base.CausesValidation; }
            set { base.CausesValidation = value; }
        }

        #endregion

        #region Public Methods

        public Image DrawToImage()
        {
            Bitmap retVal = new Bitmap(this.Width, this.Height);
            PaintEventArgs e = new PaintEventArgs(Graphics.FromImage(retVal), ClientRectangle);
            e.Graphics.Clear(BackColor);
            OnPaint(e);
            return retVal;
        }

        #endregion

        #region Private Methods

        protected override void OnResize(EventArgs e)
        {
            CalculateRegion();
            base.OnResize(e);
        }

        #endregion

        #region Painting

        protected override void OnPaint(PaintEventArgs e)
        {
            Color barColor = noSignalColor;

            if (value >= goodSignalThreshold)
                barColor = goodSignalColor;

            if (value >= poorSignalThreshold && value < goodSignalThreshold)
                barColor = poorSignalColor;

            if (value >= weakSignalThreshold && value < poorSignalThreshold)
                barColor = weakSignalColor;

            float startX = 0.0f;
            float startY = 0.0f;
            float stepX = 0.0f;
            float stepY = 0.0f;
            float barWidth = 0.0f;
            float barHeight = 0.0f;
            RectangleF barRect;
            float calcBarStep = 0.0f;
            float barHeightStep = 0.0f;

            if (barLayout == SignalStrengthLayout.LeftToRight)
            {
                calcBarStep = (this.Height - smallBarHeight) / numberOfBars;
                startX = barSpacing / 2.0f;
                barWidth = this.Width / numberOfBars - barSpacing;
                if (barWidth <= 0)
                    barWidth = 1;
                startY = this.Height - smallBarHeight;
                stepX = barWidth + barSpacing;
                stepY = -calcBarStep;
                barHeight = smallBarHeight;
                barHeightStep = calcBarStep;
            }
            else if (barLayout == SignalStrengthLayout.RightToLeft)
            {
                calcBarStep = (this.Height - smallBarHeight) / numberOfBars;
  
                barWidth = this.Width / numberOfBars - barSpacing;
                startX = this.Width - (barSpacing / 2.0f) - barWidth;
                if (barWidth <= 0)
                    barWidth = 1;
                startY = this.Height - smallBarHeight;
                stepX = -(barWidth + barSpacing);
                stepY = -calcBarStep;
                barHeight = smallBarHeight;
                barHeightStep = calcBarStep;
            }
            else if (barLayout == SignalStrengthLayout.TopToBottom)
            {
                calcBarStep = (this.Width - smallBarHeight) / numberOfBars;
                barWidth = this.Height / numberOfBars - barSpacing;
                if (barWidth <= 0)
                    barWidth = 0;

                startX = 0;
                startY = 0;
                stepX = 0;
                stepY = barWidth + barSpacing;
                barHeight = smallBarHeight;
                barHeightStep = calcBarStep;
            }
            else
            {
                calcBarStep = (this.Width - smallBarHeight) / numberOfBars;
                barWidth = this.Height / numberOfBars - barSpacing;
                if (barWidth <= 0)
                    barWidth = 0;

                startX = 0;
                startY = this.Height - barWidth;
                stepX = 0;
                stepY = -(barWidth + barSpacing);
                barHeight = smallBarHeight;
                barHeightStep = calcBarStep;
            }

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.High;

            float barPercentageStep = 1.0f / numberOfBars;
            float currentBarPercent = 0.0f;// barPercentageStep;
            float currentValue = (value - minValue) / (maxValue - minValue);

            if (currentValue > 1)
                currentValue = 1.0f;

            if (currentValue < 0)
                currentValue = 0.0f;

            for (int i = 0; i < numberOfBars; i++)
            {
                if (barLayout == SignalStrengthLayout.LeftToRight || barLayout == SignalStrengthLayout.RightToLeft)
                {
                    barRect = new RectangleF(startX, startY, barWidth, barHeight);
                }
                else
                    barRect = new RectangleF(startX, startY, barHeight, barWidth);

                if (currentValue >= currentBarPercent)
                    DrawBar(g, barRect, barColor);
                else
                    DrawBar(g, barRect, emptyBarColor);

                currentBarPercent += barPercentageStep;

                startX += stepX;
                startY += stepY;
                barHeight += barHeightStep;
            }

            if (value <= noSignalThreshold && drawXIfNoSignal)
            {
                //We just need to draw a big red X from extreme to extreme
                Pen xPen = new Pen(xColor, xPenWidth);
                g.DrawLine(xPen, 0.0f, 0.0f, this.Width, this.Height);
                g.DrawLine(xPen, this.Width, 0.0f, 0.0f, this.Height);
                xPen.Dispose();
            }

        }

        private void DrawBar(Graphics g, RectangleF barRect, Color barMainColor)
        {
            //Bars are drawn very simply by filling the rectangle, we
            //just need to figure out the linear gradient
            LinearGradientBrush lgb;
            RectangleF lgbRect;
            Color gradColor = centerGradientColor;

            if (useSolidBars)
                gradColor = barMainColor;

            if (barLayout == SignalStrengthLayout.LeftToRight || barLayout == SignalStrengthLayout.RightToLeft)
            {
                lgbRect = new RectangleF(barRect.X, barRect.Y, barRect.Width / 2.0f, barRect.Height);
                lgb = new LinearGradientBrush(lgbRect, barMainColor, gradColor, 0.0f);
                lgb.WrapMode = WrapMode.TileFlipX;
            }
            else
            {
                lgbRect = new RectangleF(barRect.X, barRect.Y, barRect.Width, barRect.Height / 2.0f);
                lgb = new LinearGradientBrush(lgbRect, barMainColor, gradColor, 90.0f);
                lgb.WrapMode = WrapMode.TileFlipX;
            }

            g.FillRectangle(lgb, barRect);
            lgb.Dispose();
        }

        #endregion
    }
}
