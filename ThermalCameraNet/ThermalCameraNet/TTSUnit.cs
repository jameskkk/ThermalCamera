using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Synthesis;

namespace ThermalCameraNet
{
    public class TTSUnit
    {
        private SpeechSynthesizer m_SpeechSynthesizer = null;

        public void InitSpeech()
        {
            m_SpeechSynthesizer = new SpeechSynthesizer();
            m_SpeechSynthesizer.SetOutputToDefaultAudioDevice();
        }

        public void SpeechAsync()
        {
            if (m_SpeechSynthesizer != null)
            {
                PromptBuilder pb = new PromptBuilder();
                pb.StartVoice("Microsoft Hanhan Desktop"); // Chinese
                pb.AppendText("警告");
                pb.AppendBreak(PromptBreak.Small);
                pb.AppendText("偵測到溫度超標");
                pb.EndVoice();
                m_SpeechSynthesizer.SpeakAsync(pb);
            }
        }
    }
}
