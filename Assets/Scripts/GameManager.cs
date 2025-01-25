using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    [SerializeField] private UIController _uiController;
    [SerializeField] private List<Color> _colorTypes;
    
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

    public void AddScore(int score)
    {
        Score += score;
    }
}