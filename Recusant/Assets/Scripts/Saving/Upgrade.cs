using UnityEngine;

[System.Serializable]
public class Upgrade
{
    public string imagePath;
    public string name;
    public string description;
    public int[] prices; //How many upgrades is determined by the length of the prices array
    public int rank;
    public int totalRanks;

    // Constructor
    public Upgrade(string imagePath, string name, string description, int rank, int totalRanks, int baseCost)
    {
        this.imagePath = imagePath;
        this.name = name;
        this.description = description;
        this.rank = rank;
        this.totalRanks = totalRanks;
        this.prices = CalculatePrices(baseCost); 
    }

    public int[] CalculatePrices(int startingPrice)
    {
        int[] pricing = new int[totalRanks];
        for (int i = 1; i <= totalRanks; i++)
        {
            pricing[i-1] = startingPrice * i;
        }
        return pricing;
    }
}
