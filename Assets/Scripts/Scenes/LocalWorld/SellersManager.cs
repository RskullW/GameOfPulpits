using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class SellersManager : MonoBehaviour
{
    public event Action OnPassItems;
    
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private UIDocument _uiDocument;

    [SerializeField] private Replics _dialogueSellerMedicines;
    [SerializeField] private Replics _dialogueSellerGuns;
    [SerializeField] private Replics _dialogueSellerItems;
    
    private Dictionary<string, Label> _dialoguesLabels;

    private bool _isDialogueWithSellerGuns;
    private bool _isDialogueWithSellerItems;
    private bool _isDialogueWithSellerMedicines;

    private Dictionary<string, VisualElement> _visualElements;
    private VisualElement _safeArea;

    private void Start()
    {
        _dialoguesLabels = new Dictionary<string, Label>();
        _visualElements = new Dictionary<string, VisualElement>();
        
        _isDialogueWithSellerGuns = _isDialogueWithSellerItems = 
            _isDialogueWithSellerMedicines = false;
        
        InitializeUIElements();
        InitializeEvents();
    }
    private void InitializeEvents()
    {
        _playerController.OnSellerGun += () => ProcessDialogsWithSellers(Sellers.Gun);
        _playerController.OnSellerMedic += () => ProcessDialogsWithSellers(Sellers.Medicine);
        _playerController.OnSellerItems += () => ProcessDialogsWithSellers(Sellers.Items);
        _playerController.OnSetMoney += SetMoneyLabel;
        
        _playerController.OnCloseDialogWithSellers += ClosePurchaseWindow;
    }
    private void InitializeUIElements()
    {
        InitializeUIMedic();
        InitializeUISellerItems();
        InitializeUISellerGuns();
        
    }

    private void InitializeUIMedic()
    {
        var root = _uiDocument.rootVisualElement;

        _visualElements["SellerMedicVisualElement"] = root.Q<VisualElement>("SellerMedic");
        _visualElements["DialogMedic"] = root.Q<VisualElement>("DialogueMedic");
            
        _dialoguesLabels["NameLeft-Triss"] = root.Q<Label>("NameTriss");
        _dialoguesLabels["Name-Triss"] = root.Q<Label>("NameLabelTriss");
        _dialoguesLabels["ReplicaLeft-Triss"] = root.Q<Label>("ReplicaTriss");
        _dialoguesLabels["NameRight-Triss"] = root.Q<Label>("NameUser-Triss");
        _dialoguesLabels["ReplicaRight-Triss"] = root.Q<Label>("ReplicaUser-Triss");
        _dialoguesLabels["Credits-Triss"] = root.Q<Label>("Credits-Triss");
        _dialoguesLabels["Choise1-Triss"] = root.Q<Label>("Choise1-Triss");
        _dialoguesLabels["Choise2-Triss"] = root.Q<Label>("Choise2-Triss");
        _dialoguesLabels["Choise3-Triss"] = root.Q<Label>("Choise3-Triss");
        
        Debug.Log(MenuManager.Language);
        if (MenuManager.Language == Language.Rus)
        {
            _dialoguesLabels["NameLeft-Triss"].text = _dialogueSellerMedicines.NameLeftRussian;
            _dialoguesLabels["Name-Triss"].text = _dialogueSellerMedicines.NameLeftRussian;
            _dialoguesLabels["NameRight-Triss"].text = _dialogueSellerMedicines.NameRightRussian;

            _dialoguesLabels["ReplicaLeft-Triss"].text = _dialogueSellerMedicines.RussianText[0];
            _dialoguesLabels["ReplicaRight-Triss"].text = _dialogueSellerMedicines.RussianText[1];

            _dialoguesLabels["Credits-Triss"].text = "Кредиты: ";

            _dialoguesLabels["Choise1-Triss"].text = _dialogueSellerMedicines.RussianChoise[0];
            _dialoguesLabels["Choise2-Triss"].text = _dialogueSellerMedicines.RussianChoise[1];
            _dialoguesLabels["Choise3-Triss"].text = _dialogueSellerMedicines.RussianChoise[2];
        }
        else
        {
            _dialoguesLabels["NameLeft-Triss"].text = _dialogueSellerMedicines.NameLeftEnglish;
            _dialoguesLabels["Name-Triss"].text = _dialogueSellerMedicines.NameLeftEnglish;
            _dialoguesLabels["NameRight-Triss"].text = _dialogueSellerMedicines.NameRightEnglish;

            _dialoguesLabels["ReplicaLeft-Triss"].text = _dialogueSellerMedicines.EnglishText[0];
            _dialoguesLabels["ReplicaRight-Triss"].text = _dialogueSellerMedicines.EnglishText[1];

            _dialoguesLabels["Credits-Triss"].text = "Credits: ";

            _dialoguesLabels["Choise1-Triss"].text = _dialogueSellerMedicines.EnglishChoise[0];
            _dialoguesLabels["Choise2-Triss"].text = _dialogueSellerMedicines.EnglishChoise[1];
            _dialoguesLabels["Choise3-Triss"].text = _dialogueSellerMedicines.EnglishChoise[2];
        }

        _dialoguesLabels["NameLeft-Triss"].text += ":";
        _dialoguesLabels["Credits-Triss"].text += _playerController.GetMoney();
        
        _visualElements["SellerMedicVisualElement"].style.opacity = 0f;
        _visualElements["DialogMedic"].style.opacity = 0f;
    }
    private void InitializeUISellerItems()
    {
        var root = _uiDocument.rootVisualElement;

        _visualElements["SellerItemsVisualElement"] = root.Q<VisualElement>("SellerItems");
        _visualElements["DialogItems"] = root.Q<VisualElement>("DialogueItems");
            
        _dialoguesLabels["NameLeft-Items"] = root.Q<Label>("NameItems");
        _dialoguesLabels["Name-Items"] = root.Q<Label>("NameLabelItems");
        _dialoguesLabels["ReplicaLeft-Items"] = root.Q<Label>("ReplicaItems");
        _dialoguesLabels["NameRight-Items"] = root.Q<Label>("NameUser-Items");
        _dialoguesLabels["ReplicaRight-Items"] = root.Q<Label>("ReplicaUser-Items");
        _dialoguesLabels["Credits-Items"] = root.Q<Label>("Credits-Items");
        _dialoguesLabels["Choise1-Items"] = root.Q<Label>("Choise1-Items");
        _dialoguesLabels["Choise2-Items"] = root.Q<Label>("Choise2-Items");

        if (MenuManager.Language == Language.Rus)
        {
            _dialoguesLabels["NameLeft-Items"].text = _dialogueSellerItems.NameLeftRussian;
            _dialoguesLabels["Name-Items"].text = _dialogueSellerItems.NameLeftRussian;
            _dialoguesLabels["NameRight-Items"].text = _dialogueSellerItems.NameRightRussian;

            _dialoguesLabels["ReplicaLeft-Items"].text = _dialogueSellerItems.RussianText[0];
            _dialoguesLabels["ReplicaRight-Items"].text = _dialogueSellerItems.RussianText[1];

            _dialoguesLabels["Credits-Items"].text = "Кредиты: ";

            _dialoguesLabels["Choise1-Items"].text = _dialogueSellerItems.RussianChoise[0];
            _dialoguesLabels["Choise2-Items"].text = _dialogueSellerItems.RussianChoise[1];
        }
        else
        {
            _dialoguesLabels["NameLeft-Items"].text = _dialogueSellerItems.NameLeftEnglish;
            _dialoguesLabels["Name-Items"].text = _dialogueSellerItems.NameLeftEnglish;
            _dialoguesLabels["NameRight-Items"].text = _dialogueSellerItems.NameRightEnglish;

            _dialoguesLabels["ReplicaLeft-Items"].text = _dialogueSellerItems.EnglishText[0];
            _dialoguesLabels["ReplicaRight-Items"].text = _dialogueSellerItems.EnglishText[1];

            _dialoguesLabels["Credits-Items"].text = "Credits: ";

            _dialoguesLabels["Choise1-Items"].text = _dialogueSellerItems.EnglishChoise[0];
            _dialoguesLabels["Choise2-Items"].text = _dialogueSellerItems.EnglishChoise[1];
        }

        _dialoguesLabels["NameLeft-Items"].text += ":";
        _dialoguesLabels["Credits-Items"].text += _playerController.GetMoney();
        
        _visualElements["SellerItemsVisualElement"].style.opacity = 0f;
        _visualElements["DialogItems"].style.opacity = 0f;
    }
    private void InitializeUISellerGuns()
    {
        var root = _uiDocument.rootVisualElement;

        _visualElements["SellerGunVisualElement"] = root.Q<VisualElement>("SellerGun");
        _visualElements["DialogGun"] = root.Q<VisualElement>("DialogueGun");
            
        _dialoguesLabels["NameLeft-Gun"] = root.Q<Label>("NameGun");
        _dialoguesLabels["Name-Gun"] = root.Q<Label>("NameLabelGun");
        _dialoguesLabels["ReplicaLeft-Gun"] = root.Q<Label>("ReplicaGun");
        _dialoguesLabels["NameRight-Gun"] = root.Q<Label>("NameUser-Gun");
        _dialoguesLabels["ReplicaRight-Gun"] = root.Q<Label>("ReplicaUser-Gun");
        _dialoguesLabels["Credits-Gun"] = root.Q<Label>("Credits-Gun");
        _dialoguesLabels["Choise1-Gun"] = root.Q<Label>("Choise1-Gun");
        _dialoguesLabels["Choise2-Gun"] = root.Q<Label>("Choise2-Gun");
        _dialoguesLabels["Choise3-Gun"] = root.Q<Label>("Choise3-Gun");
        
        if (MenuManager.Language == Language.Rus)
        {
            _dialoguesLabels["NameLeft-Gun"].text = _dialogueSellerGuns.NameLeftRussian;
            _dialoguesLabels["Name-Gun"].text = _dialogueSellerGuns.NameLeftRussian;
            _dialoguesLabels["NameRight-Gun"].text = _dialogueSellerGuns.NameRightRussian;

            _dialoguesLabels["ReplicaLeft-Gun"].text = _dialogueSellerGuns.RussianText[0];
            _dialoguesLabels["ReplicaRight-Gun"].text = _dialogueSellerGuns.RussianText[1];

            _dialoguesLabels["Credits-Gun"].text = "Кредиты: ";

            _dialoguesLabels["Choise1-Gun"].text = _dialogueSellerGuns.RussianChoise[0];
            _dialoguesLabels["Choise2-Gun"].text = _dialogueSellerGuns.RussianChoise[1];
            _dialoguesLabels["Choise3-Gun"].text = _dialogueSellerGuns.RussianChoise[2];
        }
        else
        {
            _dialoguesLabels["NameLeft-Gun"].text = _dialogueSellerGuns.NameLeftEnglish;
            _dialoguesLabels["Name-Gun"].text = _dialogueSellerGuns.NameLeftEnglish;
            _dialoguesLabels["NameRight-Gun"].text = _dialogueSellerGuns.NameRightEnglish;

            _dialoguesLabels["ReplicaLeft-Gun"].text = _dialogueSellerGuns.EnglishText[0];
            _dialoguesLabels["ReplicaRight-Gun"].text = _dialogueSellerGuns.EnglishText[1];

            _dialoguesLabels["Credits-Gun"].text = "Credits: ";

            _dialoguesLabels["Choise1-Gun"].text = _dialogueSellerGuns.EnglishChoise[0];
            _dialoguesLabels["Choise2-Gun"].text = _dialogueSellerGuns.EnglishChoise[1];
            _dialoguesLabels["Choise3-Gun"].text = _dialogueSellerGuns.EnglishChoise[2];
        }

        _dialoguesLabels["NameLeft-Gun"].text += ":";
        _dialoguesLabels["Credits-Gun"].text += _playerController.GetMoney();
        
        _visualElements["SellerGunVisualElement"].style.opacity = 0f;
        _visualElements["DialogGun"].style.opacity = 0f;
    }
    private void ProcessDialogsWithSellers(Sellers seller)
    {

        _safeArea.style.opacity = 0f;
        _playerController.SetActiveDialogue(true);

        SetMoneyLabel();
        OnPassItems?.Invoke();

        switch (seller)
        {
            case Sellers.Gun:
                AudioManager.Instance.PlaySound("HelloGun");
                _isDialogueWithSellerGuns = true;
                ShowSellerGuns();
                break;
            case Sellers.Medicine:
                AudioManager.Instance.PlaySound("HelloTriss");
                _isDialogueWithSellerMedicines = true; 
                ShowSellerMedicine(); 
                break;
            case Sellers.Items:
                AudioManager.Instance.PlaySound("HelloItems");
                _isDialogueWithSellerItems = true; 
                ShowSellerItems(); 
                break;
            
            default: break;
        }


    }
    private void ShowSellerItems()
    {
        _visualElements["SellerItemsVisualElement"].style.opacity = 1f;
        _visualElements["DialogItems"].style.opacity = 1f;
        
        AudioManager.Instance.PlaySound("HelloItems");
        Debug.Log("Show seller items");
    }
    private void ShowSellerMedicine()
    {
        _visualElements["SellerMedicVisualElement"].style.opacity = 1f;
        _visualElements["DialogMedic"].style.opacity = 1f;
        
        AudioManager.Instance.PlaySound("HelloTriss");
        Debug.Log("Show seller medicine");
    }
    private void ShowSellerGuns()
    {
        _visualElements["SellerGunVisualElement"].style.opacity = 1f;
        _visualElements["DialogGun"].style.opacity = 1f;
        
         AudioManager.Instance.PlaySound("HelloGun");
        Debug.Log("Show seller gun");
    }
    
    private void ClosePurchaseWindow()
    {
        _safeArea.style.opacity = 1f;


        _visualElements["SellerMedicVisualElement"].style.opacity = 0f;
        _visualElements["DialogMedic"].style.opacity = 0f;
        
        _visualElements["SellerGunVisualElement"].style.opacity = 0f;
        _visualElements["DialogGun"].style.opacity = 0f;
        
        _visualElements["SellerItemsVisualElement"].style.opacity = 0f;
        _visualElements["DialogItems"].style.opacity = 0f;
        
        
        _isDialogueWithSellerGuns = _isDialogueWithSellerItems = _isDialogueWithSellerMedicines = false;
        _playerController.SetActiveDialogue(false);
    }
    public void SetSafeArea(VisualElement visualElement)
    {
        _safeArea = visualElement;
    }

    private void SetMoneyLabel()
    {
        if (MenuManager.Language == Language.Rus)
        {
            _dialoguesLabels["Credits-Gun"].text = "Кредиты: ";
            _dialoguesLabels["Credits-Triss"].text = "Кредиты: ";
            _dialoguesLabels["Credits-Items"].text = "Кредиты: ";
        }

        else
        {
            _dialoguesLabels["Credits-Gun"].text = "Credits: ";
            _dialoguesLabels["Credits-Triss"].text = "Credits: ";
            _dialoguesLabels["Credits-Items"].text = "Credits: ";
        }
        
        _dialoguesLabels["Credits-Gun"].text += _playerController.GetMoney();
        _dialoguesLabels["Credits-Triss"].text += _playerController.GetMoney();
        _dialoguesLabels["Credits-Items"].text += _playerController.GetMoney();

    }
}
