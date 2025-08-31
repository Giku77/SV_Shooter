using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Gun : MonoBehaviour
{

    public UiManager uiManager;
    public enum State
    {
        Ready,
        Empty,
        Reloading
    }

    private State currentState = State.Ready;

    public State CurrentState
    {
        get { return currentState; }
        private set { currentState = value; }
    }

    public GunData gunData;

    public ParticleSystem muzzleFlash;
    //public ParticleSystem bulletImpactEffect;

    private LineRenderer lineRenderer;
    private AudioSource audioSource;

    public Transform bulletSpawnPoint;

    public GameObject fireLight;

    public int currentAmmo;
    public int currentMagazine;

    private float lastFireTime;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        lineRenderer.positionCount = 2;
        fireLight.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        //currentAmmo = gunData.startingAmmo;
        //currentMagazine = gunData.magazineSize;
        currentState = State.Ready;
        lastFireTime = 0f;
        //uiManager.SetAmmoText(currentAmmo, currentMagazine);
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    StartCoroutine(ShotEffect());
        //}

        switch (currentState)
        {
            case State.Ready:
                UpdateReadyState();
                break;
            case State.Empty:
                UpdateEmptyState();
                break;
            case State.Reloading:
                UpdateReloadingState();
                break;
        }
    }

    public void Fire()
    {
        if (Time.time >= lastFireTime + gunData.fireRate && Time.timeScale != 0f)
        {
            lastFireTime = Time.time;
            Shoot();
        }
    }

    public void AddAmmo(int amount)
    {
        currentMagazine += amount;
        //currentMagazine = Mathf.Min(currentMagazine + amount, gunData.magazineSize);
        //Debug.Log("Ammo: " + currentAmmo + " / " + currentMagazine);
        //uiManager.SetAmmoText(currentAmmo, currentMagazine);
    }

    private void Shoot()
    {
        Debug.Log("Shoot");
        Vector3 hitPos = Vector3.zero;

        RaycastHit hit;
        if (Physics.Raycast(bulletSpawnPoint.position, bulletSpawnPoint.forward, out hit, gunData.range))
        {
            Debug.Log("We hit " + hit.collider.name);
            hitPos = hit.point;
            var target = hit.collider.GetComponent<IDamagable>();
            if (target != null)
            {
                Debug.Log("We hit " + hit.collider.name + " and did " + gunData.damage + " damage.");
                target.TakeDamage(gunData.damage, hit.point, hit.normal);
            }
        }
        else
        {
            hitPos = bulletSpawnPoint.position + bulletSpawnPoint.forward * gunData.range;
        }

        StartCoroutine(ShotEffect(hitPos));

        currentAmmo--;
        //Debug.Log("Ammo: " + currentAmmo + " / " + currentMagazine);
        //uiManager.SetAmmoText(currentAmmo, currentMagazine);
        if (currentAmmo <= 0)
        {
            currentState = State.Empty;
        }
    }

    //public bool Reload()
    //{
    //    bool isReload = currentState == State.Empty && currentMagazine > 0 || currentState == State.Ready && currentMagazine > 0 && currentAmmo < gunData.magazineSize;
    //    if (isReload)
    //    {
    //        currentState = State.Reloading;
    //        //StartCoroutine(ReloadCoroutine());
    //    }
    //    return isReload;
    //}

    //private IEnumerator ReloadCoroutine()
    //{
    //    audioSource.PlayOneShot(gunData.reloadSound);

    //    yield return new WaitForSeconds(gunData.reloadTime);
    //    int ammoNeeded = gunData.magazineSize - currentAmmo;
    //    int ammoNeeded = gunData.startingAmmo - currentAmmo;
    //    if (currentMagazine >= ammoNeeded)
    //    {
    //        currentAmmo += ammoNeeded;
    //        currentMagazine -= ammoNeeded;
    //    }
    //    else
    //    {
    //        currentAmmo += currentMagazine;
    //        currentMagazine = 0;
    //    }
    //    Debug.Log("Ammo: " + currentAmmo + " / " + currentMagazine);
    //    uiManager.SetAmmoText(currentAmmo, currentMagazine);
    //    currentState = State.Ready;
    //}

    private void UpdateEmptyState()
    {
        //if (Input.GetKeyDown(KeyCode.R) && currentMagazine > 0)
        //{
        //    currentState = State.Reloading;
        //    StartCoroutine(Reload());
        //}
    }

    private void UpdateReadyState()
    {
        //if (Input.GetKey(KeyCode.Mouse0))
        //{
        //    Fire();
        //}

        //if (Input.GetKeyDown(KeyCode.R) && currentMagazine > 0 && currentAmmo < gunData.magazineSize)
        //{
        //    currentState = State.Reloading;
        //    StartCoroutine(Reload());
        //}
    }


    private void UpdateReloadingState()
    {

    }

    private IEnumerator ShotEffect(Vector3 hitPos)
    {
        audioSource.PlayOneShot(gunData.shootSound, AudioManager.instance.sfxVolume);

        muzzleFlash.Play();
        audioSource.Play();
        lineRenderer.enabled = true;
        fireLight.SetActive(true);
        lineRenderer.SetPosition(0, bulletSpawnPoint.position);

        lineRenderer.SetPosition(1, hitPos);

        yield return new WaitForSeconds(0.1f);
        lineRenderer.enabled = false;
        fireLight.SetActive(false);
    }

}
