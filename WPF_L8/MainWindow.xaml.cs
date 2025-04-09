using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_L8
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int Rows = 3;
        private const int Columns = 4;
        private List<PuzzlePiece> puzzle = new List<PuzzlePiece>();
        private Point startP;
        private PuzzlePiece draggedPiece;
        private bool Won = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        public class PuzzlePiece { 
            public Border Visual { get; set; }
            public PuzzlePos CorrectPosition { get; set; }
            public PuzzlePos CurrPosition { get; set; }

        }

        public class PuzzlePos
        {
            public int Row { get; set; }
            public int Col { get; set; }

            public PuzzlePos (int row, int col)
            {
                Row = row; Col = col;
            }

            public override bool Equals(object obj)
            {
                return obj is PuzzlePos position && Row == position.Row && Col == position.Col;
            }

            public override int GetHashCode()
            {
                return (Row << 2) ^ Col;
            }
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            StartGame();
        }

        private void StartGame()
        {
            Game.Children.Clear();
            puzzle.Clear();
            Win.Visibility = Visibility.Collapsed;
            Won = false;

            BitmapImage origIm = new BitmapImage(new Uri("pack://application:,,,/NewFolder1/shrek.jpg"));

            double pieceW = origIm.PixelWidth / Columns;
            double pieceH = origIm.PixelHeight / Rows;

            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Columns; col++)
                {
                    Int32Rect cropRect = new Int32Rect((int)(col*pieceW), (int)(row * pieceH), (int)pieceW, (int)pieceH);
                    CroppedBitmap croppedPiece = new CroppedBitmap(origIm, cropRect);
                    Image piece = new Image
                    {
                        Source = croppedPiece,
                        Width = pieceW,
                        Height = pieceH,
                        Stretch = Stretch.Fill,
                        Tag = new PuzzlePos(row, col)
                    };
                    piece.MouseLeftButtonDown += Piece_MouseLeftButtonDown;
                    piece.MouseMove += Piece_MouseMove;
                    piece.MouseLeftButtonUp += Piece_MouseLeftButtonUp;

                    Border pieceCont = new Border
                    {
                        Child = piece,
                        BorderBrush = Brushes.Black,
                        BorderThickness = new Thickness(1),
                        Background = Brushes.White,
                        Width = pieceW,
                        Height = pieceH,
                    };

                    Canvas.SetLeft(pieceCont, 50 + col * (pieceW + 5));
                    Canvas.SetTop(pieceCont, 50 + row * (pieceH + 5));
                    Game.Children.Add(pieceCont);

                    puzzle.Add(new PuzzlePiece
                    {
                        Visual = pieceCont,
                        CorrectPosition = new PuzzlePos(row, col),
                        CurrPosition = new PuzzlePos(row, col)
                    });
                }
            }
            ShufflePieces();
        }

        private void ShufflePieces()
        {
            Random rand = new Random();

            foreach (var piece in puzzle)
            {
                int newX = rand.Next(50, (int)(Game.ActualWidth - piece.Visual.Width - 50));
                int newY = rand.Next(50, (int)(Game.ActualHeight - piece.Visual.Height - 50));

                Canvas.SetLeft(piece.Visual, newX);
                Canvas.SetTop (piece.Visual, newY);

                piece.CurrPosition = new PuzzlePos(-1, -1);
            }
        }

        private void Piece_MouseLeftButtonDown (object sender, MouseButtonEventArgs e)
        {
            if (Won) return;

            startP = e.GetPosition(Game);
            var image = (Image)sender;
            draggedPiece = puzzle.Find(p => p.Visual.Child == image);

            if(draggedPiece != null)
            {
                image.CaptureMouse();
                Panel.SetZIndex(draggedPiece.Visual, 1);
            }
        }

        private void Piece_MouseMove(object sender, MouseEventArgs e)
        {
            if (Won || draggedPiece == null || e.LeftButton != MouseButtonState.Pressed) return;

            var currP = e.GetPosition(Game);
            var offset = currP - startP;

            Canvas.SetLeft(draggedPiece.Visual, Canvas.GetLeft(draggedPiece.Visual) + offset.X);
            Canvas.SetTop(draggedPiece.Visual, Canvas.GetTop(draggedPiece.Visual) + offset.Y);

            startP = currP;
        }

        private void Piece_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Won || draggedPiece == null) return;

            var image = (Image )sender;
            image.ReleaseMouseCapture();
            Panel.SetZIndex(draggedPiece.Visual, 0);

            CheckPiecePosition(draggedPiece);

            CheckPuzzleCompletion();

            draggedPiece = null;
        }

        private void CheckPiecePosition(PuzzlePiece piece)
        {
            double correctLeft = 400 + piece.CorrectPosition.Col * (piece.Visual.Width + 5);
            double correctTop = 50 + piece.CorrectPosition.Row*(piece.Visual.Height + 5);

            double currLeft = Canvas.GetLeft(piece.Visual);
            double currTop = Canvas.GetTop(piece.Visual);

            if (Math.Abs(currLeft - correctLeft) < 10 && Math.Abs(currTop - correctTop) < 10) {
                Canvas.SetLeft(piece.Visual, correctLeft);
                Canvas.SetTop(piece.Visual, correctTop);
                piece.CurrPosition = piece.CorrectPosition;
            }
        }

        private void CheckPuzzleCompletion()
        {
            foreach(var piece in puzzle)
            {
                if (piece.CurrPosition != piece.CorrectPosition) return;
            }

            Won = true;
            Win.Visibility = Visibility.Visible;

            foreach(var p in puzzle)
            {
                var image = (Image)p.Visual.Child;
                image.MouseLeftButtonDown -= Piece_MouseLeftButtonDown;
                image.MouseMove -= Piece_MouseMove;
                image.MouseLeftButtonUp -= Piece_MouseLeftButtonUp;
            }
        }
    }
}
