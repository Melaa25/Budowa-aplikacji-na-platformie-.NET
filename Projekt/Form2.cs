using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using NAudio.Wave;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;


namespace Projekt
{
    public partial class Form2 : Form
    {
        
        private WaveOutEvent _waveOut;
        private AudioFileReader _audioFileReader;

        private bool _isKeyPressed = false; 
        private bool _isRecording = false; 

        private string _csvFilePath; 
        private string _jsonFilePath; 


        private List<string> _recordedNotes = new List<string>();


        public Form2(string csvFilePath = null, string jsonFilePath = null)
        {
            InitializeComponent(); 
            InitializeButtonEvents(); 

            this.KeyPreview = true; 
            this.KeyDown += Form2_KeyDown; 
            this.KeyUp += Form2_KeyUp;


            cSVToolStripMenuItem.Click += CsvToolStripMenuItem_Click;
            jSONToolStripMenuItem.Click += jSONToolStripMenuItem_Click;

            _csvFilePath = csvFilePath;
            _jsonFilePath = jsonFilePath;

            if (!string.IsNullOrEmpty(_csvFilePath))
            {
                _ = PlayMelodyFromCsv(_csvFilePath);
            }

            if (!string.IsNullOrEmpty(_jsonFilePath))
            {
                _ = PlayMelodyFromJson(_jsonFilePath);
            }
        }

        private void InitializeButtonEvents()
        {

            button1.MouseDown += Button_MouseDown;
            button1.MouseUp += Button_MouseUp;

            button2.MouseDown += Button_MouseDown;
            button2.MouseUp += Button_MouseUp;

            button3.MouseDown += Button_MouseDown;
            button3.MouseUp += Button_MouseUp;

            button4.MouseDown += Button_MouseDown;
            button4.MouseUp += Button_MouseUp;

            button5.MouseDown += Button_MouseDown;
            button5.MouseUp += Button_MouseUp;

            button6.MouseDown += Button_MouseDown;
            button6.MouseUp += Button_MouseUp;

            button7.MouseDown += Button_MouseDown;
            button7.MouseUp += Button_MouseUp;

            button8.MouseDown += Button_MouseDown;
            button8.MouseUp += Button_MouseUp;

            button9.MouseDown += Button_MouseDown;
            button9.MouseUp += Button_MouseUp;

            button10.MouseDown += Button_MouseDown;
            button10.MouseUp += Button_MouseUp;

            button11.MouseDown += Button_MouseDown;
            button11.MouseUp += Button_MouseUp;

            button12.MouseDown += Button_MouseDown;
            button12.MouseUp += Button_MouseUp;

            button13.MouseDown += Button_MouseDown;
            button13.MouseUp += Button_MouseUp;

            button14.MouseDown += Button_MouseDown;
            button14.MouseUp += Button_MouseUp;

            button15.MouseDown += Button_MouseDown;
            button15.MouseUp += Button_MouseUp;
        }

        private async void CsvToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    await PlayMelodyFromCsv(filePath);
                }
            }
        }

        private async Task PlayMelodyFromCsv(string filePath)
        {
            try
            {
                var lines = File.ReadAllLines(filePath); 

                foreach (var line in lines)
                {
                    var parts = line.Split(','); 
                    if (parts.Length == 2)
                    {
                        string sound = parts[0].Trim();
                        int duration = int.Parse(parts[1].Trim());

                        Button button = GetButtonForSound(sound); 
                        if (button != null)
                        {
                            PlaySound(GetSoundFileForButton(button)); 
                            await VisualizeKeyPress(button, duration); 
                            StopSound(); 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas odczytu pliku CSV: {ex.Message}", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void Form2_KeyDown(object sender, KeyEventArgs e)
        {
            if (_isKeyPressed) return;
            _isKeyPressed = true;

            Button button = GetButtonForKey(e.KeyCode); 
            if (button != null)
            {
                if (IsBlackKey(button))
                {
                    button.BackColor = Color.Gray; 
                }
                else
                {
                    button.BackColor = Color.LightGray; 
                }

                string soundFile = GetSoundFileForButton(button);
                PlaySound(soundFile); 
            }
        }

        private async Task VisualizeKeyPress(Button button, int duration)
        {
            if (button != null)
            {
                if (IsBlackKey(button))
                {
                    button.BackColor = Color.Gray; 
                }
                else
                {
                    button.BackColor = Color.LightGray; 
                }

                await Task.Delay(duration); 

                if (IsBlackKey(button))
                {
                    button.BackColor = Color.Black; 
                }
                else
                {
                    button.BackColor = Color.White; 
                }
            }
        }

        private Button GetButtonForSound(string sound)
        {
            switch (sound.ToUpper())
            {
                case "C": return button1;
                case "D": return button2;
                case "E": return button4;
                case "F": return button5;
                case "G": return button6;
                case "A": return button7;
                case "H": return button8;
                case "C2": return button9;
                case "D2": return button10;
                case "C#": return button3;
                case "D#": return button11;
                case "F#": return button12;
                case "G#": return button13;
                case "A#": return button14;
                case "C#2": return button15;
                default: return null;
            }
        }

        private void Form2_KeyUp(object sender, KeyEventArgs e)
        {
            _isKeyPressed = false;

            Button button = GetButtonForKey(e.KeyCode); 
            if (button != null)
            {
                if (IsBlackKey(button))
                {
                    button.BackColor = Color.Black; 
                }
                else
                {
                    button.BackColor = Color.White; 
                }
            }

            StopSound(); 
        }

        
        private Button GetButtonForKey(Keys key)
        {
            switch (key)
            {
                case Keys.A: return button1;
                case Keys.S: return button2;
                case Keys.D: return button4;
                case Keys.F: return button5;
                case Keys.G: return button6;
                case Keys.H: return button7;
                case Keys.J: return button8;
                case Keys.K: return button9;
                case Keys.L: return button10;
                case Keys.W: return button3;
                case Keys.E: return button11;
                case Keys.T: return button12;
                case Keys.Y: return button13;
                case Keys.U: return button14;
                case Keys.O: return button15;
                default: return null;
            }
        }

        private string GetSoundFileForButton(Button button)
        {
            if (button == null) return string.Empty;

            switch (button.Name)
            {
                case "button1": return "C.wav";
                case "button2": return "D.wav";
                case "button3": return "C#.wav";
                case "button4": return "E.wav";
                case "button5": return "F.wav";
                case "button6": return "G.wav";
                case "button7": return "A.wav";
                case "button8": return "H.wav";
                case "button9": return "C2.wav";
                case "button10": return "D2.wav";
                case "button11": return "D#.wav";
                case "button12": return "F#.wav";
                case "button13": return "G#.wav";
                case "button14": return "A#.wav";
                case "button15": return "C#2.wav";
                default: return string.Empty;
            }
        }


        private bool IsBlackKey(Button button)
        {
            return button == button3 || button == button11 || button == button12 ||
                   button == button13 || button == button14 || button == button15;
        }
        private DateTime _lastNoteTime = DateTime.Now;


        private void Button_MouseDown(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                string soundFile = GetSoundFileForButton(button);
                PlaySound(soundFile);

                if (_isRecording)
                {
                    string note = button.Text.Split('\n')[0]; 
                    int duration = 500; 

                    if (_recordedNotes.Count > 0)
                    {
                        duration = (int)(DateTime.Now - _lastNoteTime).TotalMilliseconds;
                        duration = Math.Max(duration, 100); 
                    }

                    _recordedNotes.Add($"{note},{duration}"); 
                    _lastNoteTime = DateTime.Now; 
                }
            }
        }

        private void Button_MouseUp(object sender, MouseEventArgs e)
        {
            StopSound();
        }

        private void PlaySound(string soundFileName)
        {
            try
            {
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sounds", soundFileName);

                if (File.Exists(filePath))
                {
                    _audioFileReader = new AudioFileReader(filePath); 
                    _waveOut = new WaveOutEvent();
                    _waveOut.Init(_audioFileReader); 
                    _waveOut.Play(); 
                }
                else
                {
                    MessageBox.Show($"Plik dźwiękowy {filePath} nie istnieje!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas odtwarzania dźwięku: {ex.Message}");
            }
        }

        private void StopSound()
        {
            if (_waveOut != null)
            {
                _waveOut.Stop();
                _waveOut.Dispose();
                _waveOut = null;
            }

            if (_audioFileReader != null)
            {
                _audioFileReader.Dispose();
                _audioFileReader = null;
            }
        }


        private async void jSONToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_jsonFilePath))
            {
                await PlayMelodyFromJson(_jsonFilePath); 
                return;
            }

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    _jsonFilePath = openFileDialog.FileName; 
                    await PlayMelodyFromJson(_jsonFilePath);
                }
            }
        }

        private async Task PlayMelodyFromJson(string filePath)
        {
            try
            {
                string jsonContent = File.ReadAllText(filePath); 
                var melody = JsonConvert.DeserializeObject<List<MelodyNote>>(jsonContent); 

                foreach (var note in melody)
                {
                    Button button = GetButtonForSound(note.Sound); 
                    if (button != null)
                    {
                        StopSound(); 
                        PlaySound(GetSoundFileForButton(button)); 
                        await VisualizeKeyPress(button, note.Duration); 
                        StopSound(); 
                        await Task.Delay(50);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas odczytu pliku JSON: {ex.Message}", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private class MelodyNote
        {
            public string Sound { get; set; }
            public int Duration { get; set; }
        }

        private void recordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _isRecording = !_isRecording; 

            if (_isRecording)
            {
                this.BackColor = Color.Red; 
                _recordedNotes.Clear(); 
            }
            else
            {
                this.BackColor = SystemColors.Control; 
                SaveRecordingToCsv(); 
            }
        }

        private void SaveRecordingToCsv()
        {
            if (_recordedNotes.Count == 0)
            {
                MessageBox.Show("Brak nagranych nut!", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                saveFileDialog.Title = "Zapisz nagranie jako CSV";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllLines(saveFileDialog.FileName, _recordedNotes.Select(line => line.Replace("\r", "").Replace("\n", "").Trim()));

                    MessageBox.Show("Nagranie zapisane pomyślnie!", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

       

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void exitToMenuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 mainForm = new Form1();
            mainForm.Show();
            this.Close();
        }
    }
}
