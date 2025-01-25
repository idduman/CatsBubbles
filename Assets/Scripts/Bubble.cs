using System;
using UnityEditor.UIElements;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    [SerializeField] private Transform _bubbleParent;
    [SerializeField] private SpriteRenderer _bubbleSprite;

    private Rigidbody2D _rb;
    private float _hSpeed;
    private float _yOrigin;
    private float _yOffset;

    private float _timer;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _yOrigin = _rb.position.y;
    }

    private void FixedUpdate()
    {
        _timer += Time.fixedDeltaTime;
        _yOffset = _yOrigin + Mathf.Sin(_timer * 2f) * 0.07f;
        var pos = _rb.position + _hSpeed * Time.fixedDeltaTime * Vector2.right;
        pos.y = _yOffset;
        _rb.MovePosition(pos);
        
        if(_timer > 10f)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        /*if (!other.TryGetComponent<CatBehaviour>(out var cat))
            return;*/
        
        Destroy(gameObject);
    }

    public void SetHorizontalSpeed(float hSpeed)
    {
        _hSpeed = hSpeed;
    }
    
    public void SetBubbleScale(float scale)
    {
        _bubbleParent.localScale = new Vector3(scale, scale, scale);
    }

    public void SetColor(ColorType colorType)
    {
        var color = GameManager.Instance.GetColor(colorType);
        color.a = _bubbleSprite.color.a;
        _bubbleSprite.color = color;
    }
}