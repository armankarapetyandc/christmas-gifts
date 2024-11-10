using UnityEngine;

namespace ChristmasGifts.Scripts.Game.Camera
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private UnityEngine.Camera camera;
        [SerializeField] private float panSpeed;

        private void LateUpdate()
        {
            Vector3 inputVector = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
            camera.transform.Translate(inputVector * panSpeed * Time.deltaTime, Space.World);
        }
    }
}