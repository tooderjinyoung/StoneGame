using Unity.VisualScripting;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 startPos;
    private Vector2 endPos;

    private Vector2 nullvector = new Vector2(9999f,9999f);

    // 드래그 관련 설정값 (없으면 에러나서 다시 추가했습니다)
    [Header("Drag Settings")]
    [SerializeField] float powerScale = 0.05f;
    [SerializeField] float maxPower = 10f;
    [SerializeField] float mindistance = 1f;
    [SerializeField] float maxdistance = 80f;

    // CSV에서 불러올 데이터들
    [Header("Season Data (Auto Loaded)")]
    [SerializeField] float stoneSpeed;     // 힘 보정값으로 사용
    [SerializeField] float stoneFriction;  // 감속 비율 (0.9 ~ 0.99 추천)
    [SerializeField] float stoneBounce;    // (선택사항: 탄성)

    public enum TURN { BLACK, WHITE }
    private TURN myTrun= TURN.BLACK;
    private bool isMove = false;

    private float moveCheckDelay = 0.1f;
    private float currentMoveTime = 0f;

    // 로더 클래스
    Season seasonLoader;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (GameManager.Inst == null)
        {
            Debug.LogError("GameManager가 아직 초기화되지 않았습니다!");
            return;
        }



        // Season 객체 생성 (이거 없으면 에러남!)
        seasonLoader = new Season();

        string background = GameManager.Inst.selectBackground;

        // 데이터를 불러와서 내 변수에 저장하기
        Season.SeasonData data = seasonLoader.LoadSeasonConfig(background);

        if (data != null)
        {
            stoneSpeed = data.stoneSpeed;
            stoneFriction = data.stoneFriction;
            stoneBounce = data.stoneBounce;

            Debug.Log($"[{background}] 데이터 로드 완료! 마찰력: {stoneFriction}, 속도계수: {stoneSpeed}");
            PhysicsMaterial2D mat = new PhysicsMaterial2D("StoneBounce");
            mat.bounciness = stoneBounce;
            mat.friction = 0.2f;
            rb.sharedMaterial = mat;
        }
        else
        {
            stoneFriction = 0.98f;
            stoneSpeed = 1.0f;
            Debug.LogError("데이터 로드 실패. 기본값으로 설정합니다.");
        }
        if (gameObject.CompareTag("Black"))
        {
            myTrun = TURN.BLACK;
        }
        else if (gameObject.CompareTag("White"))
        {
            myTrun = TURN.WHITE;
        }


    }


    private void OnMouseDown()
    {
        if (!GameManager.Inst.IsTurn(this.myTrun)) { startPos = nullvector; return; } 

        if (GameManager.Inst.isShot) return;
        startPos = Input.mousePosition;
    }

    private void OnMouseUp()
    {
        if (startPos == nullvector ) return;
        if (GameManager.Inst.isShot) return;

        endPos = Input.mousePosition;
        Vector2 dir = (startPos - endPos).normalized;

        // 드래그 파워 계산
        float dragDistance = (endPos - startPos).magnitude;
        float power = Mathf.Clamp(dragDistance * powerScale, 0, maxPower);

        if (power < mindistance * powerScale)
            power = mindistance * powerScale;
        else if (power > maxdistance * powerScale)
            power = maxdistance * powerScale;

        // (예: stoneSpeed가 1.2면 평소보다 1.2배 세게 나감)
        // stoneSpeed가 0이면 돌이 안 움직이니 기본값 처리 주의
        float finalPower = power * (stoneSpeed > 0 ? stoneSpeed : 1.0f);
        rb.AddForce(dir * finalPower, ForceMode2D.Impulse);
        GameManager.Inst.isShot = true;
        this.isMove = true;
        this.currentMoveTime = 0f;
    }

   private void FixedUpdate()
        {

        // 마찰력 적용 (기존 코드)
        if (stoneFriction > 0)
        {
            rb.velocity *= stoneFriction;
        }
        // "내가 발사된 상태이고(isShot), 속도가 거의 0이라면"
        if (isMove && GameManager.Inst.isShot)
        {
            // 발사 후 시간이 조금 흘러야 멈춤 체크 시작 (0.1초)
            currentMoveTime += Time.fixedDeltaTime;

            if (currentMoveTime > moveCheckDelay)
            {
                // 이제 속도가 0에 가까우면 멈춘 것으로 간주
                if (rb.velocity.sqrMagnitude < 0.05f)
                {
                    // 상태 초기화
                    this.isMove = false;
                    GameManager.Inst.isShot = false;
                    rb.velocity = Vector2.zero;

                    // 턴 종료 보고
                    GameManager.Inst.CheckTurnEnd();
                }
            }
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Dead")
        {

            isMove = false;
            GameManager.Inst.isShot = false;
            GameManager.Inst.CheckTurnEnd();
            Destroy(gameObject);
        }
    }
}