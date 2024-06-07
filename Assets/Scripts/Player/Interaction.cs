using TMPro;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f;
    public float maxCheckDistance = 10.0f;
    public LayerMask layerMask;
    private float _lastCheckTime;

    private GameObject _curInteractGameObject;

    public TextMeshProUGUI promptText;
    private PlayerInputController _inputController;
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
        _inputController = GetComponent<PlayerInputController>();
    }

    private void Start()
    {
        _inputController.OnInteractEvent += GetInteract;

        layerMask = LayerMask.GetMask(Define.InteractableLayer);
    }

    private void Update()
    {
        if (Time.time - _lastCheckTime > checkRate)
        {
            _lastCheckTime = Time.time;

            Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));

            if (Physics.Raycast(ray, out RaycastHit hit, maxCheckDistance, layerMask))
            {
                Debug.DrawRay(ray.origin, ray.direction * maxCheckDistance, Color.red);

                if (hit.collider.gameObject != _curInteractGameObject && hit.collider.gameObject.TryGetComponent(out IInspectable inspectable))
                {
                    _curInteractGameObject = hit.collider.gameObject;
                    SetPrompt(inspectable);
                }
            }
            else
            {
                _curInteractGameObject = null;
                promptText.gameObject.SetActive(false);
            }
        }
    }

    private void SetPrompt(IInspectable inspectable)
    {
        promptText.gameObject.SetActive(true);
        promptText.text = inspectable.GetPrompt();
    }

    private void GetInteract()
    {
        if (_curInteractGameObject != null && _curInteractGameObject.TryGetComponent(out IInteractable interactable))
        {
            interactable.Interact();
            _curInteractGameObject = null;
            promptText.gameObject.SetActive(false);
        }
    }
}
