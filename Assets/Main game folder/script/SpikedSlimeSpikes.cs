using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikedSlimeSpikes : EnemyParentScript
{

    [SerializeField] private float _speed = 3f;
    private float _lifeTime = 1.7f;
    private Vector3 direction = new();
    // Start is called before the first frame update
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Start()
    {
        transform.parent = null;
        direction.x = (player.transform.position - transform.position).x;
        direction.y = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += _speed * Time.deltaTime * direction ;
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
