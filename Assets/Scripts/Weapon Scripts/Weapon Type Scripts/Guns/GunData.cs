using UnityEngine;

[CreateAssetMenu(fileName = "NewGun", menuName = "Weapon/Gun")]
public class GunData : ScriptableObject
{
    public string gunName = "New Gun"; // Silahın adı
    public float damage = 10f;         // Silahın verdiği hasar
    public float range = 100f;         // Silahın menzili
}

