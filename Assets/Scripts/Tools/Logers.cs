using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logers : MonoBehaviour
{
    private System.IO.FileInfo fileLogPath;
    private bool isDebug = true;
    private Queue<string> queueLogs;
    private int maxCount = 3;
    private bool isShowDetail = false;
    void OnEnable() { Application.logMessageReceived += Log; }
    void OnDisable() { Application.logMessageReceived -= Log; }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        fileLogPath = new System.IO.FileInfo(Application.persistentDataPath + "/Logs/" + System.DateTime.Now.ToLongDateString() + "_Log.txt");
        Debug.Log(fileLogPath);
        queueLogs = new Queue<string>();

        if (!fileLogPath.Exists)
        {
            if (!System.IO.Directory.Exists(fileLogPath.DirectoryName))
            {
                System.IO.Directory.CreateDirectory(fileLogPath.DirectoryName);
            }
            fileLogPath.Create();
        }
    }

    public void Log(string logString, string stackTrace, LogType type)
    {
        string[] s = stackTrace.Split('\n');
        stackTrace = string.Empty;
        foreach (string line in s)
        {
            stackTrace += "      " + line + '\n';
        }
        lock (queueLogs)
        {
            queueLogs.Enqueue(string.Format("{0}-{1} : {2}\r\n{3}  {4}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), type, logString, stackTrace, "\n"));
            if (queueLogs.Count >= maxCount)
            {
                WriteLocal();
            }
        }
    }
    void WriteLocal()
    {
        try
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileLogPath.FullName, true))
            {
                while (queueLogs.Count > 0)
                {
                    file.Write(queueLogs.Dequeue());//直接追加文件末尾  
                }
            }
        }
        catch (System.Exception e)
        {
            throw (e);
        }
    }

    public string GetLogFile()
    {
        try
        {
            return System.IO.File.ReadAllText(fileLogPath.FullName);
        }
        catch (System.Exception e)
        {
            return "日志文件读取错误:" + e.Message;
        }


    }
    void OnGUI()
    {
        if (isDebug)
        {
            if (isShowDetail)
            {
                if (GUI.Button(new Rect(Screen.width - 50, 0, 50, 50), "Hide"))
                {
                    isShowDetail = false;
                }
                GUI.TextArea(new Rect(0, 50, Screen.width, Screen.height - 50), GetLogFile());
            }
            else
            {
                if (GUI.Button(new Rect(Screen.width - 50, Screen.height - 50, 50, 50), "Show"))
                {
                    isShowDetail = true;
                }
            }
        }
    }


    void OnDestroy()
    {
        WriteLocal();
    }
}

