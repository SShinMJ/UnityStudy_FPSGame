using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static JsonExample;

public class JsonExample : MonoBehaviour
{
    [Serializable]
    public class Monster
    {
        public string name;
        public int hp;
        public int attackPower;
        public int speed;
    }

    [Serializable]
    public class StageData
    {
        public int stageNumber;
        public List<Monster> monsters;
    }

    [Serializable]
    public class TotalStageData
    {
        public List<StageData> stages;
    }

    TotalStageData totalStageData;

    public string StageDataReader()
    {
        // json 파일을 가져와 읽는다.
        string path = Application.dataPath + "/stage.json";
        FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
        StreamReader streamReader = new StreamReader(fileStream);

        // 파일 내용을 처음부터 끝까지 담는다.
        string json = streamReader.ReadToEnd();

        // stream 닫기
        streamReader.Close();
        fileStream.Close();

        return json;
    }

    void ReadJson(string json)
    {
        // string 데이터를 하나의 클래스 객체로 만들어 저장.
        totalStageData = JsonUtility.FromJson<TotalStageData>(json);

        foreach(var stage in totalStageData.stages)
        {
            print("[Stage " + stage.stageNumber + "]\n");
            foreach(var monster in stage.monsters)
            {
                print($"Monster name: {monster.name}, hp: {monster.hp}, attackPower: {monster.attackPower}, speed: {monster.speed}");
            }
        }
    }

    [Serializable]
    public class MyClass
    {
        public int level;
        public float timeElapsed;
        public string playerName;
    }

    void Start()
    {
        Monster monster1 = new Monster();
        monster1.name = "Hulk";
        monster1.hp  = 100;
        monster1.attackPower = 1000;
        monster1.speed = 50;

        // monster1을 jSON 형식으로 만들어준다.
        string json = JsonUtility.ToJson(monster1);
        //string newtonJson = JsonConvert.SerializeObject(json);

        //JObject keyValuePairs = JObject.Parse(newtonJson);

        //JObject stageHead = (JObject)keyValuePairs["stages"];
        //if (stageHead.ContainsKey("monsters"))
        //{
        //    print(stageHead["monsters"]["name"].ToString());
        //}
        // WriteJson();

        ReadJson(StageDataReader());
    }

    void WriteJson()
    {
        string json = "{\r\n \"level\": 11,\r\n \"timeElapsed\": 30.55,\r\n \"playerName > \": \"New York\"\r\n}";

        // string 데이터를 MyClass 클래스에 맞게 클래스화 한다.
        MyClass myClass = JsonUtility.FromJson<MyClass>(json);
    }
}
