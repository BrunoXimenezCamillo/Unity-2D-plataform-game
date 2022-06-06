using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Damage Zone")]
    public float _damageZoneRadius = 0.4f;
    public LayerMask _damageZoneLayer;
    public Transform _damageZone;
    public bool _isDamage;
    public bool _isDeath;

    [Header("Physics")]
    public float _direction = 1f;
    public float _speed = 3f;
    public float _jumpForce = 7f;
    public float _moveCooldown = 300f;
    public float _currentMoveCooldown;
    Rigidbody2D _rb;

    [Header("Props")]
    public float _damage;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimation();
        _isDamage = isDamage();
    }
    private void FixedUpdate()
    {
        MoveProcess();
    }
    void UpdateAnimation()
    {
        var _animator = GetComponent<Animator>();
        _animator.SetFloat("yVelocity", _rb.velocity.y);
        _animator.SetBool("_isMove", isMove());
    }
    bool isDamage()
    {
        var col = GetComponent<BoxCollider2D>();
        var size = new Vector2(col.size.x*2, col.size.y);
        var _isDmg = Physics2D.OverlapBox(_damageZone.position, size, 90f, _damageZoneLayer);
        if (_isDmg) return true;
        return false;
    }
    bool isMove()
    {
        return _rb.velocity.x != 0 || _rb.velocity.y != 0;
    }
    void MoveProcess()
    {
        if (_currentMoveCooldown <= 0 && !isDamage() && !_isDeath)
        {
            setDirection();

            //var x = _speed * _direction;
            var y = _jumpForce;
            var force = new Vector2(0, y * _rb.mass);
            _rb.AddForce(force, ForceMode2D.Impulse);
            _currentMoveCooldown = _moveCooldown;

        }
        if (_currentMoveCooldown > 0) _currentMoveCooldown--;
    }
    void setDirection()
    {
        _direction = -_direction;
        var sprite = GetComponent<SpriteRenderer>();
        sprite.flipX = !sprite.flipX;
    }

    void DamageProcess()
    {
        if (!_isDeath)
        {
            _isDeath = true;
            _rb.bodyType = RigidbodyType2D.Static;
            GetComponent<Animator>().SetBool("_isDeath", _isDeath);
            GetComponent<AudioSource>().Play();
            GetComponent<BoxCollider2D>().enabled = false;
            Destroy(this.gameObject, 0.5f);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        var _isPlayer = collision.collider.CompareTag("Player");
        if (_isPlayer)
        {
            var _player = collision.collider.GetComponent<Player>();
            if (isDamage())
            {
                DamageProcess();
                _player._isAttack = true;
            }
            else
            {
                _player.TakeDamage(_damage);
            }
        }
    }

    private void OnDrawGizmos()
    {
        var col = GetComponent<BoxCollider2D>();
        var size = new Vector2(col.size.x*2, col.size.y);
        Gizmos.color = Color.red;
        Gizmos.DrawCube(_damageZone.position, size);
    }
}


