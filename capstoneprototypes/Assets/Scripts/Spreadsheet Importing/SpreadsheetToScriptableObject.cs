using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class SpreadsheetToScriptableObject
{
    private static string enemyDataCSVPath = "/Scripts/Spreadsheet Importing/CSVs/Enemies.csv", skillsCSVPath = "/Scripts/Spreadsheet Importing/CSVs/Player Skills.csv"; //String paths for CSV files
    private const string dataListPath = "Assets/Scripts/Spreadsheet Importing/DataList.asset"; //String path for the data list housing all created assets

    //Function that returns the data list from said path if found
    private static DataList GetDataList()
    {
        DataList dl = AssetDatabase.LoadAssetAtPath<DataList>(dataListPath);
        if (dl == null)
            Debug.LogError("DataList.asset not found!");
        return dl;
    }

    [MenuItem("Generation/Generate Skills")] //Ceates a new selection under the "Generation" tab in the unity editor to generate skills
    public static void GeneratePlayerSkills()
    {
        DataList dataList = GetDataList(); //Calls for data list refrence

        //Gets each line from the skills CSV file and seperates them into an array
        string[] allLines = File.ReadAllLines(Application.dataPath + skillsCSVPath);

        dataList.allPlayerSkills.Clear(); //Clears previous list of skills

        //Loop that skips the first line of the CSV file (since they are headers) and creates a new scriptable object per line in the CSV
        for (int i = 1; i < allLines.Length; i++)
        {
            //Splits the data in the line by the "," divider
            string[] splitData = allLines[i].Split(',');

            //Checks for the skills type
            string typeDisplay;
            if(int.Parse(splitData[4]) < 1)
            {
                typeDisplay = "Heal";
            }
            else
            {
                typeDisplay = "Damage";
            }

            //Creates instance of the skills scriptable object and sets the data from the CSV into the it using the split data
            PlayerSkill skill = ScriptableObject.CreateInstance<PlayerSkill>();
            skill.id = int.Parse(splitData[0]);
            skill.skillName = splitData[1];
            skill.cost = int.Parse(splitData[2]);
            skill.prepTime = float.Parse(splitData[3]);
            skill.target = (PlayerSkill.TargetType)int.Parse(splitData[4]);
            skill.intensity = int.Parse(splitData[5]);
            skill.perfectMultiplier = float.Parse(splitData[6]);
            skill.perfectable = bool.Parse(splitData[7]);

            skill.skillDescription = $"{skill.skillName}\n\n{typeDisplay}: {skill.intensity}\nTarget: {skill.target}\nPerfect Mult: {skill.perfectMultiplier}x";

            //Set skill file name
            string fileName = $"{skill.id} - {skill.skillName}";

            //Creates the skill scriptable object as a new assets inside of its respective folder
            AssetDatabase.CreateAsset(skill, $"Assets/Scripts/Spreadsheet Importing/Test/{fileName}.asset");
            dataList.allPlayerSkills.Add(skill); //Adds the current skill to the skill list in the data list
        }
        //Saves the assets
        EditorUtility.SetDirty(dataList); //Overwrites the data list
        AssetDatabase.SaveAssets();
    }
}
