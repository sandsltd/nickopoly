using UnityEngine;

public class CameraSetup : MonoBehaviour
{
    void Start()
    {
        Camera camera = GetComponent<Camera>();
        if (camera != null)
        {
            camera.backgroundColor = Color.white;
        }
    }
}