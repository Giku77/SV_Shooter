using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    //public Text WaveText;
    public TextMeshProUGUI ScoreText;
    //public Button RestartButton;
    public GameObject HitUi;
    public GameObject PauseUi;
    //public Gun gun;
    public PlayerHealth playerHealth;

    public int leftEnemy;
    public int score;
    private int waveNumber;

    public void SetUpdateScore(int score)
    {
        this.score += score;
        if (ScoreText != null)
        {
            ScoreText.text = "SCORE : " + this.score;
        }
    }

    public void OnResumeButton()
    {
        Time.timeScale = 1f;
        PauseUi.SetActive(false);
        //Cursor.lockState = CursorLockMode.Locked;
        //GameObject.FindGameObjectWithTag("Ui").transform.GetChild(0).gameObject.SetActive(false);
    }

    //public void SetWaveInfo(int waveNumber, int leftEnemy)
    //{
    //    if (WaveText != null)
    //    {
    //        WaveText.text = "Wave : " + waveNumber + "\r\nEnemy Left : " + leftEnemy;
    //    }
    //}

    public void SetActiveHitUi(bool isActive)
    {
        if (HitUi != null)
        {
            HitUi.SetActive(isActive);
        }
    }

    private void OnEnable()
    {
        //SetWaveInfo(waveNumber, leftEnemy);
        SetUpdateScore(score);
        SetActiveHitUi(false);
    }

    private void Update()
    {
        //if (ScoreText != null) ScoreText.text = "SCORE : " + score;
    }
    public void OnRestartButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
