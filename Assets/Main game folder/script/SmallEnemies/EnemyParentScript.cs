using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyParentScript : MonoBehaviour
{
    protected GameObject player;
    protected float health;
    protected bool enemyDead;
    protected float direction;
    [SerializeField] protected string enemyID;

    [ContextMenu("GUID ID GENERATE")]
    private void GenerateGUID()
    {
        enemyID = System.Guid.NewGuid().ToString();
    }
    
    

    protected void HealthDepleteEnemy(float attackPower, ref float healthofEnemy)
    {
        DamageHolder.instance.comboNumber += 1;
        DamageHolder.instance.damageMultiplier += .25f * DamageHolder.instance.comboNumber;
       
        healthofEnemy -= attackPower;
        StartCoroutine(HitAnimation());
        
    }

    protected void PushBack()
    {
        if (player.transform.position.x <= transform.position.x)
            transform.position = transform.position + new Vector3(1.75f, 0, 0);
        else 
            transform.position = transform.position + new Vector3(-1.75f, 0, 0);
    }

    protected void flip()
    {
        if (player.transform.position.x < transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            direction = -1;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            direction = 1;  
        }

    }

    protected IEnumerator HitAnimation() {
        var _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        Color alpha1 = new Color(255, 2555, 255, 255);
        Color alpha2 = new Color(255, 255, 255, 100);
        Color aplha3 = new Color(255, 255, 255, 170);

        _spriteRenderer.color = alpha2;
        yield return new WaitForSeconds(.5f);
        _spriteRenderer.color = alpha1;
        yield return new WaitForSeconds(.5f);
        _spriteRenderer.color = aplha3;
        yield return new WaitForSeconds(.5f);
        _spriteRenderer.color = alpha1;
    }

    protected void EnemyDeath()
    {
        Destroy(gameObject);
    }
}
