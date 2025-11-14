using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    public Transform cameraTransform;
    
    void Start()
    { 
        offset = transform.position - player.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 desiredPosition = player.position + offset;
        transform.position = desiredPosition;
    }
}
