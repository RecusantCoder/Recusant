using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    public void SetHealth(int health)
    {
        slider.value = health;
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    private void Update()
    {
        Vector2 newPosition = GameManager.instance.player.transform.position;
        newPosition.y += -0.3f;
        transform.position = newPosition;
    }
}
