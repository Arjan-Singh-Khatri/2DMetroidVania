using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class EnemyParentScript : MonoBehaviour
{
    protected GameObject player;
    public float health;
    protected bool enemyDead;
    public float direction;
    [SerializeField] protected string enemyID;

    [ContextMenu("GUID ID GENERATE")]
    private void GenerateGUID(){
        enemyID = System.Guid.NewGuid().ToString();
    }
 
    protected void HealthDepleteEnemy(float attackPower, ref float healthofEnemy){
        DamageHolder.instance.comboNumber += 1;
        DamageHolder.instance.damageMultiplier += .25f * DamageHolder.instance.comboNumber;
        DamageHolder.instance.playerCharge += 1;
       
        healthofEnemy -= attackPower;
        StartCoroutine(HitAnimation());
    }

    protected void PushBack(){
        if (player.transform.position.x <= transform.position.x)
            transform.position = transform.position + new Vector3(1.75f, 0, 0);
        else 
            transform.position = transform.position + new Vector3(-1.75f, 0, 0);
    }

    protected void flip(){
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

    protected IEnumerator HitAnimation(){

        var _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        _spriteRenderer.color = new Color(255, 255, 255, .4f);
        yield return new WaitForSeconds(.25f);
        _spriteRenderer.color = new Color(255, 255, 255, 1);
        yield return new WaitForSeconds(.25f);
        _spriteRenderer.color = new Color(255, 255, 255, .6f);
        yield return new WaitForSeconds(.25f);
        _spriteRenderer.color = new Color(255, 255, 255, 1);
    }

    protected void EnemyDeath(){
        player.GetComponent<playerDeath>().Heal();
        gameObject.SetActive(false);
    }

}
