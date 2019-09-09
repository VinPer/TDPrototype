﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretHandler : MonoBehaviour
{
    public static TurretHandler instance;
    // Keep the object to reference in the actual level so we can know which turrets are being used
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance)
        {
            Destroy(instance.gameObject);
        }
        instance = this;
    }

    // Lists for the turrets that are available and selected
    public List<TurretBlueprint> allTurrets;
    public static List<TurretBlueprint> selectedTurrets;
    public int max = 5;

    // uhh ignore this
    public List<TurretBlueprint> defaultSelected;

    // Elements that hold the turrets in the overlay
    public GameObject allTurretsGUI;
    public GameObject selectedTurretsGUI;

    public Button playButton;

    public static bool active = false;
    
    void Start()
    {
        active = true;
        // Instantiate the selected turrets list
        selectedTurrets = new List<TurretBlueprint>();

        // Set the cost display for all available turrets
        for(int i = 0; i < allTurrets.Count; i++)
        {
            allTurretsGUI.transform.GetChild(i).GetComponentsInChildren<Text>()[1].text = "$" + allTurrets[i].cost;
            allTurretsGUI.transform.GetChild(i)
                .GetComponentsInChildren<Text>()[0].text = allTurrets[i].name;
        }

        // Add the first (max) turrets to the selected turrets list
        for(int i = 0; i < max; i++)
        {
            selectedTurrets.Add(allTurrets[i]);
        }

        UpdateSelectedTurrets();
    }

    // Add a turret to the selected turrets list when it is clicked from the available list
    public void AssignTurret(int turretIndex)
    {
        TurretBlueprint turret = allTurrets[turretIndex];

        // Keeps the list limited to max turrets and without duplicates
        if (selectedTurrets.Count < max && !selectedTurrets.Contains(turret)) selectedTurrets.Add(turret);
        UpdateSelectedTurrets();
    }

    // Remove a turret from the selected turrets list by clicking it
    public void RemoveTurret(int turretIndex)
    {
        // If you click an empty slot
        if (turretIndex >= selectedTurrets.Count) return;
        TurretBlueprint turret = selectedTurrets[turretIndex];
        //if (!selectedTurrets.Contains(turret)) return;
        
        //Transform element = selectedTurretsGUI.transform.GetChild(turretIndex);
        //element.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        //element.GetComponentInChildren<Text>().text = "$0";

        selectedTurrets.Remove(turret);

        UpdateSelectedTurrets();
    }

    // Updates the display of selected turrets
    private void UpdateSelectedTurrets()
    {
        Transform currentChildSelected;
        Transform currentChildAll;

        // For all turrets currently in the list, update image and cost
        for (int i = 0; i < selectedTurrets.Count; i++)
        {
            currentChildSelected = selectedTurretsGUI.transform.GetChild(i);
            currentChildAll = allTurretsGUI.transform.GetChild(
                allTurrets.IndexOf(selectedTurrets[i]));
            currentChildSelected.GetComponent<Image>().sprite = currentChildAll.GetComponent<Image>().sprite;
            currentChildSelected.GetComponent<Image>().color = currentChildAll.GetComponent<Image>().color;
            currentChildSelected.GetComponentsInChildren<Text>()[1].text = "$" + selectedTurrets[i].cost;
            currentChildSelected.GetComponentsInChildren<Text>()[0].text = selectedTurrets[i].name;

            selectedTurrets[i].color = currentChildAll.GetComponent<Image>().color;
            selectedTurrets[i].sprite = currentChildAll.GetComponent<Image>().sprite;
            //Debug.Log(currentChildSelected.GetComponent<Button>().onClick.);
        }

        // For the empty slots in the list, hide the image and set cost to $0
        for (int i = selectedTurrets.Count; i < max; i++)
        {
            currentChildSelected = selectedTurretsGUI.transform.GetChild(i);
            currentChildSelected.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            currentChildSelected.GetComponentsInChildren<Text>()[1].text = "$0";
            currentChildSelected.GetComponentsInChildren<Text>()[0].text = "";
        }
        if(selectedTurrets.Count < 1)
            playButton.interactable = false;
        else
            playButton.interactable = true;
    }
}
