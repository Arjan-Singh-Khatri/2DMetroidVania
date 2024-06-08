using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikedSlimeSpikes : EnemyParentScript
{

    [SerializeField] private float _speed = 20f;
    private float _lifeTime = 2f;
    private Vector3 directionV = Vector2.zero;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        transform.parent = null;
        transform.rotation = new Quaternion(0, 0, -0.707926929f, 0.706285775f);
        directionV.x = (player.transform.position.x - transform.position.x);
        directionV = directionV.normalized;

        Quaternion targetRotation = Quaternion.LookRotation(directionV, Vector3.forward);
        Quaternion additionalRotation = Quaternion.Euler(90, 0, 0);
        Quaternion finalRotation = targetRotation * additionalRotation;

        transform.rotation = finalRotation; 

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += _speed * Time.deltaTime * directionV ;
        LifeSpanOver();
    }

    private void LifeSpanOver()
    {
        _lifeTime -= Time.deltaTime;
        if (_lifeTime <= 0)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
}
