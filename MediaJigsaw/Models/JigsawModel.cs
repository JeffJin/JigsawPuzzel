using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MediaJigsaw.Helpers;
using MediaJigsaw.Infrastructure;
using Microsoft.Win32;

namespace MediaJigsaw.Models
{
    public class JigsawModel : PropertyChangedImplementation
    {
        // Fields
        private Dictionary<double, string> _availableSizes;
        private int _columns;
        private CommandMap _commands;
        private bool _enableReplayPuzzelButton;
        private bool _enableShowImageButton;
        private BitmapImage _imageSource;
        private string _info;
        private bool _isDragging;
        private double _leftLimit;
        private double _lowerLimit;
        private IList<IJigsawPiece> _pieces;
        private double _pieceSize;
        private double _rightLimit;
        private int _rows;
        private IList<IJigsawPiece> _shadowPieces;
        private Visibility _showImageSource;
        private Visibility _showPictureButton;
        private Visibility _showPuzzelButton;
        private Visibility _showPuzzelCanvas;
        private string _sourceFileName;
        private double _upperLimit;
        private const double Speed = 1000.0;
        public readonly int SupportedImageHeight = 800;
        public readonly int SupportedImageWidth = 800;

        public JigsawModel()
        {
            this.InitCommands();
            this.InitProperties();
        }

        //check if the pieces are in the correct position
        private bool CheckPieces()
        {
            foreach (JigsawPiece jigsawPiece in this.Pieces)
            {
                if ((jigsawPiece.CurrentRow != jigsawPiece.OriginRow) ||
                    (jigsawPiece.CurrentColumn != jigsawPiece.OriginColumn))
                {
                    return false;
                }
            }
            return true;
        }

        public static JigsawModel CreateModel()
        {
            return CreateModel(null);
        }

        internal static JigsawModel CreateModel(MainWindow mainWindow)
        {
            var model = new JigsawModel {Window = mainWindow};
            return model;
        }

        /// <summary>
        /// Destroy image file reference to prevent memory leaking
        /// </summary>
        private void DestroyImageReferences()
        {
            for (var i = this.Window.Canvas.Children.Count - 1; i >= 0; i--)
            {
                if (this.Window.Canvas.Children[i] is JigsawPiece)
                {
                    JigsawPiece p = (JigsawPiece)this.Window.Canvas.Children[i];
                    p.MouseDown -= new MouseButtonEventHandler(this.Piece_MouseDown);
                    p.MouseMove -= new MouseEventHandler(this.Piece_MouseMove);
                    p.MouseUp -= new MouseButtonEventHandler(this.Piece_MouseUp);
                    p.Clear();
                    this.Window.Canvas.Children.Remove(p);
                }
            }

            this.Window.Canvas.Children.Clear();

            for (var i = this.Pieces.Count - 1; i >= 0; i--)
            {
                this.Pieces[i].Clear();
            }

            this.Pieces.Clear();
            this.ImageSource = null;
        }

        public IList<IJigsawPiece> CreatePuzzle(Stream streamSource)
        {
            using (var wrapper = new WrappingStream(streamSource))
            {
                using (var reader = new BinaryReader(wrapper))
                {
                    BitmapImage imageSource = new BitmapImage();
                    imageSource.BeginInit();
                    imageSource.CacheOption = BitmapCacheOption.OnLoad;
                    imageSource.StreamSource = reader.BaseStream;
                    imageSource.EndInit();
                    imageSource.Freeze();
                    this.ImageSource = imageSource;
                }
            }
            var pieces = new List<IJigsawPiece>();
//            for (int row = 0; row < this._rows; row++)
//            {
//                for (int col = 0; col < this._columns; col++)
//                {
                    IJigsawPiece jigsawPiece = JigsawPieceFactory.Create(this.ImageSource, 0, 0, this.PieceSize, PieceType.PolyBezier);
                    pieces.Add(jigsawPiece);
                    jigsawPiece = JigsawPieceFactory.Create(this.ImageSource, 0, 1, this.PieceSize, PieceType.PolyBezier);
                    pieces.Add(jigsawPiece);
                    jigsawPiece = JigsawPieceFactory.Create(this.ImageSource, 0, 2, this.PieceSize, PieceType.PolyBezier);
                    pieces.Add(jigsawPiece);
                    jigsawPiece = JigsawPieceFactory.Create(this.ImageSource, 0, 3, this.PieceSize, PieceType.PolyBezier);
                    pieces.Add(jigsawPiece);
                    jigsawPiece = JigsawPieceFactory.Create(this.ImageSource, 1, 0, this.PieceSize, PieceType.PolyBezier);
                    pieces.Add(jigsawPiece);
                    jigsawPiece = JigsawPieceFactory.Create(this.ImageSource, 1, 1, this.PieceSize, PieceType.PolyBezier);
                    pieces.Add(jigsawPiece);
//                }
//            }
            this.Pieces = pieces;
            //this.Pieces = JigsawHelper.ScramblePieces(pieces, this._rows, this._columns);
            foreach (JigsawPiece piece in this.Pieces)
            {
                this.InsertPiece(this.Window.Canvas, piece);
            }
            return this.Pieces;
        }

        private IList<IJigsawPiece> CreatePuzzle(string streamFileName)
        {
            using (Stream streamSource = this.LoadImage(streamFileName))
            {
                this.Pieces = this.CreatePuzzle(streamSource);
            }
            this.EnableShowImageButton = true;
            this.EnableReplayPuzzelButton = true;
            return this.Pieces;
        }

        private IJigsawPiece FindPieceByMousePosition(Point point)
        {
            int targetRow = (int) (point.Y/this.PieceSize);
            int targetCol = (int) (point.X/this.PieceSize);
            return
                this.Pieces.SingleOrDefault<IJigsawPiece>(
                    p => ((p.CurrentColumn == targetCol) && (p.CurrentRow == targetRow)));
        }

        private IJigsawPiece FindPieceByView(IJigsawPiece piece)
        {
            return this.Pieces.SingleOrDefault<IJigsawPiece>(p => (p == piece));
        }

        private void InitCommands()
        {
            this._commands = new CommandMap();
            this._commands.AddCommand("NewPuzzelCommand", x => this.NewPuzzel());
            this._commands.AddCommand("ShowPictureCommand", x => this.ShowPicture());
            this._commands.AddCommand("ShowPuzzelCommand", x => this.ShowPuzzel());
            this._commands.AddCommand("ReplayPuzzelCommand", x => this.ReplayPuzzel());
        }

        private void InitProperties()
        {
            this.EnableShowImageButton = false;
            this.ShowPuzzelButton = Visibility.Collapsed;
            this.ShowPictureButton = Visibility.Visible;
            this.ShowImageSource = Visibility.Collapsed;
            this.ShowPuzzelCanvas = Visibility.Visible;
            this._pieces = new List<IJigsawPiece>();
            var availableSizes = new Dictionary<double, string>
                                                            {
                                                                {50.0, "50px"},
                                                                {100.0, "100px"},
                                                                {200.0, "200px"},
                                                                {400.0, "400px"}
                                                            };
            this.AvailableSizes = availableSizes;
            this.PieceSize = 200.0;
        }

        private void InsertPiece(Canvas canvas, JigsawPiece piece)
        {
            canvas.Children.Add(piece);
            Canvas.SetLeft(piece, piece.X);
            Canvas.SetTop(piece, piece.Y);
            piece.MouseDown += new MouseButtonEventHandler(this.Piece_MouseDown);
            piece.MouseMove += new MouseEventHandler(this.Piece_MouseMove);
            piece.MouseUp += new MouseButtonEventHandler(this.Piece_MouseUp);
        }

        private Stream LoadImage(string srcFileName)
        {
            this.SourceFileName = srcFileName;
            this._columns = (int) Math.Ceiling((double) (((double) this.SupportedImageHeight)/this.PieceSize));
            this._rows = (int) Math.Ceiling((double) (((double) this.SupportedImageHeight)/this.PieceSize));
            BitmapImage bi = new BitmapImage(new Uri(srcFileName));

            ImageBrush imgBrush = new ImageBrush(bi)
                                      {
                                          AlignmentX = AlignmentX.Left,
                                          AlignmentY = AlignmentY.Top,
                                          Stretch = Stretch.UniformToFill
                                      };

            Rectangle rectBlank = new Rectangle
                                      {
                                          Width = this._columns*this.PieceSize,
                                          Height = this._rows*this.PieceSize,
                                          HorizontalAlignment = HorizontalAlignment.Left,
                                          VerticalAlignment = VerticalAlignment.Top,
                                          Fill = new SolidColorBrush(Colors.White)
                                      };
            rectBlank.Arrange(new Rect(0.0, 0.0, this._columns*this.PieceSize, this._rows*this.PieceSize));
            Rectangle rectImage = new Rectangle
                                      {
                                          Width = this.SupportedImageWidth,
                                          Height = this.SupportedImageHeight,
                                          HorizontalAlignment = HorizontalAlignment.Left,
                                          VerticalAlignment = VerticalAlignment.Top,
                                          Fill = imgBrush
                                      };
            rectImage.Arrange(new Rect(((this._columns*this.PieceSize) - this.SupportedImageWidth)/2.0,
                                       ((this._rows*this.PieceSize) - this.SupportedImageHeight)/2.0,
                                       (double) this.SupportedImageWidth, (double) this.SupportedImageHeight));
            rectImage.Margin = new Thickness(((this._columns*this.PieceSize) - this.SupportedImageWidth)/2.0,
                                             ((this._rows*this.PieceSize) - this.SupportedImageHeight)/2.0,
                                             ((this._rows*this.PieceSize) - this.SupportedImageHeight)/2.0,
                                             ((this._columns*this.PieceSize) - this.SupportedImageWidth)/2.0);
            var rtb = new RenderTargetBitmap((this._columns + 1)*((int) this.PieceSize),
                                                            (this._rows + 1)*((int) this.PieceSize), bi.DpiX, bi.DpiY,
                                                            PixelFormats.Pbgra32);
            rtb.Render(rectBlank);
            rtb.Render(rectImage);
            var png = new PngBitmapEncoder
                                       {
                                           Frames = {BitmapFrame.Create(rtb)}
                                       };
            Stream ret = new MemoryStream();
            png.Save(ret);
            return ret;
        }

#region Mouse event handler 

        private void MovePiece(IJigsawPiece piece, Point toPoint)
        {
            var uiElement = piece as UIElement;
            if (uiElement != null)
            {
                Canvas.SetLeft(uiElement, toPoint.X);
                Canvas.SetTop(uiElement, toPoint.Y);
            }
            else
            {
                //log or throw exception
            }
        }

        private void MovePieces(IJigsawPiece original, IJigsawPiece target)
        {
            if ((original != null) && (target != null))
            {
                Point fromPointForTarget = new Point(target.CurrentColumn*this.PieceSize,
                                                     target.CurrentRow*this.PieceSize);
                Point toPointForTarget = new Point(original.CurrentColumn*this.PieceSize,
                                                   original.CurrentRow*this.PieceSize);
//                Point fromPointForOriginal =
//                    original.TransformToAncestor(this.Window.Canvas).Transform(new Point(0.0, 0.0));
                Point toPointForOriginal = fromPointForTarget;
                this.MovePiece(target, toPointForTarget);
                this.MovePiece(original, toPointForOriginal);
                int targetX = target.CurrentColumn;
                int targetY = target.CurrentRow;
                target.CurrentColumn = original.CurrentColumn;
                target.CurrentRow = original.CurrentRow;
                original.CurrentColumn = targetX;
                original.CurrentRow = targetY;
                if (this.CheckPieces())
                {
                    MessageBox.Show("You have successfully finished this puzzel", "Congratulations!");
                    this.ShowPicture();
                }
            }
        }

        private void Piece_MouseDown(object sender, MouseButtonEventArgs e)
        {
            JigsawPiece pieceView = (JigsawPiece) sender;
            this._isDragging = true;
            pieceView.CaptureMouse();
            pieceView.SetValue(Panel.ZIndexProperty, 0x3e8);
        }

        private void Piece_MouseMove(object sender, MouseEventArgs e)
        {
            Point canvPosToWindow = this.Window.Canvas.TransformToAncestor(this.Window).Transform(new Point(0.0, 0.0));
            this._upperLimit = (canvPosToWindow.Y + (this.PieceSize/2.0)) - 10.0;
            this._lowerLimit = ((canvPosToWindow.Y + this.Window.Canvas.ActualHeight) - (this.PieceSize/2.0)) + 10.0;
            this._leftLimit = (canvPosToWindow.X + (this.PieceSize/2.0)) - 10.0;
            this._rightLimit = ((canvPosToWindow.X + this.Window.Canvas.ActualWidth) - (this.PieceSize/2.0)) + 10.0;
            double absMouseXpos = e.GetPosition(this.Window).X;
            double absMouseYpos = e.GetPosition(this.Window).Y;
            JigsawPiece pieceView = (JigsawPiece) sender;
            if (!this._isDragging)
            {
                if (((absMouseXpos > (this._leftLimit - 50.0)) && (absMouseXpos < (this._rightLimit + 50.0))) &&
                    ((absMouseYpos > (this._upperLimit - 50.0)) && (absMouseYpos < (this._lowerLimit + 50.0))))
                {
                    Mouse.SetCursor(Cursors.Hand);
                }
            }
            else if (((absMouseXpos > this._leftLimit) && (absMouseXpos < this._rightLimit)) &&
                     ((absMouseYpos > this._upperLimit) && (absMouseYpos < this._lowerLimit)))
            {
                Point piecePosition = e.GetPosition(this.Window.Canvas);
                Canvas.SetLeft(pieceView, piecePosition.X - (pieceView.Width/2.0));
                Canvas.SetTop(pieceView, piecePosition.Y - (pieceView.Height/2.0));
            }
        }

        private void Piece_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (this._isDragging)
            {
                this._isDragging = false;
                JigsawPiece pieceView = (JigsawPiece)sender;
                pieceView.ReleaseMouseCapture();
                pieceView.SetValue(Panel.ZIndexProperty, 10);
                IJigsawPiece originalModel = this.FindPieceByView(pieceView);
                IJigsawPiece targetModel = this.FindPieceByMousePosition(e.GetPosition(this.Window.Canvas));
                this.MovePieces(originalModel, targetModel);
            }
        }

#endregion

#region Commands implementation

        private void NewPuzzel()
        {
            this.ShowPuzzel();
            var ofd = new OpenFileDialog
                          {
                              Filter =
                                  "All Image Files ( JPEG,GIF,BMP,PNG)|*.jpg;*.jpeg;*.gif;*.bmp;*.png|JPEG Files ( *.jpg;*.jpeg )|*.jpg;*.jpeg|GIF Files ( *.gif )|*.gif|BMP Files ( *.bmp )|*.bmp|PNG Files ( *.png )|*.png",
                              Title = "Select an image file for generating the puzzle"
                          };
            if (ofd.ShowDialog().Value)
            {
                try
                {
                    //clean up previous game
                    DestroyImageReferences();
                    this.SourceFileName = ofd.FileName;
                    this.Pieces = this.CreatePuzzle(this.SourceFileName);
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.ToString());
                }
            }
        }

        private void ReplayPuzzel()
        {
            this.InitProperties();
            //clean up previous game
            this.Window.Canvas.Children.Clear();
            this.Pieces.Clear();
            this.CreatePuzzle(this.SourceFileName);
        }

        private void ShowPicture()
        {
            this.ShowPuzzelButton = Visibility.Visible;
            this.ShowPictureButton = Visibility.Collapsed;
            this.EnableShowImageButton = false;
            this.ShowImageSource = Visibility.Visible;
            this.ShowPuzzelCanvas = Visibility.Collapsed;
        }

        private void ShowPuzzel()
        {
            this.ShowPuzzelButton = Visibility.Collapsed;
            this.ShowPictureButton = Visibility.Visible;
            this.EnableShowImageButton = true;
            this.ShowImageSource = Visibility.Collapsed;
            this.ShowPuzzelCanvas = Visibility.Visible;
        }

#endregion 

#region Properties to bind to UI

        public Dictionary<double, string> AvailableSizes
        {
            get { return this._availableSizes; }
            set
            {
                this._availableSizes = value;
                base.FirePropertyChanged("AvailableSizes");
            }
        }

        public CommandMap Commands
        {
            get { return this._commands; }
        }

        public bool EnableReplayPuzzelButton
        {
            get { return this._enableReplayPuzzelButton; }
            set
            {
                this._enableReplayPuzzelButton = value;
                base.FirePropertyChanged("EnableReplayPuzzelButton");
            }
        }

        public bool EnableShowImageButton
        {
            get { return this._enableShowImageButton; }
            set
            {
                this._enableShowImageButton = value;
                base.FirePropertyChanged("EnableShowImageButton");
            }
        }

        public BitmapImage ImageSource
        {
            get { return this._imageSource; }
            set
            {
                this._imageSource = value;
                base.FirePropertyChanged("ImageSource");
            }
        }

        public string Info
        {
            get { return this._info; }
            set
            {
                this._info = value;
                base.FirePropertyChanged("Info");
            }
        }

        public IList<IJigsawPiece> Pieces
        {
            get { return this._pieces; }
            set
            {
                this._pieces = value;
                base.FirePropertyChanged("Pieces");
            }
        }

        public double PieceSize
        {
            get { return this._pieceSize; }
            set
            {
                this._pieceSize = value;
                base.FirePropertyChanged("PieceSize");
            }
        }

        public Visibility ShowImageSource
        {
            get { return this._showImageSource; }
            set
            {
                this._showImageSource = value;
                base.FirePropertyChanged("ShowImageSource");
            }
        }

        public Visibility ShowPictureButton
        {
            get { return this._showPictureButton; }
            set
            {
                this._showPictureButton = value;
                base.FirePropertyChanged("ShowPictureButton");
            }
        }

        public Visibility ShowPuzzelButton
        {
            get { return this._showPuzzelButton; }
            set
            {
                this._showPuzzelButton = value;
                base.FirePropertyChanged("ShowPuzzelButton");
            }
        }

        public Visibility ShowPuzzelCanvas
        {
            get { return this._showPuzzelCanvas; }
            set
            {
                this._showPuzzelCanvas = value;
                base.FirePropertyChanged("ShowPuzzelCanvas");
            }
        }

        public string SourceFileName
        {
            get { return this._sourceFileName; }
            set { this._sourceFileName = value; }
        }

#endregion

        //Main window reference
        public MainWindow Window { get; set; }

    }
}
