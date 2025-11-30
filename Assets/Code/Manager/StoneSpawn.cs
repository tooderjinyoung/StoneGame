using System.Collections.Generic;
using System.Drawing;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

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

    private Dictionary<string, string> pattern; // 불러올 패턴 이름
    private Dictionary<string, Transform> parentDict;
    private Dictionary<string, GameObject> stonePrefab = new Dictionary<string, GameObject>();
    private float yPos = 1f;
    private float zPos = 0f;         // 기본 z위치 (2D면 0, 3D면 조정)

    [SerializeField] private GameObject whiteStonePrefab;
    [SerializeField] private GameObject blackStonePrefab;
    [SerializeField] private Transform whiteParent;
    [SerializeField] private Transform blackParent;

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
             if (kvp == GameManager.Inst.my_color) yPos = -1f;
             else if (kvp != GameManager.Inst.my_color) yPos = 1f;

            if (GameManager.Inst== null)
            {
                pattern[kvp] = "Line";
            }
            else { pattern = GameManager.Inst.arrangent; }

            List<StoneData> stoneList = ReadCSV(pattern[kvp]);
            SpawnStones(stoneList, kvp,yPos);

        }

    }

    //  pattern별 CSV 데이터를 반환하는 함수
    private List<StoneData> ReadCSV(string pattern)
    {
        List<StoneData> stoneList = new List<StoneData>();
        string path = Path.Combine(Application.dataPath, "stones.csv");

        if (!File.Exists(path))
        {
            Debug.LogError($"CSV file not found at {path}");
            return stoneList;
        }

        using (StreamReader reader = new StreamReader(path))
        {
            bool firstLine = true;
            while (!reader.EndOfStream)
            {
                string data = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(data)) continue;

                if (firstLine)
                {
                    firstLine = false;
                    continue; // 헤더 스킵
                }

                string[] splitData = data.Split(',');
                if (splitData.Length < 3) continue;

                if (splitData[0].Trim() == pattern)
                {
                    if (float.TryParse(splitData[1], out float x) &&
                        float.TryParse(splitData[2], out float y))
                    {
                        stoneList.Add(new StoneData
                        {
                            pattern = splitData[0].Trim(),
                            x = x,
                            y = y* yPos
                        });
                    }
                }
            }
        }
        if (GameManager.Inst.selectBackground.Equals("Autumn")) stoneList.Add(new StoneData {
            pattern = "Bounus"
            , x = 0,
            y = 0.75f * yPos});


        Debug.Log($"Loaded {stoneList.Count} stones for pattern: {pattern}");
        return stoneList;
    }

    //  리스트를 매개변수로 받아서 복제하는 함수
    private void SpawnStones(List<StoneData> stoneList,string color,float yPos)
    {
        if (stonePrefab == null)
        {
            return;
        }

        foreach (StoneData stone in stoneList)
        {
            Vector3 position = new Vector3(stone.x, stone.y, zPos);
            Debug.Log(stonePrefab[color]);
            GameObject newStone = Instantiate(stonePrefab[color], position, Quaternion.identity, parentDict[color]);
            newStone.name = $"{stone.pattern}_({stone.x},{stone.y})";
        }
        Debug.Log($" Spawned {stoneList.Count} stones successfully!");
    }
}
