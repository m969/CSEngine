using CSEngine.Shared;
using UnityEngine;
using System;

namespace CSEngine.Client
{   
    public class RemotePlayer : BasePlayer
    {
        private readonly LiteRingBuffer<PlayerState> _buffer = new LiteRingBuffer<PlayerState>(30);
        private float _receivedTime;
        private float _timer;
        private const float BufferTime = 0.01f; //100 milliseconds
        public Action<byte> HealthChangeAction;
        public override byte Health
        {
            get
            {
                return _health;
            }
            set
            {
                Debug.Log($"RemotePlayer Health:{value}");
                _health = value;
                HealthChangeAction?.Invoke(value);
            }
        }

        public RemotePlayer(ClientPlayerManager manager, string name, PlayerJoinedPacket pjPacket) : base(manager, name, pjPacket.InitialPlayerState.Id)
        {
            _position = pjPacket.InitialPlayerState.Position;
            _health = pjPacket.Health;
            _rotation = pjPacket.InitialPlayerState.Rotation;
            _buffer.Add(pjPacket.InitialPlayerState);
        }

        public override void Spawn(Vector2 position)
        {
            _buffer.FastClear();
            base.Spawn(position);
        }

        public void UpdatePosition(float delta)
        {
            //if (_receivedTime < BufferTime || _buffer.Count < 2)
            //    return;
            //var dataA = _buffer[0];
            //var dataB = _buffer[1];
            
            //float lerpTime = NetworkGeneral.SeqDiff(dataB.Tick, dataA.Tick)*LogicTimer.FixedDelta;
            //float t = _timer / lerpTime;
            //_position = Vector2.Lerp(dataA.Position, dataB.Position, t);
            //_rotation = Mathf.Lerp(dataA.Rotation, dataB.Rotation, t);
            //_timer += delta;
            //if (_timer > lerpTime)
            //{
            //    _receivedTime -= lerpTime;
            //    _buffer.RemoveFromStart(1);
            //    _timer -= lerpTime;
            //}
        }
        private Vector2 _lastPosition;

        public void OnPlayerState(PlayerState state)
        {
            //old command
            int diff = NetworkGeneral.SeqDiff(state.Tick, _buffer.Last.Tick);
            if (diff <= 0)
                return;

            //Debug.Log($"OnPlayerState {state}");
            _receivedTime += diff * LogicTimer.FixedDelta;
            if (_buffer.IsFull)
            {
                Debug.LogWarning("[C] Remote: Something happened");
                //Lag?
                _receivedTime = 0f;
                _buffer.FastClear();
            }
            _buffer.Add(state);

            if (_position != state.Position)
            {
                Debug.Log($"OnPlayerState _lastPosition={_lastPosition} Position={state.Position}");
                _position = state.Position;
            }
            if (_rotation != state.Rotation)
            {
                _rotation = state.Rotation;
            }
        }
    }
}