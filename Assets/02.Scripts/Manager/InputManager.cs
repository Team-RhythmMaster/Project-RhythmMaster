using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private static InputManager instance;
    public static InputManager Instance {  get { return instance; } }

    public GameObject[] keyEffects = new GameObject[4];
    private Judgement judgement;

    public LayerMask noteLayer;
    public Vector2 mousePos;

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

    private void Start()
    {
        judgement = FindFirstObjectByType<Judgement>();
    }

    private void Update()
    {
        // 왼쪽 레인 (A키)
        if (Input.GetKeyDown(KeyCode.A))
        {
            HitLane(0);
        }

        // 오른쪽 레인 (D키)
        if (Input.GetKeyDown(KeyCode.D))
        {
            HitLane(1);
        }
    }

    void HitLane(int lane)
    {
        float currentTime = AudioManager.Instance.songTime;
        NoteObject note = NoteManager.Instance.GetClosestNote(lane, currentTime);

        if (note != null)
        {
            note.TryHit();
        }
    }

    public void OnNoteLine0(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            judgement.Judge(0);
            keyEffects[0].gameObject.SetActive(true);
        }
        else if (context.canceled)
        {
            judgement.CheckLongNote(0);
            keyEffects[0].gameObject.SetActive(false);
        }
    }
    public void OnNoteLine1(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            judgement.Judge(1);
            keyEffects[1].gameObject.SetActive(true);
        }
        else if (context.canceled)
        {
            judgement.CheckLongNote(1);
            keyEffects[1].gameObject.SetActive(false);
        }
    }
    public void OnNoteLine2(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            judgement.Judge(2);
            keyEffects[2].gameObject.SetActive(true);
        }
        else if (context.canceled)
        {
            judgement.CheckLongNote(2);
            keyEffects[2].gameObject.SetActive(false);
        }
    }
    public void OnNoteLine3(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            judgement.Judge(3);
            keyEffects[3].gameObject.SetActive(true);
        }
        else if (context.canceled)
        {
            judgement.CheckLongNote(3);
            keyEffects[3].gameObject.SetActive(false);
        }
    }
}