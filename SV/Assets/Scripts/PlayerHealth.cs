using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : LivingEntity
{
    private int hashDie = Animator.StringToHash("Die");
    public Slider healthSlider;

    public AudioClip deathClip;
    public AudioClip hitClip;
    //public AudioClip itemPickuoClip;

    private AudioSource audioSource;
    private Animator animator;

    private PlayerMove playerMovement;
    private PlayerShooter playerShoot;

    private UiManager uiManager;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMove>();
        playerShoot = GetComponent<PlayerShooter>();
        uiManager = GameObject.FindGameObjectWithTag("Ui").GetComponent<UiManager>();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        Debug.Log("Player Health onEnable");
        healthSlider.gameObject.SetActive(true);
        //healthSlider.maxValue = MaxHealth;
        //healthSlider.value = Health;
        healthSlider.value = Health / MaxHealth;
        playerMovement.enabled = true;
        playerShoot.enabled = true;
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    TakeDamage(10f, Vector3.zero, Vector3.zero);
        //}
    }

    public void Heal(float healAmount)
    {
        if (Isdead) return;
        Health += healAmount;
        if (Health > MaxHealth)
        {
            Health = MaxHealth;
        }
        //healthSlider.value = Health;
        healthSlider.value = Health / MaxHealth;
        //if (audioSource != null && itemPickuoClip != null)
        //{
        //    audioSource.PlayOneShot(itemPickuoClip);
        //}
    }

    public override void TakeDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (Isdead) return;
        base.TakeDamage(damage, hitPoint, hitNormal);
        StartCoroutine(hitEffect());
        //healthSlider.value = Health;
        healthSlider.value = Health / MaxHealth;
        if (audioSource != null && hitClip != null)
        {
            audioSource.PlayOneShot(hitClip);
        }
    }

    private IEnumerator hitEffect()
    {
        //yield return new WaitForSeconds(0.1f);
        uiManager.SetActiveHitUi(true);
        yield return new WaitForSeconds(0.1f);
        uiManager.SetActiveHitUi(false);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    protected override void Die()
    {
        base.Die();
        healthSlider.gameObject.SetActive(false);
        if (animator != null)
        {
            animator.SetTrigger(hashDie);
        }
        if (audioSource != null && deathClip != null)
        {
            audioSource.PlayOneShot(deathClip);
        }
        playerMovement.enabled = false;
        playerShoot.enabled = false;
    }
}
