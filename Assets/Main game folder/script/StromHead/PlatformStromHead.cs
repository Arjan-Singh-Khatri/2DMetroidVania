using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformStromHead : MonoBehaviour
{
    [SerializeField] private ParticleSystem lighting;
    [SerializeField] private Collider2D platformCollider;

    private void Start()
    {
        StromHeadEvents.instance.StromAttackStart += AttackStart;
        StromHeadEvents.instance.StromAttackEnd += AttackEnd;
    }

    private void AttackStart()
    {
        lighting.Play();
        platformCollider.enabled = true;
    }

    private void AttackEnd()
    {
        lighting.Stop();
        platformCollider.enabled = false;   
    }

    private void OnDestroy()
    {
        StromHeadEvents.instance.StromAttackStart -= AttackEnd;
        StromHeadEvents.instance.StromAttackEnd -= AttackEnd;
    }
}
