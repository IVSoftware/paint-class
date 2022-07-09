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
        }
        private void buttonDiag_Click(object sender, EventArgs e)
        {
            _paint.DrawDiagonal(GetNextTestColor(), offsetX: 25 * _testCountDiag++);
        }
        private void buttonLine_Click(object sender, EventArgs e)
        {
            var offsetY = 15 * _testCountLine++;
            _paint.Drawline(
                GetNextTestColor(),
                new PointF(0, 100 + offsetY),
                new PointF(ClientRectangle.Width, 100 + offsetY));
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            _paint.Clear();
            _testCountDiag = _testCountLine = 0;
        }

        int _testCountDiag = 0;
        int _testCountLine = 0;
        PaintClass _paint = new PaintClass();
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            _paint.PaintAll(e.Graphics);
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            Refresh();
        }

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
