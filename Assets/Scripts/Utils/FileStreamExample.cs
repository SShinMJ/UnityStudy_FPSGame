using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileStreamExample : MonoBehaviour
{
    FileStream filestream;

    void Start()
    {
        // Application.dataPath : 프로젝트의 Assest 폴더를 가리킨다.
        string path = Application.dataPath + "/fileStream.txt";
        filestream = new FileStream(path, FileMode.Create);

        WriteStream();
        ReadStream();

        filestream.Close();
    }
    void WriteStream()
    {
        StreamWriter streamWriter = new StreamWriter(filestream);

        streamWriter.WriteLine("streamWriter 입력하기");

        streamWriter.Close();
    }

    void ReadStream()
    {
        StreamReader streamReader = new StreamReader(filestream);

        string line = streamReader.ReadLine();
        Debug.Log(line);

        streamReader.Close();
    }
}
