[System.Serializable]
public class Upgrade
{
    public string imagePath;
    public int price;
    public string name;
    public string description;

    // Constructor
    public Upgrade(string imagePath, int price, string name, string description)
    {
        this.imagePath = imagePath;
        this.price = price;
        this.name = name;
        this.description = description;
    }
}
