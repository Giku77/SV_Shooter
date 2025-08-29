using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public static readonly int idReload = Animator.StringToHash("Reload");

    public Gun gun;

    private Vector3 gunPos;
    private Quaternion gunRot;

    private Rigidbody gunRb;
    private Collider gunCollider;

    private PlayerInput playerInput;
    private Animator animator;

    public Transform gunPivot;
    //public Transform leftHandMount;
    //public Transform rightHandMount;


    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
        gunRb = gun.GetComponent<Rigidbody>();
        gunCollider = gun.GetComponent<Collider>();
        gunPos = gun.transform.localPosition;
        gunRot = gun.transform.localRotation;
    }

    private void OnEnable()
    {
        Debug.Log("PlayerShooter OnEnable");
        if (gunRb != null)
        {
            gunRb.isKinematic = true;
        }
        if (gunCollider != null)
        {
            gunCollider.enabled = false;
        }
        gun.transform.localPosition = gunPos;
        gun.transform.localRotation = gunRot;
    }

    private void OnDisable()
    {
        Debug.Log("PlayerShooter OnDisable");
        if (gunRb != null)
        {
            //gunRb.AddForce(Vector3.up * 1f, ForceMode.Impulse);
            gunRb.isKinematic = false;
        }
        if (gunCollider != null)
        {
            gunCollider.enabled = true;
        }
    }
    private void Update()
    {
        if (playerInput.Fire)
        {
            gun.Fire();
        }
    }

    //private void OnAnimatorIK(int layerIndex)
    //{
    //    gunPivot.position = animator.GetIKHintPosition(AvatarIKHint.RightElbow);
    //    animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
    //    animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
    //    animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandMount.position);
    //    animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandMount.rotation);

    //    animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
    //    animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
    //    animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandMount.position);
    //    animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandMount.rotation);
    //}
}
