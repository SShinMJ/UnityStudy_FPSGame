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
        // json ������ ������ �д´�.
        string path = Application.dataPath + "/stage.json";
        FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
        StreamReader streamReader = new StreamReader(fileStream);

        // ���� ������ ó������ ������ ��´�.
        string json = streamReader.ReadToEnd();

        // stream �ݱ�
        streamReader.Close();
        fileStream.Close();

        return json;
    }

    void ReadJson(string json)
    {
        // string �����͸� �ϳ��� Ŭ���� ��ü�� ����� ����.
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

        // monster1�� jSON �������� ������ش�.
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

        // string �����͸� MyClass Ŭ������ �°� Ŭ����ȭ �Ѵ�.
        MyClass myClass = JsonUtility.FromJson<MyClass>(json);
    }
}
