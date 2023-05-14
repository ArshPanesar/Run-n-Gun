using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BulletPresetContainer
{
    // Holds all the available Bullet Presets
    // Makes it easier for Game Objects to Access the Presets
    private static Dictionary<string, BulletPreset> Table = new Dictionary<string, BulletPreset>();

    static BulletPresetContainer()
    {
        Table.Clear();

        BulletPreset[] List = Resources.LoadAll<BulletPreset>("Presets/Bullets");
        
        for (int i = 0; i < List.Length; i++)
        {
            // Shorten Name for easy Identification
            string name = List[i].ToString();
            name = name.Substring(0, name.IndexOf(' '));

            Table.Add(name, List[i]);
        }
    }

    public static BulletPreset Get(string name)
    {
        if (Table.ContainsKey(name))
            return Table[name];
        return null;
    }
}
