using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPart : MonoBehaviour
{
    public int partNr;
    public int X { get; private set; }
    public int Y { get; private set; }
    public Dimension Dimension { get; set; }
    public Material PartMaterial { get; private set; }
    public bool Damaged { get; set; }

    public void InitiateShipPart(Player player, int partNr, Ship ship)
    {
        this.partNr = partNr;
        X = ship.ShipNr;
        Y = partNr;
        Damaged = false;
        PartMaterial = GetComponent<Renderer>().material;

        if (player.number == 1)
        {
            PartMaterial.color = new Color(0.3f, 0.12f, 0, 1); // brown
        }
        else
        {
            PartMaterial.color = new Color(0.3f, 0.3f, 0, 1); // olive
        }
    }

    public void UpdateCoordinatesRelative(int x, int y)
    {
        X += x;
        Y += y;
    }

    public void UpdateCoordinatesAbsolute(int x, int y)
    {
        X = x;
        Y = y;
    }

    public void OccupyCell()
    {
        Dimension.GetCell(X, Y).GetComponent<Cell>().Occupied = true;
    }

    public void ReleaseCell(Player player)
    {
        Dimension.GetCell(X, Y).GetComponent<Cell>().Occupied = false;
    }
}
