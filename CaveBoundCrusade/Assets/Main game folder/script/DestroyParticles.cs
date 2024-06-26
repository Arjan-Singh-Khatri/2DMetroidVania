using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticles : MonoBehaviour
{
    private float timer = .6f;

    void Update(){
        timer -= Time.deltaTime;
        if(timer <= 0f )
            Destroy(gameObject);
    }
}
