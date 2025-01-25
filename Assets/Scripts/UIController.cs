using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;

    public void SetScoreText(int score)
    {
        _scoreText.text = $"Score: {score.ToString()}";
    }
}
