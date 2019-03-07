using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdamAsmacaOyunu
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        Random rd = new Random();
        string secilen; //kelimeler arasından seçilen kelimenin tutulduğu string
        string acilan; //seçilen kelimeden açılan harflerin tutulduğu string
        string temp; //işlem yapılacak stringi geçici olarak tutacak string
        int hata = 0; //hata sayısı
        int harfLimiti = 10; //10 dan daha az tahmin yapıldığında joker vermesi için tutulan değişken
        int joker = 0; //kazanılan joker sayısını tutan değişken
        
        private void button1_Click(object sender, EventArgs e)
        {
            gameStart();
            button2.Enabled = true;
            button3.Enabled = true;
        }

        //Oyunu başlatan fonksiyon
        private void gameStart()
        {
            hata = -1; //CinAliCiz methodunda ekrandaki Cin Ali'yi temizlettirmek için kullanılır
            cinAliCiz(); //hata değişkeni bu methoddan sonra 0 olur.
            acilan = "";
            hata = 0; //hata sıfırlanır
            harfLimiti = 10; //harf limiti yeniden belirlenir
            label2.Text = "HATA: " + hata.ToString();
            label3.Text = "HARF LİMİTİ: " + harfLimiti.ToString();

            //random olarak seçilecek kelimelerin listesi
            string[] words =
            {
                "PORTAKAL",
                "BARDAK",
                "MUSTAFA",
                "PENCERE",
                "PARLAK",
                "KUTU",
                "KUMANDA",
                "TELEFON",
                "BARDAK",
                "EKRAN",
            };

            secilen = words[rd.Next(0, words.Length)]; //random olarak kelime seçer

            //seçilen kelimin harf sayısı kadar "_" karakterini aralarında " " karakteri
            //olmak üzere label1 e yazılır.
            label1.Text = "";
            for (int i = 0; i < secilen.Length; i++)
            {
                label1.Text += "_ ";
                acilan += ' ';
            }
        }

        //textBox1'e harf girildiyse harfKontrol methodu çalıştırılır
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "") harfKontrol();
        }


        private void harfKontrol()
        {
            char harf = Convert.ToChar(textBox1.Text);

            if (secilen.Contains(harf)) //girilen harf seçilen kelimede var ise label1 yeniden yazılır
            {
                label1.Text = "";
                for (int i = 0; i < secilen.Length; i++)
                {
                    if (secilen[i] == harf) //seçilen kelimede o har var ise
                    {
                        label1.Text += harf + " "; //harf ve bir boşluk ekrana yazılır 
                        yerlestir(i, harf); //açılan stringinin i. indisine açılan harf yerleştirilir
                    }
                    else if (acilan[i] != ' ') //harf daha önceden açılmış ise
                    {
                        label1.Text += acilan[i] + " "; //açılan harf bir boşluk ile yazılır.
                    }
                    else //harf dana önce açılmamışsa
                    {
                        label1.Text += "_ "; //"_" karakteri bir boşluk ile beraber yazılır.
                    }
                }
            }
            else //eğer girilen harf seçilen kelimede yoksa 
            {
                hata++; //hata sayısı artar
                label2.Text = "HATA: " + hata.ToString(); //gerekli label güncellenir
                cinAliCiz(); //cin alinin bir parçası daha çizilir
            }

            harfLimiti--; //joker vermen için tahmine getirilen harf limiti güncellenir
            label3.Text = "HARF LİMİTİ: " + harfLimiti.ToString();
            oyunBittiMi();
            textBox1.Clear();
        }

        // Oyunun bitip bitmediğini kontrol eden method
        public void oyunBittiMi()
        {
            // yapılan hata 10 a ulaşınca oyun sona erer
            if (hata >= 10)
            {
                MessageBox.Show("Oyunu Kaybettiniz");
                gameStart(); // oyunu yeniden başlatır
            }
            else  
            {
                //yapılan hata sayısı 10 dan küçük ise ve seçilen kelimedeki harflerin tamamı açıldıysa
                //kazandınız mesajı gösterilir ve oyun yeniden başlar
                if (secilen == acilan)
                {
                    //harf limiti aşılmadıysa yeni joker eklenir
                    if (harfLimiti >= 0) {
                        joker++;
                        button4.Enabled = true;
                    } 
                    label4.Text = "JOKER: " + joker; //label4'ü günceller
                    MessageBox.Show("Kazandınız Tebrikler!\nHata Sayısı: " + hata.ToString());
                    gameStart();
                    
                }
            }
        }

        // Kelime tahmini yapıldığında seçilen kelime ile eşleşiyorsa oyunu kazanırsınız
        // Aksi halde oyunun kaybedersiniz ve oyun biter ve oyun yeniden başlar
        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == secilen)
            {
                if (harfLimiti >= 0) {
                    joker++;
                    button4.Enabled = true;
                }
                label4.Text = "JOKER: " + joker;
                MessageBox.Show("Kazandınız Tebrikler!\nHata Sayısı: " + hata.ToString());
                gameStart();
            }
            else
            {
                while (hata != 10)//Cin alinin tamamı çizilebilmesi için bu döngü kullanılır
                {
                    hata++;
                    cinAliCiz();
                }
                label2.Text = "HATA: " + hata.ToString();
                oyunBittiMi(); // hata 10'a ulaştığı için oyun biter
            }
            textBox2.Clear();
        }

        // Adam asmaca oyununun adamını asan method
        private void cinAliCiz() 
        {
            Graphics g = this.CreateGraphics();
            Pen p = new Pen(Color.Black, 5);
            if (hata == -1)
            {
                g.Clear(this.BackColor);
                hata = 0;
            }
            if (hata == 1) g.DrawLine(p, 500, 400, 650, 400);
            else if (hata == 2) g.DrawLine(p, 525, 400, 525, 200);
            else if (hata == 3) g.DrawLine(p, 525, 200, 575, 200);
            else if (hata == 4) g.DrawLine(p, 575, 200, 575, 220);
            else if (hata == 5) g.DrawEllipse(p, 560, 220, 30, 30);
            else if (hata == 6) g.DrawLine(p, 575, 250, 575, 330);
            else if (hata == 7) g.DrawLine(p, 575, 250, 545, 280);
            else if (hata == 8) g.DrawLine(p, 575, 250, 605, 280);
            else if (hata == 9) g.DrawLine(p, 575, 325, 545, 355);
            else if (hata == 10) g.DrawLine(p, 575, 325, 605, 355);
        }

        //açılan stringine, a karakteri verilen indise yerleştirilir
        private void yerlestir(int indis, char a)
        {
            temp = acilan; 
            acilan = ""; 
            for (int i = 0; i < temp.Length; i++)
            {
                if (i == indis) acilan += a; 
                else acilan += temp[i]; 
            }
        }

        // Joker kullanma butonu
        private void button4_Click(object sender, EventArgs e)
        {
            if (joker > 0) //joker var ise
            {
                int x;
                //daha önce açılmamış bir harf indisi bulur
                do
                {
                    x = rd.Next(0, secilen.Length);
                } while (acilan[x] != ' ');

                //harfKontrol methodundaki gibi gerekli harfi açar
                label1.Text = "";
                for (int i = 0; i < secilen.Length; i++)
                {
                    if (secilen[i] == secilen[x])
                    {
                        label1.Text += secilen[x] + " ";
                        yerlestir(i, secilen[x]);
                    }
                    else if (acilan[i] != ' ')
                    {
                        label1.Text += acilan[i] + " ";
                    }
                    else
                    {
                        label1.Text += "_ ";
                    }
                }
                joker--;
                label4.Text = "JOKER: " + joker;
            }
            else MessageBox.Show("Yeterli jokeriniz yok.");
        }
    }
}
