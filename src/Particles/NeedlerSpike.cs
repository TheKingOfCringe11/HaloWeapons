namespace DuckGame.HaloWeapons
{
    public class NeedlerSpike : EjectedShell
    {
        public NeedlerSpike(float x, float y) : base(x, y, Paths.GetSpritePath("needlerSpike.png"), "metalBounce")
        {
            collisionSize = new Vec2(5f, 5f);
            center = collisionSize / 2f;
            collisionOffset = -center;
        }
    }
}
