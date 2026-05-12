using UnityEngine;
using UnityEngine.InputSystem;

public class CenterRaycastShooter : MonoBehaviour
{
    [Header("Raycast")]
    [SerializeField]
    private Camera m_cam;
    [SerializeField]
    private LayerMask m_hittableMask;
    [SerializeField]
    private LayerMask m_interactMask;
    [SerializeField]
    private float m_maxDistance = 100f;
    private PlayerInput _pi;
    private InputAction _Attack;
    private InputAction _interact;

    private void Awake()
    {
        _pi = GetComponent<PlayerInput>();
        _Attack = _pi.actions.FindAction("Attack", true);
        _interact = _pi.actions.FindAction("Interact", true);
        if (m_cam == null) m_cam = Camera.main;
    }


    private void OnEnable()
    {
        _Attack.performed += OnRayFire;
        _interact.performed += SphereCastExample;
    }
    private void OnDisable()
    {
        _Attack.performed -= OnRayFire;
        _interact.performed -= SphereCastExample;
    }
    private void OnRayFire(InputAction.CallbackContext _)
    {
        Vector2 screenCenter = new(Screen.width * 0.5f, Screen.height * 0.5f);

        Ray _ray = m_cam.ScreenPointToRay(screenCenter);

        if(Physics.Raycast(_ray, out RaycastHit hit, m_maxDistance, m_hittableMask, QueryTriggerInteraction.Ignore))
        {
            Debug.Log($"[CenterRaycastShooter] Hit {hit.collider.name} at {hit.point}");

            Debug.DrawLine(_ray.origin, hit.point, Color.green, 1.0f);
            
            Renderer renderer = hit.collider.GetComponent<Renderer>();
            if (renderer) renderer.material.color = Color.red; 
        }
        else
        {
            Debug.DrawLine(_ray.origin, _ray.direction * m_maxDistance, Color.yellow, 0.5f);

        }
    }

    void SphereCastExample(InputAction.CallbackContext _)
    {
        float radius = 0.5f;
        float maxDistance = 1f;

        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;


        if(Physics.SphereCast(origin, radius, direction, out RaycastHit hit,maxDistance,m_interactMask))
        {
            //Debug.Log($"Sphere Hit {hit.collider.name}");
            IInteractable interact = hit.collider.GetComponent<IInteractable>();
            if (interact == null) return;
            interact.Interact();
        }
        else
        {
            Debug.Log("No hit");
        }
    }

    void OverlapExample(InputAction.CallbackContext _)
    {
        Vector3 center = transform.position;
        float radius = 5f;

        Collider[] hitColliders = Physics.OverlapSphere(center, radius);

        foreach(Collider hitCollider in hitColliders)
        {
            Debug.Log($"Overlap hit {hitCollider.name}");
        }
    }

    private Collider[] results = new Collider[10];

    void Optimizedoverlap()
    {
        Vector3 center = transform.position;
        float radius = 5f;

        int count = Physics.OverlapSphereNonAlloc(center, radius, results);

        for(int i = 0; i < count; i++)
        {
            Debug.Log($"NonAlloc Hit : {results[i].name}");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(transform.position + transform.forward * 1f, 0.5f);
    }
}
