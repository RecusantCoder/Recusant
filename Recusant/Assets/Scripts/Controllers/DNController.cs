using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DNController : MonoBehaviour
{
    public TMP_Text myTextElement;
    private float elapsed = 0f;
    private float killTime = 1f;
    private float timePassed = 0f;

    // Start is called before the first frame update
    void Start()
    {
        //myTextElement = gameObject.GetComponent<TMPro.TextMeshPro>();
        //myTextElement = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        //was using for action every second
        elapsed += Time.deltaTime;
        if (elapsed >= 1f)
        {
            elapsed = elapsed % 1f;
            timePassed++;
            if (timePassed >= killTime)
            {
                Destroy(gameObject);
            }
        }
    }

    public void ShowDamage(float amount)
    {
        myTextElement.fontSize = 7f;
        myTextElement.text = amount + "";
        myTextElement.color = Color.white;
        
    }
    
    
}
