using System.Windows.Controls;
using System.Windows.Media;

namespace MediaJigsaw.Models.Pieces
{
    public abstract class VideoJigsawPieceBase : JigsawPieceBase, IVideoJigsawPiece
    {
        protected VideoJigsawPieceBase(MediaElement mediaElement, int col, int row, double pieceSize)
            :base(col, row, pieceSize)
        {
            this.VideoSource = mediaElement;
        }

        protected override void InitShapeProperties()
        {
            base.Stroke = new SolidColorBrush(Colors.DarkOliveGreen);
            base.StrokeThickness = 1.0;
            base.Width = this.PieceSize*3;
            base.Height = this.PieceSize*3;
            var videoBrush = new VisualBrush
                                        {
                                            Visual = this.VideoSource,
                                            Stretch = Stretch.None,
                                            Viewport = this.CreateViewport(),
                                            ViewboxUnits = BrushMappingMode.Absolute,
                                            Viewbox = this.CreateViewbox(),
                                            ViewportUnits = BrushMappingMode.Absolute
                                        };
            base.Fill = videoBrush;
            this.Data = this.CreateGeometry();
            base.SetValue(Panel.ZIndexProperty, 10);
        }
        
        public MediaElement VideoSource { get; set; }
    }
}
