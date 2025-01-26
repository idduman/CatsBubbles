using UnityEngine;
using Random = UnityEngine.Random;

public class BubbleSpawner : MonoBehaviour
{
    [SerializeField] private Bubble _bubblePrefab;
    [SerializeField] private Transform _spawnPointLeft;
    [SerializeField] private Transform _spawnPointRight;
    [SerializeField] private float _scaleMin = 0.8f;
    [SerializeField] private float _scaleMax = 1.2f;
    [SerializeField] private float _spawnIntervalMin = 0.8f;
    [SerializeField] private float _spawnIntervalMax = 1.2f;
    [SerializeField] private float _speedMin = 0.4f;
    [SerializeField] private float _speedMax = 0.6f;

    public bool Spawning;

    private bool _prevIsForward;
    private bool _isForward;
    private float _spawnTimer;
    private float _directionChance = 0.5f;

    private ColorType _lastColor = ColorType.Red;

    private void Start()
    {
        _spawnTimer = Random.Range(_spawnIntervalMin, _spawnIntervalMax);
        _prevIsForward = _isForward = Random.Range(0f, 1f) < _directionChance;
        Spawning = true;
    }

    private void Update()
    {
        if (!Spawning)
            return;

        if (_spawnTimer > 0)
        {
            _spawnTimer -= Time.deltaTime;
            return;
        }

        if (_prevIsForward == _isForward)
        {
            var sign = _isForward ? -1 : 1;
            _directionChance = Mathf.Clamp(_directionChance + sign * 0.125f, 0f, 1f);
        }
        _isForward = Random.Range(0f, 1f) < _directionChance;
        _prevIsForward = _isForward;
        
        var color = (ColorType)Random.Range(0, 6);
        while(color == _lastColor)
            color = (ColorType)Random.Range(0, 6);

        _spawnTimer = Random.Range(_spawnIntervalMin, _spawnIntervalMax);
        var pos = _isForward ? _spawnPointLeft.position : _spawnPointRight.position;
        var bubble = Instantiate(_bubblePrefab, pos, Quaternion.identity, transform);
        var speed = (_isForward ? 1f : -1f) * Random.Range(_speedMin, _speedMax);
        bubble.SetBubbleScale(Random.Range(_scaleMin, _scaleMax));
        bubble.SetHorizontalSpeed(speed);
       
        
        _lastColor = color;
        bubble.SetColor(color);
    }

    public void OnFinish()
    {
        Spawning = false;
        var bubbles = GetComponentsInChildren<Bubble>();
        foreach (var bubble in bubbles)
        {
            bubble.Pop();
        }
    }
}