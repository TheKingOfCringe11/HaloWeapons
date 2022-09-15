namespace DuckGame.HaloWeapons.src.Tilesets
{
    public class Crossbeams : HaloAutoPlatform
    {
        public Crossbeams(float x, float y) : base(x, y, Paths.GetSpritePath("crossbeams.png"))
        {
            physicsMaterial = PhysicsMaterial.Metal;
            _editorName = "Crossbeams";
        }
    }
}
