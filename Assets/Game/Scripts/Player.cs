using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    [Header("Physic")]

    public float _speed;
    public float _dashSpeed;
    public float _jumpForce;
    public float _direction;
    public bool _isMove;
    public Transform _rCollider;
    public Transform _lCollider;

    [Header("Ground check")]

    public float _groundCheckRadius;
    public Transform _groundCheck;
    public LayerMask _layerMask;

    [Header("Character Props")]
    public bool _isStageUp;
    public float _score;
    public float _maxHp;
    public float _hp;
    public float _lifes;
    public bool _isDamage;
    public bool _isAttack;
    public bool _isDeath;
    public float _recoverTime;
    float _currentRecoverTime;
    SpriteRenderer _sprite;
    Rigidbody2D _rb;
    Animator _anim;
    [Header("Audio")]
    public AudioSource _jumpSe;
    public AudioSource _damageSe;
    public AudioSource _fallSe;

    // Start is called before the first frame update
    void Start()
    {
        _sprite = this.gameObject.GetComponent<SpriteRenderer>();
        _rb = this.gameObject.GetComponent<Rigidbody2D>();
        _anim = this.gameObject.GetComponent<Animator>();
        initProps();
    }

    // Update is called once per frame
    void Update()
    {
        JumpProcess();
    }

    private void FixedUpdate()
    {
        UpdateWalkAnimation();
        UpdateJumpAnimation();
        updateHurtAnimation();
        UpdateAttack();
        MoveProcess();
        updateDash();
        updateRecoverTime();
    }

    void initProps()
    {
        _hp = _maxHp;
        _lifes = PlayerPrefs.GetFloat("_playerLifes");
        _score = PlayerPrefs.GetFloat("_playerScore");

    }

    void updateRecoverTime()
    {
        if (_currentRecoverTime == 0 && _isDamage && !_isDeath)
        {
            _isDamage = false;
        }
        if (_currentRecoverTime > 0) _currentRecoverTime--;
    }
    void setupRecoverTime()
    {
        _currentRecoverTime = _recoverTime;
    }
    public void AddItem(float score, float lifes, float hpRegen)
    {
        _score += score;
        _lifes += lifes;
        if (_hp + hpRegen <= _maxHp) _hp += hpRegen; else _hp = _maxHp;
    }

    public void LoseLife()
    {
        _fallSe.Play();
        _lifes -= 1;
        _isDeath = true;
    }

    void UpdateAttack()
    {
        if (_isAttack)
        {
            _isAttack = false;
            var force = new Vector2(0, _jumpForce * _rb.mass);
            _rb.AddForce(force, ForceMode2D.Impulse);
        }
    }
    public void TakeDamage(float damage)
    {
        _damageSe.Play();
        setupRecoverTime();
        _isDamage = true;
        if (_hp - damage > 0)
        {
            _hp -= damage;
        }
        else
        {
            _hp = 0;
            LoseLife();
        }
        var force = new Vector2(_rb.mass * 5 * -_direction, 10 * _rb.mass);
        _rb.AddForce(force, ForceMode2D.Impulse);
    }
    void JumpProcess()
    {
        if (Input.GetButtonDown("Jump") && isGround() && !_isDamage)
        {
            _jumpSe.Play();
            var velocity = new Vector2(0, _jumpForce * _rb.mass);
            _rb.AddForce(velocity, ForceMode2D.Impulse);
        }
    }
    void MoveProcess()
    {
        var input = Input.GetAxisRaw("Horizontal");
        if (input != 0 && !_isDamage)
        {
            if (input == 1 && !IsRightCollided() || input == -1 && !IsLeftCollided())
            {
                _isMove = true;
                setDirection(input);
                var velocity = new Vector2((_speed + _dashSpeed) * input, _rb.velocity.y);
                _rb.velocity = velocity;
            }
        }
        else
        {
            _isMove = false;
        }
    }
    void UpdateWalkAnimation()
    {
        _anim.SetBool("isMove", _isMove);
    }
    void UpdateJumpAnimation()
    {
        var yVelocity = _rb.velocity.y;
        _anim.SetBool("isGround", isGround());
        _anim.SetFloat("yVelocity", yVelocity);

    }

    void updateHurtAnimation()
    {
        _anim.SetBool("isDamage", _isDamage);
    }
    void setDirection(float input)
    {
        if (input == 1)
        {
            _sprite.flipX = false;
        }
        else
        {
            _sprite.flipX = true;
        }
        _direction = input;
    }

    bool isGround()
    {
        var grounded = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _layerMask);
        return !!grounded;
    }

    void updateDash()
    {
        if (Input.GetKey(KeyCode.LeftShift) && !_isDamage)
        {
            _dashSpeed = 4;
        }
        else
        {
            _dashSpeed = 0;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_groundCheck.position, _groundCheckRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_rCollider.position, _groundCheckRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(_lCollider.position, _groundCheckRadius);


    }

    bool IsRightCollided()
    {
        var isRcollided = Physics2D.OverlapCircle(_rCollider.position, _groundCheckRadius, _layerMask);
        return !!isRcollided;
    }

    bool IsLeftCollided()
    {
        var isLcollided = Physics2D.OverlapCircle(_lCollider.position, _groundCheckRadius, _layerMask);
        return !!isLcollided;
    }


}