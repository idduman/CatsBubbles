using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class CatBehaviour : MonoBehaviour
{
    [SerializeField] private Transform _bottom;
    [SerializeField] private Transform _spriteTransform; 
    [SerializeField] private Transform _flip;

    private static readonly float _speed = 0.2f;

    private CatState _state;
    
    private Animator _animator;
    private Rigidbody2D _rb;
    
    private bool _jumping;
    private bool _jumpCheck;
    private LayerMask _groundMask;

    private float _stateTimer;
    private Vector2 _originalPos;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        _originalPos = _rb.position;
    }

    private void FixedUpdate()
    {
        if (_jumping)
        {
            var vRotation = Mathf.Clamp(_rb.linearVelocityY * 30f, -65f, 65f);
            Debug.Log($"lv: {_rb.linearVelocityY}, vRotation: {vRotation}");
            _spriteTransform.localRotation = Quaternion.Euler(0f,0f,vRotation);
        }
        else
        {
            _spriteTransform.localRotation = Quaternion.identity;
            _stateTimer -= Time.fixedDeltaTime;

            if (_stateTimer <= 0)
            {
                ChangeState();
                return;
            }
            switch(_state)
            {
                case CatState.Idle:
                    break;
                case CatState.Idle2:
                    break;
                case CatState.Licking:
                    break;
                case CatState.WalkingLeft:
                    _rb.MovePosition(_rb.position + _speed * Time.fixedDeltaTime * Vector2.left);
                    if(Mathf.Abs(_rb.position.x - _originalPos.x) > 0.2f)
                        ChangeState();
                    break;
                case CatState.WalkingRight:
                    _rb.MovePosition(_rb.position + _speed * Time.fixedDeltaTime * Vector2.right);
                    if(Mathf.Abs(_rb.position.x - _originalPos.x) > 0.2f)
                        ChangeState();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
    
    private void OnMouseDown()
    {
        Jump();
    }

    private void ChangeState()
    {
        switch (_state)
        {
            case CatState.Idle:
            case CatState.Idle2:
                _stateTimer = Random.Range(0.8f, 1.6f);
                SetState((CatState)Random.Range(3,5));
                break;
            case CatState.Licking:
                break;
            case CatState.WalkingLeft:
            case CatState.WalkingRight:
                _stateTimer = Random.Range(0.8f, 1.6f);
                SetState((CatState)Random.Range(0,2));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void SetState(CatState state)
    {
        switch (state)
        {
            case CatState.Idle:
            case CatState.Idle2:
                _animator.Play("Idle");
                break;
            case CatState.Licking:
                break;
            case CatState.WalkingLeft:
                _flip.localScale = new Vector3(-1f, 1f, 1f);
                _animator.Play("Walk");
                break;
            case CatState.WalkingRight:
                _flip.localScale = new Vector3(1f, 1f, 1f);
                _animator.Play("Walk");
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
        _state = state;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Ground") && _jumpCheck)
        {
            _jumpCheck = false;
            _jumping = false;

            _animator.Play("Idle");
        }
    }

    private void Jump()
    {
        if (_jumping)
            return;

        _jumpCheck = false;
        _jumping = true;
        _rb.AddForce(Random.Range(4.6f,5.2f) * Vector2.up, ForceMode2D.Impulse);
        _animator.Play("Jump");
        StartCoroutine(JumpRoutine());
    }

    private IEnumerator JumpRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        _jumpCheck = true;
    }
}