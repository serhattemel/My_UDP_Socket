using System;
using System.Net.Sockets;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;

namespace UDP_Client
{
    public partial class MyUDPSocket : Control
    {
        public MyUDPSocket()
        {
            InitializeComponent();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.Controls.Add(button1);
            this.button1.Location = new System.Drawing.Point(274, 223);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(205, 67);
            this.button1.TabIndex = 0;
            this.button1.Text = "Mesaj Gönder";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.Controls.Add(textBox1);
            this.textBox1.Location = new System.Drawing.Point(274, 195);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(205, 20);
            this.textBox1.TabIndex = 1;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label1
            // 
            this.Controls.Add(label1);
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.Location = new System.Drawing.Point(12, 193);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(256, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Göndermek İstediğiniz Metni Girin :";
            // 
            // label2
            // 
            this.Controls.Add(label2);
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label2.Location = new System.Drawing.Point(239, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(240, 37);
            this.label2.TabIndex = 2;
            this.label2.Text = "My UDP Socket";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // listBox1
            //
            this.Controls.Add(listBox1);
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(504, 52);
            this.listBox1.Name = "listBox";
            this.listBox1.Size = new System.Drawing.Size(252, 342);
            this.listBox1.TabIndex = 4;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Butona basıldığında server ile bağlantı kuruyor ve text box içerisindeki mesajı server'a iletiyor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            byte[] data = new byte[1024];
            string input;
            int recv;
            //Bağlatı için gerekli olan ip adresimizi ve port numaramızı içeren ipep (IP End Point) nesnesini tanımlıyor.
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9050);

            // Socket sınıfını kullanarak UDP socket için gerekli olan server nesnesini tanımlıyor.
            Socket server = new Socket(AddressFamily.InterNetwork,
                           SocketType.Dgram, ProtocolType.Udp);
            server.SetSocketOption(SocketOptionLevel.Udp, SocketOptionName.NoDelay, 3000);

            //Server'a gönderdiğimiz mesaj öncesinde Bizim tarafımızdan gönderildiğini belirten yazı.
            string client = "Client Massage: ";
            //client adındaki string veri tipli değişkeni ASCII tipine dönüştürerek server'a iletiyor.
            data = Encoding.ASCII.GetBytes(client);
            server.SendTo(data, data.Length, SocketFlags.None, ipep);

            IPEndPoint send = new IPEndPoint(IPAddress.Any, 0);
            EndPoint Remote = (EndPoint)send;

            //Text Box içerisinde yazılı olan değeri input'a atıyor.
            input = textBox1.Text;
            // Listbox kısmına gönderdiğimiz mesajı yazıyor.
            listBox1.Items.Add(input);
            //server'a mesajımızı gönderiyor.
            server.SendTo(Encoding.ASCII.GetBytes(input), ipep);
            // Mesaj gönderim işlemimiz başarılı olmadıysa bu noktaya gelerek tekrar kontrol yapmamızı sağlayan point noktamızı tanımlıyor.
            point:
            try
            {
                data = new byte[1024];
                //Veri arabelleğine bir veri birimi alır ve uç noktayı depolar.
                recv = server.ReceiveFrom(data, ref Remote);
            }
            // bir soket hatası oluştuğunda bu hatayı yakalıyor.
            catch (SocketException)
            {
                // Hata aldığımızı ve mesajın tekrar iletildiğini bizlere iletiyor.
                listBox1.Items.Add("Error! The message is being resent.");
                //Mesajı tekrar server'a gönderiyor.
                server.SendTo(Encoding.ASCII.GetBytes(input), ipep);
                //Tekrar gönderdiğimiz mesajın ulaşıp ulaşmadığını kontrol etmek için try catch yapısının başına gidiyor.
                goto point;
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
