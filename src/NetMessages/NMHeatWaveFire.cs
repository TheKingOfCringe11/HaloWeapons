namespace DuckGame.HaloWeapons
{
    public class NMHeatWaveFire : NMEvent
    {
        public HeatWave HeatWave;

        public NMHeatWaveFire(HeatWave heatWave)
        {
            HeatWave = heatWave;
        }

        public NMHeatWaveFire()
        {

        }

        public override void Activate()
        {
            HeatWave.Fire();
        }
    }
}
