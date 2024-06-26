using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComboCheck : MonoBehaviour
{
    [SerializeField] Collider2D colliderForCombo;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 7) {
            DamageHolder.instance.comboNumber += 1;
            DamageHolder.instance.damageMultiplier += .25f * DamageHolder.instance.comboNumber; 
        }
    }
}
