using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Season : MonoBehaviour
{
    // 아까 정의했던 데이터 클래스 (일반 클래스)
    public class SeasonData
    {
        public int stoneCount;
        public float stoneSpeed;
        public float stoneFriction;
        public float stoneBounce;
    }
    // CSV를 읽어서 특정 계절(pattern)의 데이터를 가져오는 함수
    public SeasonData LoadSeasonConfig(string seasonName)
    {
        // 파일 경로 (주의: 빌드 시 경로 문제는 아래 설명 참고)
        string path = Path.Combine(Application.dataPath, "seasons.csv");

        if (!File.Exists(path))
        {
            Debug.LogError($"파일 없음: {path}");
            return null;
        }

        using (StreamReader reader = new StreamReader(path))
        {
            bool firstLine = true;
            while (!reader.EndOfStream)
            {
                string data = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(data)) continue;

                // 첫 줄(헤더) 스킵
                if (firstLine) { firstLine = false; continue; }

                string[] splitData = data.Split(',');

                // 데이터 개수가 맞는지 확인 (이름, 수량, 속도, 마찰, 튕김 -> 5개)
                if (splitData.Length < 5) continue;

                // 첫 번째 칸(SeasonName)이 내가 찾는 계절인지 확인
                if (splitData[0].Trim() == seasonName)
                {
                    SeasonData newSeason = new SeasonData();

                    // 문자열을 숫자로 변환 (Parse)
                    int.TryParse(splitData[1], out newSeason.stoneCount);
                    float.TryParse(splitData[2], out newSeason.stoneSpeed);
                    float.TryParse(splitData[3], out newSeason.stoneFriction);
                    float.TryParse(splitData[4], out newSeason.stoneBounce);

                    return newSeason; // 찾았으면 바로 리턴
                }
            }
        }

        Debug.LogError($"{seasonName} 계절 데이터를 CSV에서 못 찾았습니다.");
        return null;
    }
}
