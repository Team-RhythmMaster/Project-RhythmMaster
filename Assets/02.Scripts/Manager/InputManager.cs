using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager instance;
    public static InputManager Instance {  get { return instance; } }

    public GameObject[] keyEffects = new GameObject[4];

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

    private void Update()
    {
        HandleLane(0, KeyCode.A);
        HandleLane(1, KeyCode.D);
    }

    void HandleLane(int lane, KeyCode key)
    {
        if (Input.GetKeyDown(key))
        {
            NoteManager.Instance.TryHitLane(lane);
        }

        // ·Õ³ëÆ® À¯Áö
        bool isHolding = Input.GetKey(key);
        NoteManager.Instance.HoldLane(lane, isHolding);
    }
}