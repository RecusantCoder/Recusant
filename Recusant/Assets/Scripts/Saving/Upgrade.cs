[System.Serializable]
public class Upgrade
{
    public string imagePath;
    public string name;
    public string description;
    public int[] prices; //How many upgrades is determined by the length of the prices array
    public int rank; //The current rank the player can purchase

    // Constructor
    public Upgrade(string imagePath, int price, string name, string description, int[] prices, int rank)
    {
        this.imagePath = imagePath;
        this.name = name;
        this.description = description;
        this.prices = prices;
        this.rank = rank;
    }
}
