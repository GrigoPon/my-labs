using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.IO;
using System.Windows.Media.Imaging;
using NAudio.Wave;

namespace WPF_L8
{
    public partial class MainWindow : Window
    {
        private const int Rows = 3;
        private const int Columns = 4;
        private const int PieceSize = 200;
        private List<PuzzlePiece> puzzle = new List<PuzzlePiece>();
        private Point startP;
        private PuzzlePiece draggedPiece;
        private bool Won = false;
        private Border[,] gridCells = new Border[Rows, Columns];

        private SoundPlayer music;
        private SoundPlayer taking;
        private SoundPlayer drop;
        private SoundPlayer OIIA;

        private List<WaveOutEvent> _activePlayers = new List<WaveOutEvent>();
        private BitmapImage selectedImage;

        public MainWindow()
        {
            InitializeComponent();
        }
        private void PlaySound(string soundFilePath)
        {
            try
            {
                
                var audioFile = new AudioFileReader(soundFilePath);
                var outputDevice = new WaveOutEvent();

                
                outputDevice.Init(audioFile);

                
                _activePlayers.Add(outputDevice);

                
                outputDevice.Play();

                
                outputDevice.PlaybackStopped += (s, e) =>
                {
                    _activePlayers.Remove(outputDevice);
                    outputDevice.Dispose();
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при воспроизведении звука: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public class PuzzlePiece
        {
            public Border Visual { get; set; }
            public PuzzlePos CorrectPosition { get; set; }
            public PuzzlePos CurrPosition { get; set; }

            public PuzzlePiece()
            {
                
                Visual = new Border();

                
                Panel.SetZIndex(Visual, 1);

                
                CorrectPosition = new PuzzlePos(-1, -1);
                CurrPosition = new PuzzlePos(-1, -1);
            }
        }

        public class PuzzlePos
        {
            public int Row { get; set; }
            public int Col { get; set; }

            public PuzzlePos(int row, int col)
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
            try
            {
                // Останавливаем все активные устройства воспроизведения
                foreach (var player in _activePlayers)
                {
                    player.Stop();
                    player.Dispose();
                }

                // Очищаем список активных игроков
                _activePlayers.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при остановке звуков: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            StartGame();
            string soundFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sounds", "lift.wav");
            PlaySound(soundFilePath);
        }

        private void StartGame()
        {
            Game.Children.Clear();
            puzzle.Clear();
            Win.Visibility = Visibility.Collapsed;
            Won = false;



            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png|All files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    selectedImage = new BitmapImage(new Uri(openFileDialog.FileName));
                    Start.IsEnabled = true; // Активируем кнопку "Начать игру"
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки изображения: {ex.Message}");
                }
            }


            // Создаем сетку ячеек (пустые клетки)
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Columns; col++)
                {
                    Border cell = new Border
                    {
                        BorderBrush = Brushes.Black,
                        BorderThickness = new Thickness(1),
                        Width = PieceSize,
                        Height = PieceSize,
                        Background = Brushes.White,
                        Tag = new PuzzlePos(row, col)
                    };
                    Canvas.SetLeft(cell, col * PieceSize);
                    Canvas.SetTop(cell, row * PieceSize);
                    Game.Children.Add(cell);
                    gridCells[row, col] = cell;
                }
            }

            BitmapImage origIm = new BitmapImage(new Uri("pack://application:,,,/Resources/2.jpg"));

            // Создаем фрагменты пазла
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Columns; col++)
                {
                    Int32Rect cropRect = new Int32Rect(col * PieceSize, row * PieceSize, PieceSize, PieceSize);
                    CroppedBitmap croppedPiece = new CroppedBitmap(origIm, cropRect);
                    Image piece = new Image
                    {
                        Source = croppedPiece,
                        Width = PieceSize,
                        Height = PieceSize,
                        Stretch = Stretch.Fill,
                        Tag = new PuzzlePos(row, col),
                        
                        
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
                        Width = PieceSize,
                        Height = PieceSize,
                    };

                    
                    Game.Children.Add(pieceCont);
                    Panel.SetZIndex(pieceCont, 1);

                    puzzle.Add(new PuzzlePiece
                    {
                        Visual = pieceCont,
                        CorrectPosition = new PuzzlePos(row, col),
                        CurrPosition = new PuzzlePos(-1, -1) // -1 означает, что фрагмент не в сетке
                        
                    });
                    
                }
            }

            // Размещаем фрагменты в случайных местах вне сетки
            ShufflePieces();
        }

        private void ShufflePieces()
        {
            Random rand = new Random();
            

            foreach (var piece in puzzle)
            {
                int newX, newY;
                bool positionValid;
                int attempts = 0;
                const int maxAttempts = 100;

                // Пытаемся найти свободное место вне сетки
                do
                {

                    newX = rand.Next(0, (int)Game.ActualWidth - PieceSize);
                    newY = rand.Next(0, (int)Game.ActualHeight - PieceSize);

                    // Проверяем, не пересекается ли с другими фрагментами
                    Rect newRect = new Rect(newX, newY, PieceSize, PieceSize);
                    positionValid = true;

                    foreach (var otherPiece in puzzle)
                    {
                        if (otherPiece == piece) continue;

                        double otherX = Canvas.GetLeft(otherPiece.Visual);
                        double otherY = Canvas.GetTop(otherPiece.Visual);

                        if (otherX >= 0 && otherY >= 0) // Проверяем только уже размещенные фрагменты
                        {
                            Rect otherRect = new Rect(otherX, otherY, PieceSize, PieceSize);
                            if (newRect.IntersectsWith(otherRect))
                            {
                                positionValid = false;
                                break;
                            }
                        }
                    }

                    attempts++;
                    if (attempts >= maxAttempts)
                    {
                        // Если не удалось найти свободное место, размещаем в любом месте
                        newX = rand.Next(0, (int)Game.ActualWidth - PieceSize);
                        newY = rand.Next(0, (int)Game.ActualHeight - PieceSize);
                        positionValid = true;
                    }

                } while (!positionValid);

                Canvas.SetLeft(piece.Visual, newX);
                Canvas.SetTop(piece.Visual, newY);
                piece.CurrPosition = new PuzzlePos(-1, -1); // -1 означает, что фрагмент не в сетке
            }
        }

        private void Piece_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Won) return;

            startP = e.GetPosition(Game);
            var image = (Image)sender;
            draggedPiece = puzzle.Find(p => p.Visual.Child == image);

            if (draggedPiece != null)
            {
                image.CaptureMouse();
                Panel.SetZIndex(draggedPiece.Visual, 2);

                image.Opacity = 0.5;
            }

            string soundFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sounds", "Down.wav");
            PlaySound(soundFilePath);
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

            var image = (Image)sender;
            image.ReleaseMouseCapture();
            image.Opacity = 1;
            Panel.SetZIndex(draggedPiece.Visual, 0);

            var mousePos = e.GetPosition(Game);
            TrySnapToGrid(draggedPiece, mousePos);

            CheckPuzzleCompletion();

            draggedPiece = null;

            string soundFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sounds", "Take.wav");
            PlaySound(soundFilePath);
        }

        private void TrySnapToGrid(PuzzlePiece piece, Point mousePos)
        {
            // Определяем, над какой ячейкой находится курсор
            int col = (int)(mousePos.X / PieceSize);
            int row = (int)(mousePos.Y / PieceSize);

            bool wasInGrid = piece.CurrPosition.Row >= 0 && piece.CurrPosition.Col >= 0;
            // Проверяем, находится ли курсор внутри сетки
            if (row < 0 || row >= Rows || col < 0 || col >= Columns)
            {
                // Возвращаем на случайную позицию вне сетки
                piece.CurrPosition = new PuzzlePos(-1, -1);
                Panel.SetZIndex(piece.Visual, 1);
                return;
            }

            // Проверяем, занята ли ячейка другим фрагментом
            bool cellOccupied = puzzle.Any(p =>
                p != piece &&
                p.CurrPosition.Row == row &&
                p.CurrPosition.Col == col);

            if (cellOccupied)
            {
                // Возвращаем на случайную позицию вне сетки
                piece.CurrPosition = new PuzzlePos(-1, -1);
                Panel.SetZIndex(piece.Visual, 1);
                return;
            }

            // Если ячейка свободна - перемещаем фрагмент
            Canvas.SetLeft(piece.Visual, col * PieceSize);
            Canvas.SetTop(piece.Visual, row * PieceSize);
            piece.CurrPosition = new PuzzlePos(row, col);
            Panel.SetZIndex(piece.Visual, 0);
        }

        private void CheckPuzzleCompletion()
        {
            foreach (var piece in puzzle)
            {
                if (!piece.CurrPosition.Equals(piece.CorrectPosition)) return;
            }

            try
            {
                // Останавливаем все активные устройства воспроизведения
                foreach (var player in _activePlayers)
                {
                    player.Stop();
                    player.Dispose();
                }

                // Очищаем список активных игроков
                _activePlayers.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при остановке звуков: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            Won = true;
            Win.Visibility = Visibility.Visible;
            
            string soundFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sounds", "OIIA.wav");
            PlaySound(soundFilePath);

            foreach (var p in puzzle)
            {
                var image = (Image)p.Visual.Child;
                image.MouseLeftButtonDown -= Piece_MouseLeftButtonDown;
                image.MouseMove -= Piece_MouseMove;
                image.MouseLeftButtonUp -= Piece_MouseLeftButtonUp;
            }
        }
    }
}