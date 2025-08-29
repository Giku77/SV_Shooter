using UnityEngine;

[CreateAssetMenu(fileName = "ZombieData", menuName = "Scriptable Objects/ZombieData")]
public class ZombieData : ScriptableObject
{
    public float health = 100f;
    public float speed = 2f;
    public float damage = 20f;

    public GameObject modelPrefab;

}
