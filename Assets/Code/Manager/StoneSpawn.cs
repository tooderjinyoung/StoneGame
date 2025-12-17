using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StoneData
{
    public string pattern;
    public float x;
    public float y;
}

public class StoneSpawn : MonoBehaviour
{
    [Header("Settings")]
    private Dictionary<string, string> pattern = new Dictionary<string, string>(); // 초기화 추가
    private Dictionary<string, Transform> parentDict;
    private Dictionary<string, GameObject> stonePrefab = new Dictionary<string, GameObject>();
    private float yPos = 1f;
    private float zPos = 0f;

    [SerializeField] private GameObject whiteStonePrefab;
    [SerializeField] private GameObject blackStonePrefab;
    [SerializeField] private Transform whiteParent;
    [SerializeField] private Transform blackParent;
    [SerializeField]
    private TextAsset csvData;
    string color_w = "White";
    string color_b = "Black";

    private void Start()
    {
        stonePrefab = new Dictionary<string, GameObject>
        {
            {color_w, whiteStonePrefab},
            {color_b, blackStonePrefab}
        };
        parentDict = new Dictionary<string, Transform>
        {
            {color_w, whiteParent},
            {color_b, blackParent}
        };

        foreach (string kvp in stonePrefab.Keys)
        {
            // 내 색상에 따라 yPos 방향 결정
            if (GameManager.Inst != null && kvp == GameManager.Inst.my_color) yPos = -1f;
            else if (GameManager.Inst != null && kvp != GameManager.Inst.my_color) yPos = 1f;
            else yPos = 1f; // GameManager가 없을 때 대비 기본값

            if (GameManager.Inst == null)
            {
                pattern[kvp] = "Line";
            }
            else
            {
                pattern = GameManager.Inst.arrangent;
            }

            // CSV 읽어서 스폰
            List<StoneData> stoneList = ReadCSV(pattern[kvp]);
            SpawnStones(stoneList, kvp, yPos);
        }
    }

    private List<StoneData> ReadCSV(string targetPattern)
    {
        List<StoneData> stoneList = new List<StoneData>();

        // Resources 폴더 안의 "stones" 파일을 로드 (.csv 확장자 생략)
        //TextAsset csvData = Resources.Load<TextAsset>("stones");

        if (csvData == null)
        {
            Debug.LogError("Resources 폴더에 'stones.csv' 파일이 없습니다!");
            return stoneList;
        }

        string[] lines = csvData.text.Replace("\r", "").Split('\n');

        bool firstLine = true;

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            if (firstLine)
            {
                firstLine = false;
                continue; // 헤더(첫 줄) 스킵
            }

            string[] splitData = line.Split(',');
            if (splitData.Length < 3) continue;

            // CSV의 첫 번째 열(패턴 이름)이 현재 찾는 패턴과 같은지 확인
            if (splitData[0].Trim() == targetPattern)
            {
                if (float.TryParse(splitData[1], out float x) &&
                    float.TryParse(splitData[2], out float y))
                {
                    stoneList.Add(new StoneData
                    {
                        pattern = splitData[0].Trim(),
                        x = x,
                        y = y * yPos // 위아래 반전 적용
                    });
                }
            }
        }

        // 보너스 돌 추가 로직
        if (GameManager.Inst != null && GameManager.Inst.selectBackground.Equals("Autumn"))
        {
            stoneList.Add(new StoneData
            {
                pattern = "Bounus",
                x = 0,
                y = 0.75f * yPos
            });
        }

        return stoneList;
    }

    private void SpawnStones(List<StoneData> stoneList, string color, float yPos)
    {
        if (stonePrefab == null) return;

        foreach (StoneData stone in stoneList)
        {
            Vector3 position = new Vector3(stone.x, stone.y, zPos);
            GameObject newStone = Instantiate(stonePrefab[color], position, Quaternion.identity, parentDict[color]);
            newStone.name = $"{stone.pattern}_({stone.x},{stone.y})";
        }

        if (GameManager.Inst != null)
        {
            GameManager.Inst.isSpawnFinished = true;
        }
    }
}