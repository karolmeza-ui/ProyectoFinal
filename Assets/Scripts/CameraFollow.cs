using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    
  public Transform target; // Reference to the target (player) that the camera will follow


    private void LateUpdate()
    {
        if (target != null)
        {
            transform.position = new Vector3(target.position.x, target.position.y, transform.position.z); 
        }
    }   
}

