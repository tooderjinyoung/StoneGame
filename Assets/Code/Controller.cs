using UnityEngine;

public class Controller : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 startPos;
    private Vector2 endPos;
    [SerializeField] 
    float powerScale = 0.1f;
    float maxPower = 10f;
    float mindistance = 5f;
    float maxdistance = 100f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnMouseDown()
    {
        startPos = Input.mousePosition;
    }

    private void OnMouseUp()
    {
        endPos = Input.mousePosition;
        Vector2 dir = (startPos - endPos).normalized;
        float power = Mathf.Clamp((endPos - startPos).magnitude * powerScale, 0, maxPower);
        if(power < mindistance * powerScale)
            power = mindistance * powerScale;
        else if(power > maxdistance * powerScale)
            power = maxdistance * powerScale;
        rb.AddForce(dir * power, ForceMode2D.Impulse);
    }
    private void FixedUpdate()
    {
        rb.velocity *= 0.98f;
    }
}
