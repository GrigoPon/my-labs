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
using Microsoft.Win32;
using System.IO;
using System.Media;

namespace CryptoGraphy_Project
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Dictionary<char, string> _morseAlphabet = new Dictionary<char, string>
        {
            // Русские буквы
            {'А', ".-"},    {'Б', "-..."},   {'В', ".--"},    {'Г', "--."},    {'Д', "-.."},
            {'Е', "."},     {'Ё', "."},      {'Ж', "...-"},   {'З', "--.."},   {'И', ".."},
            {'Й', ".---"},  {'К', "-.-"},    {'Л', ".-.."},   {'М', "--"},     {'Н', "-."},
            {'О', "---"},   {'П', ".--."},   {'Р', ".-."},    {'С', "..."},    {'Т', "-"},
            {'У', "..-"},   {'Ф', "..-."},   {'Х', "...."},   {'Ц', "-.-."},   {'Ч', "---."},
            {'Ш', "----"},  {'Щ', "--.-"},   {'Ъ', "--.--"},  {'Ы', "-.--"},   {'Ь', "-..-"},
            {'Э', "..-.."}, {'Ю', "..--"},   {'Я', ".-.-"},
    
            // Цифры (оставляем как в оригинале)
            {'0', "-----"}, {'1', ".----"},  {'2', "..---"},  {'3', "...--"},  {'4', "....-"},
            {'5', "....."}, {'6', "-...."},  {'7', "--..."},  {'8', "---.."},  {'9', "----."},
    
            // Пробел и специальные символы (оставляем как в оригинале)
            {' ', "/"},     {'.', ".-.-.-"}, {',', "--..--"}, {'?', "..--.."}, {'!', "-.-.--"},
            {'\'', ".----."}, {'"', ".-..-."}, {'(', "-.--."}, {')', "-.--.-"}, {'&', ".-..."},
            {':', "---..."}, {';', "-.-.-."}, {'=', "-...-"}, {'+', ".-.-."}, {'-', "-....-"},
            {'_', "..--.-"}, {'$', "...-..-"}, {'@', ".--.-."}
        };

        private readonly Dictionary<string, char> _reverseMorseAlphabet;

        public MainWindow()
        {
            InitializeComponent();

            // Создаем обратный словарь для декодирования
            _reverseMorseAlphabet = new Dictionary<string, char>();
            foreach (var pair in _morseAlphabet)
            {
                _reverseMorseAlphabet[pair.Value] = pair.Key;
            }
            EncodePanel.Visibility = Visibility.Visible;
            DecodePanel.Visibility = Visibility.Collapsed;
            ActionButton.Content = "Закодировать";
            SaveButton.IsEnabled = true;
            OutputTextBox.Text = string.Empty;
        }

        private void ModeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            // Проверяем, все ли элементы уже инициализированы
            if (EncodePanel == null || DecodePanel == null || ActionButton == null || SaveButton == null || OutputTextBox == null)
                return;

            if (EncodeModeRadio.IsChecked == true)
            {
                EncodePanel.Visibility = Visibility.Visible;
                DecodePanel.Visibility = Visibility.Collapsed;
                ActionButton.Content = "Закодировать";
                SaveButton.IsEnabled = true;
            }
            else
            {
                EncodePanel.Visibility = Visibility.Collapsed;
                DecodePanel.Visibility = Visibility.Visible;
                ActionButton.Content = "Декодировать";
                SaveButton.IsEnabled = false;
            }

            OutputTextBox.Text = string.Empty;
        }

        private void ActionButton_Click(object sender, RoutedEventArgs e)
        {
            if (EncodeModeRadio.IsChecked == true)
            {
                EncodeText();
            }
            else
            {
                DecodeText();
            }
        }

        private void EncodeText()
        {
            string inputText = InputTextBox.Text.ToUpper();
            StringBuilder morseCode = new StringBuilder();

            foreach (char character in inputText)
            {
                if (_morseAlphabet.TryGetValue(character, out string code))
                {
                    morseCode.Append(code + " ");
                }
                else
                {
                    morseCode.Append("? ");
                }
            }

            OutputTextBox.Text = morseCode.ToString().Trim();
        }

        private void DecodeText()
        {
            if (string.IsNullOrWhiteSpace(OutputTextBox.Text))
            {
                MessageBox.Show("Сначала загрузите файл с кодом Морзе.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string morseCode = OutputTextBox.Text;
            string[] words = morseCode.Split(new[] { " / " }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder decodedText = new StringBuilder();

            foreach (string word in words)
            {
                string[] letters = word.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string letter in letters)
                {
                    if (_reverseMorseAlphabet.TryGetValue(letter, out char character))
                    {
                        decodedText.Append(character);
                    }
                    else
                    {
                        decodedText.Append('?');
                    }
                }
                decodedText.Append(' ');
            }

            OutputTextBox.Text = decodedText.ToString().Trim();
        }

        private void LoadFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Текстовый файл (*.txt)|*.txt",
                DefaultExt = "txt"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    string fileContent = File.ReadAllText(openFileDialog.FileName);
                    OutputTextBox.Text = fileContent;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при чтении файла: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(OutputTextBox.Text))
            {
                MessageBox.Show("Нет сообщения для сохранения.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Текстовый файл (*.txt)|*.txt",
                DefaultExt = "txt",
                AddExtension = true
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    File.WriteAllText(saveFileDialog.FileName, OutputTextBox.Text);
                    MessageBox.Show("Сообщение успешно сохранено.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении файла: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(OutputTextBox.Text))
            {
                MessageBox.Show("Нет сообщения для воспроизведения.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            PlayButton.IsEnabled = false;

            try
            {
                string morseCode = OutputTextBox.Text;
                using (var soundStream = GenerateMorseSoundStream(morseCode))
                {
                    soundStream.Position = 0; // Важно! Сбрасываем позицию потока
                    using (var player = new SoundPlayer(soundStream))
                    {
                        player.PlaySync(); // Синхронное воспроизведение
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка воспроизведения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                PlayButton.IsEnabled = true;
            }
        }

        

        private MemoryStream GenerateMorseSoundStream(string morseCode)
        {
            const int sampleRate = 44100;
            
            double theta = 2 * Math.PI * 800 / sampleRate; // Частота 800 Гц

            var stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);

            // Длина данных будет известна позже
            long dataChunkPosition = 0;
            int totalDataSize = 0;

            // Записываем заголовки RIFF/WAVE
            writer.Write(new char[] { 'R', 'I', 'F', 'F' });
            writer.Write(0); // Пока запишем 0, потом обновим
            writer.Write(new char[] { 'W', 'A', 'V', 'E' });

            // fmt чанк
            writer.Write(new char[] { 'f', 'm', 't', ' ' });
            writer.Write(16); // Размер fmt чанка
            writer.Write((short)1); // PCM
            writer.Write((short)1); // Моно
            writer.Write(sampleRate);
            writer.Write(sampleRate * 2); // Байты в секунду
            writer.Write((short)2); // Байт на семпл
            writer.Write((short)16); // Биты на семпл

            // data чанк
            writer.Write(new char[] { 'd', 'a', 't', 'a' });
            dataChunkPosition = stream.Position;
            writer.Write(0); // Длина данных

            totalDataSize = 0;

            foreach (char symbol in morseCode)
            {
                if (symbol == '.')
                {
                    WriteTone(writer, sampleRate, 100); // Точка
                    WriteSilence(writer, sampleRate, 100);
                }
                else if (symbol == '-')
                {
                    WriteTone(writer, sampleRate, 200); // Тире
                    WriteSilence(writer, sampleRate, 200);
                }
                else if (symbol == ' ')
                {
                    WriteSilence(writer, sampleRate, 400);
                }
                else if (symbol == '/')
                {
                    WriteSilence(writer, sampleRate, 600);
                }
            }

            // Обновляем размер данных
            totalDataSize = (int)(stream.Length - dataChunkPosition - 4);
            stream.Position = dataChunkPosition;
            writer.Write(totalDataSize);

            // Обновляем общий размер файла
            stream.Position = 4;
            writer.Write(36 + totalDataSize);

            return stream;
        }

        private void WriteTone(BinaryWriter writer, int sampleRate, int durationMs)
        {
            const short amplitude = 32760;
            double theta = 2 * Math.PI * 800 / sampleRate;
            int samples = (int)(sampleRate * durationMs / 1000.0);
            for (int i = 0; i < samples; i++)
            {
                short sample = (short)(amplitude * Math.Sin(theta * i));
                writer.Write(sample);
            }
        }

        private void WriteSilence(BinaryWriter writer, int sampleRate, int durationMs)
        {
            int samples = (int)(sampleRate * durationMs / 1000.0);
            for (int i = 0; i < samples; i++)
            {
                writer.Write((short)0);
            }
        }

        private void SaveSound_Click(object sender, RoutedEventArgs e)
        {
            string morseCode = OutputTextBox.Text.Trim();
            if (string.IsNullOrEmpty(morseCode))
            {
                MessageBox.Show("Нет кода Морзе для сохранения.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "WAV-файл (*.wav)|*.wav",
                DefaultExt = ".wav",
                FileName = "MorseCode"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    using (var soundStream = GenerateMorseSoundStream(morseCode))
                    {
                        using (var fileStream = File.Create(saveFileDialog.FileName))
                        {
                            soundStream.WriteTo(fileStream);
                        }
                    }

                    MessageBox.Show("Звуковой файл успешно сохранён.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении файла: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}


