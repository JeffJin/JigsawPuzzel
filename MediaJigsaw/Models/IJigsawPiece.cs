using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace MediaJigsaw.Models
{
    public interface IJigsawPiece : INotifyPropertyChanged, IInputElement, IAnimatable
    {
        int CurrentColumn { get; set; }
        int CurrentRow { get; set; }
        BitmapImage ImageSource { get; set; }
        int OriginColumn { get; }
        int OriginRow { get; }
        int PieceSize { get; }
        Point Position { get; set; }
        Point Origin { get; }
        void Clear();

        //Framework Interfaces
        void SetValue(DependencyProperty key, object value);
        object GetValue(DependencyProperty dp);
    }
}
