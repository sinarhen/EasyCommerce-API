﻿namespace Lib;

public static class Env
{
    public static void LoadFile(string path)
    {
        if (!File.Exists(path)) return;

        foreach (var line in File.ReadAllLines(path))
        {
            var separatorIndex = line.IndexOf("=");

            var key = line.Substring(0, separatorIndex).Trim();
            var value = line.Substring(separatorIndex + 1).Trim();

            Environment.SetEnvironmentVariable(key, value);
        }
    }

    public static string GetRequired(string key)
    {
        return Environment.GetEnvironmentVariable(key)
               ?? throw new KeyNotFoundException($"Environment variable: {key} wasn't found!");
    }

    public static string? GetOptional(string key)
    {
        return Environment.GetEnvironmentVariable(key);
    }
}