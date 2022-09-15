namespace DuckGame.HaloWeapons
{
    public static class Utilities
    {
        public static void SetCollisionBox(Thing thing, Vec2 collisionSize)
        {
            thing.collisionSize = collisionSize;
            thing.center = collisionSize / 2f;
            thing.collisionOffset = -thing.center;
        }
    }
}
