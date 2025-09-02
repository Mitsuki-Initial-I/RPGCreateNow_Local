using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    public string npcNmae = "NPC";
    public string dialogue = "‚±‚ñ‚É‚¿‚Í";
    private bool isPlayerNearby = false;

    private void Start()
    {
        
    }
    private void Update()
    {
        if (isPlayerNearby && Input.GetMouseButtonDown(1))
        {
            Interact();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }
    void Interact()
    {
        Debug.Log(dialogue);
    }
}
