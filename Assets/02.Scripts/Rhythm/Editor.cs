using UnityEngine;

public class Editor : MonoBehaviour
{
    private static Editor instance;
    public static Editor Instance {  get { return instance; } }

    public int currentBar = 0;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
}
