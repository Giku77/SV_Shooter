using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class UiManager : MonoBehaviour
{
    //public Text WaveText;
    public TextMeshProUGUI ScoreText;

    //public Button RestartButton;
    public GameObject HitUi;
    public GameObject PauseUi;
    //public Gun gun;
    public PlayerHealth playerHealth;

    public Slider musicSlider;
    public Slider sfxSlider;

    public Toggle soundToggle;

    public int leftEnemy;
    public int score;
    private int waveNumber;

    private void Awake()
    {
        if (musicSlider != null)
        {
            musicSlider.value = AudioManager.instance.musicVolume;
            musicSlider.onValueChanged.AddListener((value) =>
            {
                Debug.Log("Music Volume: " + value);
                AudioManager.instance.SetMusicVolume(value);
            });
        }
        if (sfxSlider != null)
        {
            sfxSlider.value = AudioManager.instance.sfxVolume;
            sfxSlider.onValueChanged.AddListener((value) =>
            {
                AudioManager.instance.SetSfxVolume(value);
            });
        }
        if (soundToggle != null)
        {
            soundToggle.isOn = AudioListener.volume > 0;
            soundToggle.onValueChanged.AddListener((isOn) =>
            {
                AudioListener.volume = isOn ? 1 : 0;
            });
        }

    }

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
        PauseUi.SetActive(false);
        StartCoroutine(Resum());
    }

    private IEnumerator Resum()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        Time.timeScale = 1f;
    }

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
    public void OnQuitButtonClicked()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // 에디터에선 재생만 중지
        #else
            Application.Quit();
        #endif
    }
}
