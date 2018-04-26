using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

/// <summary>
/// This gets attached to a grid object that will be populated with rows and columns based on a level file.
/// </summary>
[ExecuteInEditMode]
public class GridMaker : MonoBehaviour
{
    public string levelFile;
    public GameObject greenTile;
    public GameObject wallTile;

    public void MakeGrid()
    {
        char[,] levelContents = ReadLevelFile();

        int sizeX = levelContents.GetLength(0);
        int sizeY = levelContents.GetLength(1);

        for(int y = 0; y < sizeY; y++)
        {
            GameObject row = new GameObject("Row");
            row.transform.parent = transform;

            for(int x = 0; x < sizeX; x++)
            {
                Vector3 pos = new Vector3(x - (0.5f * sizeX) + transform.position.x, 
                    (-1.0f * y) + (0.5f * sizeY) + transform.position.y, transform.position.z); //Use less magic numbers later. Take scale into account?

                switch (levelContents[x, y])
                {
                    case '-':
                        break;
                    case 'W':
                        GameObject wall = GameObject.Instantiate(wallTile,
                            pos,
                            Quaternion.identity,
                            row.transform) as GameObject;
                        wall.name = "Wall";
                        break;
                    case 'G':
                        GameObject green = GameObject.Instantiate(greenTile,
                            pos,
                            Quaternion.identity,
                            row.transform) as GameObject;
                        green.name = "Green";
                        break;
                }
            }
        }
    }

    private char[,] ReadLevelFile()
    {
        char[,] output = null;

        try
        {
            string[] lines = File.ReadAllLines(levelFile);

            if (lines.Length > 0)
            {
                int sizeX = lines[0].Split(' ').Length;
                int sizeY = lines.Length;
                output = new char[sizeX, sizeY]; //X,Y;

                for(int y = 0; y < sizeY; y++)
                {
                    string[] objs = lines[y].Trim().Split(' ');
                    char[] charObjs = new char[objs.Length];
                    for(int i = 0; i < objs.Length; i++)
                    {
                        if (objs[i].Length == 1)
                        {
                            charObjs[i] = objs[i].ToCharArray()[0];
                        }
                        else
                        {
                            Debug.LogError("Improperly formatted level file!");
                        }
                    }

                    for(int x = 0; x < sizeX; x++)
                    {
                        output[x, y] = charObjs[x];
                    }
                }
            }
            else
            {
                Debug.LogWarning("Level file is empty!");
            }
        }
        catch (IOException e)
        {
            Debug.LogError("IOException; Level file couldn't be read!");
            throw e;
        }

        string dbg = "";
        for(int y = 0; y < output.GetLength(0); y++)
        {
            for(int x = 0; x < output.GetLength(1); x++)
            {
                dbg += output[x, y] + " ";
            }
            dbg += "\n";
        }
        Debug.Log(dbg);

        return output;
    }
}
