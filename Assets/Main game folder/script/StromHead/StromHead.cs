using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class StromHead : EnemyParentScript
{
    [Header("Serilizable")]
    [SerializeField] private Collider2D stromAttackCollider;
    [SerializeField] private Transform[] teleportPoints;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform[] firingLocations;
    [SerializeField] private ParticleSystem particleAttack;

    [Header("Variables")]
    private Animator animator;
    
    public bool _killed = false;
    private float telePortTimer = 0f;
    private float telePortCount = 0f;
    private float damageTaken = 0 ;
    private float sleepTimer = 10f;
    private bool _frenzy = false;
    private bool isSleeping = false;

    [SerializeField] AudioClip lightningStrike;
    [SerializeField] AudioClip lightningStrom;
    [SerializeField] AudioClip electricBuzz;
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
        health = 500;
        animator = GetComponent<Animator>();
        StromHeadEvents.instance.activateUI();
        StromHeadEvents.instance.onStromHeadHealthChanged(health);
        _audioSource.outputAudioMixerGroup = mixerGroup;
        _audioSource.clip = electricBuzz;
        _audioSource.loop = true;
        _audioSource.Play();

        Invoke(nameof(Frenzy), 2.1f);
        particleAttack.Play();


    }
    #region Save And Load
    public void SaveData(ref GameData gameData)
    {
        if (gameData.bossesKilled.ContainsKey(this.enemyID))
        {
            gameData.bossesKilled.Remove(this.enemyID);
        }
        gameData.bossesKilled.Add(this.enemyID, _killed);
    }

    public void LoadData(GameData gameData)
    {
        gameData.bossesKilled.TryGetValue(this.enemyID, out _killed);
        if (_killed)
        {
            gameObject.SetActive(false);
        }
    }

    #endregion

    // Update is called once per frame
    void Update(){
        if (_killed) return;
        if(_frenzy) Frenzy();


        SleepState();
    }

    private void SleepState()
    {
        if (isSleeping)
        {
            sleepTimer -= Time.deltaTime;
            if(sleepTimer < 0) {
                sleepTimer = 10f;
                animator.SetBool("Sleep", false);
                Frenzy(); 
            }
        }
    }


    private void Frenzy()
    {
        if (!particleAttack.isPlaying)
            particleAttack.Play();
        _frenzy = true;
        TeleportTimer();
    }

    void StromAttackStart() {
        if (telePortCount >= 5)
        {
            telePortCount = 0;
            StartCoroutine(StromAttack());
            _frenzy = false;
            telePortTimer = 0f;
        }
    }


    private void ProjectileInstantiate(){
        // the actual instantiation
        for(int i =0; i < firingLocations.Length; i++)
        {
            GameObject instantiatedObject = Instantiate(projectilePrefab, firingLocations[i]);
        }

    }

    private void TeleportTimer() {
        telePortTimer -= Time.deltaTime;
        if (telePortTimer < 0){
            telePortTimer = 4.5f;
            telePortCount++;
            Teleport();
            StromAttackStart();
        }
    }

    private void Teleport(){
        int randomIndex = Random.Range(0, teleportPoints.Length);
        transform.position = teleportPoints[randomIndex].position;
        int randomNum = Random.Range(0, 101);
        
        if(randomNum <= 45) {
            _audioSource.PlayOneShot(lightningStrike);
            ProjectileInstantiate();
        }

    }

    private IEnumerator StromAttack(){

        yield return new WaitForSeconds(.7f);
        particleAttack.Play();
        stromAttackCollider.enabled = true;
        _audioSource.clip = lightningStrom;
        _audioSource.Play();
        animator.SetTrigger("Strom");
        StromHeadEvents.instance.StromAttackStart();

    } 
    private void StromAttackEnd() {
        particleAttack.Stop();
        stromAttackCollider.enabled = false;
        StromHeadEvents.instance.StromAttackEnd();
        isSleeping = true;
        particleAttack.Stop();
        animator.SetBool("Sleep", true);
        _audioSource.Stop();
    }

    private void TurnElectricBuzzOn() {
        _audioSource.clip = electricBuzz;
        _audioSource.Play();
    }

    private void OnTriggerEnter2D(Collider2D collision){
        
        if (collision.gameObject.CompareTag("Player") && !isSleeping)
        {
            //player.GetComponent<playerDeath>().TakeDamage(DamageHolder.instance.stromHead);
            Events.instance.onPlayerTakeDamage(DamageHolder.instance.stromHead);
        }

        if (collision.gameObject.CompareTag("PlayerAttackHitBox")){
            HealthDepleteEnemy(DamageHolder.instance.playerDamage * DamageHolder.instance.damageMultiplier, ref this.health);
            damageTaken += DamageHolder.instance.playerDamage * DamageHolder.instance.damageMultiplier;

        }

        if (collision.gameObject.CompareTag("HeavyHitBox")){
            HealthDepleteEnemy(DamageHolder.instance.playerHeavyDamage * DamageHolder.instance.damageMultiplier, ref this.health);
            damageTaken += DamageHolder.instance.playerHeavyDamage * DamageHolder.instance.damageMultiplier;
        }

        StromHeadEvents.instance.onStromHeadHealthChanged(health);

        //StartDeath();

    }
    void StartDeath() {
        health = 0;
        if (health <= 0)
        {
            particleAttack.Stop();
            _killed = true;
            animator.SetTrigger("death");

            StromHeadEvents.instance.onBossDead();
            StromHeadEvents.instance.onReturnToHub();
        }
    }

}
