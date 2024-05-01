using UnityEngine;


public class Tween : MonoBehaviour
{
    public float upSpeed = 3.0f; 
    public float downSpeed = 1.0f; 
    public float stopHeight = 5.0f; 

    private bool movingUp = true;
    private float start;



    private void Start()
    {
        start = transform.position.y;
        
    }
    void Update()
    {
        Invoke("move", 2.0f);
    }

    private void move()
    {
        if (movingUp)
        {
            transform.Translate(Vector3.up * upSpeed * Time.deltaTime);
            if (Mathf.Abs(start) - Mathf.Abs(transform.position.y) >= stopHeight)
            {
                movingUp = false;
            }
        }
        else
        {
            transform.Translate(Vector3.down * downSpeed * Time.deltaTime);
            if (transform.position.y <= start)
            {
                movingUp = true;
            }

        }
    }

}










