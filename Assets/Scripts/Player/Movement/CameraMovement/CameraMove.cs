using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private Transform _cameraPosition;
    void Update()
    {
        transform.position = _cameraPosition.position;    
    }
}
