using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace WPFApp1.Recursos
{
    public static class DragHandler
    {
        private static System.Windows.Point _startPoint;
        private static DragAdorner _dragAdorner;
        private static UIElement _dragVisual;
        private static UIElement _rootVisual;
        public static void AttachDragEvents(UIElement itemContainer)
        {
            itemContainer.PreviewMouseLeftButtonDown += OnMouseLeftButtonDown;
            itemContainer.PreviewMouseMove += OnMouseMove;
        }

        private static void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _startPoint = e.GetPosition(null);
            _dragVisual = sender as UIElement;

            if (_rootVisual == null)
            {
                Window window = Window.GetWindow(_dragVisual);

                if (window != null)
                {
                    _rootVisual = window.Content as UIElement;

                    if (_rootVisual != null)
                    {
                        window.GiveFeedback -= OnGiveFeedback;
                        window.GiveFeedback += OnGiveFeedback;
                    }
                }
            }
        }

        private static void OnMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed || _dragVisual == null || _dragAdorner != null || _rootVisual == null) return;

            System.Windows.Point currentPos = e.GetPosition(null);

            if (System.Math.Abs(currentPos.X - _startPoint.X) > SystemParameters.MinimumHorizontalDragDistance ||
                System.Math.Abs(currentPos.Y - _startPoint.Y) > SystemParameters.MinimumVerticalDragDistance)
            {
                StartDrag(currentPos);
            }
        }

        private static void StartDrag(System.Windows.Point startPosition)
        {
            var frameworkElement = _dragVisual as FrameworkElement;
            if (frameworkElement == null || frameworkElement.DataContext == null) return;
            var dragData = frameworkElement.DataContext;

            var adornerLayer = AdornerLayer.GetAdornerLayer(_rootVisual);

            if (adornerLayer != null)
            {
                _dragAdorner = new DragAdorner(_rootVisual, _dragVisual);
                adornerLayer.Add(_dragAdorner);
                System.Windows.Point currentMousePos = Mouse.GetPosition(_rootVisual);
                _dragAdorner.UpdatePosition(currentMousePos);
            }

            System.Windows.DataObject data = new System.Windows.DataObject();
            data.SetData(dragData.GetType(), dragData);
            System.Windows.DragDrop.DoDragDrop(_dragVisual, data, System.Windows.DragDropEffects.Move);

            if (_dragAdorner != null && adornerLayer != null)
            {
                adornerLayer.Remove(_dragAdorner);
                _dragAdorner = null;
            }
        }

        private static void OnGiveFeedback(object sender, System.Windows.GiveFeedbackEventArgs e)
        {
            if (_dragAdorner != null && _rootVisual != null)
            {
                System.Windows.Point screenPosition = GetCursorPosition();
                System.Windows.Point wpfPoint = _rootVisual.PointFromScreen(screenPosition);

                _dragAdorner.UpdatePosition(wpfPoint);
                e.Handled = true;
            }
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetCursorPos(ref System.Drawing.Point lpPoint);

        private static System.Windows.Point GetCursorPosition()
        {
            var point = new System.Drawing.Point();
            GetCursorPos(ref point);
            return new System.Windows.Point(point.X, point.Y);
        }
    }
}
