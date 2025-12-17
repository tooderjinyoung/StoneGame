using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Controller : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector3 startPos;
    private Vector3 endPos;

    private AudioSource shootSound;

    private Vector3 nullvector = new Vector3(9999f, 9999f, 0f);

    [Header("Drag Settings (물리 힘)")]
    [SerializeField] float powerScale = 0.07f;
    [SerializeField] float maxPower = 10f;
    [SerializeField] float mindistance = 1f;
    [SerializeField] float maxdistance = 80f;

    [Header("Visual Settings (화살표 모양)")]
    [Tooltip("드래그 거리에 따라 화살표가 커지는 속도")]
    [SerializeField] 
    float visualScaleFactor = 0.2f;

    [Tooltip("화살표의 최대 길이 제한")]
    [SerializeField] 
    float maxVisualScale = 15f;

    [Header("Season Data (Auto Loaded)")]
    [SerializeField] float stoneSpeed;
    [SerializeField] float stoneFriction;
    [SerializeField] float stoneBounce;

    public GameObject child;

    public enum TURN { BLACK, WHITE }
    public TURN myTurn = TURN.BLACK;
    private bool isMove = false;

    private float moveCheckDelay = 0.1f;
    private float currentMoveTime = 0f;

    Season seasonLoader;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (GameManager.Inst == null) return;
        
        seasonLoader = new Season();
        string background = GameManager.Inst.selectBackground;
        Season.SeasonData data = seasonLoader.LoadSeasonConfig(background);

        if (data != null)
        {
            stoneSpeed = data.stoneSpeed;
            stoneFriction = data.stoneFriction;
            stoneBounce = data.stoneBounce;

            PhysicsMaterial2D mat = new PhysicsMaterial2D("StoneBounce");
            mat.bounciness = stoneBounce;
            mat.friction = 0.2f;
            rb.sharedMaterial = mat;
        }
        else
        {
            stoneFriction = 0.98f;
            stoneSpeed = 1.0f;
        }

        if (gameObject.CompareTag("Black")) myTurn = TURN.BLACK;
        else if (gameObject.CompareTag("White")) myTurn = TURN.WHITE;

        if (child == null && transform.childCount > 1)
        {
            child = transform.GetChild(1).gameObject;
        }
        if (child != null) child.SetActive(false);
    }

    private void OnMouseDown()
    {
        if (!GameManager.Inst.IsTurn(this.myTurn)) { startPos = nullvector; return; }
        if (GameManager.Inst.isShot) return;

        startPos = Input.mousePosition;

        if (child != null)
        {
            child.SetActive(true);
            UpdateArrowVisual();
        }
    }

    private void OnMouseDrag()
    {
        if (child == null || !child.activeSelf) return;
        UpdateArrowVisual();
    }

    private void UpdateArrowVisual()
    {
        Vector3 currentMousePos = Input.mousePosition;

        // 방향 계산 (당기는 반대 방향 = 날아갈 방향)
        Vector3 dragVector = startPos - currentMousePos;

        // 회전
        float angle = Mathf.Atan2(dragVector.y, dragVector.x) * Mathf.Rad2Deg;

        child.transform.rotation = Quaternion.Euler(0f, 0f, angle + 90f);

        float dist = dragVector.magnitude;
        float scaleY = Mathf.Clamp(dist * visualScaleFactor, 0, maxVisualScale);

        Vector3 newScale = child.transform.localScale;
        newScale.y = scaleY;
        child.transform.localScale = newScale;
    }

    private void OnMouseUp()
    {
        if (startPos == nullvector) return;
        if (GameManager.Inst.isShot) return;

        if (child != null)
        {
            child.SetActive(false);
            child.transform.localScale = new Vector3(10,10, 1);
        }

        endPos = Input.mousePosition;

        Vector2 dir = (startPos - endPos).normalized;

        float dragDistance = (endPos - startPos).magnitude;
        float power = Mathf.Clamp(dragDistance * powerScale, 0, maxPower);

        if (power < mindistance * powerScale) power = mindistance * powerScale;
        else if (power > maxdistance * powerScale) power = maxdistance * powerScale;

        float finalPower = power * (stoneSpeed > 0 ? stoneSpeed : 1.0f);

        rb.AddForce(dir * finalPower, ForceMode2D.Impulse);

        GameManager.Inst.isShot = true;
        this.isMove = true;
        this.currentMoveTime = 0f;

        startPos = nullvector;
    }



    private void FixedUpdate()
    {
        if (stoneFriction > 0)
        {
            rb.velocity *= stoneFriction;
        }

        if (isMove && GameManager.Inst.isShot)
        {
            currentMoveTime += Time.fixedDeltaTime;

            if (currentMoveTime > moveCheckDelay)
            {
                if (rb.velocity.sqrMagnitude < 0.05f && StoneManager.Inst.IsAllStrop())
                {
                    this.isMove = false;
                    GameManager.Inst.isShot = false;
                    rb.velocity = Vector2.zero;
                    rb.angularVelocity = 0f;
                    rb.rotation = 0f;
   
                    GameManager.Inst.CheckTurnEnd();
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Dead"))
        {
            if (GameManager.Inst.isShot)
            {
                // 1. 턴 종료 체크
                GameManager.Inst.CheckTurnEnd();

                GameManager.Inst.isShot = false;

                isMove = false;
            }
            rb.rotation = 0f;
            Destroy(gameObject);
        }
        else
        {

            shootSound = gameObject.GetComponent<AudioSource>();

            if (shootSound != null)
            {
                shootSound.Play();
            }
        }

    }





}