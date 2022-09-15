using HarmonyLib;

namespace DuckGame.HaloWeapons
{
    [EditorGroup(EditorGroups.Guns)]
    public class ShockRifle : HaloWeapon
    {
        [Binding] private float _cooldown = 0f;

        public ShockRifle(float x, float y) : base(x, y)
        {
            ammo = 9;

            _fullAuto = false;
            _kickForce = 6f;
            _barrelOffsetTL = new Vec2(30f, 4f);
            _holdOffset = new Vec2(4f, 0f);
            _fireWait = 50f;
            _fireSound = Paths.GetSoundPath("shockRifleFire.wav");
        }

        public override void Update()
        {
            base.Update();

            if (isServerForObject && _cooldown > 0f)
            {
                _cooldown--;

                if (_cooldown == 0f)
                    SFX.PlaySynchronized(Paths.GetSoundPath("shockRifleReload.wav"));
            }
        }

        public override void Fire()
        {
            if (ammo <= 0)
            {
                SFX.Play(_clickSound);

                AccessTools.Field(typeof(Gun), "_doPuff").SetValue(this, true);
                SpriteMap clickPuff = AccessTools.Field(typeof(Gun), "_clickPuff").GetValue<SpriteMap>(this);

                clickPuff.frame = 0;
                clickPuff.SetAnimation("puff");

                return;
            }

            if (_cooldown > 0f)
                return;

            if (isServerForObject)
            {
                Vec2 barrelPosition = Offset(barrelOffset + new Vec2(2f, 0f));

                Level.Add(new EnergyBeam(barrelPosition.x, barrelPosition.y)
                {
                    angle = barrelAngle,
                    responsibleProfile = responsibleProfile
                });

                DuckNetwork.SendToEveryone(new NMEnergyBeam(barrelPosition, barrelAngle));

                _cooldown = _fireWait;

                ammo--;
            }

            ApplyKick();

            PlayFireSound();
        }
    }
}
