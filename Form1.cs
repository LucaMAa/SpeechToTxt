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
using System.IO;

namespace speechtotext
{
    public partial class Form1 : Form
    {
       
        SpeechRecognitionEngine _rec = new SpeechRecognitionEngine();
        SpeechSynthesizer Vittoria = new SpeechSynthesizer();
        SpeechRecognitionEngine startLis = new SpeechRecognitionEngine();
        Random rd = new Random();
        int recTim = 0;
        DateTime data = DateTime.Now;
        public Form1()
        {
            InitializeComponent();   
        }
     
        private void Form1_Load(object sender, EventArgs e)
        {
           
            _rec.SetInputToDefaultAudioDevice();
            _rec.LoadGrammarAsync(new Grammar(new GrammarBuilder(new Choices(File.ReadAllLines("@DefaultDocument.txt")))));
            _rec.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(def_speech1);
            _rec.SpeechDetected += new EventHandler<SpeechDetectedEventArgs>(def_speech2);
            _rec.RecognizeAsync(RecognizeMode.Multiple);
            startLis.SetInputToDefaultAudioDevice();
            startLis.LoadGrammarAsync(new Grammar(new GrammarBuilder(new Choices(File.ReadAllLines("@DefaultDocument.txt")))));
            startLis.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(startLis_speechRec);
        }

        private void startLis_speechRec(object sender, SpeechRecognizedEventArgs e)
        {
            int ranNum;
            string speech = e.Result.Text;
            if(speech=="ciao")
            {
                Vittoria.SpeakAsync("ciao,sono felice di rivederti");
            }
            if(speech=="come stai?")
            {
                Vittoria.SpeakAsync("sto bene,tu cone stai?");
            }
            if(speech=="che ore sono?")
            {
                Vittoria.SpeakAsync(DateTime.Now.ToString());
            }
            if(speech=="smetti di parlare")
            {
                Vittoria.SpeakAsyncCancelAll();
                ranNum = rd.Next(1, 2);
                if(ranNum==1)
                {
                    Vittoria.SpeakAsync("scusa capo");
                }
                if (ranNum == 2)
                {
                    Vittoria.SpeakAsync("sarò più calma");
                }
            }
            if (speech == "smetti di ascoltare")
            {
                Vittoria.SpeakAsync("se dovessi avere bisogno sono qui");
                _rec.RecognizeAsyncCancel();
                startLis.RecognizeAsync(RecognizeMode.Multiple);
            }
            if(speech=="mostrami i comandi")
            {
                string[] comandi = (File.ReadAllLines("@DefaultDocument"));
                listBox1.Items.Clear();
                listBox1.SelectionMode = SelectionMode.None;
                listBox1.Visible = true;
                foreach(string command in comandi)
                {
                    listBox1.Items.Add(command);
                }
            }
            if(speech=="nascondi i comandi")
            {
                listBox1.Visible = false;
            }
           
        }

        private void def_speech2(object sender, SpeechDetectedEventArgs e)
        {
            recTim = 0;

        }

        private void def_speech1(object sender, SpeechRecognizedEventArgs e)
        {
            string speech = e.Result.Text;
            if(speech=="ci sei")
            {
                startLis.RecognizeAsyncCancel();
                Vittoria.SpeakAsync("ci sono");
                _rec.RecognizeAsync(RecognizeMode.Multiple);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(recTim==10)
            {
                _rec.RecognizeAsyncCancel();
            }
            else if(recTim==11)
            {
                timer1.Stop();
                startLis.RecognizeAsync(RecognizeMode.Multiple);
                recTim = 0;
            }
        }
    }
}
