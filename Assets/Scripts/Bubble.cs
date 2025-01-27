using UnityEngine;
using UnityEngine.Serialization;

public class Bubble : MonoBehaviour
{
    [SerializeField] private Transform _bubbleParent;
    [SerializeField] private SpriteRenderer _bubbleSprite;
    [SerializeField] private SpriteRenderer _bubbleEdgeSprite;
    [FormerlySerializedAs("_bubbleParticle")] [SerializeField] private ParticleSystem _popParticle;

    private ColorType _colorType;
    
    private Rigidbody2D _rb;
    private float _hSpeed;
    private float _yOrigin;
    private float _yOffset;

    private float _timer;

    private bool _popped;

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
        if (!_popped && other.TryGetComponent<CatBehaviour>(out var cat))
        {
            var correct = cat.ColorType == _colorType;
            GameManager.Instance.BubblePopped(transform.position, _colorType, correct);
            
            Pop();
            if (!correct)
                cat.Startle();
        }
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
        _colorType = colorType;
        var color = GameManager.Instance.GetColor(colorType);
        color.a = _bubbleSprite.color.a;
        _bubbleSprite.color = color;
        
        var edgeColor = GameManager.Instance.GetColorEdge(colorType);
        _bubbleEdgeSprite.color = edgeColor;
        _popParticle.startColor = edgeColor;
    }

    public void Pop()
    {
        if (_popped)
            return;
        
        _popped = true;
        _bubbleParent.gameObject.SetActive(false);
        _popParticle.Play();
        Destroy(gameObject, 1f);
    }
}