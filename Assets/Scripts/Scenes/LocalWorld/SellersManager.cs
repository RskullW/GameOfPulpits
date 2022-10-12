using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class SellersManager : MonoBehaviour
{
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
        
        MenuManager.SetLanguage(Language.Rus);

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

        _visualElements["SellerMedicVisualElement"].style.opacity = 0f;
        _visualElements["DialogMedic"].style.opacity = 0f;
    }

    private void InitializeUISellerItems()
    {
        
    }
    private void InitializeUISellerGuns()
    {
        
    }
    private void ProcessDialogsWithSellers(Sellers seller)
    {

        _safeArea.style.opacity = 0f;
        _playerController.SetActiveDialogue(true);
        
        switch (seller)
        {
            case Sellers.Gun:
                _isDialogueWithSellerGuns = true;
                ShowSellerGuns();
                break;
            case Sellers.Medicine:
                // AudioManager.Instance.PlaySound("HelloTriss");
                _isDialogueWithSellerMedicines = true; 
                ShowSellerMedicine(); 
                break;
            case Sellers.Items:
                _isDialogueWithSellerItems = true; 
                ShowSellerItems(); 
                break;
            
            default: break;
        }


    }
    private void ShowSellerItems()
    {
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
        Debug.Log("Show seller gun");   
    }
    
    private void ClosePurchaseWindow()
    {
        _safeArea.style.opacity = 1f;


        _visualElements["SellerMedicVisualElement"].style.opacity = 0f;
        _visualElements["DialogMedic"].style.opacity = 0f;
        
        
        _isDialogueWithSellerGuns = _isDialogueWithSellerItems = _isDialogueWithSellerMedicines = false;
        _playerController.SetActiveDialogue(false);
    }
    public void SetSafeArea(VisualElement visualElement)
    {
        _safeArea = visualElement;
    }
}
