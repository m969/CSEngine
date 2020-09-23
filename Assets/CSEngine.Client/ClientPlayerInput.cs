using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace CSEngine.Client
{
    public class ClientPlayerInput : MonoBehaviour
    {
        private Camera _mainCamera;


        private void Start()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            var _player = G.ClientPlayerManager.OurPlayer;
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
    }
}