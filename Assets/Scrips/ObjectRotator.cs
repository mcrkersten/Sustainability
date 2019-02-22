using UnityEngine;
using System.Collections;

public class ObjectRotator : MonoBehaviour
{

    private float _sensitivity;
    private Vector3 _mouseReference;
    private Vector3 _mouseOffset;
    private Vector3 _rotation;
    private bool _isRotating;
    public GameObject ship;

    void Start()
    {
        _sensitivity = 0.4f;
        _rotation = Vector3.zero;
    }

    void Update()
    {
        if (_isRotating)
        {
            // offset
            _mouseOffset = (Input.mousePosition - _mouseReference);
            // apply rotation
            //_rotation.y = -(_mouseOffset.x + _mouseOffset.y) * _sensitivity;
            _rotation.y = -(_mouseOffset.x) * _sensitivity;
            _rotation.x = -(_mouseOffset.y) * _sensitivity;
            // rotate
            //transform.Rotate(_rotation);
            ship.transform.eulerAngles += _rotation;
            // store mouse
            _mouseReference = Input.mousePosition;
        }
    }

    public void OnCLick()
    {
        // rotating flag
        _isRotating = true;

        // store mouse
        _mouseReference = Input.mousePosition;
    }

    public void OnRelease()
    {
        // rotating flag
        _isRotating = false;
    }

}