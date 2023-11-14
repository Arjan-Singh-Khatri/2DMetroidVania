using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikedSlimeSpikes : MonoBehaviour
{

    [SerializeField] private float _speed = 4f;
    private Vector3 posiitonVector = Vector3.right;
    private float _lifeTime = 2f;
    // Start is called before the first frame update
    void Start()
    {
        transform.parent = null;   
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += _speed * Time.deltaTime * posiitonVector;
        LifeSpanOver();
    }

    private void LifeSpanOver()
    {
        _lifeTime -= Time.deltaTime;
        if (_lifeTime <= 0)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
}
