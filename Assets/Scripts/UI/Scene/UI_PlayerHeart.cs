using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PlayerHeart : MonoBehaviour
{
    public GameObject[] Hearts;

    private PlayerHeartStamina playerHeartStamina;
    private PlayerAttributeHandler playerAttributeHandler;
    private void Start()
    {
        playerHeartStamina = GameObject.FindGameObjectWithTag(Define.PlayerTag).GetComponent<PlayerHeartStamina>();
        playerAttributeHandler = GameObject.FindGameObjectWithTag(Define.PlayerTag).GetComponent<PlayerAttributeHandler>();

        playerHeartStamina.OnChangeHealthMana += UpdateHeart;

        foreach (GameObject heart in Hearts)
        {
            heart.SetActive(true);
        }
    }

    private void UpdateHeart()
    {
        Debug.Log($"(CurrentHeart: {playerHeartStamina.CurrentHeart}, MaxHeart: {playerHeartStamina.MaxHeart})");
        if (playerHeartStamina.CurrentHeart == 2)
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
