using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance { get; private set; }

    [Header("Movement Settings")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpingPower;
    [SerializeField] public int extraJumps;
    [SerializeField] private float jumpSpeedMultiplier = 1.5f; // Zıplama hızı artırıcı
    [SerializeField] private float fallMultiplier = 2.5f; // Düşüş hızını artırır
    [SerializeField] private float lowJumpMultiplier = 2f; // Hafif zıplama için hız artırıcı

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask slipperyLayer; // Kaygan zemin katmanı

    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashCooldown;
    [SerializeField] private int maxDashCount;
    [SerializeField] private GameObject DashEffect;
    [SerializeField] private bool isDashing = false;
    public bool canDash = true;

    [Header("Slippy Settings")]
    [SerializeField] private float slipperyFactor = 0.9f; // Kayganlık yavaşlama oranı
    private bool onSlipperySurface = false; // Kaygan zeminde olup olmadığını kontrol etmek için

    private float horizontal;
    private bool isFacingRight = true;
    private int remainingJumps;
    private int remainingDashes;
    private SpriteRenderer spriteRenderer;

    private float flipScale = 1f; // Yatay ölçek çarpanı
    private float jumpScale = 1f; // Dikey ölçek çarpanı

    [SerializeField] private ParticleSystem dustFx;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        remainingDashes = maxDashCount;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (isDashing) return; // Dash sırasında hareket engellenir

        // Hareket input'u
        horizontal = Input.GetAxisRaw("Horizontal");

        // Kaygan zemini kontrol et
        onSlipperySurface = IsOnSlipperySurface();

        // Zıplama
        if (IsGrounded())
        {
            dustFx.Play();
            remainingJumps = extraJumps;
            remainingDashes = maxDashCount; // Yere değince dash sıfırlanır
        }

        if (Input.GetButtonDown("Jump") && (IsGrounded() || remainingJumps > 0))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower * jumpSpeedMultiplier);
            remainingJumps--;
            StartCoroutine(ScaleDuringJump());
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        if (rb.velocity.y < 0)
        {
            dustFx.Play();
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        // Dash kontrolü
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && remainingDashes > 0)
        {
            dustFx.Play();
            StartCoroutine(Dash());
        }

        Flip();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Water"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    private void FixedUpdate()
    {
        if (isDashing) return;

        if (onSlipperySurface)
        {
            // Kaygan zemin hareketi
            Vector2 targetVelocity = new Vector2(horizontal * 15, rb.velocity.y);
            rb.velocity = Vector2.Lerp(rb.velocity, targetVelocity, 1 - slipperyFactor);
        }
        else
        {
            // Normal hareket
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private bool IsOnSlipperySurface()
    {
        // Kaygan zemini kontrol et
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, slipperyLayer);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            dustFx.Play();
            isFacingRight = !isFacingRight;
            flipScale = -flipScale; // Yatay ölçeği ters çevir
            UpdateScale();
        }
    }
    private IEnumerator ChangeEngineColour(Color StartColor, Color EndColor, float _duration)
    {
        float tick = 0;
        while (tick < 1f)
        {
            spriteRenderer.color = Color.Lerp(StartColor, EndColor, tick);
            tick += Time.deltaTime / _duration;

            yield return null;
        }

        yield return new WaitForSeconds(dashDuration);
        spriteRenderer.color = EndColor;

    }
    private IEnumerator Dash()
    {
        isDashing = true;
        spriteRenderer.color = Color.white;
        if (DashEffect != null)
        {
            Instantiate(DashEffect, transform.position, Quaternion.identity);
        }
        remainingDashes--; // Kullanılan dash sayısını azalt
        canDash = false;

        Vector2 dashDirection = new Vector2(horizontal, Input.GetAxisRaw("Vertical"));

        if (dashDirection == Vector2.zero)
        {
            dashDirection = isFacingRight ? Vector2.right : Vector2.left; // Varsayılan yön sağa veya sola
        }

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f; // Yerçekimini devre dışı bırak

        rb.velocity = dashDirection.normalized * dashSpeed; // Dash yönüne doğru hız ver

        yield return new WaitForSeconds(dashDuration); // Dash süresi

        rb.gravityScale = originalGravity; // Yerçekimini geri getir
        rb.velocity = Vector2.zero; // Hızı sıfırla
        isDashing = false;
        StartCoroutine(ChangeEngineColour(Color.white, Color.white, dashCooldown * 0.8f));
        yield return new WaitForSeconds(dashCooldown); // Cooldown bekle
        canDash = true;
    }

    private IEnumerator ScaleDuringJump()
    {
        float targetJumpScale = 0.9f; // Zıplama sırasında hedef dikey ölçek
        float duration = 0.2f; // Ölçekleme süresi
        float elapsedTime = 0;

        // Yavaşça hedef dikey ölçeğe git
        while (elapsedTime < duration)
        {
            jumpScale = Mathf.Lerp(1f, targetJumpScale, elapsedTime / duration);
            UpdateScale();
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        jumpScale = targetJumpScale; // Hedef ölçeğe ayarla
        UpdateScale();

        // Hedef ölçekten eski ölçeğe dönüş
        elapsedTime = 0;
        while (elapsedTime < duration)
        {
            jumpScale = Mathf.Lerp(targetJumpScale, 1f, elapsedTime / duration);
            UpdateScale();
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        jumpScale = 1f; // Eski dikey ölçeğe dön
        UpdateScale();
    }

    private void UpdateScale()
    {
        // Yatay ve dikey ölçeği birleştirerek uygula
        transform.localScale = new Vector3(flipScale, jumpScale, 1f);
    }
}