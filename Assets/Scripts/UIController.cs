using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private Transform _scorePanel;
    [SerializeField] private Transform _scorePopupParent;
    [SerializeField] private RectTransform _scorePopupPrefab;

    private Sequence _punchSequence;
    
    public void SetScoreText(int score)
    {
        _scoreText.text = $"Score: {score.ToString()}";
    }

    public void AddScorePopup(Vector3 screenPos, Color color)
    {
        var scorePopup = Instantiate(_scorePopupPrefab, screenPos, Quaternion.identity, _scorePopupParent);

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
            OnScorePopup();
        });
    }

    private void OnScorePopup()
    {
        GameManager.Instance.AddScore(10);
        PunchScore();
    }

    private void PunchScore()
    {
        _punchSequence.Kill();

        _punchSequence = DOTween.Sequence().SetEase(Ease.Linear);
        _punchSequence.Append(_scorePanel.DOScale(1.1f, 0.05f).SetEase(Ease.OutQuad));
        _punchSequence.Append(_scorePanel.DOScale(1f, 0.05f).SetEase(Ease.InQuad));
    }
}