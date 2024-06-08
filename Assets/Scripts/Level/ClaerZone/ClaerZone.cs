using UnityEngine;

public class ClaerZone : MonoBehaviour
{
    private PlayerMovement _playerMovement;

    private void Start()
    {
        _playerMovement = GameObject.FindWithTag(Define.PlayerTag).GetComponent<PlayerMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Define.PlayerTag))
        {
            GameManager.Instance.GameClear();
            _playerMovement.ToggleCursor();
        }
    }
}
