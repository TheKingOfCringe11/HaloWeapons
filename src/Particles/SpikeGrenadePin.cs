namespace DuckGame.HaloWeapons
{
    public class SpikeGrenadePin : EjectedShell
    {
        public SpikeGrenadePin(float x, float y) : base(x, y, Paths.GetSpritePath("spikeGrenadePin.png"), "metalBounce")
        {
            collisionSize = new Vec2(3f, 4f);
            center = collisionSize / 2f;
            collisionOffset = -center;
        }
    }
}
