using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] PlaceObjectOnPlane objectPlaceScript;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text finalScore;
    [SerializeField] ARSession session;
    int score = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        objectPlaceScript.enabled = false;
    }

    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = score.ToString();
    }

    public void FinalScore()
    {
        finalScore.text = $"Score : {score}";
    }
}
