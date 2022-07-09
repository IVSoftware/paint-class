using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace paint_class
{
    class PaintClass : List<PaintClassContext>
    {
        public new void Add(PaintClassContext context)
        {
            base.Add(context);
            _modified = true;
            Refresh?.Invoke(this, EventArgs.Empty);
        }
        public event EventHandler Refresh;
        private bool _modified = false;
        public void Drawline(Color color, PointF start, PointF end) =>
            Add(new PaintClassContext
            {
                PaintOp = PaintOp.DrawLine,
                Color = color,
                Start = start,
                End = end,
            });

        public new void Clear()
        {
            base.Clear();
            Add(new PaintClassContext
            {
                PaintOp = PaintOp.Clear,
            });
        }
        public void DrawDiagonal(Color color, float offsetX = 0, float offsetY = 0) =>
            Add(new PaintClassContext
            {
                PaintOp = PaintOp.DrawDiagonal,
                Color = color,
                OffsetX = offsetX,
                OffsetY = offsetY,
            });

        private void paint(Graphics graphics, PaintClassContext context)
        {
            switch (context.PaintOp)
            {
                case PaintOp.DrawDiagonal:
                    using (var pen = new Pen(context.Color, 2f))
                    {
                        var rect = graphics.VisibleClipBounds;
                        var diagonalStart = new PointF(
                                rect.X + context.OffsetX,
                                rect.Y + context.OffsetY);
                        var diagonalEnd = new PointF(
                                rect.X + rect.Width + context.OffsetX,
                                rect.Y + rect.Height + context.OffsetY);
                        graphics.DrawLine(pen, diagonalStart, diagonalEnd);
                    }
                    break;
                case PaintOp.DrawLine:
                    using (var pen = new Pen(context.Color, 2f))
                    {
                        graphics.DrawLine(pen, context.Start, context.End);
                    }
                    break;
                case PaintOp.Clear:
                    using (var brush = new SolidBrush(Color.Transparent))
                    {
                        graphics.FillRectangle(brush, graphics.VisibleClipBounds);
                    }
                    base.Clear(); // Remove the clear action
                    break;
                default:
                    throw new NotImplementedException("ToDo");
            }
        }

        internal void PaintAll(Graphics graphics, bool always = true)
        {
            if (always || _modified)
            {
                foreach (var context in this.ToArray())
                {
                    paint(graphics, context);
                }
            }
            _modified = false;
        }
    }
    enum PaintOp{ DrawLine, Clear, DrawDiagonal}
    class PaintClassContext
    {
        public PaintOp PaintOp { get; set; }
        public Color Color { get; set; }
        public PointF Start { get; set; }
        public PointF End { get; set; }
        public float OffsetX { get; set; }
        public float OffsetY { get; set; }
    }
}
