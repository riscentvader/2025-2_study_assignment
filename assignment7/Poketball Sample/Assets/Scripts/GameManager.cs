using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    UIManager MyUIManager;

    public GameObject BallPrefab;   // prefab of Ball

    // Constants for SetupBalls
    public static Vector3 StartPosition = new Vector3(0, 0, -6.35f);
    public static Quaternion StartRotation = Quaternion.Euler(0, 90, 90);
    const float BallRadius = 0.286f;
    const float RowSpacing = 0.02f;

    GameObject PlayerBall;
    GameObject CamObj;

    const float CamSpeed = 3f;

    const float MinPower = 15f;
    const float PowerCoef = 1f;

    void Awake()
    {
        // PlayerBall, CamObj, MyUIManager를 얻어온다.
        // ---------- TODO ---------- 
        PlayerBall = GameObject.Find("PlayerBall");
        CamObj = GameObject.Find("Main Camera");
        MyUIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        // -------------------- 
    }

    void Start()
    {
        SetupBalls();
    }

    // Update is called once per frame
    void Update()
    {
        // 좌클릭시 raycast하여 클릭 위치로 ShootBallTo 한다.
        // ---------- TODO ---------- 
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                ShootBallTo(hit.point);
            }
        }
        // -------------------- 
    }

    void LateUpdate()
    {
        CamMove();
    }

    void SetupBalls()
    {
        // 15개의 공을 삼각형 형태로 배치한다.
        // 가장 앞쪽 공의 위치는 StartPosition이며, 공의 Rotation은 StartRotation이다.
        // 각 공은 RowSpacing만큼의 간격을 가진다.
        // 각 공의 이름은 {index}이며, 아래 함수로 index에 맞는 Material을 적용시킨다.
        // Obj.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/ball_1");
        // ---------- TODO ---------- 
        int ballIndex = 1; 

        // 5개의 행 (row)
        for (int row = 0; row < 5; row++)
        {
            
            for (int col = 0; col <= row; col++)
            {
                if (ballIndex > 15) return; 

                
                float zOffset = -row * (2 * BallRadius + RowSpacing);

                
                float xOffset = (-row * BallRadius) + (col * 2 * BallRadius);

                Vector3 currentPos = StartPosition + new Vector3(xOffset, 0, zOffset);
                
                
                GameObject newBall = Instantiate(BallPrefab, currentPos, StartRotation);
                
                
                newBall.name = ballIndex.ToString();

                
                string materialPath = "Materials/ball_" + ballIndex;
                newBall.GetComponent<MeshRenderer>().material = Resources.Load<Material>(materialPath);
                
                ballIndex++;
            }
        }
        // -------------------- 
    }
    void CamMove()
    {
        // CamObj는 PlayerBall을 CamSpeed의 속도로 따라간다.
        // ---------- TODO ---------- 
        Vector3 targetPos = PlayerBall.transform.position;
        targetPos.y = CamObj.transform.position.y; 
        CamObj.transform.position = Vector3.Lerp(CamObj.transform.position, targetPos, CamSpeed * Time.deltaTime);
        // -------------------- 
    }

    float CalcPower(Vector3 displacement)
    {
        return MinPower + displacement.magnitude * PowerCoef;
    }

    void ShootBallTo(Vector3 targetPos)
    {
        // targetPos의 위치로 공을 발사한다.
        // 힘은 CalcPower 함수로 계산하고, y축 방향 힘은 0으로 한다.
        // ForceMode.Impulse를 사용한다.
        // ---------- TODO ---------- 
        Rigidbody rb = PlayerBall.GetComponent<Rigidbody>();
        if (rb == null) return; 
        Vector3 displacement = targetPos - PlayerBall.transform.position;
        displacement.y = 0; 
        Vector3 direction = displacement.normalized; 
        float power = CalcPower(displacement); 
        rb.AddForce(direction * power, ForceMode.Impulse);
        // -------------------- 
    }
    
    // When ball falls
    public void Fall(string ballName)
    {
        // "{ballName} falls"을 1초간 띄운다.
        // ---------- TODO ---------- 
        MyUIManager.DisplayText(ballName + " falls", 1f);
        // -------------------- 
    }
}
