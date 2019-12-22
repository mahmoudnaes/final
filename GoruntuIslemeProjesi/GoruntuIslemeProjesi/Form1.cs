using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge;
using AForge.Imaging;
using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Imaging.Filters;



namespace GoruntuIslemeProjesi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private FilterInfoCollection CaptureDevices;
        private VideoCaptureDevice videoSource;

        private void Form1_Load(object sender, EventArgs e)
        {
            CaptureDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo Device in CaptureDevices)
            {
                comboBox1.Items.Add(Device.Name);
            }
            comboBox1.SelectedIndex = 0;
            videoSource = new VideoCaptureDevice();
        }

        private void start_Click(object sender, EventArgs e)
        {
            videoSource = new VideoCaptureDevice(CaptureDevices[comboBox1.SelectedIndex].MonikerString);
            videoSource.NewFrame += new NewFrameEventHandler(VideoSource_NewFrame);
            videoSource.Start();
        }

        private void VideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            //resimler tanimlama
            Bitmap image = (Bitmap)eventArgs.Frame.Clone();
            Bitmap image1 = (Bitmap)eventArgs.Frame.Clone();
            Bitmap image2 = (Bitmap)eventArgs.Frame.Clone();

            //aynalama uygulamasi
            Mirror ayna = new Mirror(false, true);
            ayna.ApplyInPlace(image);
            ayna.ApplyInPlace(image1);
            ayna.ApplyInPlace(image2);
            
            //orginal video picturebox1 aktarmasi
            pictureBox1.Image = image;

            //euclidean filtresi uygulamasi
            EuclideanColorFiltering filter = new EuclideanColorFiltering();
            filter.CenterColor = new RGB(Color.FromArgb(0, 152, 125)); //mavi ve yesil icin
            filter.Radius = 100;
            filter.ApplyInPlace(image1);
            //filter.CenterColor = new RGB(Color.FromArgb(0, 140, 10)); //yesil icin
            filter.CenterColor = new RGB(Color.FromArgb(170, 0, 0)); //kirmizi icin
            filter.Radius = 100;
            filter.ApplyInPlace(image2);
            //toplama filtresi uygulamasi
            Add topla = new Add(image2);
            Bitmap resultImage = topla.Apply(image1);

            //sonucu picturebox2 aktarmasi
            pictureBox2.Image = resultImage;


            

        }
        
        private void Stop_Click(object sender, EventArgs e)
        {
            if (videoSource.IsRunning==true)
            {
                videoSource.Stop();
            }
            Application.Exit(null);
        }
    }
}
