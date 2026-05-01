using UnityEngine;

public class Platform_Base : MonoBehaviour
{
    public bool IsOnPlatform;
    private float SKey_HeldFor;
   
    void Update()
    {
        if(Input.GetKey(KeyCode.S) && IsOnPlatform == true)
        {
            SKey_HeldFor += 1 * Time.deltaTime;
            if(SKey_HeldFor > 0.2f)
            {
                transform.GetComponent<PlatformEffector2D>().rotationalOffset = 180;
            }
        }
        else
        {
            SKey_HeldFor = 0;
            transform.GetComponent<PlatformEffector2D>().rotationalOffset = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            IsOnPlatform = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            IsOnPlatform = false;
        }
    }
}
