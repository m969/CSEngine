using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace CSEngine.Client
{
    public class ClientPlayerView : MonoBehaviour, IPlayerView
    {
        [SerializeField] private TextMesh _name;
        [SerializeField] private Image _healthCircle;
        private ClientPlayer _player;
        private Camera _mainCamera;

        public static ClientPlayerView Create(ClientPlayerView prefab, ClientPlayer player)
        {
            Quaternion rot = Quaternion.Euler(0f, player.Rotation, 0f);
            var obj = Instantiate(prefab, player.Position, rot);
            obj._player = player;
            obj._name.text = player.Name;
            obj._mainCamera = Camera.main;
            player.HealthChangeAction += obj.OnHealthChanged;
            return obj;
        }

        private void Update()
        {
            var vert = Input.GetAxis("Vertical");
            var horz = Input.GetAxis("Horizontal");
            var fire = Input.GetAxis("Fire1");
            
            Vector2 velocty = new Vector2(horz, vert);

            Vector2 mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dir = mousePos - _player.Position;
            float rotation = Mathf.Atan2(dir.y, dir.x);
            _player.SetInput(velocty, rotation, fire > 0f);

            float lerpT = ClientLogic.LogicTimer.LerpAlpha;
            transform.position = Vector2.Lerp(_player.LastPosition, _player.Position, lerpT);
            float angle = Mathf.Lerp(_player.LastRotation, _player.Rotation, lerpT);
            transform.rotation = Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg );
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