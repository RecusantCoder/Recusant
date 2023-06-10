using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : Interactable
{
    public float xpDrop;
    public string thisName;
    private GameObject _player;
    private bool isPickedUp = false;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
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
        
        LevelBar.instance.AddExperience(xpDrop);
        FindObjectOfType<AudioManager>().Play("pickup");
        
        Destroy(gameObject);
    }
}
