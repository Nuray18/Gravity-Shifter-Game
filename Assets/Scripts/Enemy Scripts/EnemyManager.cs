using System.Collections.Generic;
using UnityEngine;

// her scenenin kendine ozel enemyManager scripti olacak.
// Yani bu ayni kodlari ben baska scenelerde farkli sekilde kullanacagim icin ve birbirlerine karismasinlar
// diye boyle yapilacak.

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    private List<IEnemy> enemies = new List<IEnemy>(); // Tüm düşmanları takip eden liste

    private void Awake()
    {
        instance = this;
    }

    public void RegisterEnemy(IEnemy enemy)
    {
        if (enemy != null && !enemies.Contains(enemy))
        {
            enemies.Add(enemy);
        }
    }

    public void UnregisterEnemy(IEnemy enemy)
    {
        if (enemy != null && enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
        }
    }

    public void ResetEnemies()
    {
        Debug.Log("ResetEnemies called!");

        // Silinmiş (yok edilmiş) nesneleri listeden çıkarıyoruz
        enemies.RemoveAll(enemy => enemy == null || ((MonoBehaviour)enemy).gameObject == null);

        // Eğer liste tamamen boşsa, işlem yapma
        if (enemies.Count == 0) return;

        // Listeyi kontrol ederek geçerli olan düşmanları sıfırlıyoruz
        foreach (IEnemy enemy in new List<IEnemy>(enemies))
        {
            if (enemy != null && ((MonoBehaviour)enemy).gameObject != null) // Hem null kontrolü, hem de gameObject geçerliliğini kontrol et
            {
                enemy.ResetEnemy();
            }
        }
    }

    // Oyunu tamamen resetlerken düşman listesini temizle
    public void ClearEnemies()
    {
        Debug.Log("ClearEnemies called!");
        enemies.Clear();
        Debug.Log("Düşman sayısı: " + enemies.Count);
    }
}
