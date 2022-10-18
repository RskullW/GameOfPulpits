using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class ResourcesLoad : MonoBehaviour
{
    private static Dictionary<string, Sprite> _sprites;
    private static bool IS_LOADED_SPRITES;
    
    void Awake()
    {
        IS_LOADED_SPRITES = false;
        LoadSprites();
    }

    public static void LoadSprites()
    {
        if (_sprites == null)
        {
            _sprites = new Dictionary<string, Sprite>();
        }
        
        _sprites["StartRus"] = Resources.Load<Sprite>("Sprites/rus/Buttons/start");
        _sprites["ContinueRus"] = Resources.Load<Sprite>("Sprites/rus/Buttons/continue");
        _sprites["SettingsRus"] = Resources.Load<Sprite>("Sprites/rus/Buttons/settings");
        _sprites["SetRus"] = Resources.Load<Sprite>("Sprites/rus/Buttons/set");
        _sprites["ExitRus"] = Resources.Load<Sprite>("Sprites/rus/Buttons/exit");
        _sprites["DeleteRus"] = Resources.Load<Sprite>("Sprites/rus/Buttons/delete");
        _sprites["BackRus"] = Resources.Load<Sprite>("Sprites/rus/Buttons/back");
        _sprites["LoadGameRus"] = Resources.Load<Sprite>("Sprites/rus/Buttons/load");
        _sprites["ExitMenuRus"] = Resources.Load<Sprite>("Sprites/rus/Buttons/exitMenu");
        
        _sprites["StartEng"] = Resources.Load<Sprite>("Sprites/eng/Buttons/start");
        _sprites["ContinueEng"] = Resources.Load<Sprite>("Sprites/eng/Buttons/continue");
        _sprites["SettingsEng"] = Resources.Load<Sprite>("Sprites/eng/Buttons/settings");
        _sprites["SetEng"] = Resources.Load<Sprite>("Sprites/eng/Buttons/set");
        _sprites["ExitEng"] = Resources.Load<Sprite>("Sprites/eng/Buttons/exit");
        _sprites["DeleteEng"] = Resources.Load<Sprite>("Sprites/eng/Buttons/delete");
        _sprites["BackEng"] = Resources.Load<Sprite>("Sprites/eng/Buttons/back");
        _sprites["LoadGameEng"] = Resources.Load<Sprite>("Sprites/eng/Buttons/load");
        _sprites["ExitMenuEng"] = Resources.Load<Sprite>("Sprites/eng/Buttons/exitMenu");

        IS_LOADED_SPRITES = true;
    }

    public static bool GetIsLoadedSprites()
    {
        return IS_LOADED_SPRITES;
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
