using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] PlayerInputReader inputReader;
    [SerializeField] Animator m_animator;
    [SerializeField] int AttackCount = 0;

    [SerializeField] int defaultDamage = 20;
    [SerializeField] float currentDamage = 0;
    [SerializeField] float equipDamage = 0;
    public float Dmaage => currentDamage;

    int attackHash = -999;

    [SerializeField] LayerMask m_interactMask;
    [SerializeField] float radius = 0.5f;
    [SerializeField] float maxDistance = 1f;
    private Collider[] results = new Collider[5];
    private void Awake()
    {
        Initialize();
        attackHash = Animator.StringToHash("Attack");
    }
    void Initialize()
    {
        if(inputReader == null)
            inputReader = GetComponent<PlayerInputReader>();
        if(m_animator == null)
            m_animator = GetComponent<Animator>();
        currentDamage = defaultDamage;
    }
    public void EquipWeapon(float amount)
    {
        equipDamage = amount;
    }

    private void Update()
    {
        AttackRotation();
        DefalutAttack();
    }
    void AttackRotation()
    {
        if (!inputReader.IsAttackPerformedThisFrame) return;
        Transform carmeraTransform = Camera.main.transform;
        Vector3 forward = carmeraTransform.forward;
        forward.y = 0f;

        forward.Normalize();
        Vector3 moveDir = forward;
        Quaternion rot = Quaternion.LookRotation(moveDir);

        transform.rotation = rot;
    }
    void DefalutAttack()
    {
        if (!inputReader.IsAttackPerformedThisFrame) return;
        m_animator.SetTrigger(attackHash);
        //m_animator.SetInteger("AttackCount",AttackCount);
        SphereCastAttack();
    }
    public void LastAttack()
    {
        currentDamage = (defaultDamage + equipDamage) * 1.3f;
    }
    public void StatAttack()
    {
        //weapon.StartAttack();
    }
    public void EndAttack()
    {
        //weapon.EndAttack();
        currentDamage = (defaultDamage + equipDamage);
    }

    void SphereCastAttack()
    {
        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;

        int count = Physics.OverlapSphereNonAlloc(origin + direction * maxDistance, radius,results,m_interactMask);

        for (int i = 0; i < count; i++)
        {
            IDamageable damageable = results[i].GetComponent<IDamageable>();
            if (damageable == null) return;
            damageable.TakeDamage(defaultDamage);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(transform.position + transform.forward * maxDistance, radius);
    }
}
