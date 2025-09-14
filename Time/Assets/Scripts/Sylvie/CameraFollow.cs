using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Enemies enemies;
    public Animator animationCamera;

    bool transition;
    float speed;

    public void Update()
    {
        if (transition)
        {
            speed += 0.01f;
        }
     
    }

    public void SetTarget()
    {
        switch (enemies.currentEnemy) 
        {
            case 0:
                animationCamera.SetTrigger("CameraOne");
                break;
            case 1:
                animationCamera.SetTrigger("CameraTwo");
                break;
            case 2:
                animationCamera.SetTrigger("CameraThree");
                break;
        }
    }

}
