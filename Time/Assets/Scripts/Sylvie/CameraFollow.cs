using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform target;

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 pos = target.position;
            pos.z = transform.position.z; // Garder la profondeur de la caméra
            transform.position = pos;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
