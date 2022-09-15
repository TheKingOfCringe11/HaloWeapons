﻿namespace DuckGame.HaloWeapons
{
    public class ATPulseCarbine : AmmoType
    {
        public ATPulseCarbine()
        {
            bulletSpeed = 5.5f;
            penetration = 1f;
            accuracy = 0.85f;
            range = 500f;
            bulletType = typeof(PulseBullet);
            sprite = new Sprite(Paths.GetSpritePath("pulseBullet.png"));
            sprite.CenterOrigin();
            bulletThickness = 8f;
            bulletLength = 8f;
            speedVariation = 0.1f;
        }
    }
}
