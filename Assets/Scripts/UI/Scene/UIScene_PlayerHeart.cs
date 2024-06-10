using UnityEngine;

public class UIScene_PlayerHeart : MonoBehaviour
{
    public GameObject[] Hearts;

    private GameObject player;
    private PlayerHeartStamina playerHeartStamina;
    private PlayerAttributeHandler playerAttributeHandler;
    private DataManager dataManager;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag(Define.PlayerTag);
        playerHeartStamina = player.GetComponent<PlayerHeartStamina>();
        playerAttributeHandler = player.GetComponent<PlayerAttributeHandler>();

        playerHeartStamina.OnChangeHealthMana += SetHeart;

        foreach (GameObject heart in Hearts)
        {
            heart.SetActive(true);
        }

        DataManager.Instance.OnDataLoad += SetHeart;
    }

    private void SetHeart()
    {
        Debug.Log($"(CurrentHeart: {playerHeartStamina.CurrentHeart}, MaxHeart: {playerHeartStamina.MaxHeart})");
        if (playerHeartStamina.CurrentHeart == 3)
        {
            Hearts[2].SetActive(true);
            Hearts[1].SetActive(true);
            Hearts[0].SetActive(true);
        }
        else if (playerHeartStamina.CurrentHeart == 2)
        {
            Hearts[2].SetActive(false);
            Hearts[1].SetActive(true);
            Hearts[0].SetActive(true);
        }
        else if (playerHeartStamina.CurrentHeart == 1)
        {
            Hearts[2].SetActive(false);
            Hearts[1].SetActive(false);
            Hearts[0].SetActive(true);

        }
        else if (playerHeartStamina.CurrentHeart == 0)
        {
            Hearts[2].SetActive(false);
            Hearts[1].SetActive(false);
            Hearts[0].SetActive(false);
        }
    }
}
