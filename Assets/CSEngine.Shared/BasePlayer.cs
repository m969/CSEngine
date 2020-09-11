using UnityEngine;

namespace CSEngine.Shared
{
    public abstract class BasePlayer
    {
        public readonly string Name;

        private float _speed = 3f;
        private GameTimer _shootTimer = new GameTimer(0.2f);
        private BasePlayerManager _playerManager;

        protected Vector2 _position;
        protected float _rotation;
        protected byte _health;
        protected byte _maxHealth;

        public const float Radius = 0.5f;
        public bool IsAlive => _health > 0;
        public virtual byte Health { get => _health; set => _health = value; }
        public byte MaxHealth => _maxHealth;
        public Vector2 Position => _position;
        public float Rotation => _rotation;
        public readonly byte Id;
        public int Ping;

        protected BasePlayer(BasePlayerManager playerManager, string name, byte id)
        {
            Id = id;
            Name = name;
            _playerManager = playerManager;
        }

        public virtual void Spawn(Vector2 position)
        {
            _position = position;
            _rotation = 0;
            _health = 100;
        }

        private void Shoot()
        {
            const float MaxLength = 20f;
            Vector2 dir = new Vector2(Mathf.Cos(_rotation), Mathf.Sin(_rotation));
            var player = _playerManager.CastToPlayer(_position, dir, MaxLength, this);
            Vector2 target = _position + dir * (player != null ? Vector2.Distance(_position, player._position) : MaxLength);
            _playerManager.OnShoot(this, target, player);
            if (player != null)
            {
                player.Health -= 1;
            }
        }

        public virtual void ApplyInput(PlayerInputPacket command, float delta)
        {
            Vector2 velocity = Vector2.zero;
            
            if ((command.Keys & MovementKeys.Up) != 0)
                velocity.y = -1f;
            if ((command.Keys & MovementKeys.Down) != 0)
                velocity.y = 1f;
            
            if ((command.Keys & MovementKeys.Left) != 0)
                velocity.x = -1f;
            if ((command.Keys & MovementKeys.Right) != 0)
                velocity.x = 1f;     
            
            _position += velocity.normalized * _speed * delta;
            _rotation = command.Rotation;

            if ((command.Keys & MovementKeys.Fire) != 0)
            {
                if (_shootTimer.IsTimeElapsed)
                {
                    _shootTimer.Reset();
                    Shoot();
                }
            }
        }

        public virtual void Update(float delta)
        {
            _shootTimer.UpdateAsCooldown(delta);
        }
    }
}

