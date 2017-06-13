using System;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Brochure.WPF.Client
{
    public class Attach
    {
        private static RoutedEvent _event = ButtonBase.ClickEvent;
        private static Delegate _handle;
        public static readonly DependencyProperty AttachsEventProperty =
            DependencyProperty.RegisterAttached("AttachsEvent", typeof(RoutedEvent), typeof(Attach),
                new FrameworkPropertyMetadata(ButtonBase.ClickEvent, OnRouteEventChanged));

        public static readonly DependencyProperty AttachsHandleProperty =
            DependencyProperty.RegisterAttached("AttachsHandle", typeof(Delegate), typeof(Attach),
                new FrameworkPropertyMetadata(new RoutedEventHandler(DefaultDelegate), OnHandleChanged));

        public static RoutedEvent GetAttachsEvent(DependencyObject d)
        {
            return (RoutedEvent)d.GetValue(AttachsEventProperty);
        }

        public static void SetAttachsEvent(DependencyObject d, RoutedEvent value)
        {
            d.SetValue(AttachsEventProperty, value);
        }

        public static Delegate GetAttachsHandle(DependencyObject d)
        {
            return (Delegate)d.GetValue(AttachsHandleProperty);
        }

        public static void SetAttachsHandle(DependencyObject d, Delegate value)
        {
            d.SetValue(AttachsHandleProperty, value);
        }

        private static void OnRouteEventChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            UIElement ui = sender as UIElement;
            if (_event != null && _handle != null)
                ui?.RemoveHandler(_event, _handle);
            _event = (RoutedEvent)e.NewValue;
            if (_event != null && _handle != null)
                ui?.AddHandler(_event, _handle);
        }
        private static void OnHandleChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            UIElement ui = sender as UIElement;
            if (_event != null && _handle != null)
                ui?.RemoveHandler(_event, _handle);
            _handle = (RoutedEventHandler)e.NewValue;
            if (_handle == null)
                throw new Exception("事件没有绑定值");
            if (_event != null && _handle != null)
                ui?.AddHandler(_event, _handle);
        }

        private static void DefaultDelegate(object sender, RoutedEventArgs e)
        {
            //做初始值使用
        }
    }
}
