using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MediaJigsaw.Models.Pieces
{
    public abstract class JigsawPieceBase : Shape, IJigsawPiece
    {
        protected static readonly double MaxImageSize = 800.0;

        // Fields
        private int _currentColumn;
        private int _currentRow;
        protected int _originColumn;
        protected int _originRow;
        private int _pieceSize;
        public static readonly DependencyProperty DataProperty;

        // Events
        public event PropertyChangedEventHandler PropertyChanged;

        protected JigsawPieceBase(int col, int row, double pieceSize)
        {
            this.OriginColumn = col;
            this.OriginRow = row;
            this.CurrentColumn = col;
            this.CurrentRow = row;
            this.PieceSize = (int)pieceSize;
        }

        //clean up previous setup for this image piece
        public virtual void Clear()
        {
            base.Fill = null;
            this.Data = null;

            this.OriginColumn = -1;
            this.OriginRow = -1;
            this.CurrentColumn = -1;
            this.CurrentRow = -1;
            this.PieceSize = 0;
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

        /// <summary>
        /// Initialize image or video sources to create visual brush or image brush
        /// </summary>
        protected abstract void InitShapeProperties();

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
        /// <summary>
        /// Original position
        /// </summary>
        public abstract Point Origin { get; }

        /// <summary>
        /// Current position of the piece
        /// </summary>
        public abstract Point Position { get; set; }
    }
}
