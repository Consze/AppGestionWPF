using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Documents;
using System.Runtime.InteropServices;
using System.Drawing;

namespace WPFApp1.Recursos
{
    public class DragSourceBehavior : Behavior<UIElement>
    {
        public static readonly DependencyProperty DragDataProperty =
            DependencyProperty.Register("DragData", typeof(object), typeof(DragSourceBehavior));

        public object DragData
        {
            get => GetValue(DragDataProperty);
            set => SetValue(DragDataProperty, value);
        }

        private System.Windows.Point _startPoint;
        private DragAdorner _dragAdorner;
        private UIElement _dragVisual;
        private UIElement _rootVisual;

        protected override void OnAttached()
        {
            base.OnAttached();
            _dragVisual = AssociatedObject;

            _rootVisual = System.Windows.Application.Current.MainWindow.Content as UIElement;

            _dragVisual.PreviewMouseLeftButtonDown += OnMouseLeftButtonDown;
            _dragVisual.PreviewMouseMove += OnMouseMove;

            if (System.Windows.Application.Current.MainWindow != null)
            {
                System.Windows.Application.Current.MainWindow.GiveFeedback += OnGiveFeedback;
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            _dragVisual.PreviewMouseLeftButtonDown -= OnMouseLeftButtonDown;
            _dragVisual.PreviewMouseMove -= OnMouseMove;

            if (System.Windows.Application.Current.MainWindow != null)
            {
                System.Windows.Application.Current.MainWindow.GiveFeedback -= OnGiveFeedback;
            }
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _startPoint = e.GetPosition(null);
        }

        private void OnMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed || DragData == null || _dragAdorner != null || _rootVisual == null) return;

            System.Windows.Point currentPos = e.GetPosition(null);

            if (Math.Abs(currentPos.X - _startPoint.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(currentPos.Y - _startPoint.Y) > SystemParameters.MinimumVerticalDragDistance)
            {
                StartDrag(currentPos);
            }
        }

        private void StartDrag(System.Windows.Point startPosition)
        {
            var adornerLayer = AdornerLayer.GetAdornerLayer(_rootVisual);

            if (adornerLayer != null)
            {
                _dragAdorner = new DragAdorner(_rootVisual, _dragVisual);
                adornerLayer.Add(_dragAdorner);

                _dragAdorner.UpdatePosition(startPosition);
            }

            System.Windows.DataObject data = new System.Windows.DataObject();
            data.SetData(DragData.GetType(), DragData);

            DragDrop.DoDragDrop(_dragVisual, data, System.Windows.DragDropEffects.Move);

            if (_dragAdorner != null && adornerLayer != null)
            {
                adornerLayer.Remove(_dragAdorner);
                _dragAdorner = null;
            }
        }

        private void OnGiveFeedback(object sender, System.Windows.GiveFeedbackEventArgs e)
        {
            if (_dragAdorner != null)
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

        private System.Windows.Point GetCursorPosition()
        {
            var point = new System.Drawing.Point();
            GetCursorPos(ref point);
            return new System.Windows.Point(point.X, point.Y);
        }
    }
}

