using ArenaDeBatalha.GameLogic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArenaDeBatalhaGameLogic
{
    // quando colocado : na frente da clase significa que ela e derivada de tal clase
    public class Background : GameObject
    {
        public Background(Size bounds, Graphics screen) : base(bounds, screen)
        {
            this.Left = 0;
            this.Top = 0;
            this.Speed = 0;
        }
        public override Bitmap GetSprite()
        {
            return Media.Background;
        }

    }
}
