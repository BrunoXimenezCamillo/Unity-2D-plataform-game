using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plataform : MonoBehaviour
{
    Rigidbody2D _rb;
    [Header("Plataform Props")]
    public float _speedX = 2f;
    public float _speedY = 2f;
    public float _directionX = 1f;
    public float _directionY = 1f;
    public float _rangeX;
    public float _rangeY;
    Vector3 _initPosition;
    // Start is called before the first frame update
    void Start()
    {
        _rb = this.gameObject.GetComponent<Rigidbody2D>();
        _initPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {
        MoveProcess();

    }
    void MoveProcess()
    {
        setDirection();
        var velocity = new Vector2(_speedX * _directionX, _speedY * _directionY);
        _rb.velocity = velocity;
    }

    void setDirection()
    {
        var pos = transform.position;

        if (pos.x >= _initPosition.x + _rangeX || pos.x <= _initPosition.x - 0.1f)
        {
            _directionX = -_directionX;
        }
        if (pos.y >= _initPosition.y + _rangeY || pos.y <= _initPosition.y - 0.1f)
        {
            _directionY = -_directionY;
        }
    }


}
