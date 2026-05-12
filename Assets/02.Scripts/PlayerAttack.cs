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
        float radius = 0.5f;
        float maxDistance = 1f;

        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;


        if (Physics.SphereCast(origin, radius, direction, out RaycastHit hit, maxDistance, m_interactMask))
        {
            //Debug.Log($"Sphere Hit {hit.collider.name}");
            IDamageable Damageable = hit.collider.GetComponent<IDamageable>();
            if (Damageable == null) return;
            Damageable.TakeDamage(defaultDamage);
        }
        else
        {
            Debug.Log("No hit");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(transform.position + transform.forward * 1f, 0.5f);
    }
}
