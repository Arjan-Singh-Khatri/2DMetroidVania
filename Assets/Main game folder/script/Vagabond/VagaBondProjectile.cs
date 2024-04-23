using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ProjectileType {
    Normal,Heavy
}


public class VagaBondProjectile : MonoBehaviour
{
    [SerializeField]private float _speedNormal;
    [SerializeField]private float _speedHeavy;
    [SerializeField]private float _timeToLive;
    private ProjectileType _currentType;

    void Start(){
        if(gameObject.name.CompareTo("") == 0) 
            _currentType = ProjectileType.Normal;
        else
            _currentType=ProjectileType.Heavy;  
    }

    // Update is called once per frame
    void Update(){

        Movement();
        RotationForNormalAttack();
    }

    void Movement() { 
        
    }

    void RotationForNormalAttack() {
        if (_currentType == ProjectileType.Heavy) return;
    }

    private void OnCollisionEnter2D(Collision2D collision){
        
    }
}
