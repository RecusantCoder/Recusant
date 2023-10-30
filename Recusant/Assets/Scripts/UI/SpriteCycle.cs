using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SpriteCycle : MonoBehaviour
{
    public List<(string, string)> comboList = new List<(string, string)>();
    public List<(int, string)> evolutionList = new List<(int, string)>();

    public Item targetEvolution;

    public List<Sprite> sprites;
    public Image image;
    public float cycleInterval = 0.1f;
    public float totalDuration = 3.0f;

    private int currentIndex = 0;

    private void Start()
    {
        
        comboList.Add(("Fleshy", "Qimmiq"));
        comboList.Add(("Flamethrower", "Body_Armor"));
        comboList.Add(("Flashbang", "Grenade"));
        comboList.Add(("Fulmen", "Haurio"));
        comboList.Add(("Glock", "Helmet"));
        comboList.Add(("LazerGun", "Targeting_Computer"));
        comboList.Add(("Machete", "Exolegs"));
        comboList.Add(("Mossberg", "Molotov"));
        
        evolutionList.Add((0, "SwoleSamoyed"));
        evolutionList.Add((1, "FlameShield"));
        evolutionList.Add((2, "MiniNuke"));
        evolutionList.Add((3, "PerfectStorm"));
        evolutionList.Add((4, "Bosco"));
        evolutionList.Add((5, "LivingPlasma"));
        evolutionList.Add((6, "BladeStorm"));
        evolutionList.Add((7, "Che16"));
    }

    public void RunCycleSprites()
    {
        //Add list of available sprites to list
        foreach (var item in GameManager.instance.weaponsList)
        {
            sprites.Add(item.icon);
            Debug.Log(item.name + " added to sprite list");
        }
        // Start the coroutine to cycle through sprites
        StartCoroutine(CycleSprites());
    }

    private IEnumerator CycleSprites()
    {
        int count = 0;
        float elapsedTime = 0f;
        float startTime = Time.realtimeSinceStartup;
        
        //Make image field visible
        Color currentColor = image.color;
        currentColor.a = 255f;
        image.color = currentColor;
        
        while (elapsedTime < totalDuration && count < 20)
        {
            image.sprite = sprites[currentIndex];
            Debug.Log("Displaying sprite number: " +  count);
            
            // Calculate the time elapsed since the coroutine started
            elapsedTime = Time.realtimeSinceStartup - startTime;
            if (elapsedTime >= cycleInterval)
            {
                // Increment the current index, or reset it if at the end of the list
                currentIndex = (currentIndex + 1) % sprites.Count;

                // Increment the count
                count++;

                // Reset the start time for the next cycle
                startTime = Time.realtimeSinceStartup;
            }
            yield return null;
        }

        if (ListOfQualifyingEvolutions().Count != 0)
        {
            foreach (var evol in GameManager.instance.evolutionWeaponsList)
            {
                if (evol.itemName == ListOfQualifyingEvolutions()[0])
                {
                    targetEvolution = evol;
                }
            }
            image.sprite = targetEvolution.icon;
            Inventory.instance.Add(targetEvolution, false);
        }

        Debug.Log("Ended CycleSprites.");
    }

    //checks to see if 2 weapons are level 10 that can combine to evolve
    //returns the list of available evolutions
    private List<string> ListOfQualifyingEvolutions()
    {
        List<string> availableEvolutions = new List<string>();
        foreach (var item in Inventory.instance.items)
        {
            if (GameManager.instance.weaponLevelCount[item.itemName] == 10)
            {
                foreach ((string comboOne, string comboTwo) in comboList)
                {
                    if (item.itemName == comboOne)
                    {
                        if (GameManager.instance.weaponLevelCount.ContainsKey(comboTwo))
                        {
                            if (GameManager.instance.weaponLevelCount[comboTwo] == 10)
                            {
                                int comboIndex = comboList.FindIndex(t => t.Item1 == comboOne && t.Item2 == comboTwo);
                                bool alreadyInInventory = false;
                                foreach (var itemName in Inventory.instance.items)
                                {
                                    string comboEvolutionString = evolutionList[comboIndex].Item2;
                                    if (itemName.itemName == comboEvolutionString)
                                    {
                                        alreadyInInventory = true;
                                    }
                                }

                                if (!alreadyInInventory)
                                {
                                    availableEvolutions.Add(evolutionList[comboIndex].Item2);
                                }
                            }
                        }
                    }
                    else if (item.itemName == comboTwo)
                    {
                        if (GameManager.instance.weaponLevelCount.ContainsKey(comboOne))
                        {
                            if (GameManager.instance.weaponLevelCount[comboOne] == 10)
                            {
                                int comboIndex = comboList.FindIndex(t => t.Item1 == comboOne && t.Item2 == comboTwo);
                                bool alreadyInInventory = false;
                                foreach (var itemName in Inventory.instance.items)
                                {
                                    string comboEvolutionString = evolutionList[comboIndex].Item2;
                                    if (itemName.itemName == comboEvolutionString)
                                    {
                                        alreadyInInventory = true;
                                    }
                                }

                                if (!alreadyInInventory)
                                {
                                    availableEvolutions.Add(evolutionList[comboIndex].Item2);
                                }
                            }
                        }
                    }
                }
            }
        }
        return availableEvolutions;
    }
    
    
}

