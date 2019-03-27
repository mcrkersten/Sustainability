using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonAnimation : MonoBehaviour
{
    [SerializeField]
    private float _offset;
    [SerializeField]
    private float _speed;

    private Vector3 targetOffset;

    [SerializeField]
    private float _counter=60;
    private Vector3 originalPos;

    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.position;
        targetOffset = new Vector3(transform.position.x, transform.position.y + _offset, transform.position.z);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _counter--;

        if (_counter <= 0)
            NewTarget();

        var newPos = Mathf.Lerp(transform.position.y, targetOffset.y, _speed);
        transform.position = new Vector3(transform.position.x, newPos, transform.position.z);
    }

    void NewTarget()
    {
        _offset *= -1;
        targetOffset = new Vector3(originalPos.x, originalPos.y + _offset, originalPos.z);

        _counter = 60;
    }
}
