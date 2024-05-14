using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;

public class playerDeath : MonoBehaviour, IDataPersistance
{
    private Animator anim;
    private Rigidbody2D rig;
    public float health = 40;
    [SerializeField]private float maxHealth = 100;

    [SerializeField] Collider2D damageCollider;
    [SerializeField] SpriteRenderer _spriteRenderer;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
        Events.instance.onPlayerTakeDamage += TakeDamage;
    }

    public void SaveData(ref GameData gameData)
    {
        gameData._health = this.health;
    }

    public void LoadData(GameData gameData)
    {
        this.health = gameData._health;
    }

    public void Heal() {
        this.health += 10;
        if (health > maxHealth)
            health = 100;
        Events.instance.onHealthChangePlayer(health);
    }
 
    private void Restartlevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void TakeDamage(float damage){
        damageCollider.enabled = false;
        DamageHolder.instance.damageMultiplier = 1;
        DamageHolder.instance.comboNumber = 0;
        health -= damage;
        Events.instance.onHealthChangePlayer?.Invoke(health);
        StartCoroutine(HitAnimation());
        StartCoroutine(ColliderTrigger());
        if (health <= 0)
        {
            Death();
        }
    }

    private void Death() {
        rig.bodyType = RigidbodyType2D.Static;
        anim.SetTrigger("death");
    }

    IEnumerator ColliderTrigger(){

        damageCollider.enabled = false;
        yield return new WaitForSeconds(1f);
        damageCollider.enabled = true;
    }

    protected IEnumerator HitAnimation(){

        Color alpha1 = new Color(255, 2555, 255, 1);
        Color aplha2 = new Color(255, 255, 255, .4f);
        Color aplha3 = new Color(255, 255, 255, .6f);

        _spriteRenderer.color = aplha2;
        yield return new WaitForSeconds(.3f);
        _spriteRenderer.color = alpha1;
        yield return new WaitForSeconds(.3f);
        _spriteRenderer.color = aplha3;
        yield return new WaitForSeconds(.3f);
        _spriteRenderer.color = alpha1;
    }

    private void OnTriggerEnter2D(Collider2D collision){

        if (collision.gameObject.CompareTag("trap"))
        {
            Death();
        }

        //Enemy Attack Colliders
        if (collision.gameObject.CompareTag("Strom"))
            TakeDamage(DamageHolder.instance.strom);
        
        if (collision.gameObject.CompareTag("TwoHeadAttack2"))
            TakeDamage(DamageHolder.instance.twoHeadAttackTwoDamage);
        
        if (collision.gameObject.CompareTag("FireBall"))
            TakeDamage(DamageHolder.instance.fireBallDamage);
        
        if (collision.gameObject.CompareTag("CrabAttack"))
            TakeDamage(DamageHolder.instance.crabAttack);
        
        if (collision.gameObject.CompareTag("SpikedSlimeAttack"))
            TakeDamage(DamageHolder.instance.spikedSlimeAttack);
      
    }

    private void OnDisable()
    {
        Events.instance.onPlayerTakeDamage -= TakeDamage;
    }
}
