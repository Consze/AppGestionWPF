using System.Windows.Media;
using System.Windows.Documents;
using System.Windows;

namespace WPFApp1.Recursos
{
    public class DragAdorner : Adorner
    {
        private readonly VisualBrush _visualBrush;
        private System.Windows.Point _position;

        public DragAdorner(UIElement adornedElement, UIElement dragVisual)
            : base(adornedElement)
        {
            _visualBrush = new VisualBrush(dragVisual)
            {
                Opacity = 0.7
            };
            IsHitTestVisible = false;
        }

        public void UpdatePosition(System.Windows.Point newPosition)
        {
            _position = newPosition;
            InvalidateVisual();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            double drawX = _position.X - (AdornedElement.RenderSize.Width / 2);
            double drawY = _position.Y - (AdornedElement.RenderSize.Height / 2);

            drawingContext.DrawRectangle(_visualBrush, null, new Rect(
                drawX,
                drawY,
                AdornedElement.RenderSize.Width,
                AdornedElement.RenderSize.Height));
        }

        protected override System.Windows.Size MeasureOverride(System.Windows.Size constraint) => AdornedElement.RenderSize;
        protected override System.Windows.Size ArrangeOverride(System.Windows.Size finalSize) => finalSize;
    }
}
