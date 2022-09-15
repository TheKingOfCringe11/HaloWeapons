using System;
using System.Linq;

namespace DuckGame.HaloWeapons
{
    public class UITile
    {
        private readonly BitmapFont _font = new BitmapFont("biosFont", 8)
        {
            scale = new Vec2(0.4f)
        };

        private EventHandler _click;
        private int _blinkTimer;

        public event EventHandler Click
        {
            add
            {
                _click += value;
            }

            remove
            {
                _click -= value;
            }
        }

        public string Text { get; set; }
        public Vec2 Position { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public Color Color { get; set; } = Color.LightGray;
        public Color BorderColor { get; set; } = UI.MenuItemColor;
        public Sprite Sprite { get; set; }
        public bool Enabled { get; set; } = true;

        protected BitmapFont Font => _font;

        public virtual void Draw()
        {
            var rectangle = new Rectangle(Position, Position + new Vec2(Width, Height));

            Graphics.DrawRect(rectangle, Color, 2.9f);
            Graphics.DrawRect(rectangle, BorderColor, 2.9f, false);

            float centerX = Position.x + Width / 2;

            if (Sprite is not null)
                Graphics.Draw(Sprite, centerX - Sprite.width / 2f, Position.y + Height / 2f - Sprite.height / 2f, 3f);

            if (!string.IsNullOrEmpty(Text))
                _font.Draw(Text, new Vec2(centerX - _font.GetWidth(Text) / 2f, Position.y + Height - _font.height - 3f), Color.Black, 3f);

            if (!Enabled)
            {
                Graphics.DrawRect(rectangle, Color.Gray * 0.5f, 3f);
            }
            else if (_blinkTimer > 0)
            {
                Graphics.DrawRect(rectangle, Color.White * 0.7f, 3f);

                _blinkTimer--;
            }
        }

        public void OnClick()
        {
            if (!Enabled)
                return;

            _blinkTimer = 4;

            _click?.Invoke(this, EventArgs.Empty);
        }
    }
}
