using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class CatBehaviour : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _catSprite;
    [SerializeField] private Transform _spriteTransform;
    [SerializeField] private Transform _flip;
    [SerializeField] private ParticleSystem _startleParticle;
    [SerializeField] private ColorType _colorType;

    public ColorType ColorType => _colorType;

    private static readonly float _speed = 0.1f;

    private CatState _state;
    
    private Animator _animator;
    private Rigidbody2D _rb;

    private bool _finished;
    private bool _jumping;
    private bool _startling;
    private bool _jumpCheck;
    private LayerMask _groundMask;

    private float _stateTimer;
    private Vector2 _originalPos;

    private float _sideMoveSpeed;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        _originalPos = _rb.position;
        _finished = false;
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Ground") && _jumpCheck)
        {
            _jumpCheck = false;
            _jumping = false;

            SetState((CatState)Random.Range(0,2));
        }
    }

    private void FixedUpdate()
    {
        if (_finished || _jumping || _startling)
            return;
        
        _rb.MovePosition(_rb.position + _sideMoveSpeed * Time.fixedDeltaTime * Vector2.right);
    }

    private void Update()
    {
        if (_finished || _startling)
            return;
        
        if (_jumping)
        {
            var vRotation = Mathf.Clamp(_rb.linearVelocityY * 30f, -65f, 65f);
            _spriteTransform.localRotation = Quaternion.Euler(0f,0f,vRotation);
        }
        else
        {
            _spriteTransform.localRotation = Quaternion.identity;
            _stateTimer -= Time.deltaTime;

            if (_stateTimer <= 0)
            {
                ChangeState();
                return;
            }
            switch(_state)
            {
                case CatState.Idle:
                case CatState.Idle2:
                case CatState.Licking:
                    _sideMoveSpeed = 0f;
                    break;
                case CatState.WalkingLeft:
                    _sideMoveSpeed = -_speed;
                    if(Mathf.Abs(_rb.position.x - _originalPos.x) > 0.09f)
                        ChangeState();
                    break;
                case CatState.WalkingRight:
                    _sideMoveSpeed = _speed;
                    if(Mathf.Abs(_rb.position.x - _originalPos.x) > 0.09f)
                        ChangeState();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private void ChangeState()
    {
        switch (_state)
        {
            case CatState.Idle:
            case CatState.Idle2:
                _stateTimer = Random.Range(0.8f, 1.4f);
                
                if(_rb.position.x - _originalPos.x > 0.06f)
                    SetState(CatState.WalkingLeft);
                else if(_rb.position.x - _originalPos.x < -0.06f)
                    SetState(CatState.WalkingRight);
                else
                    SetState((CatState)Random.Range(3,5));
                
                break;
            case CatState.Licking:
                break;
            case CatState.WalkingLeft:
            case CatState.WalkingRight:
                _stateTimer = Random.Range(0.8f, 1.4f);
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

    public void Jump()
    {
        if (_jumping)
            return;

        _jumpCheck = false;
        _jumping = true;
        _rb.AddForce(Random.Range(5.2f,5.4f) * Vector2.up, ForceMode2D.Impulse);
        _animator.Play("Jump");
        StartCoroutine(JumpRoutine());
    }
    
    public void Startle()
    {
        if (_startling)
            return;

        _startling = true;
        _animator.Play("Startle");
        _rb.bodyType = RigidbodyType2D.Static;
        _spriteTransform.DORotateQuaternion(Quaternion.identity, 0.05f)
            .SetEase(Ease.Linear);
        _rb.DOMove(0.08f * Vector2.up, 0.05f).SetRelative().SetEase(Ease.Linear);
        StartCoroutine(StartleRoutine());
    }

    public void Finish()
    {
        _finished = true;
        _spriteTransform.DORotateQuaternion(Quaternion.identity, 0.05f)
            .SetEase(Ease.Linear);
        _animator.Play("Idle");
    }

    private IEnumerator JumpRoutine()
    {
        yield return new WaitForSeconds(0.45f);
        _jumpCheck = true;
    }

    private IEnumerator StartleRoutine()
    {
        yield return new WaitForSeconds(0.07f);
        _startleParticle.Play();
        
        /*_catSprite.DOColor(new Color(1f, 0.7f, 0.7f), 0.3f)
            .SetEase(Ease.Linear)
            .OnComplete(() => _catSprite.DOColor(Color.white, 0.3f)
                .SetEase(Ease.Linear));*/
        
        _spriteTransform.DOScale(1.15f, 0.3f).SetEase(Ease.Linear)
            .OnComplete(() => 
                _spriteTransform.DOScale(1f, 0.3f).SetEase(Ease.Linear));
        
        yield return new WaitForSeconds(0.6f);
        _rb.bodyType = RigidbodyType2D.Dynamic;
        _startling = false;
    }
}