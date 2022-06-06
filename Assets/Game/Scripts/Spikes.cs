using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    // Start is called before the first frame update
    public int _damage;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var _isPlayer = collision.collider.CompareTag("Player");
        if (_isPlayer)
        {
            var _player = collision.collider.GetComponent<Player>();
            _player.TakeDamage(_damage);
        }
    }
}
