using Spine.Unity;
using Unity.Android.Gradle.Manifest;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class PlayerController : MonoBehaviour
{
    private SkeletonAnimation skeletonAnimation;
    private Rigidbody2D rb;

    public AnimationReferenceAsset idleAnim;
    public AnimationReferenceAsset jumpAnim;

    private Slider hpSlider;
    private Text hpGuageText;

    private float currentHP = 100f;
    private float jumpVelocity = 12f;
    private float fallMultiplier = 3.0f;
    private float gravityUp = 2f;

    private bool isGrounded = true;

    private void Awake()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        rb = GetComponent<Rigidbody2D>();

        hpSlider = GameObject.Find("HPSlider").GetComponent<Slider>();
        hpGuageText = GameObject.Find("HPGaugeText").GetComponent<Text>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        CheckLanding();
    }

    // 점프 처리
    private void Jump()
    {
        isGrounded = false;
        rb.gravityScale = gravityUp;
        SetAnim(jumpAnim, false, 2.0f);
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpVelocity);
    }

    // 착지 체크
    private void CheckLanding()
    {
        // 착지 체크
        if (!isGrounded && rb.linearVelocity.y <= 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;

            // 바닥 체크
            if (IsGrounded())
            {
                Debug.Log(":: IsGround ::");
                isGrounded = true;
                rb.gravityScale = 1.0f;
                SetAnim(idleAnim, true);
            }
        }
    }

    // 데미지 처리
    public void OnDamage(float _damage)
    {
        currentHP -= _damage;
        currentHP = Mathf.Clamp(currentHP, 0f, 100f);

        hpSlider.value = (float)(currentHP / 100f);
        hpGuageText.text = string.Format("{0} / 100", (int)currentHP);

        if (currentHP <= 0f)
        {
             Debug.Log("Game Over!");
        }
    }

    // 애니메이션 에셋 변경
    private void SetAnim(AnimationReferenceAsset _anim, bool _loop, float _timeScale = 1.0f)
    {
        var entry = skeletonAnimation.AnimationState.SetAnimation(0, _anim, _loop);
        entry.TimeScale = _timeScale;
    }

    // 바닥 체크
    private bool IsGrounded()
    {
        Vector2 origin = (Vector2)transform.position + Vector2.down * 0.5f;
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, 0.1f, LayerMask.GetMask("Ground"));
        return hit.collider != null;
    }
}