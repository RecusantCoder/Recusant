using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteelContainer : Interactable
{
    public string thisName;
    private GameObject _player;
    private bool isPickedUp = false;
    public GameObject indicator;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        CreateIndicator();
    }

    public override void Interact()
    {
        base.Interact();
        if (!isPickedUp)
        {
            isPickedUp = true;
            PickUp();
        }
    }

    void PickUp()
    {
        StartCoroutine(MoveToPlayerAndDestroy());
    }
    
    IEnumerator MoveToPlayerAndDestroy()
    {
        Transform playerTransform = _player.transform;
        float speed = 2f; // Adjust the speed at which the object moves towards the player
        
        while (Vector3.Distance(transform.position, playerTransform.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);
            yield return null;
        }

        AudioManager.instance.Play("pickup");

        //Call to open UI for random item from steel container
        
        GameManager.instance.ShowSteelContainerScreen();
        
        Destroy(gameObject);
    }

    public void CreateIndicator()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        GameObject myIndicator = Instantiate(indicator, canvas.transform, false);
        myIndicator.transform.localPosition = Vector3.zero;
        myIndicator.GetComponent<Indicator>().target = transform;
    }
}
