using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class ResourcesLoad : MonoBehaviour
{
    private static Dictionary<string, Sprite> _sprites;
    
    void Awake()
    {
        _sprites = new Dictionary<string, Sprite>();
        
        LoadSprites();
    }

    private void LoadSprites()
    {
        _sprites["StartRus"] = Resources.Load<Sprite>("Sprites/rus/Buttons/start");
        _sprites["ContinueRus"] = Resources.Load<Sprite>("Sprites/rus/Buttons/continue");
        _sprites["SettingsRus"] = Resources.Load<Sprite>("Sprites/rus/Buttons/settings");
        _sprites["SetRus"] = Resources.Load<Sprite>("Sprites/rus/Buttons/set");
        _sprites["ExitRus"] = Resources.Load<Sprite>("Sprites/rus/Buttons/exit");
        _sprites["BackRus"] = Resources.Load<Sprite>("Sprites/rus/Buttons/back");
        
        _sprites["StartEng"] = Resources.Load<Sprite>("Sprites/eng/Buttons/start");
        _sprites["ContinueEng"] = Resources.Load<Sprite>("Sprites/eng/Buttons/continue");
        _sprites["SettingsEng"] = Resources.Load<Sprite>("Sprites/eng/Buttons/settings");
        _sprites["SetEng"] = Resources.Load<Sprite>("Sprites/eng/Buttons/set");
        _sprites["ExitEng"] = Resources.Load<Sprite>("Sprites/eng/Buttons/exit");
        _sprites["BackEng"] = Resources.Load<Sprite>("Sprites/eng/Buttons/back");
    }

    public static Sprite GetSprite(string index)
    {
        if (!_sprites.ContainsKey(index))
        {
            Debug.LogError("Error in ResourcesLoad.GetSprite(index). Index = " + index);
            return null;
        } 
        
        return _sprites[index];
    }
}
