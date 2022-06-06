using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    AudioSource _audioController;

    [Header("Physics")]
    Rigidbody2D _rb;
    Vector3 _initPosition;
    public float _directionY = 1f;
    public float _speed;
    public float _range;

    [Header("Score Props")]
    public bool _isCaugth;
    public int _score = 0;
    public int _life = 0;
    public int _hp = 0;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _initPosition = transform.position;
        _audioController = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        MoveProcess();
    }
    void catchProcess(Collider2D collision)
    {
        var _isPlayer = collision.CompareTag("Player");
        if (_isPlayer && !_isCaugth)
        {
            _isCaugth = true;
            _audioController.Play();
            collision.GetComponent<Player>().AddItem(_score, _life, _hp);
            GetComponent<Animator>().SetBool("_isCaugth", _isCaugth);
            Destroy(gameObject, 0.5f);
        }
    }
    void MoveProcess()
    {
        SetDirection();
        var velocity = new Vector2(0, _speed * _directionY);
        _rb.velocity = velocity;
    }

    void SetDirection()
    {
        var pos = transform.position.y;
        if (pos >= _initPosition.y + _range || pos <= _initPosition.y - 0.1f)
        {
            _directionY = -_directionY;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        catchProcess(collision);
    }
}
