using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class stores info about every level. This info is either stored to playerprefs or read by game managers.
/// </summary>
public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [System.Serializable]
    public struct ParInfo
    {
        //Par values.
        public int parWindmills { get; private set; }
        public int parCannons { get; private set; }
        public int parPutters { get; private set; }

        //What the player actually used to complete the level. Initialized to -1.
        public int usedWindmills;
        public int usedCannons;
        public int usedPutters;

        public ParInfo(int parWindmills, int parPutters, int parCannons)
        {
            this.parWindmills = parWindmills;
            this.parPutters = parPutters;
            this.parCannons = parCannons;

            this.usedWindmills = -1;
            this.usedPutters = -1;
            this.usedCannons = -1;
        }

        public override string ToString()
        {
            string output = "";
            output += "Par";
            output += "\n\tWindmills: " + parWindmills;
            output += "\n\tPutters: " + parPutters;
            output += "\n\tCannons: " + parCannons;
            output += "\nPlayer Used";
            output += "\n\tWindmills: " + usedWindmills;
            output += "\n\tPutters: " + usedPutters;
            output += "\n\tCannons: " + usedCannons;
            return output;
        }
    }
    public Dictionary<string, ParInfo> LevelInfo { get; private set; } //Level names and their par info.

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            InitializeParInfo();
            DontDestroyOnLoad(instance.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Sets up initial par info. Also resets par info.
    /// </summary>
    public void InitializeParInfo()
    {
        Dictionary<string, ParInfo> newInfo = new Dictionary<string, ParInfo>();

        //SET UP INITIAL PAR INFO FOR ALL LEVELS HERE.
        newInfo.Add("Level_1", new ParInfo(0, 0, 0));
        newInfo.Add("Level_2", new ParInfo(1, 2, 0));
        newInfo.Add("Level_3", new ParInfo(4, 2, 0));
        newInfo.Add("Level_4", new ParInfo(0, 4, 1));
        newInfo.Add("Level_5", new ParInfo(3, 1, 1));
        newInfo.Add("Level_6", new ParInfo(1, 2, 1));
        newInfo.Add("Level_7", new ParInfo(0, 2, 1));

        LevelInfo = newInfo;
    }

    public ParInfo GetLevelInfo(string levelName)
    {
        return LevelInfo[levelName];
    }
}
