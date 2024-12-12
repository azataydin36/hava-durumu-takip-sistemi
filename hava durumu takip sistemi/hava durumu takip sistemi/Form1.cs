
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace hava_durumu_takip_sistemi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        public void islem1()
        {
            if (TB1.Text == "")
            {
                MessageBox.Show("Lütfen ilk önce bir lokasyon giriniz!", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                string htmlCode = "";

                using (WebClient client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    htmlCode = client.DownloadString($"https://api.openweathermap.org/data/2.5/weather?q={TB1.Text}&appid=c69926e5a99b43afe8808e2bac6a6be8&lang=tr");
                }

                dynamic stuff = JObject.Parse(htmlCode);

                dynamic sicaklik = stuff.main.temp;
                int veriSicaklik = (int)(sicaklik - 273.15); // Kelvin'den Celsius'a dönüşüm
                L1.Text = veriSicaklik.ToString() + "°C";
                L1.Location = new Point(this.Size.Width / 2 - L1.Size.Width / 2 - 10, 160);

                dynamic picture = stuff.weather[0].icon;
                PB1.Load($"https://openweathermap.org/img/w/{picture}.png");

                dynamic status = stuff.weather[0].description;
                L2.Text = status;
                L2.Location = new Point(this.Size.Width / 2 - L2.Size.Width / 2 - 10, 227);

                dynamic country = stuff.sys.country;
                dynamic name = stuff.name;

                L3.Text = $"{name}, {country}";
                L3.Location = new Point(this.Size.Width / 2 - L3.Size.Width / 2 - 10, 441);

                TB1.Text = "";
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Thread islem = new Thread(() => islem1());
            islem.Start();
        }
    }
}