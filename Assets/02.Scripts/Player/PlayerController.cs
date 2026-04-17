using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private SkeletonAnimation currentCharacter;
    public SkeletonDataAsset[] charactersData;

    private Slider hpSlider;
    private Text hpGuageText;

    private float currentHP = 100f;

    private float jumpHeight = 13f;
    private float apexCutoff = 0.5f;       // 정점 근처 감속 타이밍

    private float fallMultiplier = 6.0f;   // 추락 중력 가속도
    private float riseMultiplier = 4.5f;   // 상승 중력 가속도
    private float apexSlowMultiplier = 4f; // 정점 감속 가속도

    private float landingTimer = 0f;
    private float landingDelay = 0.2f;

    private bool isLanding = false;
    private bool isGrounded = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentCharacter = GetComponent<SkeletonAnimation>();
        hpSlider = GameObject.Find("HPSlider").GetComponent<Slider>();
        hpGuageText = GameObject.Find("HPGaugeText").GetComponent<Text>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            if (isLanding)
                return;

            Jump();
        }

        CheckLanding();
        ApplyBetterJump();
    }

    // 플레이어 상태 초기화
    public void ResetState()
    {
        isGrounded = true;
        isLanding = false;

        rb.linearVelocity = Vector2.zero;

        currentCharacter.ClearState();
        currentCharacter.Skeleton.SetToSetupPose();

        currentCharacter.AnimationState.SetAnimation(0, "idle", true);
    }

    private void Jump()
    {
        isGrounded = false;
        SetAnim("jump", false, 3.0f);

        float gravity = Physics2D.gravity.y * rb.gravityScale;
        float jumpVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpVelocity);
    }

    // 중력 가속도 적용
    private void ApplyBetterJump()
    {
        float gravity = Physics2D.gravity.y;

        // 초반 빠르게 상승
        if (rb.linearVelocity.y > 0)
            rb.linearVelocity += Vector2.up * gravity * (riseMultiplier - 1) * Time.deltaTime;

        // 정점에서 강제로 높이 유지
        if (rb.linearVelocity.y > 0 && rb.linearVelocity.y < apexCutoff)
            rb.linearVelocity += Vector2.up * gravity * (apexSlowMultiplier - 1) * Time.deltaTime;

        // 하강 빠르게
        if (rb.linearVelocity.y < 0)
            rb.linearVelocity += Vector2.up * gravity * (fallMultiplier - 1) * Time.deltaTime;
    }

    // 땅에 닿았는지 체크
    private void CheckLanding()
    {
        if (!isGrounded && rb.linearVelocity.y <= 0)
        {
            if (IsGrounded())
            {
                isGrounded = true;
                isLanding = true;
                landingTimer = landingDelay;
            }
        }

        if (isLanding)
        {
            landingTimer -= Time.deltaTime;

            if (landingTimer <= 0f)
            {
                isLanding = false;
                SetAnim("idle", true);
            }
        }
    }

    public void OnDamage(float _damage)
    {
        currentHP -= _damage;
        currentHP = Mathf.Clamp(currentHP, 0f, 100f);

        hpSlider.value = (float)(currentHP / 100f);
        hpGuageText.text = string.Format("{0} / 100", (int)currentHP);

        if (currentHP <= 0f)
        {
            // Game Over 처리
            //RhythmPartManager.Instance.EndGame();
        }
    }

    private bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, LayerMask.GetMask("Ground"));
        return hit.collider != null;
    }

    // 애니메이션 변경
    private void SetAnim(string _animName, bool _loop, float _timeScale = 1.0f)
    {
        var anim = currentCharacter.Skeleton.Data.FindAnimation(_animName);

        if (anim == null)
            return;

        var entry = currentCharacter.AnimationState.SetAnimation(0, _animName, _loop);
        entry.TimeScale = _timeScale;
    }

    // 캐릭터 변경 → CharacterManager로 이동
    public void ChangeCharacter(int _index)
    {
        if (_index < 0 || _index >= charactersData.Length)
            return;

        currentCharacter.ClearState();
        currentCharacter.skeletonDataAsset = charactersData[_index];
        currentCharacter.Initialize(true);

        currentCharacter.AnimationState.SetAnimation(0, "idle", true);
    }
}