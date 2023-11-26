using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Interactable
{
    private GameObject _player;
    private bool isPickedUp = false;
    public CircleCollider2D circleCollider;
    public bool movingToPlayer;
    [SerializeField]
    private int coinValue;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        if (movingToPlayer)
        {
            PickUp();
        }
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
        Debug.Log("Coin moving to player");
        Transform playerTransform = _player.transform;
        float speed = 2f; // Adjust the speed at which the object moves towards the player

        circleCollider.enabled = false;
        
        while (Vector3.Distance(transform.position, playerTransform.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);
            yield return null;
        }
        
        CoinCounter.instance.CoinsFound(coinValue);
        
        AudioManager.instance.Play("Coin");
        
        Destroy(gameObject);
    }
}
