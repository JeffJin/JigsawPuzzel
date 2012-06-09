using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace MediaJigsaw.Helpers
{
    public static class CanvasAssistant
    {
        // Fields
        public static readonly DependencyProperty BoundChildrenProperty = DependencyProperty.RegisterAttached("BoundChildren", typeof(object), typeof(CanvasAssistant), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(CanvasAssistant.onBoundChildrenChanged)));

        // Methods
        private static void onBoundChildrenChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            Canvas canvas;
            if (dependencyObject != null)
            {
                canvas = dependencyObject as Canvas;
                if (canvas != null)
                {
                    ObservableCollection<UIElement> objects = (ObservableCollection<UIElement>)e.NewValue;
                    if (objects == null)
                    {
                        canvas.Children.Clear();
                    }
                    else
                    {
                        objects.CollectionChanged += delegate(object sender, NotifyCollectionChangedEventArgs args)
                        {
                            if (args.Action == NotifyCollectionChangedAction.Add)
                            {
                                foreach (object item in args.NewItems)
                                {
                                    canvas.Children.Add((UIElement)item);
                                }
                            }
                            if (args.Action == NotifyCollectionChangedAction.Remove)
                            {
                                foreach (object item in args.OldItems)
                                {
                                    canvas.Children.Remove((UIElement)item);
                                }
                            }
                        };
                        foreach (UIElement item in objects)
                        {
                            canvas.Children.Add(item);
                        }
                    }
                }
            }
        }

        public static void SetBoundChildren(DependencyObject dependencyObject, string value)
        {
            dependencyObject.SetValue(BoundChildrenProperty, value);
        }
    }


}
