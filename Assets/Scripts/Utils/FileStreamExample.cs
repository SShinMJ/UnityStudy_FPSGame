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
        // Application.dataPath : ������Ʈ�� Assest ������ ����Ų��.
        string path = Application.dataPath + "/fileStream.txt";
        filestream = new FileStream(path, FileMode.Create);

        WriteStream();
        ReadStream();

        filestream.Close();
    }
    void WriteStream()
    {
        StreamWriter streamWriter = new StreamWriter(filestream);

        streamWriter.WriteLine("streamWriter �Է��ϱ�");

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
