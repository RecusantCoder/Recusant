[System.Serializable]
public class Total
{
    public string name;
    public int value;

    // Constructor
    public Total(int value, string name)
    {
        this.value = value;
        this.name = name;
    }
}