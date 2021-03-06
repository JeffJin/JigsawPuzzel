﻿using System;
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
using System.Windows.Navigation;
using MediaJigsaw.Helpers;
using MediaJigsaw.Infrastructure;
using MediaJigsaw.Models.Pieces;
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
        private MediaElement _videoSource;
        private string _info;
        private bool _isDragging;
        private double _leftLimit;
        private double _lowerLimit;
        private IList<IJigsawPiece> _pieces;
        private double _pieceSize;
        private double _rightLimit;
        private int _rows;
        private Visibility _showImageSource;
        private Visibility _showPictureButton;
        private Visibility _showPuzzelButton;
        private Visibility _showPuzzelCanvas;
        private double _upperLimit;
        public readonly int SupportedImageHeight = 800;
        public readonly int SupportedImageWidth = 800;

        public JigsawModel()
        {
            this.InitCommands();
            this.InitProperties();
            this.PieceSize = 200.0;
            this.PieceType = PieceType.SimpleBezier;
        }

        //check if the pieces are in the correct position
        private bool CheckPieces()
        {
            foreach (IJigsawPiece jigsawPiece in this.Pieces)
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
                var p = this.Window.Canvas.Children[i] as JigsawPieceBase;
                if (p != null)
                {
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

        private void InitImagePuzzles(Stream streamSource)
        {
            //create imedia source from the stream
            this.ImageSource = JigsawHelper.CreateImageSource(streamSource);

            //create image pieces
            IList<IJigsawPiece> pieces = JigsawPieceFactory.CreateImagePuzzelPieces(this.ImageSource, this._columns, 
                this._rows, this.PieceSize, this.PieceType);

            //scramble the pieces
            this.Pieces = JigsawHelper.ScramblePieces(pieces, this._rows, this._columns);

            //insert into canvas
            foreach (JigsawPieceBase piece in this.Pieces)
            {
                this.InsertPiece(this.Window.Canvas, piece);
            }
        }

        private void InitVideoPuzzles(Stream streamSource)
        {
            //create imedia source from the stream
            this.VideoSource = JigsawHelper.CreateVideoSource(streamSource);

            //create image pieces
            IList<IJigsawPiece> pieces = JigsawPieceFactory.CreateVideoPuzzelPieces(this.VideoSource, this._columns, 
                this._rows, this.PieceSize, this.PieceType);

            //scramble the pieces
            this.Pieces = JigsawHelper.ScramblePieces(pieces, this._rows, this._columns);

            //insert into canvas
            foreach (JigsawPieceBase piece in this.Pieces)
            {
                this.InsertPiece(this.Window.Canvas, piece);
            }
        }

        private IList<IJigsawPiece> CreateJigsawPieces(string streamFileName)
        {
            using (Stream streamSource = this.LoadStream(streamFileName))
            {
                if (streamFileName.EndsWith("wmv") || streamFileName.EndsWith("mp4"))
                {
                    this.InitVideoPuzzles(streamSource);
                }
                else
                {
                    this.InitImagePuzzles(streamSource);
                }
            }

            this.EnableShowImageButton = true;
            this.EnableReplayPuzzelButton = true;
            return this.Pieces;
        }

        private IJigsawPiece FindPieceByMousePosition(Point point)
        {
            //TODO: Should find the piece by mouse position
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
            this._commands.AddCommand("ShowVideoPuzzel", x => this.ShowVideoPuzzel());
        }

        private void InitProperties()
        {
            this.EnableShowImageButton = false;
            this.ShowPuzzelButton = Visibility.Collapsed;
            this.ShowPictureButton = Visibility.Visible;
            this.ShowImageSource = Visibility.Collapsed;
            this.ShowPuzzelCanvas = Visibility.Visible;
            this._pieces = new List<IJigsawPiece>();
            this.AvailableSizes = new Dictionary<double, string>
                                                            {
                                                                {400.0, "400px"},
                                                                {200.0, "200px"},
                                                                {100.0, "100px"},
                                                                {50.0, "50px"},
                                                            };
        }

        private void InsertPiece(Canvas canvas,  JigsawPieceBase piece)
        {
            canvas.Children.Add(piece);
            Canvas.SetLeft(piece, piece.Position.X);
            Canvas.SetTop(piece, piece.Position.Y);
            piece.MouseDown += new MouseButtonEventHandler(this.Piece_MouseDown);
            piece.MouseMove += new MouseEventHandler(this.Piece_MouseMove);
            piece.MouseUp += new MouseButtonEventHandler(this.Piece_MouseUp);
        }

        //Load image stream
        private Stream LoadImage(string srcFileName)
        {
            this._columns = (int)Math.Ceiling((double)(((double)this.SupportedImageHeight) / this.PieceSize));
            this._rows = (int)Math.Ceiling((double)(((double)this.SupportedImageHeight) / this.PieceSize));
            var bi = new BitmapImage(new Uri(srcFileName));

            var imgBrush = new ImageBrush(bi)
            {
                AlignmentX = AlignmentX.Left,
                AlignmentY = AlignmentY.Top,
                Stretch = Stretch.UniformToFill
            };

            var rectBlank = new Rectangle
            {
                Width = this._columns * this.PieceSize,
                Height = this._rows * this.PieceSize,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Fill = new SolidColorBrush(Colors.White)
            };
            rectBlank.Arrange(new Rect(0.0, 0.0, this._columns * this.PieceSize, this._rows * this.PieceSize));
            var rectImage = new Rectangle
            {
                Width = this.SupportedImageWidth,
                Height = this.SupportedImageHeight,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Fill = imgBrush
            };
            rectImage.Arrange(new Rect(((this._columns * this.PieceSize) - this.SupportedImageWidth) / 2.0,
                                       ((this._rows * this.PieceSize) - this.SupportedImageHeight) / 2.0,
                                       (double)this.SupportedImageWidth, (double)this.SupportedImageHeight));
            rectImage.Margin = new Thickness(((this._columns * this.PieceSize) - this.SupportedImageWidth) / 2.0,
                                             ((this._rows * this.PieceSize) - this.SupportedImageHeight) / 2.0,
                                             ((this._rows * this.PieceSize) - this.SupportedImageHeight) / 2.0,
                                             ((this._columns * this.PieceSize) - this.SupportedImageWidth) / 2.0);
            var rtb = new RenderTargetBitmap((this._columns + 1) * ((int)this.PieceSize),
                                                            (this._rows + 1) * ((int)this.PieceSize), bi.DpiX, bi.DpiY,
                                                            PixelFormats.Pbgra32);
            rtb.Render(rectBlank);
            rtb.Render(rectImage);
            var png = new PngBitmapEncoder
            {
                Frames = { BitmapFrame.Create(rtb) }
            };
            Stream ret = new MemoryStream();
            png.Save(ret);
            return ret;
        }

        private Stream LoadVideo(string srcFileName)
        {
            return new FileStream(srcFileName, FileMode.Open, FileAccess.Read);
        }

        private Stream LoadStream(string srcFileName)
        {
            this.SourceFileName = srcFileName;
            //Need to refactor with strategy pattern
            if(srcFileName.EndsWith("wmv") || srcFileName.EndsWith("mp4"))
            {
                return LoadVideo(srcFileName);
            }
            else
            {
                return LoadImage(srcFileName);
            }
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
        }

        private void MovePieces(IJigsawPiece original, IJigsawPiece target)
        {
            if ((original != null) && (target != null))
            {
                this.MovePiece(target, original.Position);
                this.MovePiece(original, target.Position);
                //swap position
                var tempOriginPoint = new Point(original.Position.X, original.Position.Y);
                original.Position = new Point(target.Position.X, target.Position.Y);
                target.Position = tempOriginPoint;
                //Swap column and row numbers
                int targetX = target.CurrentColumn;
                int targetY = target.CurrentRow;
                target.CurrentColumn = original.CurrentColumn;
                target.CurrentRow = original.CurrentRow;
                original.CurrentColumn = targetX;
                original.CurrentRow = targetY;

                //Adjust Z Index
                int zIndex = (int)original.GetValue(Panel.ZIndexProperty);
                original.SetValue(Panel.ZIndexProperty, zIndex + 1);
                if (this.CheckPieces())
                {
                    MessageBox.Show("You have successfully finished this puzzel", "Congratulations!");
                    this.ShowPicture();
                }
            }
        }

        private Point _mouseDownPosition;
        private string _mouseDownInfo;
        private void Piece_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var pieceView = (IJigsawPiece) sender;
            this._isDragging = true;
            this._mouseDownPosition = e.GetPosition(this.Window.Canvas);
            pieceView.CaptureMouse();
            pieceView.SetValue(Panel.ZIndexProperty, 100);

            _mouseDownInfo = string.Format("MouseDown X = {0}, MouseDown Y = {1}, " +
                                           "CurrentColumn = {2}, CurrentRow = {3}," +
                                           " OriginColumn = {4}, OriginRow = {5}",
                this._mouseDownPosition.X, this._mouseDownPosition.Y, 
                pieceView.CurrentColumn, pieceView.CurrentRow,
                pieceView.OriginColumn, pieceView.OriginRow);
            Info = _mouseDownInfo;
        }

        private void Piece_MouseMove(object sender, MouseEventArgs e)
        {
            Point canvPosToWindow = this.Window.Canvas.TransformToAncestor(this.Window).Transform(new Point(0.0, 0.0));
            this._upperLimit = (canvPosToWindow.Y + (this._mouseDownPosition.Y)) - 10.0;
            this._lowerLimit = ((canvPosToWindow.Y + this.Window.Canvas.ActualHeight) - (this._mouseDownPosition.Y)) + 10.0;
            this._leftLimit = (canvPosToWindow.X + (this._mouseDownPosition.X)) - 10.0;
            this._rightLimit = ((canvPosToWindow.X + this.Window.Canvas.ActualWidth) - (this._mouseDownPosition.X)) + 10.0;
            double absMouseXpos = e.GetPosition(this.Window.Canvas).X;
            double absMouseYpos = e.GetPosition(this.Window.Canvas).Y;
            Info = string.Format("{0};  Mouse X = {1}, Mouse Y = {2}", _mouseDownInfo, absMouseXpos, absMouseYpos);

            var pieceView = (JigsawPieceBase) sender;
            if (!this._isDragging)
            {
                if (((absMouseXpos > (this._leftLimit - 50.0)) && (absMouseXpos < (this._rightLimit + 50.0))) &&
                    ((absMouseYpos > (this._upperLimit - 50.0)) && (absMouseYpos < (this._lowerLimit + 50.0))))
                {
                    Mouse.SetCursor(Cursors.Hand);
                }
            }
            else if (((absMouseXpos > 0) && (absMouseXpos < this.SupportedImageWidth)) &&
                     ((absMouseYpos > 0) && (absMouseYpos < this.SupportedImageHeight)))
            {
                Point piecePosition = e.GetPosition(this.Window.Canvas);
                var left = piecePosition.X - _mouseDownPosition.X + pieceView.Position.X;
                var right = piecePosition.Y - _mouseDownPosition.Y + pieceView.Position.Y;
                
                Info = string.Format("{0}; Mouse Move X = {1}, Mouse Move Y = {2}, " +
                                     "CurrnetColumn = {3}, CurrentRow = {4}, " +
                                     "OriginColumn = {5}, OriginRow = {6}",
                    _mouseDownInfo, left, right, pieceView.CurrentColumn, pieceView.CurrentRow, 
                pieceView.OriginColumn, pieceView.OriginRow);
                Canvas.SetLeft(pieceView, left);//
                Canvas.SetTop(pieceView, right);//
            }
        }

        private void Piece_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (this._isDragging)
            {
                this._isDragging = false;
                var pieceView = (JigsawPieceBase)sender;
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
                    this.Pieces = this.CreateJigsawPieces(this.SourceFileName);
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.ToString());
                }
            }
        }

        private void ShowVideoPuzzel()
        {
            var window = new Window()
                             {
                                 Title = "Video Brush",
                                 Content = new VideoPanel(),
                                 Height = 800,
                                 Width = 800
                             };
            window.ShowDialog();
        }

        private void ReplayPuzzel()
        {
            this.InitProperties();
            //clean up previous game
            this.Window.Canvas.Children.Clear();
            this.Pieces.Clear();
            this.CreateJigsawPieces(this.SourceFileName);
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
        
        public string SourceFileName { get; set; }

        public Dictionary<double, string> AvailableSizes
        {
            get { return this._availableSizes; }
            set
            {
                this._availableSizes = value;
                base.FirePropertyChanged("AvailableSizes");
            }
        }
        private Dictionary<PieceType, string> _availablePieceTypes;
        public Dictionary<PieceType, string> AvailablePieceTypes
        {
            get
            {
                if(_availablePieceTypes == null)
                {
                    _availablePieceTypes = new Dictionary<PieceType, string>()
                           {
                               {PieceType.Rectangle, "Rectangle"}, 
                               {PieceType.SimpleBezier, "Simple Bezier"}, 
                               {PieceType.PolyBezier, "Poly Bezier"}
                           };
                }
                return _availablePieceTypes;
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

        public MediaElement VideoSource
        {
            get { return this._videoSource; }
            set
            {
                this._videoSource = value;
                base.FirePropertyChanged("VideoSource");
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

        private PieceType _pieceType;
        public PieceType PieceType
        {
            get { return this._pieceType; }
            set
            {
                this._pieceType = value;
                base.FirePropertyChanged("PieceType");
            }
        }

#endregion

        //Main window reference
        public MainWindow Window { get; set; }

    }
}
