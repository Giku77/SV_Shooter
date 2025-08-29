using UnityEngine;

[CreateAssetMenu(fileName = "GunData", menuName = "Scriptable Objects/GunData")]
public class GunData : ScriptableObject
{
    public AudioClip shootSound;
    //public AudioClip reloadSound;

    public float damage = 25f;

    //public int startingAmmo = 25;
    //public int magazineSize = 25;

    public float fireRate = 0.12f;
    //public float reloadTime = 1.8f;

    public float range = 50f;
}
