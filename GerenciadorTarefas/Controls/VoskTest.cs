using System;
using Vosk;
using NAudio.Wave;

namespace GerenciadorTarefas.Controls;

public class VoskTest : IDisposable
    {
        private readonly VoskRecognizer recognizer;
        private readonly Model model;
        private readonly WaveInEvent waveIn;

        public VoskTest(string relativeModelPath = "Libs/libVosk/vosk")
        {
            Vosk.Vosk.SetLogLevel(0); // Silencia logs

            // Caminho absoluto baseado no local do executável
            var baseDir = AppContext.BaseDirectory;
            var modelPath = Path.Combine(baseDir, relativeModelPath);

            if (!Directory.Exists(modelPath))
            {
                Console.WriteLine($"❌ Caminho do modelo não encontrado: {modelPath}");
                throw new DirectoryNotFoundException("O modelo Vosk não foi encontrado.");
            }

            model = new Model(modelPath);
            recognizer = new VoskRecognizer(model, 16000.0f);

            waveIn = new WaveInEvent
            {
                WaveFormat = new WaveFormat(16000, 1)
            };

            waveIn.DataAvailable += WaveIn_DataAvailable;
        }


        private void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (recognizer.AcceptWaveform(e.Buffer, e.BytesRecorded))
            {
                var resultado = recognizer.Result();
                var texto = ExtrairTexto(resultado);
                if (!string.IsNullOrWhiteSpace(texto))
                {
                    Console.WriteLine($"🗣️ Reconhecido: {texto}");
                }
            }
        }

        private string ExtrairTexto(string json)
        {
            // JSON no formato: {"text":"palavra reconhecida"}
            int start = json.IndexOf("\"text\":") + 8;
            int end = json.LastIndexOf("\"");
            return end > start ? json.Substring(start, end - start) : "";
        }

        public void Iniciar()
        {
            Console.WriteLine("🎙️ Fale algo no microfone...");
            waveIn.StartRecording();
        }

        public void Parar()
        {
            waveIn.StopRecording();
        }

        public void Dispose()
        {
            Parar();
            waveIn?.Dispose();
            recognizer?.Dispose();
            model?.Dispose();
        }
    }
