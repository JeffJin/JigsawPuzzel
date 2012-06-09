using System;
using System.Windows;

namespace MediaJigsaw.Helpers
{
    internal static class RoutedEventHelper
    {
        // Methods
        internal static void AddHandler(DependencyObject element, RoutedEvent routedEvent, Delegate handler)
        {
            UIElement uie = element as UIElement;
            if (uie != null)
            {
                uie.AddHandler(routedEvent, handler);
            }
            else
            {
                ContentElement ce = element as ContentElement;
                if (ce != null)
                {
                    ce.AddHandler(routedEvent, handler);
                }
            }
        }

        internal static void RaiseEvent(DependencyObject target, RoutedEventArgs args)
        {
            if (target is UIElement)
            {
                (target as UIElement).RaiseEvent(args);
            }
            else if (target is ContentElement)
            {
                (target as ContentElement).RaiseEvent(args);
            }
        }

        internal static void RemoveHandler(DependencyObject element, RoutedEvent routedEvent, Delegate handler)
        {
            UIElement uie = element as UIElement;
            if (uie != null)
            {
                uie.RemoveHandler(routedEvent, handler);
            }
            else
            {
                ContentElement ce = element as ContentElement;
                if (ce != null)
                {
                    ce.RemoveHandler(routedEvent, handler);
                }
            }
        }
    }

}
