using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Synthesis;
using System.Speech.Recognition;
using System.Xml;


namespace SpeechBOT
{
    public partial class frmSpeechBOT : Form
    {
        SpeechSynthesizer SPKE = new SpeechSynthesizer();
        Choices list = new Choices();
        Choices SpeechCommands = new Choices();

        List<WordPattern> SpeechDictionary = new List<WordPattern>();
        private Grammar grBuilder = null;

        bool IsListening = false;
        string temp;
        private string condition;
        private string high;
        private string low;

        private string City = "Delhi";
        private string StateCode = "DL";

        public frmSpeechBOT()
        {
            SetupVoiceBOT();



            TalkToMe();


            InitializeComponent();
        }

        private void SetupVoiceBOT()
        {
            // Setup Voice Gender (Femal/Male)
            SPKE.SelectVoiceByHints((VoiceGender.Female));

            // Read Commands & Reply from File
            string[] cmdsFromFile = File.ReadLines(AppDomain.CurrentDomain.BaseDirectory + 
                                                    @"\SpeechCommands.txt").ToArray();

            // Load the Speech Grammer and Replies.
            foreach (var item in cmdsFromFile)
            {
                string[] str = item.Split('|').ToArray();
                WordPattern wp = new WordPattern();

                wp.Key = str[0];

                if (str[1] == "F")
                    wp.IsProcessCommand = false;
                else
                    wp.IsProcessCommand = true;

                wp.Command = str[2];
                wp.Reply = str[3];

                SpeechDictionary.Add(wp);
            }

            if (SpeechDictionary.Count > 0)
            {
                // Add Grammer Command words
                SpeechCommands.Add(SpeechDictionary.Select(x => x.Command).ToArray());

                // Create Grammer builder
                grBuilder = new Grammar(new GrammarBuilder(SpeechCommands));

                // Build Reply collection
                string[] replies = SpeechDictionary.Select(x => x.Reply).ToArray();

            }

        }

        private void TalkToMe()
        {
            SpeechRecognitionEngine SRE = new SpeechRecognitionEngine();
            list.Add(new string[] {"hello", "ok siri",
                                    "how are you", "what are you doing" , "where are you" , "open inet",
                                    "nice to meet you", "how is the weather",
                                    "what is the time", "what is the date", "please shut down", "sleep mode" });



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

            if (str != "hello")
            {
                txtCommands.AppendText(str + Environment.NewLine);
                txtCommands.ScrollToCaret();
                txtCommands.Refresh();
            }



            System.Windows.Forms.Application.DoEvents();

            // open and close must be hardcoded..
            if (str == "ok siri")
            {
                IsListening = true;
                ReplyToMe("Hi!, how can i help you?");
                return;
            }
            if (str == "please shut down")
            {
                IsListening = false;
                ReplyToMe("OK!, see you next time!");
                Application.Exit();
                System.Environment.Exit(0);
            }

            WordPattern spokenCommand = null;

            if (IsListening) 
            {
                //spokenCommand = SpeechDictionary.FirstOrDefault(x => x.Command == str);
                spokenCommand = SpeechDictionary.Where(x => x.Command == str).LastOrDefault();

                
                if(spokenCommand!=null)
                {
                    //WordPattern spokenCommand = SpeechDictionary.FirstOrDefault(x => x.Command == str);
                    if (spokenCommand != null)
                    {
                        switch (spokenCommand.Key.ToString())
                        {
                            case "Nor":
                            {
                                ReplyToMe(spokenCommand.Reply);
                                break;
                            }
                            case "Pro":
                            {
                                Process.Start(spokenCommand.Reply);
                                ReplyToMe("Executing!");
                                break;

                            }
                            case "Tim":
                            {
                                ReplyToMe("The time is " + System.DateTime.Now.ToString("h:mm tt"));
                                break;
                            }
                            case "Dat":
                            {
                                ReplyToMe("The date is " + System.DateTime.Now.ToString("M/d/yyyy"));
                                break;
                            }
                            case "Wea":
                            {
                                ReplyToMe("Let me get a look outside the window!");

                                int tempSI = (int)((Convert.ToInt32(GetWeather("temp", City, StateCode)) - 32) * 0.5556);
                                string weatherCondition = GetWeather("cond", City, StateCode).ToString();

                                ReplyToMe("OK!, Looks like " + weatherCondition + " in " + City + "!");
                                ReplyToMe(" with temperature of around " + tempSI + " degrees");
                                break;
                            }
                            default:
                            {
                                return;
                            }


                        }

                        



                    }
                }

                //switch (str.ToString().ToLower())
                //{


                //    case "hello":
                //    {
                //        //ReplyToMe("Hi!":;
                //        break;
                //    }
                //    case "how are you":
                //    {
                //        ReplyToMe("Awesome!, how about you?");
                //        break;

                //    }
                //    case "nice to meet you":
                //    {
                //        ReplyToMe("Glad to be here for you!");
                //        break;
                //    }
                //    case "what are you doing":
                //    {
                //        ReplyToMe("talking to you!");
                //                break;
                //    }
                //    case "where are you":
                //    {
                //        ReplyToMe("Always near you!");
                //            break;
                //    }
                //    case "open inet":
                //    {
                //        Process.Start("chrome", "http://www.google.co.in");
                //        ReplyToMe("Opening now!");
                //        break;
                //    }   
                //    case "what is the time":
                //    {
                //        ReplyToMe("The time is " + System.DateTime.Now.ToString("h:mm tt"));
                //        break;
                //    }
                //    case "what is the date":
                //    {
                //        ReplyToMe("The date is " + System.DateTime.Now.ToString("M/d/yyyy"));
                //        break;
                //    }
                //    case "sleep mode":
                //    {
                //        ReplyToMe("OK!, going to Sleep now!");
                //        IsListening = false;
                //        break;
                //    }
                //    case "how is the weather":
                //    {
                //        ReplyToMe("Let me get a look outside the window!");

                //        int tempSI = (int)((Convert.ToInt32(GetWeather("temp", City, StateCode)) - 32) * 0.5556);
                //        string weatherCondition = GetWeather("cond", City, StateCode).ToString();

                //        ReplyToMe("OK!, Looks like " + weatherCondition + " in " + City + "!");
                //        ReplyToMe(" with temperature of around " + tempSI + " degrees");
                //        break;
                //    }
                //    default:
                //    {
                //        return;
                //    }
                //}
            }    
        }

        private void ReplyToMe(string s)
        {
            txtResponse.AppendText(s + Environment.NewLine);
            txtResponse.ScrollToCaret();
            txtResponse.Refresh();

            SPKE.Speak(s);
        }



        private void label1_Click(object sender, EventArgs e)
        {

        }

        public String GetWeather(String input, string State, string StateCode)
        {
            String query = String.Format("https://query.yahooapis.com/v1/public/yql?q=select * from weather.forecast where woeid in (select woeid from geo.places(1) where text='" + State + ", "+ StateCode +"')&format=xml&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys");
            XmlDocument wData = new XmlDocument();
            wData.Load(query);

            XmlNamespaceManager manager = new XmlNamespaceManager(wData.NameTable);
            manager.AddNamespace("yweather", "http://xml.weather.yahoo.com/ns/rss/1.0");

            XmlNode channel = wData.SelectSingleNode("query").SelectSingleNode("results").SelectSingleNode("channel");
            XmlNodeList nodes = wData.SelectNodes("query/results/channel");
            try
            {
                temp = channel.SelectSingleNode("item").SelectSingleNode("yweather:condition", manager).Attributes["temp"].Value;
                condition = channel.SelectSingleNode("item").SelectSingleNode("yweather:condition", manager).Attributes["text"].Value;
                high = channel.SelectSingleNode("item").SelectSingleNode("yweather:forecast", manager).Attributes["high"].Value;
                low = channel.SelectSingleNode("item").SelectSingleNode("yweather:forecast", manager).Attributes["low"].Value;
                if (input == "temp")
                {
                    return temp;
                }
                if (input == "high")
                {
                    return high;
                }
                if (input == "low")
                {
                    return low;
                }
                if (input == "cond")
                {
                    return condition;
                }
            }
            catch
            {
                return "Error Reciving data";
            }
            return "error";
        }
    }
}
