using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    #region  내부 
    [SerializeField] private float mouseSensitivity = 200f;
    private float xRotation = 0f;
    public Transform cameraHolder;
    #endregion

    void Update()
    {
        if (GameManager.Instance.currentGameDirection == GameManager.GameDirection.Playing)
        {
            HandleRotate();
        }
    }

    void HandleRotate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // 상하 회전 값 계산 및 제한
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -70f, 70f);

        cameraHolder.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // 상하 회전(카메라)
        transform.Rotate(Vector3.up * mouseX); // 좌우 회전(플레이어 몸)
    }
}