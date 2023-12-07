[System.Serializable]
public class Achievement
{
    public string imagePath;
    public bool unlocked;
    public string name;
    public string description;

    // Constructor
    public Achievement(string imagePath, bool unlocked, string name, string description)
    {
        this.imagePath = imagePath;
        this.unlocked = unlocked;
        this.name = name;
        this.description = description;
    }
}

