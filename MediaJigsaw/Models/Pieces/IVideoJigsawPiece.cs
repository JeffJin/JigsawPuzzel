using System.Windows.Controls;

namespace MediaJigsaw.Models.Pieces
{
    public interface IVideoJigsawPiece : IJigsawPiece
    {
        MediaElement VideoSource { get; set; }
    }
}