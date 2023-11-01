using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    private float speed = 6f;
    private float lifeSpanTimer = 6f;
    // Start is called before the first frame update
    void Start()
    {
        
        transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {

        transform.position += speed * Time.deltaTime * transform.right;
        lifeSpanTimer -= Time.deltaTime;
        if(lifeSpanTimer <= 0) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
}
