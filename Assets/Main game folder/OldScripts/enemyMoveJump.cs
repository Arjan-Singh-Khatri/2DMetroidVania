using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMoveJump : EnemyParentScript , IDataPersistance
{
    private Rigidbody2D rig;
    private float startPoint;
    private Vector2[] endPonints = new Vector2[2];
    [SerializeField] float speed = 5;
    private int wayPointIndex = 0;
    [SerializeField] private float jumpSpeed = 4.5f;
    [SerializeField] private float jumpInterval = 1f;
    private float nextJumpTime = 0f;
    private bool _killed;


    // Start is called before the first frame update
    void Start()
    {
        startPoint = transform.position.x;
        endPonints[0] = new Vector3(startPoint-5, transform.position.y);
        endPonints[1] = new Vector3(startPoint + 5, transform.position.y);
        player = GameObject.FindGameObjectWithTag("Player");
        rig = GetComponent<Rigidbody2D>();
        health = 40f;
        wayPointIndex = Random.Range(0, 2);
    }

    #region Save And Load
    public void SaveData(ref GameData gameData)
    {
        if (gameData.enemyKilled.ContainsKey(this.enemyID))
        {
            gameData.enemyKilled.Remove(this.enemyID);
        }
        gameData.enemyKilled.Add(this.enemyID, _killed);
    }

    public void LoadData(GameData gameData)
    {
        gameData.enemyKilled.TryGetValue(this.enemyID, out _killed);
        if (_killed)
        {
            gameObject.SetActive(false);
        }
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            _killed = true;
            EnemyDeath();
            return;
        }
        EnemyMove();
    }

    

    void EnemyMove()
    {
        if (Vector2.Distance(transform.position, endPonints[wayPointIndex]) > .5)
        {
            transform.position = Vector2.MoveTowards(transform.position, endPonints[wayPointIndex], speed * Time.deltaTime);
            Hop();
        }
        else
        {
            wayPointIndex++;
            if (wayPointIndex >= endPonints.Length)
            {
                wayPointIndex = 0;
            }
        }
    }

    private void Hop()
    {
        if (Time.time >= nextJumpTime)
        {
            rig.AddForce(new Vector2(0f, jumpSpeed), ForceMode2D.Impulse);
            nextJumpTime = Time.time + jumpInterval;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttackHitBox"))
        {
            PushBack();
            HealthDepleteEnemy(DamageHolder.instance.playerDamage, ref health);

        }

    }
}
