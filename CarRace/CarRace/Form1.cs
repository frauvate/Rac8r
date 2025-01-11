using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRace
{
    public partial class Form1 : Form
    {
        // Trafik araçlarının hızını belirten değişken.
        int trafficSpeed;
        // Oyuncu aracının hızı sabit olarak 12 olarak belirlenmiş.
        int playerSpeed = 12;
        // Oyun sırasında oyuncunun topladığı skor.
        int score;
        // Trafikte rastgele bir araba görseli seçmek için kullanılan değişken.
        int carImage;

        // Rastgele değerler üretmek için kullanılan Random nesneleri.
        Random rand = new Random();
        Random carPosition = new Random();

        // Oyuncunun sola ve sağa hareket edip etmediğini takip eden değişkenler.
        bool goleft, goright;

        // Form1 sınıfının yapıcı metodu. Oyunu başlatmak için ResetGame çağrılıyor.
        public Form1()
        {
            InitializeComponent();
            ResetGame();
        }

        // Klavye tuşuna basıldığında çalıştırılan olay işleyici.
        private void keyisdown(object sender, KeyEventArgs e)
        {
            // Sol ok tuşuna basıldığında oyuncunun sola hareket etmesi sağlanır.
            if (e.KeyCode == Keys.Left)
            {
                goleft = true;
            }
            // Sağ ok tuşuna basıldığında oyuncunun sağa hareket etmesi sağlanır.
            if (e.KeyCode == Keys.Right)
            {
                goright = true;
            }
        }

        // Klavye tuşu bırakıldığında çalıştırılan olay işleyici.
        private void keyisup(object sender, KeyEventArgs e)
        {
            // Sol ok tuşu bırakıldığında oyuncunun sola hareket etmesi durdurulur.
            if (e.KeyCode == Keys.Left)
            {
                goleft = false;
            }
            // Sağ ok tuşu bırakıldığında oyuncunun sağa hareket etmesi durdurulur.
            if (e.KeyCode == Keys.Right)
            {
                goright = false;
            }
        }

        // Oyun zamanlayıcısı her tetiklendiğinde çalıştırılan olay işleyici.
        private void gameTimerEvent(object sender, EventArgs e)
        {
            // Skor metni güncellenir.
            txtScore.Text = "Score: " + score;
            // Skor her döngüde bir artırılır.
            score++;

            // Oyuncu sola hareket ediyorsa ve sınırın dışına çıkmıyorsa sola hareket ettirilir.
            if (goleft == true && player.Left > 10)
            {
                player.Left -= playerSpeed;
            }
            // Oyuncu sağa hareket ediyorsa ve sınırın dışına çıkmıyorsa sağa hareket ettirilir.
            if (goright == true && player.Left < 415)
            {
                player.Left += playerSpeed;
            }

            // Trafikteki AI araçlarının aşağıya doğru hareket etmesi sağlanır.
            AI1.Top += trafficSpeed;
            AI2.Top += trafficSpeed;

            // AI1 aracı ekranın altına ulaştığında yeni bir pozisyon ve görüntü alır.
            if (AI1.Top > 530)
            {
                changeAIcars(AI1);
            }
            // AI2 aracı ekranın altına ulaştığında yeni bir pozisyon ve görüntü alır.
            if (AI2.Top > 530)
            {
                changeAIcars(AI2);
            }

            // Oyuncu, herhangi bir AI aracı ile çarpışırsa oyun biter.
            if (player.Bounds.IntersectsWith(AI1.Bounds) || player.Bounds.IntersectsWith(AI2.Bounds))
            {
                gameOver();
            }

            // Skor belirli aralıkları geçtiğinde ödül görüntüsü değişir.
            if (score > 40 && score < 500)
            {
                award.Image = Properties.Resources.bronzemedalcard;
            }
            if (score > 500 && score < 2000)
            {
                award.Image = Properties.Resources.silvermedalcard;
                trafficSpeed = 22; // Trafik hızı artırılır.
            }
            if (score > 2000)
            {
                award.Image = Properties.Resources.goldmedalcard;
                trafficSpeed = 27; // Trafik hızı daha da artırılır.
            }
        }

        // AI araçlarının görüntüsünü ve pozisyonunu değiştiren yöntem.
        private void changeAIcars(PictureBox tempCar)
        {
            // Rastgele bir araç görüntüsü seçilir.
            carImage = rand.Next(1, 9);

            // Araç görüntüsü carImage değerine göre değiştirilir.
            switch (carImage)
            {
                case 1:
                    tempCar.Image = Properties.Resources.carpurple;
                    break;
                case 2:
                    tempCar.Image = Properties.Resources.cargreen;
                    break;
                case 3:
                    tempCar.Image = Properties.Resources.carred;
                    break;
                case 4:
                    tempCar.Image = Properties.Resources.carorange;
                    break;
                case 5:
                    tempCar.Image = Properties.Resources.carblue;
                    break;
                case 6:
                    tempCar.Image = Properties.Resources.cars2;
                    break;
            }

            // Araç pozisyonu ekranın üstünde rastgele bir yere yerleştirilir.
            tempCar.Top = carPosition.Next(100, 400) * -1;

            // Araçların yatay pozisyonları tag değerine göre belirlenir.
            if ((string)tempCar.Tag == "carLeft")
            {
                tempCar.Left = carPosition.Next(5, 200);
            }
            if ((string)tempCar.Tag == "carRight")
            {
                tempCar.Left = carPosition.Next(245, 422);
            }
        }

        // Oyun sona erdiğinde çalışan yöntem.
        private void gameOver()
        {
            // Oyun zamanlayıcısı durdurulur.
            gameTimer.Stop();

            // Ödül görünür hale getirilir.
            award.Visible = true;
            award.BringToFront(); // Ödül formda ön plana alınır.

            // Yeniden başlat butonu aktif edilir.
            button1.Enabled = true;
        }

        // Oyunu sıfırlayan yöntem.
        private void ResetGame()
        {
            // Yeniden başlat butonu devre dışı bırakılır.
            button1.Enabled = false;
            // Ödül gizlenir.
            award.Visible = false;
            // Hareket değişkenleri sıfırlanır.
            goleft = false;
            goright = false;
            // Skor sıfırlanır.
            score = 0;
            // Başlangıçta bronz madalya görüntüsü gösterilir.
            award.Image = Properties.Resources.bronzemedalcard;

            // Trafik hızı varsayılan değere döndürülür.
            trafficSpeed = 15;

            // AI araçlarının başlangıç pozisyonları rastgele belirlenir.
            AI1.Top = carPosition.Next(200, 500) * -1;
            AI1.Left = carPosition.Next(5, 200);

            AI2.Top = carPosition.Next(200, 500) * -1;
            AI2.Left = carPosition.Next(245, 422);

            // Oyun zamanlayıcısı başlatılır.
            gameTimer.Start();
        }

        // Yeniden başlat butonuna tıklandığında ResetGame çağrılır.
        private void restartGame(object sender, EventArgs e)
        {
            ResetGame();
        }
    }
}
