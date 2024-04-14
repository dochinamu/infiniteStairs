using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public bool Game_Start = false; // 게임 시작 체크

    public float Current_Time = 0.0f; // 현재 남은 시간
    public float Destination_Time = 10.0f; // 전체 시간(10초)
    public Slider Slider; // 시간 타이머
    public float Add_Time_Flow = 0.0005f; // 감소 시간

    public Text Text; // 점수 텍스트

    public int Score = 0; // 점수

    public Text GameOverText;


    public GameObject Character; // 캐릭터
    public Transform Platform_Parents; // 정리를 위한 발판들의 부모 오브젝트
    public GameObject Platform; // 계단
    private List<GameObject> Platform_List = new List<GameObject>(); // 발판 리스트
    private List<int> Platform_Check_List = new List<int>(); // 발판의 위치 리스트 (왼쪽: 0, 오른쪽: 1)


    //땅에 닫기까지 걸리는 시간
    protected float timer;
    protected float timeToFloor;


    // Start is called before the first frame update
    void Start()
    {
        Data_Load();
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Game_Start) { // 키보드 입력 체크
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Character.transform.rotation = Quaternion.Euler(0, -180, 0); // 캐릭터 우로 굴러
                Check_Platform(Character_Pos_Idx, 1);
            } else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Character.transform.rotation = Quaternion.Euler(0, 0, 0); // 캐릭터 좌로 굴러
                Check_Platform(Character_Pos_Idx, 0);
            }
            Destination_Time = Destination_Time - Add_Time_Flow;
            Current_Time = Current_Time - Time.deltaTime; // 프레임 사이 시간이 일정하지 않기 때문에, delta time씩 더해주어 어떤 환경에서든 일정한 시간씩 흐르도록 함

            Slider.value = Current_Time / Destination_Time;
            
            if (Current_Time < 0f)
            {
                GameOverText.text = "Time out!";
                Result();
            }
        } else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Init();
            }
        }
    }

    public void Data_Load() // 데이터 로드, 발판 오브젝트 생성
    {
        for(int i=0; i < 20; i++)
        {
            GameObject t_obj = Instantiate(Platform, Vector3.zero, Quaternion.identity); //복사할 오브젝트, 복제된 오브젝트의 위치, 복제된 프로젝트의 방향(부모 좌표축)
            t_obj.transform.parent = Platform_Parents;
            Platform_List.Add(t_obj);
            Platform_Check_List.Add(0);
        }

        Platform.SetActive(false);
    }


    private int Pos_Idx = 0; // 발판의 마지막 위치
    private int Character_Pos_Idx = 0; // 캐릭터 위치


    public void Init() // 캐릭터 위치, 발판 위치 초기화
    {
        GameOverText.enabled = false;
        Character.transform.position = Vector3.zero; 

        for (Pos_Idx = 0; Pos_Idx < 20;)
        {
            Next_Platform(Pos_Idx);
        }

        Destination_Time = 10.0f;
        Current_Time = Destination_Time;

        Character_Pos_Idx = 0;
        Score = 0;
        Text.text = Score.ToString();



        Game_Start = true;
    }

    public void Next_Platform(int idx)
    {
        int pos = Random.Range(0, 2); //0 or 1
        if (idx == 0) // 첫번째 발판이라면
        {
            Platform_List[idx].transform.position = new Vector3(0, -0.5f, 0);
        }else
        {
            if (Pos_Idx < 20)
            {
                if (pos == 0) // 왼쪽 발판이라면
                {
                    Platform_Check_List[idx - 1] = pos;
                    Platform_List[idx].transform.position = Platform_List[idx - 1].transform.position + new Vector3(-1f, 0.5f, 0);
                }
                else // 오른쪽 발판이라면
                {
                    Platform_Check_List[idx - 1] = pos;
                    Platform_List[idx].transform.position = Platform_List[idx - 1].transform.position + new Vector3(1f, 0.5f, 0);
                }
            } else
            {
                if (pos == 0) // 왼쪽 발판이라면
                {
                    if (idx % 20 == 0)
                    {
                        Platform_Check_List[20 - 1] = pos;
                        Platform_List[idx % 20].transform.position = Platform_List[20 - 1].transform.position + new Vector3(-1f, 0.5f, 0);
                    } else
                    {
                        Platform_Check_List[idx % 20 - 1] = pos;
                        Platform_List[idx % 20].transform.position = Platform_List[idx % 20 - 1].transform.position + new Vector3(-1f, 0.5f, 0);
                    }
                }
                else // 오른쪽 발판이라면
                {
                    if (idx % 20 == 0)
                    {
                        Platform_Check_List[20 - 1] = pos;
                        Platform_List[idx % 20].transform.position = Platform_List[20 - 1].transform.position + new Vector3(1f, 0.5f, 0);
                    }
                    else
                    {
                        Platform_Check_List[idx % 20 - 1] = pos;
                        Platform_List[idx % 20].transform.position = Platform_List[idx % 20 - 1].transform.position + new Vector3(1f, 0.5f, 0);
                    }
                }
            }
            
        }
        Score++;
        Text.text = Score.ToString();
        Pos_Idx++;
    }

    void Check_Platform(int idx, int direction)
    {
        Debug.Log("idx: " + idx % 20 + " /Platform: " + Platform_Check_List[idx % 20] + " /Direction: " + direction);
        if (Platform_Check_List[idx % 20] == direction) // 발판이 있음
        {
            Character_Pos_Idx++;
            //Character.transform.position = Platform_List[Character_Pos_Idx % 20].transform.position + new Vector3(0f, 0.5f, 0); // 발판 위에 올라가도록 캐릭터 위치 이동
            Move(Character_Pos_Idx);
            Next_Platform(Pos_Idx);
        } else
        {
            ShowEndSentence();
            Result();
        }
    }



    void Move(int Character_Pos_Idx)
    {
        Vector3 firstPos = Character.transform.position;
        Vector3 secondPos = (firstPos + Platform_List[Character_Pos_Idx % 20].transform.position)/2f + new Vector3(0, 2f, 0);
        Vector3 thirdPos = Platform_List[Character_Pos_Idx % 20].transform.position + new Vector3(0, 0.5f, 0);

        // 후보1: 점프 무빙
        Character.transform.DOJump(thirdPos, 0.3f, 1, 0.1f);
        // 성공적으로 움직였을 시, 0.001f초 보너스 시간 부여
        if (Destination_Time - Current_Time > Time.deltaTime*30)
        {
            Current_Time = Current_Time + Time.deltaTime*30;
        }
        else
        {
            Current_Time = Destination_Time;
        }
        //후보 2: 포물선이 꺾이고 꺾이는 롤러코스터 경로
        //Character.transform.DOPath(new[] { secondPos, firstPos + Vector3.up, secondPos + Vector3.left * 2, thirdPos, secondPos + Vector3.right * 2, thirdPos + Vector3.up }, 1f, PathType.CubicBezier).SetEase(Ease.Unset);



    }



    public void Result()
    {
        ShowEndSentence();
        Debug.Log("Game Over");
        Game_Start = false;
    }

    public void ShowEndSentence()
    {
        GameOverText.enabled = true;
    }
}
