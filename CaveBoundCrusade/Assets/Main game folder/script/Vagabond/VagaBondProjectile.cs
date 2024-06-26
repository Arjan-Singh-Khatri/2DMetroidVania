using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ProjectileType {
    Normal,Heavy, player
}


public class VagaBondProjectile : EnemyParentScript
{
    private float _speedNormal = 22f;
    private float _speedHeavy = 15;
    private const float LIFE_TIME = 2.9f;
    private float localLifeTime = 0.1f;
    private ProjectileType _currentType;
    private float _damage;
    private float directionForProjectile;
    private Vector3 directionVector;

    [SerializeField] AudioClip _audioClipNormal;
    [SerializeField] AudioClip _audioClipHeavy;
    void Start(){

        _audioSource = GetComponent<AudioSource>();
        _audioSource.outputAudioMixerGroup = mixerGroup;

        player = GameObject.FindGameObjectWithTag("Player");

        transform.parent = null;

        if (gameObject.name.CompareTo("VagaBondShockEffect(Clone)") == 0) { 
            _currentType = ProjectileType.Heavy;
            _audioSource.clip = _audioClipHeavy;
            _damage = 50f;
        }
        else{ 
            transform.position = new Vector2(transform.position.x, transform.position.y - 2f);
            _currentType=ProjectileType.Normal;
            _audioSource.clip = _audioClipNormal;
            _damage = 35;
        }

        _audioSource.loop = true;
        _audioSource.Play();

        if (player.transform.position.x < transform.position.x)
        {
            directionForProjectile = -1;
        }
        else
        {
            directionForProjectile = 1;
        }

        directionVector = new Vector3(directionForProjectile, 0f);
    }

    // Update is called once per frame
    void Update(){
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

    private void OnTriggerEnter2D(Collider2D collision){

        if(collision.gameObject.layer == 9)
            Destroy(gameObject);

        if(collision.gameObject.CompareTag("Player"))
            player.GetComponent<playerDeath>().TakeDamage(_damage);
            
    }
}