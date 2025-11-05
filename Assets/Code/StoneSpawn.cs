using System.Collections.Generic;
using System.IO;
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
    [SerializeField] private string pattern = "Line"; // 불러올 패턴 이름
    [SerializeField] private GameObject stonePrefab;
    private float yPos = 1f;
    private float zPos = 0f;         // 기본 z위치 (2D면 0, 3D면 조정)

    private void Start()
    {
        if(stonePrefab.tag !="White") yPos = -1f;

        List<StoneData> stoneList = ReadCSV(pattern);
        SpawnStones(stoneList);
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

        Debug.Log($"Loaded {stoneList.Count} stones for pattern: {pattern}");
        return stoneList;
    }

    //  리스트를 매개변수로 받아서 복제하는 함수
    private void SpawnStones(List<StoneData> stoneList)
    {
        if (stonePrefab == null)
        {
            Debug.LogError("❌ Stone prefab not assigned in Inspector!");
            return;
        }

        foreach (StoneData stone in stoneList)
        {
            Vector3 position = new Vector3(stone.x, stone.y, zPos);
            GameObject newStone = Instantiate(stonePrefab, position, Quaternion.identity);
            newStone.name = $"{stone.pattern}_({stone.x},{stone.y})";
        }

        Debug.Log($" Spawned {stoneList.Count} stones successfully!");
    }
}
