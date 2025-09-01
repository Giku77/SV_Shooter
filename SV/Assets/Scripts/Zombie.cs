using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class Zombie : LivingEntity
{
    public enum State
    {
        Idle,
        Trace,
        Attack,
        Die
    }
    private enum Type
    {
        Default,
        Speed,
        Heavy
    }

    private static readonly int hashDie = Animator.StringToHash("Die");
    private static readonly int hashTarget = Animator.StringToHash("HasTarget");

    private State currentState;

    private Transform target;

    public float traceDist = 10.0f;
    public float attackDist = 2.0f;

    private UiManager uiManager;

    public ParticleSystem bloodE;

    public float damage = 20.0f;
    public float lastAttackTime;
    public float attackDelay = 1.0f;
    public AudioClip zombieHit;
    public AudioClip zombieDie;

    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private CapsuleCollider capsuleCollider;
    private AudioSource audioSource;
    //public Slider healthSlider;

    private bool sinking = false;

    public State state
    {
        get { return currentState; }
        set
        {
            var prev = currentState;
            currentState = value;
            switch (currentState)
            {
                case State.Idle:
                    animator.SetBool(hashTarget, false);
                    navMeshAgent.isStopped = true;
                    break;
                case State.Trace:
                    animator.SetBool(hashTarget, true);
                    navMeshAgent.isStopped = false;
                    break;
                case State.Attack:
                    animator.SetBool(hashTarget, false);
                    navMeshAgent.isStopped = true;
                    break;
                case State.Die:
                    animator.SetTrigger(hashDie);
                    navMeshAgent.isStopped = true;
                    break;
            }
        }
    }

    public void SetZombieData(ZombieData data)
    {
        MaxHealth = data.health;
        Health = data.health;
        damage = data.damage;
        //attackDelay = data.attackDelay;
        //traceDist = data.traceDist;
        //attackDist = data.attackDist;
        navMeshAgent.speed = data.speed;
    }

    //public ZombieData GetRandZombieData()
    //{
    //    switch (UnityEngine.Random.Range(0, 3))
    //    {
    //        case 0:
    //            zombieData = Resources.Load<ZombieData>("ZombieData Default");
    //            break;
    //        case 1:
    //            zombieData = Resources.Load<ZombieData>("ZombieData Speed");
    //            break;
    //        case 2:
    //            zombieData = Resources.Load<ZombieData>("ZombieData Heavy");
    //            break;
    //        default:
    //            zombieData = Resources.Load<ZombieData>("DefaultZombieData");
    //            break;
    //    }
    //    return zombieData;
    //}
    public void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        audioSource = GetComponent<AudioSource>();
        uiManager = GameObject.FindGameObjectWithTag("Ui").GetComponent<UiManager>();
    }

    private IEnumerator bloodEffect(Vector3 hitpos)
    {
        audioSource.PlayOneShot(zombieHit, AudioManager.instance.sfxVolume);

        bloodE.transform.position = hitpos;
        bloodE.Play();
        yield return new WaitForSeconds(1.0f);
    }

    private void Update()
    {
        if (sinking)
            transform.Translate(Vector3.down * 2f * Time.deltaTime, Space.World);
        switch (currentState)
        {
            case State.Idle:
                UpdateIdle();
                break;
            case State.Trace:
                UpdateTrace();
                break;
            case State.Attack:
                UpdateAttack();
                break;
            case State.Die:
                UpdateDie();
                break;
        }
    }

    private void UpdateDie()
    {
        //Debug.Log("Zombie is dead.");
    }

    private void UpdateAttack()
    {
        if (target == null || (target != null && Vector3.Distance(transform.position, target.position) > attackDist))
        {
            state = State.Trace;
            return;
        }
        //transform.LookAt(target);
        var lookPos = target.position;
        lookPos.y = transform.position.y;
        transform.LookAt(lookPos);

        if (Time.time - lastAttackTime > attackDelay)
        {
            lastAttackTime = Time.time;
            //animator.SetTrigger("Attack");
            //target.GetComponent<LivingEntity>().TakeDamage(damage, transform.position, Vector3.up);
            var damageable = target.GetComponent<IDamagable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage, transform.position, -transform.forward);
            }
        }
    }

    private void UpdateTrace()
    {
        if (target != null && Vector3.Distance(transform.position, target.position) <= attackDist)
        {
            state = State.Attack;
            return;
        }
        if (target == null && Vector3.Distance(transform.position, target.position) > traceDist)
        {
            state = State.Idle;
            return;
        }
        //animator.SetBool("HasTarget", true);
        navMeshAgent.SetDestination(target.position);
    }

    private void UpdateIdle()
    {
        if (target != null && Vector3.Distance(transform.position, target.position) <= traceDist)
        {
            state = State.Trace;
        }

        target = FindTarget(traceDist);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    public override void TakeDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        base.TakeDamage(damage, hitPoint, hitNormal);
        //healthSlider.value = Health / MaxHealth;

        StartCoroutine(bloodEffect(hitPoint));
    }

    public void StartSinking() 
    {
        if (navMeshAgent) navMeshAgent.enabled = false;

        var rb = GetComponent<Rigidbody>();
        if (rb)
        {
            rb.isKinematic = true;
            rb.detectCollisions = false;
        }

        sinking = true;
        Destroy(gameObject, 5f);
    }

    protected override void Die()
    {
        audioSource.PlayOneShot(zombieDie, AudioManager.instance.sfxVolume);
        base.Die();
        capsuleCollider.enabled = false;
        //healthSlider.gameObject.SetActive(false);
        //Destroy(healthSlider);
        //Destroy(capsuleCollider);
        state = State.Die;
        uiManager.SetUpdateScore(10);
        //uiManager.score += 10;
    }

    private IEnumerator onDead()
    {
        yield return new WaitForSeconds(5.0f);
        Destroy(gameObject);
        //gameObject.SetActive(false);
    }

    public LayerMask targetPlayer;

    protected Transform FindTarget(float radius)
    {
        var colliders = Physics.OverlapSphere(transform.position, radius, targetPlayer.value);
        if (colliders.Length == 0)
        {
            return null;
        }
        //if (colliders.Length > 0)
        //{
        //    return colliders[0].transform;
        //}
        return colliders.OrderBy(x => Vector3.Distance(x.transform.position, transform.position)).First().transform;
        //return colliders.OrderBy( _=> Vector3.Distance(transform.position, target.position)).First().transform;
        //return colliders.Min(x => Vector3.Distance(x.transform.position, target.position));
    }
}
