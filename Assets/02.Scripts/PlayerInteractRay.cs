using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractRay : MonoBehaviour
{
    [Header("Raycast")]
    [SerializeField]
    private Camera m_cam;
    [SerializeField] float maxDistance;
    [SerializeField]
    private LayerMask m_interactMask;

    PlayerInput _pi;
    private InputAction _interact;
    private void Awake()
    {
        m_cam = Camera.main;
        _pi = GetComponent<PlayerInput>();
        _interact = _pi.actions.FindAction("Interact", true);
    }
    private void OnEnable()
    {
        _interact.performed += OnInteractInput;
        _interact.Enable();
    }

    private void OnDisable()
    {
        _interact.performed -= OnInteractInput;
        _interact.Disable();
    }

    private void CheckRay()
    {
        Vector2 screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        Ray ray = m_cam.ScreenPointToRay(screenCenter);

        Vector3 endPoint = ray.origin + ray.direction * maxDistance;

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, m_interactMask, QueryTriggerInteraction.Ignore))
        {
            Debug.DrawLine(ray.origin, endPoint, Color.green);
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.DebugInfo();
            }
        }
        else
        {
            Debug.DrawLine(ray.origin, endPoint, Color.yellow);
        }
    }

    private void Update()
    {
        CheckRay();
    }
    private void OnInteractInput(InputAction.CallbackContext context)
    {
        Vector2 screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        Ray ray = m_cam.ScreenPointToRay(screenCenter);

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, m_interactMask, QueryTriggerInteraction.Ignore))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }
}
