[System.Serializable]
public class Character
{
    public string imagePath;
    public bool unlocked;
    public string name;
    public string description;
    public string weaponImagePath;

    // Constructor
    public Character(string imagePath, bool unlocked, string name, string description, string weaponImagePath)
    {
        this.imagePath = imagePath;
        this.unlocked = unlocked;
        this.name = name;
        this.description = description;
        this.weaponImagePath = weaponImagePath;
    }
}