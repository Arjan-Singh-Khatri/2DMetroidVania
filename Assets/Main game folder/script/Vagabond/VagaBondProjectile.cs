using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ProjectileType {
    Normal,Heavy
}


public class VagaBondProjectile : Vagabond
{
    private float _speedNormal = 15f;
    private float _speedHeavy = 12f;
    private const float LIFE_TIME = 1.9f;
    private float localLifeTime = 0.1f;
    private ProjectileType _currentType;
    private Vector3 directionVector = new();

    void Start(){

        if (gameObject.name.CompareTo("VagaBondShockEffect(Clone)") == 0) 
            _currentType = ProjectileType.Heavy;
        else
            _currentType=ProjectileType.Normal;  
    }

    // Update is called once per frame
    void Update(){
        Debug.Log(directionVector);
        Movement();
        ScaleOverLifeTime();
    }

    void Movement() {
        if (_currentType == ProjectileType.Normal)
            transform.position += _speedNormal * Time.deltaTime * directionVector;
        else
            transform.position += _speedHeavy * Time.deltaTime * directionVector;
    }

    void ScaleOverLifeTime()
    {
        if (localLifeTime >= LIFE_TIME)
            Destroy(gameObject);

        localLifeTime += Time.deltaTime;
        float offset = Mathf.Lerp(2f, .1f, localLifeTime / LIFE_TIME);

        float randomScale = 1 + (float)Random.Range(-3, 7) / 10;

        if (_currentType == ProjectileType.Normal) return;

        if (localLifeTime < LIFE_TIME / 2)
        {
            transform.localScale = new Vector2(offset, randomScale);
        }
        else if (localLifeTime > LIFE_TIME / 2)
        {
            transform.localScale = new Vector2(offset, randomScale);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.CompareTag("Player")) {
            if (_currentType == ProjectileType.Normal)
                player.GetComponent<playerDeath>().TakeDamage(_damageNormalProjectile);
            else
                player.GetComponent<playerDeath>().TakeDamage(_damageHeavyProjectile);
        }
            
    }
}
