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

        if (amount < 254)
        {
            float red = 255;    
            float green = 255 - amount;  
            float blue = 255 - amount;     

            float normalizedRed = red / 255f;
            float normalizedGreen = green / 255f;
            float normalizedBlue = blue / 255f;

            Color customColor = new Color(normalizedRed, normalizedGreen, normalizedBlue);
            
            myTextElement.color = customColor;
            //Debug.Log(amount + " <- damage amount " + normalizedBlue + " <- normalizedBlue " + blue + " <- blue");
        }
        else
        {
            myTextElement.color = Color.red;
        }
        
        
        StartCoroutine(AnimateFontSize());
        
    }
    
    private IEnumerator AnimateFontSize()
    {
        float initialFontSize = myTextElement.fontSize;
        float targetFontSize = initialFontSize * 1.5f; // Increase font size by 50%
        float duration = 0.5f;
        float elapsed = 0f;

        // Increase font size
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            float currentFontSize = Mathf.Lerp(initialFontSize, targetFontSize, t);

            myTextElement.fontSize = currentFontSize;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Decrease font size
        elapsed = 0f;
        Color initialColor = myTextElement.color;
        Color transparentColor = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            float currentFontSize = Mathf.Lerp(targetFontSize, initialFontSize, t);

            myTextElement.fontSize = currentFontSize;
            myTextElement.color = Color.Lerp(initialColor, transparentColor, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        myTextElement.fontSize = initialFontSize; // Reset font size
    }

    
    
}
