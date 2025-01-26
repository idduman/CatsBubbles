using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    [SerializeField] private Camera _mainCamera;
    [SerializeField] private InputController _inputController;
    [SerializeField] private UIController _uiController;
    [SerializeField] private BubbleSpawner _bubbleSpawner;
    [SerializeField] private CanvasGroup _endGameCanvas;
    [SerializeField] private TMP_Text _endGameScoreText;
    [SerializeField] private List<Color> _colorTypes;
    [SerializeField] private List<Color> _colorTypesEdge;
    [SerializeField] private float _roundTime = 60f;
    
    private int _score = 0;
    private int _comboCount = 0;

    private float _roundTimer;
    private bool _started;

    private CatBehaviour[] _cats;

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

        _cats = FindObjectsByType<CatBehaviour>(FindObjectsSortMode.None);
    }

    private void Start()
    {
        Score = 0;
        _comboCount = 0;
        _roundTimer = _roundTime;
        _uiController.SetTimerText(_roundTimer);
        _endGameCanvas.alpha = 0f;
        _endGameCanvas.gameObject.SetActive(false);

        _started = true;
    }

    private void Update()
    {
        if (!_started)
            return;

        if (_roundTimer > 0f)
        {
            _roundTimer -= Time.deltaTime;
            _uiController.SetTimerText(_roundTimer);
        }
        else
        {
            _uiController.SetTimerText(0f);
            FinishGame();
        }

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

    public void BubblePopped(Vector3 worldPos, ColorType colorType, bool correct)
    {
        var screenPos = _mainCamera.WorldToScreenPoint(worldPos);
        var color = _colorTypesEdge[(int)colorType];
        
        if (correct)
        {
            _uiController.AddScorePopup(screenPos, color, 10 + _comboCount * 5, _comboCount);
            _comboCount++;
        }
        else
            _comboCount = 0;
    }

    public void AddScore(int score)
    {
        Score += score;
    }

    private void FinishGame()
    {
        if (!_started)
            return;
        
        _started = false;
        _inputController.enabled = false;
        _bubbleSpawner.OnFinish();

        foreach (var cat in _cats)
        {
            cat.Finish();
        }
        
        _endGameScoreText.text = Score.ToString();
        StartCoroutine(FinishGameRoutine());
    }

    public void PlayAgain()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    private IEnumerator FinishGameRoutine()
    {
        _uiController.gameObject.SetActive(false);
        _endGameCanvas.gameObject.SetActive(true);
        while (_endGameCanvas.alpha < 1f)
        {
            _endGameCanvas.alpha += Time.deltaTime;
            yield return null;
        }
    }
}