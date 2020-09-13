using ET;
using UnityEngine;

namespace CSEngine.Client
{
    public class RemotePlayerView : MonoBehaviour, IPlayerView
    {
        private RemotePlayer _player;
        private Vector2 _lastPosition;

        public static RemotePlayerView Create(RemotePlayerView prefab, RemotePlayer player)
        {
            Quaternion rot = Quaternion.Euler(0f, player.Rotation, 0f);
            var obj = Instantiate(prefab, player.Position, rot);
            obj._player = player;
            return obj;
        }

        private void Update()
        {
            _player.UpdatePosition(Time.deltaTime);
            if (_lastPosition != _player.Position)
            {
                Log.Debug($"RemotePlayerView _lastPosition={_lastPosition} Position={_player.Position}");
                _lastPosition = transform.position = _player.Position;
            }
            transform.rotation =  Quaternion.Euler(0f, 0f, _player.Rotation * Mathf.Rad2Deg );
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}