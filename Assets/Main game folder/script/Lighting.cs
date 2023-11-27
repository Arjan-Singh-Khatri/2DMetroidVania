
using DG.Tweening;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UIElements;

public class Lighting : EnemyParentScript
{

    private float lifeSpanTimer = 3.5f;
    private float _speed = 5f;
    private bool followPlayer = false;
    private float offset;
    private Vector3 _position;  
    // Start is called before the first frame update
    void Start()
    {
        transform.parent = null;
        player = GameObject.FindGameObjectWithTag("Player");
        offset = Random.Range(2, 4) + Random.Range(1,10)/10;
        _position = new Vector3(transform.position.x + offset, transform.position.y + offset);

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
        transform.position = Vector3.MoveTowards(_position, player.transform.position,_speed);
    }

    private void Move()
    {
        if (followPlayer)
            FollowPlayer();
        else 
            transform.position += _speed * Time.deltaTime *Vector3.right;
    }

    private void FollowPlayerTrigger()
    {
        followPlayer = true;
    }

    private void OnDestroy()
    {
        Events.instance.followPlayer -= FollowPlayerTrigger;
    }
}
