
using DG.Tweening;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UIElements;

public class Lighting : EnemyParentScript
{

    private float lifeSpanTimer = 4.5f;
    private float _speed = 10f;
    public bool followPlayer = false;
    // Start is called before the first frame update
    void Start()
    {
        transform.parent = null;
        player = GameObject.FindGameObjectWithTag("Player");

        Events.instance.followPlayer += FollowPlayerTrigger;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        LifeSpan();
    }

    private void LifeSpan()
    {
        lifeSpanTimer -= Time.deltaTime;
        if (lifeSpanTimer < 0)
            Destroy(gameObject);
    }

    private void FollowPlayer()
    {
        int offset = Random.Range(1, 4) + Random.Range(0, 10) / 10;
        transform.position = Vector3.MoveTowards(transform.position, 
            player.transform.position,(_speed + offset) *  Time.deltaTime);
    }

    private void Move()
    {
        FollowPlayer();
    }

    public void FollowPlayerTrigger()
    {
        followPlayer = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player")) Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Events.instance.followPlayer -= FollowPlayerTrigger;
    }
}
