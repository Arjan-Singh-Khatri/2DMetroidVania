
using DG.Tweening;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UIElements;

public class Lighting : EnemyParentScript
{

    [SerializeField]private float lifeSpanTimer = 20f;
    [SerializeField]private float _speed = 1f;
    [SerializeField]private ParticleSystem particle; 
    // Start is called before the first frame update
    void Start()
    {
        transform.parent = null;
        player = GameObject.FindGameObjectWithTag("Player");
        particle.Play();
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
        transform.position = Vector3.MoveTowards(transform.position + new Vector3(offset,offset,0), 
            player.transform.position,(_speed) *  Time.deltaTime);
    }

    private void Move()
    {
        FollowPlayer();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) {
            //player.GetComponent<playerDeath>().TakeDamage(DamageHolder.instance.lightning);    
            Events.instance.onPlayerTakeDamage(DamageHolder.instance.lightning);
            Destroy(gameObject);
        }
    }


}
