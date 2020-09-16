using ET;
using UnityEngine;
using UnityEngine.UI;

namespace CSEngine.Client
{
    public class RemotePlayerView : MonoBehaviour, IPlayerView
    {
        [SerializeField] private TextMesh _name;
        [SerializeField] private Image _healthCircle;
        private RemotePlayer _player;

        public static RemotePlayerView Create(RemotePlayerView prefab, RemotePlayer player)
        {
            Quaternion rot = Quaternion.Euler(0f, player.Rotation, 0f);
            var obj = Instantiate(prefab, player.Position, rot);
            obj._player = player;
            player.HealthChangeAction += obj.OnHealthChanged;
            return obj;
        }

        private Vector2 _lastPosition;
        private void Update()
        {
            _player.UpdatePosition(Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, _player.Position, 0.05f);
            transform.rotation =  Quaternion.Euler(0f, 0f, _player.Rotation * Mathf.Rad2Deg );
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        private void OnHealthChanged(byte v)
        {
            _healthCircle.fillAmount = v / 100f;
        }
    }
}