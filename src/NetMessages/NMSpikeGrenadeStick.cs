namespace DuckGame.HaloWeapons
{
    public class NMSpikeGrenadeStick : NMEvent
    {
        public SpikeGrenade Grenade;
        public MaterialThing StickThing;

        public NMSpikeGrenadeStick(SpikeGrenade grenade, MaterialThing stickThing)
        {
            Grenade = grenade;
            StickThing = stickThing;
        }

        public NMSpikeGrenadeStick()
        {

        }

        public override void Activate()
        {
            Thing.SuperFondle(Grenade, DuckNetwork.localConnection);
            Grenade.Stick(StickThing);
        }
    }
}
