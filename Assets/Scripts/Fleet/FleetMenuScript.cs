﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class FleetMenuScript : MonoBehaviour
{
    private Player player;
    private GameObject[] shipButtons;
    private ShipButton currentButton;
    private string x = "--";
    private string y = "--";
    private string dimensionNr = "01";
    private TextMeshProUGUI xCoord;
    private TextMeshProUGUI yCoord;
    private TextMeshProUGUI dimension;
    private GameObject dimensionsHeader;
    private GameObject[] dimensions;
    private int currenDimension;

    public GameObject firstSelectedButton;
    public GameObject selectedElement;

    private void Awake()
    {
        if(this.name == "FleetMenu1")
        {
            player = OverworldData.player1;
        }
        else
        {
            player = OverworldData.player2;
        }

        shipButtons = new GameObject[OverworldData.FleetSize];

        if (player.number == 1)
        {
            xCoord = GameObject.Find("X-Koordinate1").GetComponent<TextMeshProUGUI>();
            yCoord = GameObject.Find("Y-Koordinate1").GetComponent<TextMeshProUGUI>();
            dimension = GameObject.Find("Dimension1").GetComponent<TextMeshProUGUI>();
            dimensionsHeader = GameObject.Find("DimensionsHeader1");
        }
        else
        {
            xCoord = GameObject.Find("X-Koordinate2").GetComponent<TextMeshProUGUI>();
            yCoord = GameObject.Find("Y-Koordinate2").GetComponent<TextMeshProUGUI>();
            dimension = GameObject.Find("Dimension2").GetComponent<TextMeshProUGUI>();
            dimensionsHeader = GameObject.Find("DimensionsHeader2");
        }
    }

    private void Start()
    {
        CreateShipButtons();
        CreateHUDDimensions();
        currenDimension = 1;
        player.fleet.ActivateShip(currentButton.ShipButtonNr, player);
    }

    private void Update()
    {
        xCoord.text = x;
        yCoord.text = y;
        dimension.text = dimensionNr;
    }

    //FleetMenu actionMap
    public void OnDimensionUp()
    {
        if (currenDimension < OverworldData.DimensionsCount)
        {
            dimensions[currenDimension - 1].SetActive(false);
            currenDimension++;
            dimensions[currenDimension - 1].SetActive(true);
        }
    }

    public void OnDimensionDown()
    {
        if (currenDimension > 1)
        {
            dimensions[currenDimension - 1].SetActive(false);
            currenDimension--;
            dimensions[currenDimension - 1].SetActive(true);
        }
    }

    public void UpdateFleetMenuCoords(int xCoord, int yCoord)
    {
        if (xCoord.ToString().Length < 2)
        {
            x = "0" + xCoord.ToString();
        }
        else
        {
            x = xCoord.ToString();
        }

        if (yCoord.ToString().Length < 2)
        {
            y = "0" + yCoord.ToString();
        }
        else
        {
            y = yCoord.ToString();
        }
    }

    public void UpdateFleetMenuCoords()
    {
        x = "--";
        y = "--";
    }

    public void UpdateFleetMenuDimension(int dimension)
    {
        dimensionNr = "0" + (dimension + 1).ToString();
    }

    public GameObject[] GetShipButtons()
    {
        return shipButtons;
    }

    private void CreateShipButtons()
    {
        GameObject buttonObj;
        Button button;

        for (int i = 0; i < OverworldData.FleetSize; i++)
        {
            buttonObj = TMP_DefaultControls.CreateButton(new TMP_DefaultControls.Resources());
            Transform textObject = buttonObj.transform.Find("Text (TMP)");
            Object.Destroy(textObject.gameObject);
            button = buttonObj.GetComponent<Button>();

            CreateButton(buttonObj, button, i);
            DesignButton(button);

            for (int j = 0; j <= i; j++)
            {
                CreateShipPart(buttonObj);
            }
        }
    }

    private void CreateButton(GameObject buttonObj, Button button, int i)
    {
        Transform parentsTransform;

        if (name == "FleetMenu1")
        {
            buttonObj.name = "Ship1." + (i + 1);
            buttonObj.layer = 11;
            parentsTransform = GameObject.Find("ShipButtons1").GetComponent<Transform>();
        }
        else
        {
            buttonObj.name = "Ship2." + (i + 1);
            buttonObj.layer = 12;
            parentsTransform = GameObject.Find("ShipButtons2").GetComponent<Transform>();
        }

        button.transform.SetParent(parentsTransform, false);
        Navigation buttonNavigation = button.navigation;
        buttonNavigation.mode = Navigation.Mode.None;
        buttonObj.AddComponent<ShipButton>();
        buttonObj.GetComponent<ShipButton>().ShipButtonNr = i;
        buttonObj.AddComponent<HorizontalLayoutGroup>().childAlignment = TextAnchor.MiddleCenter;


        if (i == 0)
        {
            firstSelectedButton = buttonObj;
            SetFirstSelecetedButton();
        }

        shipButtons[i] = buttonObj;
    }

    public void SetFirstSelecetedButton()
    {
        player.eventSystem.firstSelectedGameObject = firstSelectedButton;
        currentButton = firstSelectedButton.GetComponent<ShipButton>();
        player.CurrentShipButton = currentButton;
    }

    private void DesignButton(Button button)
    {
        button.image.type = Image.Type.Simple;
        button.image.sprite = Resources.Load<Sprite>("HUD_Elemente/ButtonElements/Button") as Sprite;
        button.image.SetNativeSize();
        button.transition = Selectable.Transition.SpriteSwap;
        Sprite buttonSelected = Resources.Load<Sprite>("HUD_Elemente/ButtonElements/Selection") as Sprite;

        SpriteState spriteState = new();
        spriteState.selectedSprite = buttonSelected;
        button.spriteState = spriteState;
    }

    private void CreateShipPart(GameObject buttonObj)
    {
        GameObject shipPart = new("ShipPart", typeof(CanvasRenderer), typeof(Image));
        shipPart.transform.SetParent(buttonObj.transform, false);

        Image shipPartImage = shipPart.GetComponent<Image>();
        shipPartImage.sprite = Resources.Load<Sprite>("HUD_Elemente/ButtonElements/ShipPart") as Sprite;
        shipPartImage.type = Image.Type.Simple;
        shipPartImage.preserveAspect = true;

        if (player.number == 1)
        {
            shipPart.layer = 11;
        }
        else
        {
            shipPart.layer = 12;
        }
    }

    private void CreateHUDDimensions()
    {
        dimensions = new GameObject[OverworldData.DimensionsCount];

        for (int i = 0; i < OverworldData.DimensionsCount; i++)
        {
            GameObject HUDDimension = new("HUDDimension0" + (i + 1), typeof(CanvasRenderer), typeof(Image));
            HUDDimension.transform.SetParent(dimensionsHeader.transform, false);

            Image HUDDimensionImage = HUDDimension.GetComponent<Image>();
            HUDDimensionImage.sprite = Resources.Load<Sprite>("HUD_Elemente/Levels/Dimension0" + (i + 1)) as Sprite;
            HUDDimensionImage.type = Image.Type.Simple;
            HUDDimensionImage.SetNativeSize();
            dimensions[i] = HUDDimension;

            if (i != 0)
            {
                HUDDimension.SetActive(false);
            }
        }
    }
}
