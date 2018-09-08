using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Synthesis;
using System.Speech.Recognition;


namespace SpeechBOT
{
    public partial class frmSpeechBOT : Form
    {
        SpeechSynthesizer SPKE = new SpeechSynthesizer();
        Choices list = new Choices();
        bool IsListening = false;


        public frmSpeechBOT()
        {
            IntroduceTheBOT();

            TalkToMe();


            InitializeComponent();
        }

        private void TalkToMe()
        {
            SpeechRecognitionEngine SRE = new SpeechRecognitionEngine();
            list.Add(new string[] {"hello", "listen",
                                    "how are you",
                                    "nice to meet you",
                                    "what is the time", "can you tell me the date", "go away", "please shut down", "sleep" });

            Grammar gr = new Grammar(new GrammarBuilder(list));

            try
            {
                SRE.RequestRecognizerUpdate();
                SRE.LoadGrammar(gr);
                SRE.SetInputToDefaultAudioDevice();
                SRE.RecognizeAsync(RecognizeMode.Multiple);
                SRE.SpeechRecognized += sphRec_SpeechRecogonized;
            }
            catch
            { return; }

        }

        private void sphRec_SpeechRecogonized(object sender, SpeechRecognizedEventArgs e)
        {
            string str = e.Result.Text;

            if (str == "listen")
            {
                IsListening = true;
                ReplyToMe("Hi! how can I help you?");
            }
            if(str == "please shut down")
            {
                IsListening = false;
                ReplyToMe("OK!, Catch you later!");
                Application.Exit();
                System.Environment.Exit(0);
            }

            if (IsListening) 
            {
                

                if (str == "hello")
                {
                    //ReplyToMe("Hi!");
                }
                else if (str == "how are you")
                {
                    ReplyToMe("Awesome!, how about you?");
                }
                else if (str == "nice to meet you")
                {
                    ReplyToMe("Glad to be here for you!");
                }
                else if (str == "what is the time")
                {
                    ReplyToMe("The time is " + System.DateTime.Now.ToString("h:mm tt"));
                }
                else if (str == "can you tell me the date")
                {
                    ReplyToMe("The date is " + System.DateTime.Now.ToString("M/d/yyyy"));
                }
                else if (str == "sleep")
                {
                    ReplyToMe("OK!, Sleeping now!");
                    IsListening = false;
                }
                else
                {
                    return;
                }
            }

        }

        private void ReplyToMe(string s)
        {
            SPKE.Speak(s);
        }

        private void IntroduceTheBOT()
        {
            SPKE.SelectVoiceByHints((VoiceGender.Female));
            //SPKE.Speak("Hello, I am your assistant. How are you today?");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
