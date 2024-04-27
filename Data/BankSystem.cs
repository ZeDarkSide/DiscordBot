using System;
using System.Collections.Generic;
using System.IO;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class BankSystem
{
    private Dictionary<ulong, int> userPoints = new Dictionary<ulong, int>();
    private readonly string filePath;

    public BankSystem(string filePath)
    {
        this.filePath = filePath;
        LoadUserData();
    }

    private void LoadUserData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            userPoints = JsonConvert.DeserializeObject<Dictionary<ulong, int>>(json);
        }
    }

    public void SaveUserData()
    {
        string json = JsonConvert.SerializeObject(userPoints, Formatting.Indented);
        File.WriteAllText(filePath, json);
    }

    public int GetPoints(ulong userId)
    {
        if (userPoints.ContainsKey(userId))
            return userPoints[userId];
        else
            return 0; // Default to 0 if user not found
    }



}

