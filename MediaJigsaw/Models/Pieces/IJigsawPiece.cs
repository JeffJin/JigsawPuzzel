using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Animation;

namespace MediaJigsaw.Models.Pieces
{
    public interface IJigsawPiece : INotifyPropertyChanged, IInputElement, IAnimatable
    {
        int CurrentColumn { get; set; }
        int CurrentRow { get; set; }
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
