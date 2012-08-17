using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MediaJigsaw.Models.Pieces
{
    public abstract class ImageJigsawPieceBase : JigsawPieceBase, IImageJigsawPiece
    {
        protected ImageJigsawPieceBase(BitmapImage bitmapImage, int col, int row, double pieceSize) 
            : base(col, row, pieceSize)
        {
            this.ImageSource = bitmapImage;
        }

        protected override void InitShapeProperties()
        {
            base.Stroke = new SolidColorBrush(Colors.DarkOliveGreen);
            base.StrokeThickness = 1.0;
            base.Width = this.PieceSize * 3;
            base.Height = this.PieceSize * 3;
            var imageBrush = new ImageBrush
                                 {
                                     ImageSource = this.ImageSource,
                                     Stretch = Stretch.None,
                                     Viewport = this.CreateViewport(),
                                     ViewboxUnits = BrushMappingMode.Absolute,
                                     Viewbox = this.CreateViewbox(),
                                     ViewportUnits = BrushMappingMode.Absolute
                                 };
            base.Fill = imageBrush;
            this.Data = this.CreateGeometry();
            base.SetValue(Panel.ZIndexProperty, 10);
        }

        public override void Clear()
        {
            this.ImageSource = null;
            base.Clear();
        }

        public BitmapImage ImageSource { get; set; }
    }
}
