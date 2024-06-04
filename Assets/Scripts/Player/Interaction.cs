using TMPro;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f;
    public float maxCheckDistance = 3.0f;
    public LayerMask layerMask;
    private float lastCheckTime;

    private GameObject curInteractGameObject;

    public TextMeshProUGUI promptText;
    private Camera _camera;

    private PlayerInputController _inputController;

    private void Awake()
    {
        _camera = Camera.main;
        _inputController = GetComponent<PlayerInputController>();
    }

    private void Start()
    {
        _inputController.OnInteractEvent += GetInteract;
    }

    private void Update()
    {
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));

            if (Physics.Raycast(ray, out RaycastHit hit, maxCheckDistance, layerMask))
            {
                if (hit.collider.gameObject != curInteractGameObject && hit.collider.gameObject.TryGetComponent(out IInspectable inspectable))
                {
                    curInteractGameObject = hit.collider.gameObject;
                    SetPrompt(inspectable);
                }
            }
            else
            {
                curInteractGameObject = null;
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
        if (curInteractGameObject.TryGetComponent(out IInteractable interactable))
        {
            interactable.Interact();
            curInteractGameObject = null;
            promptText.gameObject.SetActive(false);
        }
    }
}
