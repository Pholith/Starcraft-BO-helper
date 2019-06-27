using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Starcraft_BO_helper
{
    static class DepedencyObjectExtension
    {
        public static IEnumerable<T> SelectAllRecursively<T>(this IEnumerable<T> items, Func<T, IEnumerable<T>> func)
        {
            return (items ?? Enumerable.Empty<T>()).SelectMany(o => new[] { o }.Concat(SelectAllRecursively(func(o), func)));
        }


        public static IEnumerable<DependencyObject> FindChildren(this DependencyObject obj)
        {
            return Enumerable.Range(0, VisualTreeHelper.GetChildrenCount(obj))
                .Select(i => VisualTreeHelper.GetChild(obj, i));
        }


        public static IEnumerable<DependencyObject> FindAllChildren(this DependencyObject obj)
        {
            return obj.FindChildren().SelectAllRecursively(o => o.FindChildren());
        }

    }
}
