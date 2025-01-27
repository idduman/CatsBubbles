using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private Slider _timerSlider;
    [SerializeField] private Transform _scorePanel;
    [SerializeField] private Transform _scorePopupParent;
    [SerializeField] private Transform _comboPopupParent;
    [SerializeField] private RectTransform _scorePopupPrefab;
    [SerializeField] private RectTransform _comboPopupPrefab;

    private Sequence _punchSequence;
    
    public void SetScoreText(int score)
    {
        _scoreText.text = $"Score: {score.ToString()}";
    }

    public void AddScorePopup(Vector3 screenPos, Color color, int score, int comboCount)
    {
        var scorePopup = Instantiate(_scorePopupPrefab, screenPos, Quaternion.identity, _scorePopupParent);
        if (comboCount > 0)
        {
            var comboPopup = Instantiate(_comboPopupPrefab,
                screenPos+50f*Vector3.up, Quaternion.identity, _scorePopupParent);
            var comboText = comboPopup.GetComponentInChildren<TextMeshProUGUI>();
            comboText.text = $"Combo x{comboCount + 1}";
            comboText.color = color;
            Destroy(comboPopup.gameObject, 1f);
        }
        
        var image = scorePopup.GetComponentInChildren<Image>();
        image.color = color;
        var scoreSequence = DOTween.Sequence().SetEase(Ease.Linear);
        scoreSequence.Insert(0f, 
            scorePopup.DOMove(_scorePanel.position, 0.3f).SetEase(Ease.InQuad));
        scoreSequence.Insert(0f, 
            scorePopup.DOScale(0.1f, 0.3f).SetEase(Ease.InQuad));
        scoreSequence.OnComplete(() =>
        {
            Destroy(scorePopup.gameObject);
            OnScorePopup(score);
        });
    }

    private void OnScorePopup(int score)
    {
        GameManager.Instance.AddScore(score);
        PunchScore();
    }

    private void PunchScore()
    {
        _punchSequence.Kill();

        _punchSequence = DOTween.Sequence().SetEase(Ease.Linear);
        _punchSequence.Append(_scorePanel.DOScale(1.1f, 0.05f).SetEase(Ease.OutQuad));
        _punchSequence.Append(_scorePanel.DOScale(1f, 0.05f).SetEase(Ease.InQuad));
    }

    public void SetTimerText(float roundTimer, float maxTime)
    {
        _timerText.text = $"Time: {Mathf.CeilToInt(roundTimer)}";
        _timerSlider.value = Mathf.Clamp(roundTimer / maxTime, 0f, 1f);
    }
}