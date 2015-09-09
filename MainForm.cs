
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.IO;
using System.Diagnostics;
using MySql.Data.MySqlClient;
using System.Speech;
using System.Speech.Recognition;
namespace MultiFaceRec
{
    public partial class FrmPrincipal : Form
    {




        //Declararation of all variables, vectors and haarcascades
        Image<Bgr, Byte> currentFrame;
       
        Capture grabber;
        HaarCascade face;
        HaarCascade eye;
        MCvFont font = new MCvFont(FONT.CV_FONT_HERSHEY_TRIPLEX, 0.5d, 0.5d);
        Image<Gray, byte> result, TrainedFace = null;
        Image<Gray, byte> gray = null;
        List<Image<Gray, byte>> trainingImages = new List<Image<Gray, byte>>();
        List<string> labels = new List<string>();
        List<string> NamePersons = new List<string>();
        int ContTrain, NumLabels, t;
        string name, names = null;
        string prakhar;
   
        private bool captureInProgress;
        string server = "MYSQL5009.HostBuddy.com";
        string database = "db_9cf135_hawk";
        string uid = "9cf135_hawk";
        string password = "ticcttmosfet";

        public FrmPrincipal()
        {

           
    

   

           
            InitializeComponent();
            //Load haarcascades for face detection
            face = new HaarCascade("haarcascade_frontalface_default.xml");
            //eye = new HaarCascade("haarcascade_eye.xml");
            try
            {
                //Load of previus trainned faces and labels for each image
                string Labelsinfo = File.ReadAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt");
                string[] Labels = Labelsinfo.Split('%');
                NumLabels = Convert.ToInt16(Labels[0]);
                ContTrain = NumLabels;
                string LoadFaces;

                for (int tf = 1; tf < NumLabels + 1; tf++)
                {
                    LoadFaces = "face" + tf + ".bmp";
                    trainingImages.Add(new Image<Gray, byte>(Application.StartupPath + "/TrainedFaces/" + LoadFaces));
                    labels.Add(Labels[tf]);
                }

            }
            catch (Exception e)
            {
                //MessageBox.Show(e.ToString());
                MessageBox.Show("Nothing in binary database, please add at least a face(Simply train the prototype with the Add Face Button).", "Triained faces load", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            
        }

 
        private void button1_Click(object sender, EventArgs e)
        {
            //Initialize the capture device
            if (grabber == null)
            {
                try
                {
                    grabber = new Capture();
                    grabber.QueryFrame();
                }
                catch (NullReferenceException excpt)
                {
                    MessageBox.Show(excpt.Message);
                }
            }
            if (grabber != null)
            {
                if (captureInProgress)
                {

                    button1.Text = "Start live cam"; 
                    Application.Idle -= new EventHandler(FrameGrabber);
            
                }
                else
                {

                    button1.Text = "Stop live cam";
                    Application.Idle += FrameGrabber;
                }

                captureInProgress = !captureInProgress;
            }
            //Initialize the FrameGraber event
            

        }
        
        

        private void button2_Click(object sender, System.EventArgs e)
        {
            try
            {
                //Trained face counter
                ContTrain = ContTrain + 1;

                //Get a gray frame from capture device
                gray = grabber.QueryGrayFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

                //Face Detector
                MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(
                face,
                1.2,
                10,
                Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                new Size(20, 20));

                //Action for each element detected
                foreach (MCvAvgComp f in facesDetected[0])
                {
                    TrainedFace = currentFrame.Copy(f.rect).Convert<Gray, byte>();
                    break;
                }

                //resize face detected image for force to compare the same size with the 
                //test image with cubic interpolation type method
                TrainedFace = result.Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                trainingImages.Add(TrainedFace);
                labels.Add(textBox1.Text);

                //Show face added in gray scale
                imageBox1.Image = TrainedFace;

                //Write the number of triained faces in a file text for further load
                File.WriteAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt", trainingImages.ToArray().Length.ToString() + "%");

                //Write the labels of triained faces in a file text for further load
                for (int i = 1; i < trainingImages.ToArray().Length + 1; i++)
                {
                    trainingImages.ToArray()[i - 1].Save(Application.StartupPath + "/TrainedFaces/face" + i + ".bmp");
                    trainingImages.ToArray()[i - 1].Save(Application.StartupPath + "/photo/" + textBox1.Text + ".jpg");
                    File.AppendAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt", labels.ToArray()[i - 1] + "%");
                }

                MessageBox.Show(textBox1.Text + "´s face detected and added :)", "Training OK", MessageBoxButtons.OK, MessageBoxIcon.Information);

                
                string connectionString;
                connectionString = "SERVER=" + server + ";" + "DATABASE=" +
                database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
                MySqlConnection con = new MySqlConnection(connectionString);
                con.Open();
                string path = "photo/" + textBox1.Text + ".jpg";
                MySqlCommand cmd = new MySqlCommand("insert into user (`id`,`photo`) values('" + textBox1.Text + "','" + path + "')", con);
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch
            {
                MessageBox.Show("Enable the face detection first", "Training Fail", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


        void FrameGrabber(object sender, EventArgs e)
        {
            label3.Text = "0";
            //label4.Text = "";
            NamePersons.Add("");


            //Get the current frame form capture device
            currentFrame = grabber.QueryFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

            //Convert it to Grayscale
            gray = currentFrame.Convert<Gray, Byte>();

            //Face Detector
            MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(
          face,
          1.2,
          10,
          Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
          new Size(20, 20));


            foreach (MCvAvgComp f in facesDetected[0])
            {
                t = t + 1;
                result = currentFrame.Copy(f.rect).Convert<Gray, byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                //draw the face detected in the 0th (gray) channel with blue color
                currentFrame.Draw(f.rect, new Bgr(Color.Red), 2);


                if (trainingImages.ToArray().Length != 0)
                {
                    //TermCriteria for face recognition with numbers of trained images like maxIteration
                    MCvTermCriteria termCrit = new MCvTermCriteria(ContTrain, 0.001);

                    //Eigen face recognizer
                    EigenObjectRecognizer recognizer = new EigenObjectRecognizer(
                       trainingImages.ToArray(),
                       labels.ToArray(),
                       3000,
                       ref termCrit);

                    name = recognizer.Recognize(result);



                    //Draw the label for each face detected and recognized
                    currentFrame.Draw(name, ref font, new Point(f.rect.X - 2, f.rect.Y - 2), new Bgr(Color.LightGreen));

                }



                NamePersons[t - 1] = name;
                NamePersons.Add("");


                //Set the number of faces detected on the scene
                label3.Text = facesDetected[0].Length.ToString();




            }
            t = 0;

            //Names concatenation of persons recognized
            for (int nnn = 0; nnn < facesDetected[0].Length; nnn++)
            {
                names = names + NamePersons[nnn] + ", ";
            }
            //Show the faces procesed and recognized
            imageBoxFrameGrabber.Image = currentFrame;
            label4.Text = names;

            names = "";
            //Clear the list(vector) of names
            NamePersons.Clear();


        }




        private void button3_Click(object sender, EventArgs e)
        {
/*  MCvTermCriteria termCrit = new MCvTermCriteria(ContTrain, 0.001);

            EigenObjectRecognizer recognizer = new EigenObjectRecognizer(trainingImages.ToArray(), labels.ToArray(), 3000, ref termCrit);

            name = recognizer.Recognize(result);
            prakhar = name;


            info frm = new info();
            frm.MyProperty = prakhar;
            frm.Show();

            */
            panel1.Visible = true;
            for (int i = 0; i <= 908; i=i+5)
            {
                panel1.Size = new Size(i,575);
            }

        }

        private void FrmPrincipal_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {


                currentFrame = new Image<Bgr, byte>(openFileDialog1.FileName);


                imageBoxFrameGrabber.Image = currentFrame;


                label3.Text = "0";
                //label4.Text = "";
                NamePersons.Add("");


                //Get the current frame form capture device
                currentFrame = currentFrame.Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

                //Convert it to Grayscale
                gray = currentFrame.Convert<Gray, Byte>();

                //Face Detector
                MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(
              face,
              1.2,
              10,
              Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
              new Size(20, 20));


                foreach (MCvAvgComp f in facesDetected[0])
                {
                    t = t + 1;
                    result = currentFrame.Copy(f.rect).Convert<Gray, byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                    //draw the face detected in the 0th (gray) channel with blue color
                    currentFrame.Draw(f.rect, new Bgr(Color.Red), 2);


                    if (trainingImages.ToArray().Length != 0)
                    {
                        //TermCriteria for face recognition with numbers of trained images like maxIteration
                        MCvTermCriteria termCrit = new MCvTermCriteria(ContTrain, 0.001);

                        //Eigen face recognizer
                        EigenObjectRecognizer recognizer = new EigenObjectRecognizer(
                           trainingImages.ToArray(),
                           labels.ToArray(),
                           3000,
                           ref termCrit);

                        name = recognizer.Recognize(result);



                        //Draw the label for each face detected and recognized
                        currentFrame.Draw(name, ref font, new Point(f.rect.X - 2, f.rect.Y - 2), new Bgr(Color.LightGreen));

                    }



                    NamePersons[t - 1] = name;
                    NamePersons.Add("");


                    //Set the number of faces detected on the scene
                    label3.Text = facesDetected[0].Length.ToString();




                }
                t = 0;

                //Names concatenation of persons recognized
                for (int nnn = 0; nnn < facesDetected[0].Length; nnn++)
                {
                    names = names + NamePersons[nnn] + ", ";
                }
                //Show the faces procesed and recognized
                imageBoxFrameGrabber.Image = currentFrame;
                label4.Text = names;

                names = "";
                //Clear the list(vector) of names
                NamePersons.Clear();


            }

        }

        private void button5_Click(object sender, EventArgs e)
        {

            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                currentFrame = new Image<Bgr, byte>(openFileDialog2.FileName);
                //Trained face counter
                imageBoxFrameGrabber.Image = currentFrame;
                currentFrame = currentFrame.Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                ContTrain = ContTrain + 1;
                //Convert it to Grayscale
                gray = currentFrame.Convert<Gray, Byte>();
                //Get a gray frame from capture device


                //Face Detector
                MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(
                face,
                1.2,
                10,
                Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                new Size(20, 20));

                //Action for each element detected
                foreach (MCvAvgComp f in facesDetected[0])
                {
                    currentFrame.Draw(f.rect, new Bgr(Color.Red), 2);
                    result = currentFrame.Copy(f.rect).Convert<Gray, byte>();
                   
                    break;
                }
                TrainedFace = result.Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
               
                imageBox1.Image = TrainedFace;
               if(textBox1.Text.Length>0)
               {
                //resize face detected image for force to compare the same size with the 
                //test image with cubic interpolation type method
              
                trainingImages.Add(TrainedFace);
                labels.Add(textBox1.Text);

                //Show face added in gray scale
               

                    //Write the number of triained faces in a file text for further load
                    File.WriteAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt", trainingImages.ToArray().Length.ToString() + "%");

                    //Write the labels of triained faces in a file text for further load
                    for (int i = 1; i < trainingImages.ToArray().Length + 1; i++)
                    {
                        trainingImages.ToArray()[i - 1].Save(Application.StartupPath + "/TrainedFaces/face" + i + ".bmp");
                        trainingImages.ToArray()[i - 1].Save(Application.StartupPath + "/photo/" + textBox1.Text + ".jpg");
                        File.AppendAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt", labels.ToArray()[i - 1] + "%");
                    }

                    MessageBox.Show(textBox1.Text + "´s face detected and added :)", "Training OK", MessageBoxButtons.OK, MessageBoxIcon.Information);

                   
                    string connectionString;
                    connectionString = "SERVER=" + server + ";" + "DATABASE=" +
                    database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
                    MySqlConnection con = new MySqlConnection(connectionString);
                    con.Open();
                    string path = "photo/" + textBox1.Text + ".jpg";
                    MySqlCommand cmd = new MySqlCommand("insert into user (`id`,`photo`) values('" + textBox1.Text + "','" + path + "')", con);
                    cmd.ExecuteNonQuery();
                    con.Close();

                
            }
        }
            }

        private void button7_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }


        // Handle the SpeechRecognized event.
        static void recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            Console.WriteLine("Recognized text: " + e.Result.Text);
        }
        private void buttonX1_Click(object sender, EventArgs e)
        {
            
        }

        
        

        private void button10_Click_1(object sender, EventArgs e)
        {
            SpeechRecognitionEngine recognizer = new SpeechRecognitionEngine();
            Choices colors = new Choices();
            colors.Add(new string[] { "red", "green", "blue", "who", "mehak" });
            GrammarBuilder gb = new GrammarBuilder();
            gb.Append(colors);

            // Create the Grammar instance and load it into the speech recognition engine.
            Grammar g = new Grammar(gb);
            recognizer.LoadGrammar(g);



            try
            {
                
                recognizer.SetInputToDefaultAudioDevice();
                RecognitionResult result = recognizer.Recognize();
                if (result == null)
                {
                    label7.Text = "";
                }
                else
                {
                    label7.Text = result.Text;
                    if (label7.Text == "red")
                    {
                        button3.PerformClick();
                    }
                   
                }
            }
            catch (InvalidOperationException exception)
            {
                button1.Text = String.Format("Could not recognize input from default aduio device. Is a microphone or sound card available?\r\n{0} - {1}.", exception.Source, exception.Message);
            }
            finally
            {
                recognizer.UnloadAllGrammars();
            }            
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Form frm2 = new name();
            frm2.Show();
        }

     

       

        private void button12_Click_1(object sender, EventArgs e)
        {
            panel1.Hide();
        }

        private void FrmPrincipal_Load(object sender, EventArgs e)
        {

        }

        
    
       
    }
}


        
        

    
