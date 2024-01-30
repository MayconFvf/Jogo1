using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace ArenaDeBatalhaGameLogic
{
    public abstract class GameObject
    {
        //#region eu crio como uma partiçaõ para alocar uma parte do meu codigo
        #region Game Object Properties
        // imagem do jogo
        public Bitmap Sprite { get; set; }
        public bool Active { get; set; }
        public int Speed { get; set; }
        // largura e altura do quadrado relacionado ao objeto
        public int Left { get; set; }
        public int Top { get; set; }
        // vai pegar a largura e a altura do sprite
        public int Width { get { return this.Sprite.Width; } }
        public int Height { get { return this.Sprite.Height; } }
        public Size Bounds { get; set; }
        // vai gerar o retangulo para ver ce tem interação com outros objetos
        public Rectangle Rectangle { get; set; }
        public Stream Sound { get; set; }
        public Graphics Screen { get; set; }
        // som do player durante o jogo
        private SoundPlayer soundPlayer { get; set; }
        // aqui eu finalizo onde o #region vai fechar
        #endregion

        #region Game Object Methods

        public abstract Bitmap GetSprite();

        public GameObject(Size bounds, Graphics screen)
        {
            this.Bounds = bounds;
            this.Screen = screen;
            this.Active = true;
            this.soundPlayer = new SoundPlayer();
            this.Sprite = this.GetSprite();
            this.Rectangle = new Rectangle(this.Left, this.Top, this.Width, this.Height);
        }
        public virtual void UpdateObject()
        {
            this.Rectangle = new Rectangle(this.Left, this.Top, this.Width, this.Height);
            this.Screen.DrawImage(this.Sprite, this.Rectangle);
        }

        public virtual void MoveLeft()
        {
            if (this.Left > 0)
            {
                this.Left -= this.Speed;
            }
        }
        public virtual void MoveRight()
        {
            if (this.Left < this.Bounds.Width - this.Width)
            {
                this.Left += this.Speed;
            }
        }
        public virtual void MoveDown()
        {
            this.Top += this.Speed;
        }
        public virtual void MoveUp()
        {
            this.Top -= this.Speed;
        }

        public bool IsOutOfBounds()
        {
            return
                (this.Top > this.Bounds.Height + this.Height) ||
                (this.Top < -this.Height) ||
                (this.Left > this.Bounds.Width + this.Width) ||
                (this.Left < -this.Width);
        }
        public void PlaySond()
        {
            soundPlayer.Stream = this.Sound;
            soundPlayer.Play();
        }
        public bool IsCollidingWith(GameObject gameObject)
        {
            if (this.Rectangle.IntersectsWith(gameObject.Rectangle))
            {
                this.PlaySond();
                return true;
            }
            else return false;
        }

        public void Destroy()
        {
            this.Active = false;
        }


        #endregion
    }
}
