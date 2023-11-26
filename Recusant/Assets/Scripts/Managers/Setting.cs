[System.Serializable]
public class Setting
{
    public bool isOn;
    public string name;

    // Constructor
    public Setting(bool isOn, string name)
    {
        this.isOn = isOn;
        this.name = name;
    }
}