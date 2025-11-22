using UnityEngine;

public class Controller : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 startPos;
    private Vector2 endPos;

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
            // 데이터를 못 찾았을 때 기본값 설정 (안전장치)
            stoneFriction = 0.98f;
            stoneSpeed = 1.0f;
            Debug.LogError("데이터 로드 실패. 기본값으로 설정합니다.");
        }
    }
    private void OnMouseDown()
    {

        // turn 만들예정 (지금은 그냥 막 던지기 가능)
        startPos = Input.mousePosition;
    }

    private void OnMouseUp()
    {
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
    }

        private void FixedUpdate()
        {

            if (stoneFriction > 0)
            {
                rb.velocity *= stoneFriction;
            }
        }
}