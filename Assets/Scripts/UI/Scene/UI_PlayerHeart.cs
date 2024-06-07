using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PlayerHeart : MonoBehaviour
{
    public GameObject[] Hearts;

    public PlayerHeartStamina playerHeartStamina;
    int currentHeart = 0;

    private void Start()
    {
        playerHeartStamina = GameObject.FindGameObjectWithTag(Define.PlayerTag).GetComponent<PlayerHeartStamina>();
        currentHeart = playerHeartStamina.CurrentHeart;
        playerHeartStamina.OnChangeHealthMana += UpdateHeart;
    }

    private void UpdateHeart()
    {
        if (currentHeart == playerHeartStamina.CurrentHeart)
        {
            return;
        }

        currentHeart = playerHeartStamina.CurrentHeart;

        Hearts[currentHeart-1].SetActive(false);
    }

}
