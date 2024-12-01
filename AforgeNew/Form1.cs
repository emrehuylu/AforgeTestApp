using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using ZXing;


namespace AforgeNew
{
	public partial class PictureTake : Form
	{
		public PictureTake()
		{
			InitializeComponent();
		}
		FilterInfoCollection fico;   //Bilgisayardaki kameraları tutuyor
		VideoCaptureDevice vcd;
		private void Form1_Load(object sender, EventArgs e)
		{
			fico = new FilterInfoCollection(FilterCategory.VideoInputDevice);

			//Bilgisayarıma bağlı olan kameraların isimlerini combobox'a getirir.
			foreach (FilterInfo f in fico)
			{
				comboBox1.Items.Add(f.Name);  //bağlı tüm kameralar
				comboBox1.SelectedIndex = 0;  // 0'ıncı indexteki kamera
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			vcd = new VideoCaptureDevice(fico[comboBox1.SelectedIndex].MonikerString);   //Seçilen Kameraya takma bir isim ver.
			vcd.NewFrame += Vcd_NewFrame;
			vcd.Start();                //Video Capture Device başlat
			timer1.Start();
			richTextBox1.Clear();
		}

		//kameradan aldığımız çerçevemizi aktarıyoruz.
		private void Vcd_NewFrame(object sender, NewFrameEventArgs EventArgs)
		{
			pictureBox1.Image = (Bitmap)EventArgs.Frame.Clone();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			SaveFileDialog s = new SaveFileDialog();
			s.Filter = "(*.jpg) |*.jpg";
			DialogResult dr = s.ShowDialog();
			if (dr == DialogResult.OK)
			{
				pictureBox1.Image.Save(s.FileName);

			}
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			//barkod okuma işlemi

			if (pictureBox1.Image != null)
			{
				BarcodeReader brd = new BarcodeReader();
				Result sonuc = brd.Decode((Bitmap)pictureBox1.Image);

				if (sonuc != null)
				{
					richTextBox1.Text = sonuc.ToString();
					timer1.Stop();
				}
			}

		}
	}
}
