using UnityEngine;

public class EnemyHP : MonoBehaviour, IDamageable
{
    [SerializeField] int maxHP = 50;
    [SerializeField] int currentHP;
    public bool IsDead => currentHP <= 0;
    void Awake()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int amount)
    {
        if(IsDead) return;
        currentHP = Mathf.Max(currentHP - amount, 0);
        Debug.Log($"남은체력 {currentHP}");
        if (IsDead)
        {
            Destroy(gameObject);
        }
    }


}
