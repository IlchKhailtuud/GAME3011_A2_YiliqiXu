using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LockPickBehaviour : MonoBehaviour
{
    public Camera cam;
    public Transform innerLock;
    public Transform pickPosition;

    public float maxAngle = 90;
    public float lockSpeed = 10;
    
    private float lockRange;
    public float LockSpeed
    {
        get => lockSpeed;
        set => lockSpeed = value;
    }

    private float pickAngle;
    private float unlockAngle;
    private Vector2 unlockRange;

    private float keyPressTime = 0;

    private bool movePick = true;

    public TMP_Text timerText;
    
    private float timer = 60f;
    private float timeElapsed;
    
    void Start()
    {
        lockRange = GameManager.Instance.Difficulty;
        InitLock();
    }
    
    void Update()
    {
        if (timer > 0)
            CountDownTimer();
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene("lose");
        
        timerText.text = timer.ToString(); 
        
        transform.localPosition = pickPosition.position;

        if(movePick)
        {
            Vector3 dir = Input.mousePosition - cam.WorldToScreenPoint(transform.position);

            pickAngle = Vector3.Angle(dir, Vector3.up);

            Vector3 cross = Vector3.Cross(Vector3.up, dir);
            if (cross.z < 0) { pickAngle = -pickAngle; }

            pickAngle = Mathf.Clamp(pickAngle, -maxAngle, maxAngle);

            Quaternion pinRotation = Quaternion.AngleAxis(pickAngle, Vector3.forward);
            transform.rotation = pinRotation;
        }

        if(Input.GetKeyDown(KeyCode.D))
        {
            movePick = false;
            keyPressTime = 1;
        }
        if(Input.GetKeyUp(KeyCode.D))
        {
            movePick = true;
            keyPressTime = 0;
        }

        float percentage = Mathf.Round(100 - Mathf.Abs(((pickAngle - unlockAngle) / 100) * 100));
        float lockRotation = ((percentage / 100) * maxAngle) * keyPressTime;
        float maxRotation = (percentage / 100) * maxAngle;

        float lockLerp = Mathf.LerpAngle(innerLock.eulerAngles.z, lockRotation, Time.deltaTime * lockSpeed);
        innerLock.eulerAngles = new Vector3(0, 0, lockLerp);

        if(lockLerp >= maxRotation -1)
        {
            if (pickAngle < unlockRange.y && pickAngle > unlockRange.x)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("win");
                // Debug.Log("Unlocked!");
                // InitLock();

                //movePick = true;
                //keyPressTime = 0;
            }
            else
            {
                float randomRotation = Random.insideUnitCircle.x;
                transform.eulerAngles += new Vector3(0, 0, Random.Range(-randomRotation, randomRotation));
            }
        }
    }

    void InitLock()
    {
        unlockAngle = Random.Range(-maxAngle + lockRange, maxAngle - lockRange);
        unlockRange = new Vector2(unlockAngle - lockRange, unlockAngle + lockRange);
    }
    
    void CountDownTimer()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed >= 1)
        {
            timer--;
            timeElapsed = 0;
        }
    }
}
