using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Continuity.Movement 
{



public class HeadBobController : MonoBehaviour
{
    #region Variables
    [SerializeField] private bool _enable = true;

    [SerializeField, Range(0, 0.1f)] public float _amplitude = 0.015f;
    [SerializeField, Range(0, 30)] public float _frequency = 10.0f;

    [SerializeField] private Transform _cam = null;
    [SerializeField] private Transform _camHolder = null;

    [HideInInspector]
    public float _startingFrequency;
    [HideInInspector]
    public float _startingAmp;

    private float _toggleSpeed = 3.0f;
    private Vector3 _startPos;
    public Rigidbody rb
    {
        get { return GetComponent<Rigidbody>(); }
    }
    PlayerMovement PM 
    {
        get { return GetComponent<PlayerMovement>(); }
    }
    #endregion
    private void Awake() 
    {
        _startingFrequency = _frequency;
        _startingAmp = _amplitude;
        _startPos = _cam.localPosition;
    }

    void Update() 
    {
        if (!_enable) return;

        CheckMotion();
        ResetPosition();
        _cam.LookAt(FocusTarget());
    }

    private void CheckMotion() 
    {
        float speed = new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;

        if (speed < _toggleSpeed) return;
        if (!PM.isGrounded) return;

        PlayMotion(FootStepMotion());
    }

    private void PlayMotion(Vector3 motion)
    {
        _cam.localPosition += motion;
    }

    private void ResetPosition() 
    {
        if (_cam.localPosition == _startPos) return;
        _cam.localPosition = Vector3.Lerp(_cam.localPosition, _startPos, 1 * Time.deltaTime);
    }

    private Vector3 FootStepMotion() 
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * _frequency) * _amplitude;
        pos.x += Mathf.Cos(Time.time * _frequency / 2) * _amplitude * 2;
        return pos;
    }

    private Vector3 FocusTarget()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + _camHolder.localPosition.y, transform.position.z);
        pos += _camHolder.forward * 15.0f;
        return pos;
    }

}
}
