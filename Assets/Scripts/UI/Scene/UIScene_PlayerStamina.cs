using UnityEngine;
using UnityEngine.UI;

public class UIScene_PlayerStamina : MonoBehaviour
{
    public PlayerHeartStamina playerHeartStamina;
    public Image staminaImage;

    private void Start()
    {
        playerHeartStamina = GameObject.FindGameObjectWithTag(Define.PlayerTag).GetComponent<PlayerHeartStamina>();
        playerHeartStamina.OnChangeHealthMana += UpdateStamina;

    }

    private void Update()
    {
        UpdateStamina();  //데이터로드시 한번 체크
    }

    private void UpdateStamina()
    {
        float fillAmount = (float)(playerHeartStamina.CurrentStamina / playerHeartStamina.MaxStamina);
        staminaImage.fillAmount = fillAmount;
    }
}
