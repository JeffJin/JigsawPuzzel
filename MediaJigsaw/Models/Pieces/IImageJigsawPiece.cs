using System.Windows.Media.Imaging;

namespace MediaJigsaw.Models.Pieces
{
    public interface IImageJigsawPiece : IJigsawPiece
    {
        BitmapImage ImageSource { get; set; }
    }
}