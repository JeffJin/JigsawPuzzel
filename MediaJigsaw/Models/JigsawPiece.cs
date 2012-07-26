using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MediaJigsaw.Models
{
    public abstract class JigsawPiece : Shape, IJigsawPiece
    {
        protected static readonly double MaxImageSize = 800.0;

        // Fields
        private int _currentColumn;
        private int _currentRow;
        private int _originColumn;
        private int _originRow;
        private int _pieceSize;
        public static readonly DependencyProperty DataProperty;

        // Events
        public event PropertyChangedEventHandler PropertyChanged;

        protected JigsawPiece()
        {
            
        }

        //clean up previous setup for this image piece
        public void Clear()
        {
            this.ImageSource = null;
            base.Fill = null;
            this.Data = null;

            this.OriginColumn = -1;
            this.OriginRow = -1;
            this.CurrentColumn = -1;
            this.CurrentRow = -1;
            this.PieceSize = 0;
        }

        // Methods
        protected JigsawPiece(BitmapImage imageSource, int col, int row, double pieceSize)
        {
            this.ImageSource = imageSource;
            this.OriginColumn = col;
            this.OriginRow = row;
            this.CurrentColumn = col;
            this.CurrentRow = row;
            this.PieceSize = (int)pieceSize;
            this.InitShapeProperties();
        }

        protected abstract Geometry CreateGeometry();
        protected abstract Rect CreateViewbox();
        protected abstract Rect CreateViewport();
        protected void FirePropertyChanged(string property)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        private void InitShapeProperties()
        {
            base.Stroke = new SolidColorBrush(Colors.DarkOliveGreen);
            base.StrokeThickness = 1.0;
            base.Width = this.PieceSize*3;
            base.Height = this.PieceSize*3;
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

        // Properties
        public int CurrentColumn
        {
            get
            {
                return this._currentColumn;
            }
            set
            {
                this._currentColumn = value;
            }
        }

        public int CurrentRow
        {
            get
            {
                return this._currentRow;
            }
            set
            {
                this._currentRow = value;
            }
        }

        public Geometry Data { get; set; }

        protected override Geometry DefiningGeometry
        {
            get
            {
                return this.CreateGeometry();
            }
        }

        public BitmapImage ImageSource { get; set; }

        public int OriginColumn
        {
            get
            {
                return this._originColumn;
            }
            private set
            {
                this._originColumn = value;
            }
        }

        public int OriginRow
        {
            get
            {
                return this._originRow;
            }
            private set
            {
                this._originRow = value;
            }
        }

        public int PieceSize
        {
            get
            {
                return this._pieceSize;
            }
            private set
            {
                this._pieceSize = value;
            }
        }
    }
}
