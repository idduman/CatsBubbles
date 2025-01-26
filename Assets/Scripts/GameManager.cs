using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    [SerializeField] private Camera _mainCamera;
    [SerializeField] private UIController _uiController;
    [SerializeField] private List<Color> _colorTypes;
    [SerializeField] private List<Color> _colorTypesEdge;
    
    private int _score = 0;

    public int Score
    {
        get => _score;
        private set
        {
            _score = value;
            _uiController.SetScoreText(_score);
        }
    }
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        Score = 0;
    }
    
    public Color GetColor(ColorType colorType)
    {
        if((int)colorType > _colorTypes.Count - 1)
            throw new IndexOutOfRangeException("color type index out of bound");

        return _colorTypes[(int)colorType];
    }
    
    public Color GetColorEdge(ColorType colorType)
    {
        if((int)colorType > _colorTypesEdge.Count - 1)
            throw new IndexOutOfRangeException("color type index out of bound");

        return _colorTypesEdge[(int)colorType];
    }

    public void BubblePopped(Vector3 worldPos, ColorType colorType)
    {
        var screenPos = _mainCamera.WorldToScreenPoint(worldPos);
        var color = _colorTypes[(int)colorType];
        _uiController.AddScorePopup(screenPos, color);
    }

    public void AddScore(int score)
    {
        Score += score;
    }
}