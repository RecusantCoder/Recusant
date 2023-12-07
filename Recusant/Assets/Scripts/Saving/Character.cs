[System.Serializable]
public class Character
{
    public string imagePath;
    public bool unlocked;
    public string name;
    public string description;

    // Constructor
    public Character(string imagePath, bool unlocked, string name, string description)
    {
        this.imagePath = imagePath;
        this.unlocked = unlocked;
        this.name = name;
        this.description = description;
    }
}