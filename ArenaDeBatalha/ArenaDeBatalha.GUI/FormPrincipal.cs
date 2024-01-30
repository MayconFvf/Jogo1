using ArenaDeBatalha.GameLogic;
using ArenaDeBatalhaGameLogic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;

namespace ArenaDeBatalha.GUI
{
    public partial class FormPrincipal : Form
    {
        

        DispatcherTimer enemySpawTimer { get; set; }

        DispatcherTimer gameLoopTimer { get; set; }

        Graphics screenPainter { get; set; }
        Bitmap screenBuffer { get; set; }
        Background background { get; set; }
        Player player { get; set; }
        GameOver gameOver { get; set; }


        List<GameObject> gameObjects { get; set; }
        public Random random { get; set; }
            
        bool canShoot;
        public FormPrincipal()
        {
            InitializeComponent();

            this.random = new Random();
            this.ClientSize = Media.Background.Size;
            this.screenBuffer = new Bitmap(Media.Background.Width, Media.Background.Height);
            this.screenPainter = Graphics.FromImage(this.screenBuffer);
            this.gameObjects = new List<GameObject>();
            this.background = new Background(this.screenBuffer.Size, this.screenPainter);
            this.player = new Player(this.screenBuffer.Size, this.screenPainter);
            this.gameOver = new GameOver(this.screenBuffer.Size, this.screenPainter);

            this.gameLoopTimer = new DispatcherTimer(DispatcherPriority.Render);
            this.gameLoopTimer.Interval = TimeSpan.FromMilliseconds(16.66666);
            this.gameLoopTimer.Tick += GameLoop;

            this.enemySpawTimer = new DispatcherTimer(DispatcherPriority.Render);
            this.enemySpawTimer.Interval = TimeSpan.FromMilliseconds(1000);
            this.enemySpawTimer.Tick += SpawnEnemy;

            
            StartGame();
        }

        public void StartGame()
        {
            this.gameObjects.Clear();
            this.gameObjects.Add(background);
            this.gameObjects.Add(player);
            this.player.SetStartPosition();
            this.player.Active = true;
            this.enemySpawTimer.Start();
            this.gameLoopTimer.Start();
            this.canShoot = true;
        }

        public void EndGame()
        {
            this.gameObjects.RemoveAll(x => x is GameObject);
            this.gameLoopTimer.Stop();
            this.enemySpawTimer.Stop();
            this.background.UpdateObject();
            this.gameOver.UpdateObject();
            Invalidate();
        }

        public void SpawnEnemy(object sender, EventArgs e)
        {

            Point enemyPosition = new Point(this.random.Next(10, this.screenBuffer.Width - 74), - 62);
            Enemy enemy = new Enemy(this.screenBuffer.Size, this.screenPainter, enemyPosition);
            this.gameObjects.Add(enemy);
        }

        public void GameLoop(object sender, EventArgs e)
        {
            this.gameObjects.RemoveAll(x => !x.Active);
            this.ProcessControls();

            foreach(GameObject go in this.gameObjects)
            {
                go.UpdateObject();

                if (go.IsOutOfBounds())
                {
                    go.Destroy();
                }

                if(go is Enemy)
                {
                    if (go.IsCollidingWith(player))
                    {
                        player.Destroy();
                        player.PlaySond();
                        EndGame();
                        return;
                    }
                    foreach (GameObject bullet in this.gameObjects.Where(x => x is Bullet))
                    {
                        if (go.IsCollidingWith(bullet))
                        {
                            go.Destroy();
                            bullet.Destroy();
                        }

                    }
                }

            }
            this.Invalidate();
        }

        private void FormPrincipal_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.DrawImage(this.screenBuffer, 0, 0);
        }

        private void ProcessControls()
        {
            if (Keyboard.IsKeyDown(Key.Left)) player.MoveLeft();
            if (Keyboard.IsKeyDown(Key.Right)) player.MoveRight();
            if (Keyboard.IsKeyDown(Key.Up)) player.MoveUp();
            if (Keyboard.IsKeyDown(Key.Down)) player.MoveDown();
            if (Keyboard.IsKeyDown(Key.Space)&& canShoot)
            {
                
                this.gameObjects.Insert(1, player.Shoot());
                this.canShoot = false;
            }
            if (Keyboard.IsKeyUp(Key.Space)) canShoot = true;
        }

        private void FormPrincipal_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.R)
            {
                StartGame();
            }
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }
    }
}
