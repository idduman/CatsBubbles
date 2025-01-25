using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BubbleSpawner : MonoBehaviour
{
    [SerializeField] private Bubble _bubblePrefab;
    [SerializeField] private bool _flipDirection;
    [SerializeField] private float _scaleMin = 0.8f;
    [SerializeField] private float _scaleMax = 1.2f;
    [SerializeField] private float _spawnIntervalMin = 0.8f;
    [SerializeField] private float _spawnIntervalMax = 1.2f;
    [SerializeField] private float _speedMin = 0.4f;
    [SerializeField] private float _speedMax = 0.6f;

    public bool Spawning;

    private float _spawnTimer;

    private void Start()
    {
        _spawnTimer = Random.Range(_spawnIntervalMin, _spawnIntervalMax);
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

        _spawnTimer = Random.Range(_spawnIntervalMin, _spawnIntervalMax);
        var bubble = Instantiate(_bubblePrefab, transform);
        var speed = (_flipDirection ? -1f : 1f) * Random.Range(_speedMin, _speedMax);
        bubble.SetBubbleScale(Random.Range(_scaleMin, _scaleMax));
        bubble.SetHorizontalSpeed(speed);
    }
}
