using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HouseWithConnections
{
    public House house;
    public List<int> listInt;
}

public class WorldManager : MonoBehaviour
{
    [SerializeField]
    private GameObject lastWire;
    
    [SerializeField]
    private List<HouseWithConnections> houses;

    private List<int> checkedHouses = new List<int>();

    private void Awake()
    {
        foreach (var house in houses)
        {
            house.house.OnCharged += CheckForWin;
            house.house.OnDeactivated += CheckForLoss;
        }
    }

    private void OnDestroy()
    {
        foreach (var house in houses)
        {
            house.house.OnCharged -= CheckForWin;
            house.house.OnDeactivated -= CheckForLoss;
        }
    }

    private void CheckForWin()
    {
        if(CheckHouseConnections(1))
        {
            checkedHouses.Clear();
            lastWire.SetActive(true);
            Debug.Log("You Win!");//todo: add actual winning
        }
    }
    
    private void CheckForLoss()
    {
        bool notLoss = false;
        foreach (var house in houses)
        {
            if (house.house.isCharged)
            {
                notLoss = true;
                break;
            }
        }
        if(!notLoss)
            Debug.Log("You Lost!");//todo: add actual losing
    }

    private bool CheckHouseConnections(int houseIndex)
    {
        if(!houses[houseIndex].house.isCharged) return false;
        if (houseIndex == 0 && houses[houseIndex].house.isCharged) return true;
        checkedHouses.Add(houseIndex);
        foreach (int connectedHouse in houses[houseIndex].listInt)
        {
            if(checkedHouses.Contains(connectedHouse)) continue;
            if(CheckHouseConnections(connectedHouse)) return true;
        }
        checkedHouses.RemoveAt(checkedHouses.Count-1);
        return false;
    }

}
