using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAmount_Food : MonoBehaviour
{
    [SerializeField]
    Food Food_Shrimp;
    [SerializeField]
    Food Food_SmallOctopus;
    [SerializeField]
    Food Food_Sardine;
    [SerializeField]
    Food Food_Mackerel;
    [SerializeField]
    Food Food_Squid;
    [SerializeField]
    Food Food_Octopus;


    public void ChangeAmount(int __Amount)
    {
        Food_Shrimp.changeAmount(__Amount);
        Food_SmallOctopus.changeAmount(__Amount);
        Food_Sardine.changeAmount(__Amount);
        Food_Mackerel.changeAmount(__Amount);
        Food_Squid.changeAmount(__Amount);
        Food_Octopus.changeAmount(__Amount);

        Food_Shrimp.Cost_Use = Food_Shrimp.Cost_Use * __Amount;
        Food_SmallOctopus.Cost_Use = Food_SmallOctopus.Cost_Use * __Amount;
        Food_Sardine.Cost_Use = Food_Sardine.Cost_Use * __Amount;
        Food_Mackerel.Cost_Use = Food_Mackerel.Cost_Use * __Amount;
        Food_Squid.Cost_Use = Food_Squid.Cost_Use * __Amount;
        Food_Octopus.Cost_Use = Food_Octopus.Cost_Use * __Amount;
    }
}
