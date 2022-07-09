using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace paint_class
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            _paint.Refresh += (sender, e) =>
            {
                // Causes the control to repaint.
                Refresh();
                Text = CurrentTestColor.ToString();
            };

            // Draw diagonal line adding 25 to offset each time.
            buttonDiag.Click += (sender, e) =>
                _paint.DrawDiagonal(GetNextTestColor(), offsetX: 25 * _testCountDiag++);

            buttonLine.Click += (sender, e) =>
            {
                var offsetY = 25 * _testCountLine++;
                _paint.Drawline(
                    GetNextTestColor(),
                    new PointF(0, 100 + offsetY),
                    new PointF(ClientRectangle.Width, 100 + offsetY));
            };

            buttonClear.Click += (sender, e) =>
            {
                _paint.Clear();
                _testCountDiag = _testCountLine = 0;
            };
        }
        int _testCountDiag = 0;
        int _testCountLine = 0;
        PaintClass _paint = new PaintClass();
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            _paint.PaintAll(e.Graphics, always: _always);
            _always = false;
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if(!_always)
            {
                _always = true;
                Refresh();
            }
        }
        bool _always = false;

        internal Color GetNextTestColor()
        {
            CurrentTestColor = _knownColors[_randomColorForTest.Next(_knownColors.Length)];
            return CurrentTestColor; // For convenience
        }

        // T E S T I N G
        Color[] _knownColors { get; } =
                Enum.GetValues(typeof(KnownColor))
                .Cast<KnownColor>()
                .Select(known => Color.FromKnownColor(known))
                .ToArray();
        public Color CurrentTestColor { get; private set; }
        Random _randomColorForTest = new Random();
    }
}
