using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

public static class AnalyticsManager
{
    public static int totalMessagesSent { get; private set; }
    public static int totalWordCount { get; private set; }
    public static int totalAppLaunches { get; private set; }


    public static float totalUsageTime { get; private set; }

    public static DateTime lastSessionDate { get; private set; }


    private static readonly string filePath =
       Path.Combine(UnityEngine.Application.persistentDataPath, "analytics.json");

    public static void RecordMessageSent(int numMessages)
    {
        totalMessagesSent += numMessages;
    }

    public static void RecordWordsCount(int numWords)
    {
        totalWordCount += numWords;
    }

    public static void RecordAppLaunches(int numAppLaunches)
    {
        totalAppLaunches += numAppLaunches;
    }
    
    public static void RecordUsageTime(float usageTime)
    {
        totalUsageTime += usageTime;
    }
    
    public static void RecordSessionDate(DateTime latestDateTime)
    {
        lastSessionDate = latestDateTime;
    }



    public static void Save()
    {
        JObject analyticsData = new JObject
        {
            ["totalMessagesSent"] = totalMessagesSent,
            ["totalWordCount"] = totalWordCount,
            ["totalAppLaunches"] = totalAppLaunches,
            ["totalUsageTime"] = totalUsageTime,
            ["lastSessionDate"] = lastSessionDate.ToString("o")
        };

        File.WriteAllText(filePath, analyticsData.ToString(Formatting.Indented));
    }

    public static void Load()
    {
        if (!File.Exists(filePath))
            return;

        JObject data = JObject.Parse(File.ReadAllText(filePath));

        totalMessagesSent = (int?)data["totalMessagesSent"] ?? 0;
        totalWordCount = (int?)data["totalWordCount"] ?? 0;
        totalAppLaunches = (int?)data["totalAppLaunches"] ?? 0;
        totalUsageTime = (float?)data["totalUsageTime"] ?? 0f;

        string dateString = (string?)data["lastSessionDate"];
        if (DateTime.TryParse(dateString, out DateTime parsedDate))
            lastSessionDate = parsedDate;
    }

    public static JObject GetAllData()
    {
        JObject analyticsData = new JObject
        {
            ["totalMessagesSent"] = totalMessagesSent,
            ["totalWordCount"] = totalWordCount,
            ["totalAppLaunches"] = totalAppLaunches,
            ["totalUsageTime"] = totalUsageTime,
            ["lastSessionDate"] = lastSessionDate.ToString("o")
        };

        return analyticsData;
    }



}

