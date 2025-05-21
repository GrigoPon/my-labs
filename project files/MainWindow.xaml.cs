using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
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
using System.Runtime.InteropServices;


namespace CryptoApp
{
    public partial class MainWindow : Window
    {
        // Режимы работы (теперь просто для выделения активного инструмента)
        private string _currentInput = "";
        private bool _enigmaIsLocked = false;
        private bool _allowInput = true;


        // Данные для Азбуки Морзе
        private readonly Dictionary<char, string> _morseAlphabet = new Dictionary<char, string>
        {
            // Русские буквы
            {'А', ".-"}, {'Б', "-..."}, {'В', ".--"}, {'Г', "--."}, {'Д', "-.."},
            {'Е', "."}, {'Ж', "...-"}, {'З', "--.."}, {'И', ".."},
            {'Й', ".---"}, {'К', "-.-"}, {'Л', ".-.."}, {'М', "--"}, {'Н', "-."},
            {'О', "---"}, {'П', ".--."}, {'Р', ".-."}, {'С', "..."}, {'Т', "-"},
            {'У', "..-"}, {'Ф', "..-."}, {'Х', "...."}, {'Ц', "-.-."}, {'Ч', "---."},
            {'Ш', "----"}, {'Щ', "--.-"}, {'Ъ', "--.--"}, {'Ы', "-.--"}, {'Ь', "-..-"},
            {'Э', "..-.."}, {'Ю', "..--"}, {'Я', ".-.-"},
            
            // Цифры и знаки препинания
            {'0', "-----"}, {'1', ".----"}, {'2', "..---"}, {'3', "...--"}, {'4', "....-"},
            {'5', "....."}, {'6', "-...."}, {'7', "--..."}, {'8', "---.."}, {'9', "----."},
            {' ', "/"}, {'.', ".-.-.-"}, {',', "--..--"}, {'?', "..--.."}, {'!', "-.-.--"},
            {'\'', ".----."}, {'"', ".-..-."}, {'(', "-.--."}, {')', "-.--.-"}, {'&', ".-..."},
            {':', "---..."}, {';', "-.-.-."}, {'=', "-...-"}, {'+', ".-.-."}, {'-', "-....-"},
            {'_', "..--.-"}, {'$', "...-..-"}, {'@', ".--.-."}
        };

        private readonly Dictionary<string, char> _reverseMorseAlphabet;

        // Данные для Энигмы
        private EnigmaMachine _enigma;
        private readonly string[] _rotorWirings =
        {
            "ЕКМФЛГДЖЗНРСТУВЩХЫПАИБЬЮЯЧШЁЦЪЭОЙ0123456789.,:?!()*+-=",
            "АДКПЮРФИШМСБЛТЩЧГВХЕЗЖЁЙЫЬЯУОЭНЦЪ0123456789.,:?!()*+-=",
            "ПОЛВСМТРХДЖЫФБИЮЭЪЧШЯГНЁАКУЩЗЦЕЙЬ0123456789.,:?!()*+-="

            
        };

        private readonly char[] _rotorNotches = { 'Е', 'А', 'П' };
        private readonly string _reflectorWiring = "=-+*)(!?:,.9876543210ЯЮЭЬЫЪЩШЧЦХФУТСРПОНМЛКЙИЗЖЁЕДГВБА";

        public MainWindow()
        {
            InitializeComponent();
            InputMethod.SetIsInputMethodEnabled(this, true);
            InputMethod.Current.ImeState = InputMethodState.On;
            RotorSetTextBox.Text = "";
            InitializeRotorPositions();
            CreateEnigmaMachine();
            this.PreviewKeyDown += MainWindow_PreviewKeyDown;
            
            EncodingButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            // Создаём обратный словарь для Морзе
            _reverseMorseAlphabet = _morseAlphabet
                .Where(kv => kv.Key != ' ')
                .ToDictionary(kv => kv.Value, kv => kv.Key);

            // Добавляем '/' как пробел только если такого ключа еще нет
            if (!_reverseMorseAlphabet.ContainsKey("/"))
            {
                _reverseMorseAlphabet["/"] = ' ';
            }

            LeftRotorPosition.SelectionChanged += RotorPosition_SelectionChanged;
            MiddleRotorPosition.SelectionChanged += RotorPosition_SelectionChanged;
            RightRotorPosition.SelectionChanged += RotorPosition_SelectionChanged;
            RotorSetTextBox.GotFocus += (s, e) => { };
            RotorSetTextBox.LostFocus += (s, e) => { };
        }

        private void RotorPosition_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateRotorSetTextBox();
        }

        private void ApplyRotorPositionsFromTextBox()
        {
            if (RotorSetTextBox.Text.Length == 3)
            {
                char left = char.ToUpper(RotorSetTextBox.Text[0]);
                char middle = char.ToUpper(RotorSetTextBox.Text[1]);
                char right = char.ToUpper(RotorSetTextBox.Text[2]);

                SetComboBoxSelectedItem(LeftRotorPosition, left);
                SetComboBoxSelectedItem(MiddleRotorPosition, middle);
                SetComboBoxSelectedItem(RightRotorPosition, right);

                CreateEnigmaMachine(); // Пересоздаём машину с новыми позициями
            }
            else
            {
                MessageBox.Show("Установлено(ы) некорректное/не все значение(я) позиций роторов","ОШИБКА!");
            }
        }

        private void SetComboBoxSelectedItem(ComboBox comboBox, char value)
        {
            string target = value.ToString();
            if (comboBox.Items.Contains(target))
            {
                comboBox.SelectedItem = target;
            }
            else if (comboBox.Items.Count > 0)
            {
                comboBox.SelectedIndex = 0;
            }
        }

        private void UpdateRotorSetTextBox()
        {
            string left = LeftRotorPosition.SelectedItem?.ToString() ?? "";
            string middle = MiddleRotorPosition.SelectedItem?.ToString() ?? "";
            string right = RightRotorPosition.SelectedItem?.ToString() ?? "";

            RotorSetTextBox.Text = left + middle + right;
        }

        private void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Если фокус на RotorSetTextBox — не обрабатываем здесь
            if (Keyboard.FocusedElement is TextBox && Keyboard.FocusedElement == RotorSetTextBox)
            {
                // Разрешаем Backspace и Enter
                if (e.Key == Key.Back || e.Key == Key.Enter)
                    return;

                // Всё остальное пропускаем — пусть обработает TextBox
                return;
            }
            if (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt))
            {
                e.Handled = true;
                return;
            }
            // Игнорируем служебные клавиши
            if (e.Key == Key.Escape || e.Key == Key.CapsLock ||
                e.Key == Key.LeftAlt || e.Key == Key.RightAlt ||
                e.Key == Key.RightShift || e.Key == Key.LeftShift ||
                e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl ||
                e.Key == Key.Tab || e.Key == Key.Enter)
            {
                e.Handled = true;
                return;
            }

            if (e.Key == Key.Back)
            {
                if (_currentInput.Length > 0)
                {
                    _currentInput = _currentInput.Remove(_currentInput.Length - 1);
                    UpdateOutputs();
                }
                e.Handled = true;
                return;
            }

            char pressedChar = KeyToChar(e.Key, Keyboard.Modifiers.HasFlag(ModifierKeys.Shift));
            pressedChar = char.ToUpper(pressedChar);

            if (IsValidChar(pressedChar))
            {
                _currentInput += pressedChar;
                UpdateOutputs();
            }

            e.Handled = true;
        }

        private bool IsValidChar(char c)
        {
            // Допускаем только русские буквы, цифры и основные знаки препинания
            return (c >= 'А' && c <= 'Я') || (c >= '0' && c <= '9') ||
                   c == ' ' || c == '.' || c == ',' || c == '?' || c == '!' || c == '\"' || c == '+' ||
                   c == '-' || c == '_' || c == '=' || c == '(' || c == ')' || c == '[' || c == ']' ||
                   c == ':' || c == '\'' || c == '@' || c == '#' || c == '$' || c == '*' || c == 'ё' || c == 'Ё';
        }

        private char ConvertEnglishToRussian(char c)
        {
            switch (char.ToLower(c))
            {
                case 'q': return 'Й';
                case 'w': return 'Ц';
                case 'e': return 'У';
                case 'r': return 'К';
                case 't': return 'Е';
                case 'y': return 'Н';
                case 'u': return 'Г';
                case 'i': return 'Й';   // Теперь правильно
                case 'o': return 'Ц';   // Теперь правильно
                case '[': return 'Х';
                case ']': return 'Ъ';
                case 'a': return 'Ф';
                case 's': return 'Ы';
                case 'd': return 'В';
                case 'f': return 'А';
                case 'g': return 'П';
                case 'h': return 'Р';
                case 'j': return 'О';
                case 'k': return 'Л';
                case 'l': return 'Д';
                case ';': return 'Ж';
                case '\'': return 'Э';
                case 'z': return 'Я';
                case 'x': return 'Ч';
                case 'c': return 'С';
                case 'v': return 'М';
                case 'b': return 'И';
                case 'n': return 'Т';
                case 'm': return 'Ь';
                case ',': return 'Б';
                case '.': return 'Ю';
                default: return c;
            }
        }

        private void UpdateOutputs()
        {
            // Проверяем, есть ли ввод
            bool hasInput = _currentInput.Length > 0;

            if (hasInput && !_enigmaIsLocked)
            {
                RotorSetTextBox.IsReadOnly = true;
                LeftRotorPosition.IsEnabled = false;
                MiddleRotorPosition.IsEnabled = false;
                RightRotorPosition.IsEnabled = false;
                LeftRotorType.IsEnabled = false;
                RightRotorType.IsEnabled = false;
                MiddleRotorType.IsEnabled = false;
                RotorSetButton.IsEnabled = false;
                EnigmaRandomButton.IsEnabled = false;
                _enigmaIsLocked = true;

                // Добавляем подсказку о блокировке
                ToolTip toolTip = new ToolTip { Content = "Настройки роторов заблокированы" };
                ToolTipService.SetToolTip(RotorSetTextBox, toolTip);
            }
            else if (!hasInput && _enigmaIsLocked)
            {
                RotorSetTextBox.IsReadOnly = false;
                LeftRotorPosition.IsEnabled = true;
                MiddleRotorPosition.IsEnabled = true;
                RightRotorPosition.IsEnabled = true;
                LeftRotorType.IsEnabled = true;
                RightRotorType.IsEnabled = true;
                MiddleRotorType.IsEnabled = true;
                RotorSetButton.IsEnabled = true;
                EnigmaRandomButton.IsEnabled = true;
                _enigmaIsLocked = false;

                // Убираем подсказку
                ToolTipService.SetToolTip(RotorSetTextBox, null);
            }

            // Обновляем вывод Морзе
            MorseOutputTextBox.Text = TextToMorse(_currentInput);

            // Обновляем вывод Энигмы
            CreateEnigmaMachine(); // Пересоздаем машину с актуальными настройками
            EnigmaOutputTextBox.Text = _enigma.ProcessText(_currentInput);

            // Прокручиваем текст вниз
            MorseOutputTextBox.ScrollToEnd();
        }

            private string MorseToText(string morseCode)
        {
            if (_reverseMorseAlphabet == null) return "ошибка декодирования";

            string[] words = morseCode.Split(new[] { " / " }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder decodedText = new StringBuilder();

            foreach (string word in words)
            {
                string[] letters = word.Split(' ', (char)StringSplitOptions.RemoveEmptyEntries);
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

            return decodedText.ToString().Trim();
        }

        private string TextToMorse(string text)
        {
            StringBuilder morseCode = new StringBuilder();
            foreach (char character in text.ToUpper().Replace('Ё', 'Е'))
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
            return morseCode.ToString().Trim();
        }

        private async void MorsePlayButton_Click(object sender, RoutedEventArgs e)
        {
            string morseCode = MorseOutputTextBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(morseCode))
            {
                MessageBox.Show("Нет сообщения для воспроизведения.",
                              "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MorsePlayButton.IsEnabled = false;

            try
            {
                foreach (char symbol in morseCode)
                {
                    if (symbol == '.')
                    {
                        PlayDot();
                        await Task.Delay(300);
                    }
                    else if (symbol == '-')
                    {
                        PlayDash();
                        await Task.Delay(300);
                    }
                    else if (symbol == ' ')
                    {
                        await Task.Delay(700);
                    }
                    else if (symbol == '/')
                    {
                        await Task.Delay(1000);
                    }
                }
            }
            finally
            {
                MorsePlayButton.IsEnabled = true;
            }
        }

        private void MorseSaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog
            {
                Filter = "Текстовый файл (*.txt)|*.txt",
                DefaultExt = "txt",
                Title = "Сохранить код Морзе"
            };

            if (saveDialog.ShowDialog() == true)
            {
                try
                {
                    File.WriteAllText(saveDialog.FileName, MorseOutputTextBox.Text);
                    MessageBox.Show("Результат успешно сохранен.",
                                   "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении: {ex.Message}",
                                  "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void PlayDot()
        {
            byte[] tone = GenerateTone(800, 100); // точка — 100 мс
            using (var stream = new MemoryStream(tone))
            using (var player = new SoundPlayer(stream))
            {
                player.PlaySync();
            }
        }

        private void PlayDash()
        {
            byte[] tone = GenerateTone(800, 300); // тире — 300 мс
            using (var stream = new MemoryStream(tone))
            using (var player = new SoundPlayer(stream))
            {
                player.PlaySync();
            }
        }

        // ===== Код для Энигмы =====
        private void InitializeRotorPositions()
        {
            foreach (var comboBox in new[] { LeftRotorPosition, MiddleRotorPosition, RightRotorPosition })
            {
                comboBox.Items.Clear();
                for (char c = 'А'; c <= 'Я'; c++)
                {
                    comboBox.Items.Add(c.ToString());
                }
                comboBox.SelectedIndex = 0;
                RotorSetTextBox.Text += comboBox.SelectedValue.ToString();
            }
        }

        private void CreateEnigmaMachine()
        {
            int leftType = LeftRotorType.SelectedIndex;
            int middleType = MiddleRotorType.SelectedIndex;
            int rightType = RightRotorType.SelectedIndex;

            int leftPos = LeftRotorPosition.SelectedIndex;
            int middlePos = MiddleRotorPosition.SelectedIndex;
            int rightPos = RightRotorPosition.SelectedIndex;

            var leftRotor = new Rotor(_rotorWirings[leftType], _rotorNotches[leftType], leftPos, 0);
            var middleRotor = new Rotor(_rotorWirings[middleType], _rotorNotches[middleType], middlePos, 0);
            var rightRotor = new Rotor(_rotorWirings[rightType], _rotorNotches[rightType], rightPos, 0);
            var reflector = new Reflector(_reflectorWiring);

            _enigma = new EnigmaMachine(leftRotor, middleRotor, rightRotor, reflector);
        }

        private void EnigmaRandomButton_Click(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();

            LeftRotorType.SelectedIndex = rnd.Next(3);
            MiddleRotorType.SelectedIndex = rnd.Next(3);
            RightRotorType.SelectedIndex = rnd.Next(3);

            LeftRotorPosition.SelectedIndex = rnd.Next(33);
            MiddleRotorPosition.SelectedIndex = rnd.Next(33);
            RightRotorPosition.SelectedIndex = rnd.Next(33);

            CreateEnigmaMachine();

            MessageBox.Show("Установлены случайные настройки роторов",
                          "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void EnigmaSaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog
            {
                Filter = "Текстовый файл (*.txt)|*.txt",
                DefaultExt = "txt",
                Title = "Сохранить результат Энигмы"
            };

            if (saveDialog.ShowDialog() == true)
            {
                try
                {
                    string metadata = $"{EnigmaOutputTextBox.Text}";

                    File.WriteAllText(saveDialog.FileName, metadata);
                    MessageBox.Show("Результат с настройками сохранен",
                                  "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении: {ex.Message}",
                                  "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void LoadEnigmaCodeButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog
            {
                Filter = "Текстовый файл (*.txt)|*.txt",
                DefaultExt = "txt",
                Title = "Загрузить код Энигмы"
            };
            if (openDialog.ShowDialog() == true)
            {
                try
                {
                    string fileContent = File.ReadAllText(openDialog.FileName);

                    DecryptedOutputTextBox.Text = DecryptEnigma(fileContent.Trim());

                    RotorSetTextBox.IsReadOnly = true;
                    LeftRotorPosition.IsEnabled = false;
                    MiddleRotorPosition.IsEnabled = false;
                    RightRotorPosition.IsEnabled = false;
                    LeftRotorType.IsEnabled = false;
                    RightRotorType.IsEnabled = false;
                    MiddleRotorType.IsEnabled = false;
                    RotorSetButton.IsEnabled = false;
                    EnigmaRandomButton.IsEnabled = false;
                    _enigmaIsLocked = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке: {ex.Message}",
                                  "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private string DecryptEnigma(string encryptedText)
        {
            CreateEnigmaMachine(); // Пересоздаём машину с текущими настройками
            return _enigma.ProcessText(encryptedText.ToUpper());
        }

        private void LoadMorseCodeButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog
            {
                Filter = "Текстовый файл (*.txt)|*.txt",
                DefaultExt = "txt",
                Title = "Загрузить код Морзе"
            };
            if (openDialog.ShowDialog() == true)
            {
                try
                {
                    string fileContent = File.ReadAllText(openDialog.FileName);
                    DecryptedOutputTextBox.Text = MorseToText(fileContent.Trim());
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке: {ex.Message}",
                                  "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void SaveDecryptedButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(DecryptedOutputTextBox.Text))
            {
                MessageBox.Show("Нет данных для сохранения.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SaveFileDialog saveDialog = new SaveFileDialog
            {
                Filter = "Текстовый файл (*.txt)|*.txt",
                DefaultExt = "txt",
                FileName = "расшифрованное_сообщение.txt",
                Title = "Сохранить расшифрованное сообщение"
            };

            if (saveDialog.ShowDialog() == true)
            {
                try
                {
                    File.WriteAllText(saveDialog.FileName, DecryptedOutputTextBox.Text);
                    MessageBox.Show("Файл успешно сохранён.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ParseEnigmaSettings(string settings)
        {
            try
            {
                string[] lines = settings.Split('\n');

                foreach (string line in lines)
                {
                    if (line.Contains("Левый ротор:"))
                    {
                        var match = System.Text.RegularExpressions.Regex.Match(line, @"(\d+) \(([А-Я])\)");
                        if (match.Success)
                        {
                            LeftRotorType.SelectedIndex = int.Parse(match.Groups[1].Value) - 1;
                            LeftRotorPosition.SelectedItem = match.Groups[2].Value;
                        }
                    }
                    else if (line.Contains("Средний ротор:"))
                    {
                        var match = System.Text.RegularExpressions.Regex.Match(line, @"(\d+) \(([А-Я])\)");
                        if (match.Success)
                        {
                            MiddleRotorType.SelectedIndex = int.Parse(match.Groups[1].Value) - 1;
                            MiddleRotorPosition.SelectedItem = match.Groups[2].Value;
                        }
                    }
                    else if (line.Contains("Правый ротор:"))
                    {
                        var match = System.Text.RegularExpressions.Regex.Match(line, @"(\d+) \(([А-Я])\)");
                        if (match.Success)
                        {
                            RightRotorType.SelectedIndex = int.Parse(match.Groups[1].Value) - 1;
                            RightRotorPosition.SelectedItem = match.Groups[2].Value;
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Не удалось прочитать настройки роторов",
                              "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void KeyButton_Click(object sender, RoutedEventArgs e)
        {
            LeftRotorPosition.IsEnabled = false;
            MiddleRotorPosition.IsEnabled = false;
            RightRotorPosition.IsEnabled = false;
            EnigmaRandomButton.IsEnabled = false;
            LeftRotorType.IsEnabled = false;
            MiddleRotorType.IsEnabled = false;
            RightRotorType.IsEnabled = false;
            if (sender is Button button)
            {
                string content = button.Content.ToString();

                if (content == "Пробел")
                {
                    // Проверяем, активен ли TextBox настройки роторов
                    if (Keyboard.FocusedElement == RotorSetTextBox) return;

                    _currentInput += ' ';
                    UpdateOutputs();
                    return;
                }

                if (content.Length == 1)
                {
                    char pressedChar = content[0];
                    pressedChar = char.ToUpper(pressedChar);

                    // Если фокус на RotorSetTextBox — игнорируем
                    if (Keyboard.FocusedElement == RotorSetTextBox) return;

                    if (IsValidChar(pressedChar))
                    {
                        _currentInput += pressedChar;
                        UpdateOutputs();
                    }
                }
            }
        }

        private void BackspaceButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentInput.Length > 0)
            {
                _currentInput = _currentInput.Remove(_currentInput.Length - 1);
                UpdateOutputs();
            }
        }

        private byte[] GenerateTone(double frequency, int durationMs)
        {
            const int sampleRate = 44100;
            const short amplitude = 30000;

            double theta = 2 * Math.PI * frequency / sampleRate;
            int samples = (int)(durationMs * sampleRate / 1000);

            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                // WAV заголовок
                WriteWavHeader(writer, samples * 2); // моно, 16 бит

                for (int i = 0; i < samples; i++)
                {
                    short s = (short)(amplitude * Math.Sin(theta * i));
                    writer.Write(s);
                }

                return stream.ToArray();
            }
        }

        private void WriteWavHeader(BinaryWriter writer, int dataLength)
        {
            writer.Write(new char[4] { 'R', 'I', 'F', 'F' });
            writer.Write(dataLength + 36); // размер файла - 8
            writer.Write(new char[4] { 'W', 'A', 'V', 'E' });
            writer.Write(new char[4] { 'f', 'm', 't', ' ' });
            writer.Write(16); // fmt chunk size
            writer.Write((short)1); // format = PCM
            writer.Write((short)1); // каналов = 1
            writer.Write(44100); // частота
            writer.Write(44100 * 2); // byte rate
            writer.Write((short)2); // block align
            writer.Write((short)16); // bits per sample
            writer.Write(new char[4] { 'd', 'a', 't', 'a' });
            writer.Write(dataLength); // размер данных
        }

        // Общий метод генерации звука


        private MemoryStream GenerateMorseWavStream(string morseCode)
        {
            const int sampleRate = 44100;
            const short amplitude = 30000;

            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);

            writer.Write(new char[] { 'R', 'I', 'F', 'F' }); // RIFF
            writer.Write(36); // заголовок
            writer.Write(new char[] { 'W', 'A', 'V', 'E' }); // WAVE

            writer.Write(new char[] { 'f', 'm', 't', ' ' }); // fmt
            writer.Write(16); // fmt size
            writer.Write((short)1); // PCM
            writer.Write((short)1); // каналы
            writer.Write(sampleRate); // частота
            writer.Write(sampleRate * 2); // байт в секунду
            writer.Write((short)2); // блок
            writer.Write((short)16); // бит на семпл

            writer.Write(new char[] { 'd', 'a', 't', 'a' }); // data
            long dataSizePos = stream.Position;
            writer.Write(0); // пока 0, потом заполним

            double theta = 2 * Math.PI * 800 / sampleRate; // 800 Гц

            long dataStart = stream.Position;

            foreach (char symbol in morseCode)
            {
                if (symbol == '.')
                {
                    WriteTone(writer, theta, amplitude, 100);
                    WriteSilence(writer, 300);// точка
                }
                else if (symbol == '-')
                {
                    WriteTone(writer, theta, amplitude, 400);
                    WriteSilence(writer, 300);// тире
                }
                else if (symbol == ' ')
                {
                    WriteSilence(writer, 1000); // между буквами
                }
                else if (symbol == '/')
                {
                    WriteSilence(writer, 1500); // между словами
                }
            }

            long dataEnd = stream.Position;

            // Записываем размер данных
            writer.Seek(4, SeekOrigin.Begin);
            writer.Write((int)(dataEnd - dataStart));

            writer.Seek(40, SeekOrigin.Begin);
            writer.Write((int)(dataEnd - dataStart));

            writer.Flush();

            return stream;
        }

        private byte[] GenerateSilence(int durationMs)
        {
            const int sampleRate = 44100;
            int samples = (int)(durationMs * sampleRate / 1000);

            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                for (int i = 0; i < samples; i++)
                {
                    writer.Write((short)0); // тишина
                }
                return stream.ToArray();
            }
        }

        private void WriteSilence(BinaryWriter writer, int durationMs)
        {
            int samples = (int)(durationMs * 44.1);
            for (int i = 0; i < samples; i++)
            {
                writer.Write((short)0); // тишина
            }
        }

        private void WriteTone(BinaryWriter writer, double theta, short amplitude, int durationMs)
        {
            int samples = (int)(durationMs * 44.1); // 44.1 kHz
            for (int i = 0; i < samples; i++)
            {
                short s = (short)(amplitude * Math.Sin(theta * i));
                writer.Write(s);
            }
        }

        // Импорт WinAPI функций
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);

        [DllImport("user32.dll")]
        private static extern IntPtr GetKeyboardLayout(uint idThread);

        [DllImport("user32.dll")]
        private static extern bool GetKeyboardState(byte[] lpKeyState);

        [DllImport("user32.dll")]
        private static extern int ToUnicodeEx(uint wVirtKey, uint wScanCode, byte[] lpKeyState,
            [Out] StringBuilder pwszBuff, int cchBuff, uint wFlags, IntPtr dwhkl);

        [DllImport("user32.dll")]
        private static extern uint MapVirtualKey(uint uCode, uint uMapType);

        private char KeyToChar(Key key, bool isShiftPressed)
        {
            if (key == Key.Space) return ' ';

            // Специальные OEM-ключи (зависят от раскладки и клавиатуры)
            if (key == Key.OemComma) return isShiftPressed ? 'Б' : 'б';
            if (key == Key.OemPeriod) return isShiftPressed ? 'Ю' : 'ю';
            if (key == Key.OemQuestion) return isShiftPressed ? ',' : '.'; // Может отличаться
            if (key == Key.OemPipe) return isShiftPressed ? '\\' : '\\';  // Обратный слеш
            if (key == Key.OemMinus) return isShiftPressed ? '_' : '-';
            if (key == Key.OemPlus) return isShiftPressed ? '+' : '=';

            string keyName = key.ToString();

            switch (key)
            {
                // Основные буквы
                case Key.A: return isShiftPressed ? 'Ф' : 'ф';
                case Key.B: return isShiftPressed ? 'И' : 'и';
                case Key.C: return isShiftPressed ? 'С' : 'с';
                case Key.D: return isShiftPressed ? 'В' : 'в';
                case Key.E: return isShiftPressed ? 'У' : 'у';
                case Key.F: return isShiftPressed ? 'А' : 'а';
                case Key.G: return isShiftPressed ? 'П' : 'п';
                case Key.H: return isShiftPressed ? 'Р' : 'р';
                case Key.I: return isShiftPressed ? 'Ш' : 'ш';
                case Key.J: return isShiftPressed ? 'О' : 'о';
                case Key.K: return isShiftPressed ? 'Л' : 'л';
                case Key.L: return isShiftPressed ? 'Д' : 'д';
                case Key.M: return isShiftPressed ? 'Ь' : 'ь';
                case Key.N: return isShiftPressed ? 'Т' : 'т';
                case Key.O: return isShiftPressed ? 'Щ' : 'щ';
                case Key.P: return isShiftPressed ? 'З' : 'з';
                case Key.Q: return isShiftPressed ? 'Й' : 'й';
                case Key.R: return isShiftPressed ? 'К' : 'к';
                case Key.S: return isShiftPressed ? 'Ы' : 'ы';
                case Key.T: return isShiftPressed ? 'Е' : 'е';
                case Key.U: return isShiftPressed ? 'Г' : 'г';
                case Key.V: return isShiftPressed ? 'М' : 'м';
                case Key.W: return isShiftPressed ? 'Ц' : 'ц';
                case Key.X: return isShiftPressed ? 'Ч' : 'ч';
                case Key.Y: return isShiftPressed ? 'Н' : 'н';
                case Key.Z: return isShiftPressed ? 'Я' : 'я';
                    
                // Цифры
                case Key.D0: return isShiftPressed ? ')' : '0';
                case Key.D1: return isShiftPressed ? '!' : '1';
                case Key.D2: return isShiftPressed ? '@' : '2';
                case Key.D3: return isShiftPressed ? '#' : '3';
                case Key.D4: return isShiftPressed ? '$' : '4';
                case Key.D5: return isShiftPressed ? '%' : '5';
                case Key.D6: return isShiftPressed ? '^' : '6';
                case Key.D7: return isShiftPressed ? '?' : '7';
                case Key.D8: return isShiftPressed ? '*' : '8';
                case Key.D9: return isShiftPressed ? '(' : '9';

                // Дополнительные символы и буквы
                case Key.OemOpenBrackets: return isShiftPressed ? 'Х' : 'х';
                case Key.OemCloseBrackets: return isShiftPressed ? 'Ъ' : 'ъ';
                case Key.OemSemicolon: return isShiftPressed ? 'Ж' : 'ж';
                case Key.OemQuotes: return isShiftPressed ? 'Э' : 'э';
                case Key.Divide: return '/'; // NumPad / или Shift + ?
                case Key.Multiply: return '*'; // NumPad *
                case Key.Subtract: return '-'; // NumPad -
                case Key.Add: return '+'; // NumPad +
                case Key.Decimal: return '.'; // NumPad .

                default:
                    return '?';
            }
        }

        // Классы для Энигмы
        public class Rotor
        {
            private string wiring;
            private int position;
            private int ringSetting;
            private char notch;

            public Rotor(string wiring, char notch, int startPosition = 0, int ringSetting = 0)
            {
                this.wiring = wiring;
                this.notch = notch;
                this.position = startPosition;
                this.ringSetting = ringSetting;
            }

            public char EncryptForward(char c)
            {
                int index = (RussianAlphabet.IndexOf(c) + position - ringSetting + RussianAlphabet.Length) % RussianAlphabet.Length;
                char encrypted = wiring[index];
                return RussianAlphabet.Alphabet[(RussianAlphabet.IndexOf(encrypted) - position + ringSetting + RussianAlphabet.Length) % RussianAlphabet.Length];
            }

            public char EncryptBackward(char c)
            {
                if (!RussianAlphabet.Contains(c))
                {
                    return c; // Пропускаем символы не из алфавита
                }

                int index = (RussianAlphabet.IndexOf(c) + position - ringSetting + RussianAlphabet.Length) % RussianAlphabet.Length;
                char sourceChar = RussianAlphabet.Alphabet[index];
                int wiringIndex = wiring.IndexOf(sourceChar);

                if (wiringIndex == -1)
                {
                    return '?'; // или можно вернуть исходный символ
                }

                char encryptedChar = RussianAlphabet.Alphabet[wiringIndex];
                return RussianAlphabet.Alphabet[
                    (RussianAlphabet.IndexOf(encryptedChar) - position + ringSetting + RussianAlphabet.Length) % RussianAlphabet.Length];
            }

            public void Rotate()
            {
                position = (position + 1) % RussianAlphabet.Length;
            }

            public bool AtNotch() => RussianAlphabet.Alphabet[position] == notch;

            public int Position
            {
                get => position;
                set => position = value % RussianAlphabet.Length;
            }

            public static class RussianAlphabet
            {
                public const string Alphabet = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ0123456789.,:?!()*+-=";
                public static int Length => Alphabet.Length;
                public static int IndexOf(char c)
                {
                    return Alphabet.IndexOf(c); // Убрали ToUpper(), так как знаки препинания не имеют регистра
                }
                public static bool Contains(char c) => Alphabet.Contains(char.ToUpper(c));
            }
        }

        public class Reflector
        {
            private string wiring;

            public Reflector(string wiring)
            {
                this.wiring = wiring;
            }

            public char Reflect(char c)
            {
                return wiring[Rotor.RussianAlphabet.IndexOf(c)];
            }
        }

        public class EnigmaMachine
        {
            private Rotor leftRotor;
            private Rotor middleRotor;
            private Rotor rightRotor;
            private Reflector reflector;

            public EnigmaMachine(Rotor left, Rotor middle, Rotor right, Reflector reflector)
            {
                leftRotor = left;
                middleRotor = middle;
                rightRotor = right;
                this.reflector = reflector;
            }

            public char Encrypt(char c)
            {
                if (!Rotor.RussianAlphabet.Contains(c))
                {
                    return c; // Пропускаем символы не из алфавита
                }

                RotateRotors();

                // Проход вперёд
                char result = rightRotor.EncryptForward(c);
                result = middleRotor.EncryptForward(result);
                result = leftRotor.EncryptForward(result);

                // Отражатель
                result = reflector.Reflect(result);

                // Обратно
                result = leftRotor.EncryptBackward(result);
                result = middleRotor.EncryptBackward(result);
                result = rightRotor.EncryptBackward(result);

                return result;
            }

            private void RotateRotors()
            {
                // Правый ротор всегда поворачивается
                bool middleShouldRotate = rightRotor.AtNotch();
                bool leftShouldRotate = middleRotor.AtNotch();

                rightRotor.Rotate();

                if (middleShouldRotate)
                {
                    middleRotor.Rotate();

                    if (leftShouldRotate)
                        leftRotor.Rotate();
                }
            }

            public string ProcessText(string text)
            {
                StringBuilder result = new StringBuilder();
                foreach (char c in text)
                {
                    result.Append(Encrypt(c));
                }
                return result.ToString();
            }

            public void SetRotorPositions(int left, int middle, int right)
            {
                leftRotor.Position = left;
                middleRotor.Position = middle;
                rightRotor.Position = right;
            }
        }

        private void EncodingButton_Click(object sender, RoutedEventArgs e)
        {

            
            RotorSetTextBox.IsReadOnly = false;
            MorseOutputTextBox.IsEnabled = true;
            EnigmaOutputTextBox.IsEnabled = true;
            EncodingButton.Background = Brushes.Green;
            DecodingButton.Background = Brushes.Gray;
            EncodingGrid.Visibility = Visibility.Visible;
            decodingGrid.Visibility = Visibility.Collapsed;
            DecryptedOutputTextBox.Text = null;
            MorseOutputTextBox.Text = null;
            EnigmaOutputTextBox.Text = null;
            DecryptedOutputTextBox.IsEnabled = false;
            _currentInput = "";

            OnScreenKeyboard.IsEnabled = true;
            LeftRotorPosition.IsEnabled = true;
            MiddleRotorPosition.IsEnabled = true;
            RightRotorPosition.IsEnabled = true;
            LeftRotorType.IsEnabled = true;
            RightRotorType.IsEnabled = true;
            MiddleRotorType.IsEnabled = true;
            RotorSetButton.IsEnabled = true;
            EnigmaRandomButton.IsEnabled = true;
            _enigmaIsLocked = false;
            //KeyButton_Click(null, null);
            //BackspaceButton_Click(null, null);
        }

        private void DecodingButton_Click(object sender, RoutedEventArgs e)
        {
            RotorSetTextBox.IsReadOnly = false;
            DecryptedOutputTextBox.IsEnabled = true;
            DecodingButton.Background = Brushes.Green;
            EncodingButton.Background = Brushes.Gray;
            EncodingGrid.Visibility = Visibility.Collapsed;
            decodingGrid.Visibility = Visibility.Visible;
            MorseOutputTextBox.Text = null;
            EnigmaOutputTextBox.Text = null;
            MorseOutputTextBox.IsEnabled = false;
            EnigmaOutputTextBox.IsEnabled = false;
            _currentInput = "";

            
            LeftRotorPosition.IsEnabled = true;
            MiddleRotorPosition.IsEnabled = true;
            RightRotorPosition.IsEnabled = true;
            LeftRotorType.IsEnabled = true;
            RightRotorType.IsEnabled = true;
            MiddleRotorType.IsEnabled = true;
            RotorSetButton.IsEnabled = true;
            EnigmaRandomButton.IsEnabled = true;
            OnScreenKeyboard.IsEnabled = false;
            //_enigmaIsLocked = true;
        }

        private void RotorSetButton_Click(object sender, RoutedEventArgs e)
        {
            string input = RotorSetTextBox.Text.ToUpper();
            //LeftRotorPosition.IsEnabled = false;
            //RightRotorPosition.IsEnabled = false;
            //MiddleRotorPosition.IsEnabled = false;
            //LeftRotorType.IsEnabled = false;
            //RightRotorType.IsEnabled = false;
            //MiddleRotorType.IsEnabled = false;
            //RotorSetTextBox.IsReadOnly = true;
            //RotorSetButton.IsEnabled = false;

            if (input.Length != 3)
            {
                MessageBox.Show("Введите ровно 3 русские буквы.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SetComboBoxSelectedItem(LeftRotorPosition, input[0]);
            SetComboBoxSelectedItem(MiddleRotorPosition, input[1]);
            SetComboBoxSelectedItem(RightRotorPosition, input[2]);

            CreateEnigmaMachine(); // Пересоздаём машину с новыми позициями
        }

        private void RotorSetTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ApplyRotorPositionsFromTextBox();
            }
        }

        private void RotorSetTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = RotorSetTextBox.Text;

            // Ограничиваем длину до 3 символов
            if (text.Length > 3)
            {
                RotorSetTextBox.Text = text.Substring(0, 3);
                RotorSetTextBox.CaretIndex = 3;
                return;
            }

            StringBuilder filtered = new StringBuilder();
            foreach (char c in text.ToUpper())
            {
                if (Rotor.RussianAlphabet.Contains(c))
                {
                    filtered.Append(c);
                }
            }

            // Если есть изменения — обновляем текст
            if (filtered.ToString() != RotorSetTextBox.Text)
            {
                RotorSetTextBox.Text = filtered.ToString();
                RotorSetTextBox.CaretIndex = filtered.Length;
            }
        }

        private async void SaveWavButton_Click(object sender, RoutedEventArgs e)
        {
            string morseCode = MorseOutputTextBox.Text.Trim();
            if (string.IsNullOrEmpty(morseCode))
            {
                MessageBox.Show("Нет данных для сохранения.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var wavStream = GenerateMorseWavStream(morseCode);

            SaveFileDialog saveDialog = new SaveFileDialog
            {
                Filter = "Звуковой файл (*.wav)|*.wav",
                DefaultExt = ".wav",
                FileName = "мorse_code.wav"
            };

            if (saveDialog.ShowDialog() == true)
            {
                try
                {
                    using (var fileStream = File.Create(saveDialog.FileName))
                    {
                        wavStream.Seek(0, SeekOrigin.Begin);
                        await wavStream.CopyToAsync(fileStream);
                    }

                    MessageBox.Show("Файл успешно сохранён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
