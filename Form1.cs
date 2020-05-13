using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gamecaro
{
    public partial class Form1 : Form
    {

        private bool co=true;
        private List<List<Button>> matrix;
        private List<Button> ds;
        private int playercurrent;

        public int Playercurrent
        {
            get { return playercurrent; }
            set { playercurrent = value; }
        }

        public List<Button> Ds
        {
            get { return ds; }
            set { ds = value; }
        }
        public List<List<Button>> Matrix
        {
            get { return matrix; }
            set { matrix = value; }
        }
        public Form1()
        {
            InitializeComponent();
            
        }
        public void khoitao()
        {
            Playercurrent = 0;
            Button btn=new Button();
            Button btn2=new Button();
            btn.Image=Image.FromFile(Application.StartupPath + "\\Images\\icons8-circled-0-42.png");
            btn2.Image = Image.FromFile(Application.StartupPath + "\\\\Images\\icons8-x-42.png");
            this.Ds = new List<Button>();
            Ds.Add(btn);
            Ds.Add(btn2);
           
           
            
          
        }
        public void thoigiangame()
        {
            prb1.Step = Banco.buocnhay;
            prb1.Maximum = Banco.tongthoigian;
            prb1.Value = 0;

            tmThoigian.Interval = Banco.inveral;
      
            
                tmThoigian.Start();
        }
        public void dungthoigianchoigame()
        {
            prb1.Value = 0;
            tmThoigian.Stop();
        }
     
        public void taobanco()
        {
            panel1.Enabled = true;
              thoigiangame();
            khoitao();
            Matrix = new List<List<Button>>();
            Button oldButton = new Button() { Width = 0, Location = new Point(0, 0) };
            for (int i = 0; i < Banco.banco_height; i++)
            {

                Matrix.Add(new List<Button>());
                for (int j = 0; j < Banco.banco_width; j++)
                {
                Button btn = new Button();
                btn.Size = new Size(Banco.co_witdh, Banco.co_height);
              //btn.Location = new Point(i*btn.Width,j*btn.Height+1);
               btn.Location = new Point(oldButton.Location.X + oldButton.Width, oldButton.Location.Y);
                btn.BackgroundImageLayout = ImageLayout.Stretch;
                btn.Tag = i.ToString();
            
                btn.Click += btn_click;
                panel1.Controls.Add(btn);
                Matrix[i].Add(btn);
               oldButton = btn;
              
               
                }
                oldButton.Location = new Point(0, oldButton.Location.Y + Banco.co_height);
                oldButton.Width = 0;
                oldButton.Height = 0;
            
            }
            
        }

        private void btn_click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn.BackgroundImage != null)
            {
                thoigiangame();
                return;
            }

            else
            {
                thoigiangame();
                prb1.Value = 0;
            }

                btn.BackgroundImage = Ds[Playercurrent].Image;
            if (Playercurrent == 0)
            {
                Playercurrent = 1;
              
            }
            else
                Playercurrent = 0;
         
            if (IsEndgame(btn))
            {
                EndGame();
                dungthoigianchoigame();
                
                panel1.Enabled = false;
            }
           
        }

        private void kiemtranguoithang()
        {
            if (Playercurrent == 0)
            {
                MessageBox.Show("Người chơi o thắng");

            }
            else
                MessageBox.Show("Người chơi x thắng");
        }

        private void EndGame()
        {
            kiemtranguoithang();


        }

        private  bool EndGameThoiGian()
        {
            if (prb1.Value >= prb1.Maximum)
            {
                tmThoigian.Stop();
                
                return true;
            }
            return false;
        }
        private bool IsEndgame(Button btn)
        {

            return IsEndgameNgang(btn) || isEndVertical(btn) || isEndPrimary(btn) || isEndSub(btn);
        
        }
        private Point laypoint(Button btn)
        {
            
            int doc = Convert.ToInt32(btn.Tag);
            int ngang = Matrix[doc].IndexOf(btn);
              
            Point point = new Point(ngang,doc);

            return point;
        }
        private bool IsEndgameNgang(Button btn)
        {
            Point point = laypoint(btn);

            int countLeft = 0;
            for (int i = point.X; i >= 0; i--)
            {
                if (Matrix[point.Y][i].BackgroundImage == btn.BackgroundImage)
                {
                    countLeft++;
                }
                else
                    break;
            }

            int countRight = 0;
            for (int i = point.X + 1; i < Banco.banco_width; i++)
            {
                if (Matrix[point.Y][i].BackgroundImage == btn.BackgroundImage)
                {
                    countRight++;
                }
                else
                    break;
            }

            return countLeft + countRight == 5;
            
        }
        private bool isEndVertical(Button btn)
        {
            Point point = laypoint(btn);

            int countTop = 0;
            for (int i = point.Y; i >= 0; i--)
            {
                if (Matrix[i][point.X].BackgroundImage == btn.BackgroundImage)
                {
                    countTop++;
                }
                else
                    break;
            }

            int countBottom = 0;
            for (int i = point.Y + 1; i < Banco.banco_height; i++)
            {
                if (Matrix[i][point.X].BackgroundImage == btn.BackgroundImage)
                {
                    countBottom++;
                }
                else
                    break;
            }

            return countTop + countBottom == 5;
        }
        private bool isEndPrimary(Button btn)
        {
            Point point = laypoint(btn);

            int countTop = 0;
            for (int i = 0; i <= point.X; i++)
            {
                if (point.X - i < 0 || point.Y - i < 0)
                    break;

                if (Matrix[point.Y - i][point.X - i].BackgroundImage == btn.BackgroundImage)
                {
                    countTop++;
                }
                else
                    break;
            }

            int countBottom = 0;
            for (int i = 1; i <= Banco.banco_width - point.X; i++)
            {
                if (point.Y + i >= Banco.banco_height || point.X + i >= Banco.banco_width)
                    break;

                if (Matrix[point.Y + i][point.X + i].BackgroundImage == btn.BackgroundImage)
                {
                    countBottom++;
                }
                else
                    break;
            }

            return countTop + countBottom == 5;
        }
        private bool isEndSub(Button btn)
        {
            Point point = laypoint(btn);

            int countTop = 0;
            for (int i = 0; i <= point.X; i++)
            {
                if (point.X + i > Banco.banco_width || point.Y - i < 0)
                    break;

                if (Matrix[point.Y-i][point.X + i].BackgroundImage == btn.BackgroundImage)
                {
                    countTop++;
                }
                else
                    break;
            }

            int countBottom = 0;
            for (int i = 1; i <= Banco.banco_width - point.X; i++)
            {
                if (point.Y + i >= Banco.banco_height || point.X - i < 0)
                    break;

                if (Matrix[point.Y + i][point.X - i].BackgroundImage == btn.BackgroundImage)
                {
                    countBottom++;
                }
                else
                    break;
            }

            return countTop + countBottom == 5;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            taobanco();
          
        }

        private void button1_Click(object sender, EventArgs e)
        {
         
        }

        private void tmThoigian_Tick(object sender, EventArgs e)
        {
            prb1.PerformStep();
            if (EndGameThoiGian())
            {
                EndGame();
                panel1.Enabled = false;
           
            }
                 
         
        }

     

        private void newGameToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            dungthoigianchoigame();
            taobanco();

        }
        public void test()
        {
        }
      

       

      

                   
    }
}
