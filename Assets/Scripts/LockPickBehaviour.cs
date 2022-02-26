using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockPickBehaviour : MonoBehaviour
{
    [SerializeField]
    private Camera cam;
    
    [SerializeField]
    private Transform innerLock;
    
    [SerializeField]
    private Transform pickPos;
    
    private float maxAngle = 90f;
    
    private float lockSpeed = 10f;
    
    public float lockRange = 10;

    private float eulerAngle;
    private float unlockAngle;
    private Vector2 unlockRange;

    private float keyPressTime;
    private bool movePick = true;
    
    void Start()
    {
        InitLock();
    }
    
    void Update()
    {
        transform.localPosition = pickPos.position;

        if (movePick)
        {
            Vector3 dir = Input.mousePosition - cam.ViewportToScreenPoint(transform.position);
            eulerAngle = Vector3.Angle(dir, Vector3.up);

            Vector3 cross = Vector3.Cross(Vector3.up, dir);
            if (cross.z < 0)
                eulerAngle = -eulerAngle;

            eulerAngle = Mathf.Clamp(eulerAngle, -maxAngle, maxAngle);

            Quaternion rotateTo = Quaternion.AngleAxis(eulerAngle, Vector3.forward);
            transform.rotation = rotateTo;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            movePick = false;
            keyPressTime = 1;
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            movePick = true;
            keyPressTime = 0;
        }

        float pct = Mathf.Round(100 - Mathf.Abs((100 - (eulerAngle / -unlockAngle) / 100) * 100));
        float lockRotation = ((pct / 100) * maxAngle) * keyPressTime;
        float maxRotation = (pct / 100) * maxAngle;

        float lockLerp = Mathf.Lerp(innerLock.eulerAngles.z, lockRotation, Time.deltaTime * lockSpeed);
        innerLock.eulerAngles = new Vector3(0, 0, lockLerp);

        if (lockLerp >= maxRotation - 1)
        {
            if (eulerAngle < unlockRange.y && eulerAngle > unlockRange.x)
            {
                Debug.Log("unlocked");
            }
            else
            {
                float lockShake = Random.insideUnitCircle.x;
                transform.eulerAngles += new Vector3(0, 0, Random.Range(-lockShake, lockShake));
            }
        }
    }

    void InitLock()
    {
        unlockAngle = Random.Range(-maxAngle + lockRange, maxAngle - lockRange);
        unlockRange = new Vector2(unlockAngle - lockRange, unlockAngle + lockRange);
    }
}
