using System.Collections.Generic;
using System.Linq;

namespace DuckGame.HaloWeapons
{
    [EditorGroup(EditorGroups.Guns)]
    public class HeatWave : HaloWeapon
    {
        private readonly Sprite _upperSight;
        private readonly Sprite _lowerSight;

        private readonly float _barrelWidth = 11f;
        private readonly int _bulletsPerFire = 5;

        private readonly Dictionary<sbyte, BulletMode> _bulletModes = new Dictionary<sbyte, BulletMode>
        {
            [0] = BulletMode.Spread,
            [1] = BulletMode.Distance
        };

        [Binding] private sbyte _bulletModeIndex;
        [Binding] private Vec2 _sightOffset;

        public HeatWave(float x, float y) : base(x, y)
        {
            ammo = 4;

            _ammoType = new ATHeatWave();
            _fireSound = Paths.GetSoundPath("heatWaveFire.wav");
            _upperSight = Resources.LoadSprite("heatWaveSight");
            _upperSight.center = new Vec2(2.5f);
            _lowerSight = _upperSight.Clone();
            _sightOffset = _bulletModes[_bulletModeIndex].SightOffset;
            _holdOffset = new Vec2(3f, 0f);
            _barrelOffsetTL = new Vec2(24f, 4f);
            _fireWait = 5f;
            _kickForce = 5f;
        }

        private BulletMode Mode => _bulletModes[_bulletModeIndex];

        public override void Update()
        {
            base.Update();

            _ammoType.bulletSpeed = Mode.Speed;
            _kickForce = Mode.KickForce;

            bool turnedRight = offDir > 0;
            float halfPI = Maths.PI / 2f;

            _upperSight.angle = turnedRight ? angle : angle - halfPI;

            float upperSightAngle = _upperSight.angle;

            _lowerSight.angle = turnedRight ? upperSightAngle + halfPI : upperSightAngle - halfPI;

            if (!isServerForObject)
                return;

            Vec2 destination = Mode.SightOffset;
            float distance = Vec2.Distance(_sightOffset, destination);

            _sightOffset = Lerp.Vec2(_sightOffset, destination, distance / 8f);
        }

        public override void OnPressAction()
        {
            if (!isServerForObject || (_wait > 0f && ammo > 0))
                return;

            Fire();
            DuckNetwork.SendToEveryone(new NMHeatWaveFire(this));

            _wait = _fireWait;
        }

        public override void Fire()
        {
            if (ammo > 0)
            {
                ApplyKick();

                float initialOffset = -_barrelWidth / 2f;

                float angleRange = Mode.AngleRange;
                float initialAngle = -angleRange / 2f;

                for (int i = 0; i < _bulletsPerFire; i++)
                {
                    Vec2 offset = barrelOffset + new Vec2(0f, initialOffset + _barrelWidth / _bulletsPerFire * i);
                    float barrelAngle = angle;
                    float bulletAngle = initialAngle + angleRange / _bulletsPerFire * i;

                    if (offDir < 0)
                    {
                        barrelAngle -= Maths.PI;
                        bulletAngle *= -1f;
                    }

                    _ammoType.rebound = i == 0 || i == _bulletsPerFire - 1 ? false : Mode.Rebound;

                    float shootAngle = Maths.RadToDeg(barrelAngle + bulletAngle);
                    _ammoType.FireBullet(Offset(offset), owner, shootAngle, this);
                }

                PlayFireSound();

                if (isServerForObject)
                {
                    ammo--;

                    if (owner is null)
                    {
                        Vec2 fly = barrelVector * Rando.Float(1f, 3f);
                        fly.y += Rando.Float(2f);
                        velocity += fly;
                    }
                }
            }
            else
            {
                DoAmmoClick();
            }
        }

        public override void Draw()
        {
            base.Draw();

            if (duck is null)
                return;

            Vec2 upperSightPosition = Offset(_sightOffset);
            Vec2 lowerSightPosition = Offset(new Vec2(_sightOffset.x, -_sightOffset.y));

            Graphics.Draw(_upperSight, upperSightPosition.x, upperSightPosition.y, 3f);
            Graphics.Draw(_lowerSight, lowerSightPosition.x, lowerSightPosition.y, 3f);
        }

        protected override void OnQuack()
        {
            base.OnQuack();

            if (isServerForObject)
            {
                _bulletModeIndex = _bulletModeIndex < 1 ? (sbyte)1 : (sbyte)0;
                SFX.PlaySynchronized(Mode.ActivateSoundPath);
            }
        }

        protected override Sprite CreateGraphics(string spritePath)
        {
            var sprite = new SpriteMap(spritePath, 30, 9);

            sprite.AddAnimation("idle", 0.02f, true, Enumerable.Range(0, 2).ToArray());
            sprite.SetAnimation("idle");

            return sprite;
        }

        private sealed class BulletMode
        {
            public static readonly BulletMode Spread = new BulletMode(new Vec2(35f, -16f), Paths.GetSoundPath("heatWaveSpreadMode.wav"), 10f, 4f, Maths.PI / 12f, true);

            public static readonly BulletMode Distance = new BulletMode(new Vec2(25f, -8f), Paths.GetSoundPath("heatWaveDistanceMode.wav"), 12f, 6f, 0f, false);

            private BulletMode(Vec2 sightOffset, string activateSound, float speed, float kickForce, float angleRange, bool rebound)
            {
                SightOffset = sightOffset;
                ActivateSoundPath = activateSound;
                Speed = speed;
                KickForce = kickForce;
                AngleRange = angleRange;
                Rebound = rebound;
            }

            public Vec2 SightOffset { get; private init; }
            public string ActivateSoundPath { get; private init; }
            public float Speed { get; private init; }
            public float KickForce { get; private init; }
            public float AngleRange { get; private init; }
            public bool Rebound { get; private init; }
        }
    }
}
