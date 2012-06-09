using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace MediaJigsaw.Helpers
{
    public static class JigsawPieceAnimation
    {
        // Fields
        public static readonly RoutedEvent MovePieceEvent = EventManager.RegisterRoutedEvent("MovePiece",
                                                                                             RoutingStrategy.Bubble,
                                                                                             typeof (RoutedEventHandler),
                                                                                             typeof (
                                                                                                 JigsawPieceAnimation));

        // Methods
        internal static RoutedEventArgs RaiseAlarmEvent(DependencyObject target)
        {
            if (target == null)
            {
                return null;
            }
            var routedEventArgs = new RoutedEventArgs
                                                  {
                                                      RoutedEvent = MovePieceEvent
                                                  };
            if (target is UIElement)
            {
                (target as UIElement).RaiseEvent(routedEventArgs);
            }
            else if (target is ContentElement)
            {
                (target as ContentElement).RaiseEvent(routedEventArgs);
            }
            return routedEventArgs;
        }
    }
}
