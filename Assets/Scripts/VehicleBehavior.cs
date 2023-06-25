using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class VehicleBehavior : MonoBehaviour
{
    private Player player;
    private Vector3 vector;
    private Vector3 zoomOut;
    private bool zoomedOut = false;
    private Vector3 currentPosition;

    public int CurrentDimension { get; private set; }

    public void Start()
    {
        if (this.name == "CameraVehicle1")
        {
            player = OverworldData.player1;
        }
        else
        {
            player = OverworldData.player2;
        }

        transform.position += new Vector3(OverworldData.DimensionSize / 2, OverworldData.DimensionSize * 2 / 3, -OverworldData.DimensionSize);
        vector = new Vector3(OverworldData.DimensionSize / 2, OverworldData.DimensionSize * 2, 0);

        //int halfDimensionsCount = OverworldData.DimensionsCount / 2;
        zoomOut = new Vector3(OverworldData.DimensionSize / 2, OverworldData.DimensionSize * OverworldData.DimensionsCount, -OverworldData.DimensionSize * OverworldData.DimensionsCount * 2);

        CurrentDimension = 0;
    }

    public void OnDimensionUp(CallbackContext ctx)
    {
        if (ctx.performed && OverworldData.DimensionsCount - 1 > player.ActiveDimension.DimensionNr)
        {
            CurrentDimension += 1;
            player.world.SetNewDimension(player.ActiveDimension.DimensionNr + 1);
            transform.position += vector;
            player.fleetMenu.UpdateFleetMenuDimension(CurrentDimension);
        }
    }

    public void OnDimensionDown(CallbackContext ctx)
    {
        if (ctx.performed && player.ActiveDimension.DimensionNr > 0)
        {
            CurrentDimension -= 1;
            player.world.SetNewDimension(player.ActiveDimension.DimensionNr - 1);
            transform.position -= vector;
            player.fleetMenu.UpdateFleetMenuDimension(CurrentDimension);
        }
    }

    public void OnZoom(CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (zoomedOut)
            {
                transform.position = currentPosition;
                zoomedOut = !zoomedOut;
            }
            else
            {
                currentPosition = transform.position;
                transform.position = zoomOut;
                zoomedOut = !zoomedOut;
            }
        }
    }
}
