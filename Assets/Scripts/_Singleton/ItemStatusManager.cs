using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ItemStatusManager : MonoBehaviour
{
    [SerializeField]
    WarmItem Warm_Cocoa;
    [SerializeField]
    WarmItem Warm_Handwarmer;
    [SerializeField]
    WarmItem Warm_Cushion;
    [SerializeField]
    WarmItem Warm_Beanie;
    [SerializeField]
    WarmItem Warm_Muffler;
    [SerializeField]
    WarmItem Warm_Cape;

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

    [SerializeField]
    GlobalWarming GW_Diet;
    [SerializeField]
    GlobalWarming GW_Recycling;
    [SerializeField]
    GlobalWarming GW_Energy;
    [SerializeField]
    GlobalWarming GW_RenewableEnergy;
    [SerializeField]
    GlobalWarming GW_Transport;
    [SerializeField]
    GlobalWarming GW_Industry;
    [SerializeField]
    GlobalWarming GW_MarineProtection;
    [SerializeField]
    GlobalWarming GW_ForestAndSoil;

    public int Food_Status_Level = 0;
    public int WarmItem_Status_Level = 0;
    public int GW_Status_Level = 0;

    //GameObject Foods;
    //GameObject Items;
    //GameObject GlobalWarming;

    public static ItemStatusManager sharedInstance = null;

    void Start()
    {
        if (sharedInstance != null && sharedInstance != this)
            Destroy(gameObject);
        else
            sharedInstance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void Lock_Unlock_WarmItem(bool isUnlock = false) // false면 잠김 , true면 풀림
    {
        SwitchValue_Beanie(isUnlock);
        SwitchValue_Cape(isUnlock);
        SwitchValue_Cocoa(isUnlock);
        SwitchValue_Cushion(isUnlock);
        SwitchValue_Handwarmer(isUnlock);
        SwitchValue_Muffler(isUnlock);
    }

    public void Lock_Unlock_FoodItem(bool isUnlock = false)
    {
        SwitchValue_Shrimp(isUnlock);
        SwitchValue_SmallOctopus(isUnlock);
        SwitchValue_Sardine(isUnlock);
        SwitchValue_Mackerel(isUnlock);
        SwitchValue_Squid(isUnlock);
        SwitchValue_Octopus(isUnlock);
    }

    //public void Load_ItemStatus(int Food, int WarmItem, int GlobalWarming) 
    //{
    //    int result = Food + (WarmItem * 10) + (GlobalWarming * 100);
    //    int temp = 0;
    //    for (int i = 0; i < 3; i++) // 0 = Food / 1 = WarmItem / 2 = GlobalWarming Status
    //    {
    //        temp = result % 10;

    //        if (i == 0) Load_UnlockItems_Food(temp);
    //        else if (i == 1) Load_UnlockItems_WarmItem(temp);
    //        else if (i == 2) Load_UnlockItems_GlobalWarming(temp);

    //        result /= 10;
    //    }
    //}

    public void Load_ItemStatus(ItemInfo info)
    {
        if (info.b_Shrimp)
        {
            Food_Shrimp.isPurchased = true;
            Food_Shrimp.go_Lock.SetActive(false);
            Food_Shrimp.go_UseItem.SetActive(true);
        }
        if(info.b_SmallOctopus)
        {
            Food_SmallOctopus.isPurchased = true;
            Food_SmallOctopus.go_Lock.SetActive(false);
            Food_SmallOctopus.go_UseItem.SetActive(true);
        }
        if (info.b_Sardine)
        {
            Food_Sardine.isPurchased = true;
            Food_Sardine.go_Lock.SetActive(false);
            Food_Sardine.go_UseItem.SetActive(true);
        }
        if (info.b_Mackerel)
        {
            Food_Mackerel.isPurchased = true;
            Food_Mackerel.go_Lock.SetActive(false);
            Food_Mackerel.go_UseItem.SetActive(true);
        }
        if (info.b_Squid)
        {
            Food_Squid.isPurchased = true;
            Food_Squid.go_Lock.SetActive(false);
            Food_Squid.go_UseItem.SetActive(true);
        }
        if (info.b_Octopus)
        {
            Food_Octopus.isPurchased = true;
            Food_Octopus.go_Lock.SetActive(false);
            Food_Octopus.go_UseItem.SetActive(true);
        }


        if (info.b_Cocoa)
        {
            Warm_Cocoa.isPurchased = true;
            Warm_Cocoa.go_Lock.SetActive(false);
            Warm_Cocoa.go_UnlockItem.SetActive(false);
            Warm_Cocoa.go_UseItem.SetActive(true);
        }
        if (info.b_Handwarmer)
        {
            Warm_Handwarmer.isPurchased = true;
            Warm_Handwarmer.go_Lock.SetActive(false);
            Warm_Handwarmer.go_UnlockItem.SetActive(false);
            Warm_Handwarmer.go_UseItem.SetActive(true);
        }
        if (info.b_Cushion)
        {
            Warm_Cushion.isPurchased = true;
            Warm_Cushion.go_Lock.SetActive(false);
            Warm_Cushion.go_UnlockItem.SetActive(false);
            Warm_Cushion.go_UseItem.SetActive(true);
        }
        if (info.b_Beanie)
        {
            Warm_Beanie.isPurchased = true;
            Warm_Beanie.go_Lock.SetActive(false);
            Warm_Beanie.go_UnlockItem.SetActive(false);
            Warm_Beanie.go_UseItem.SetActive(true);
        }
        if (info.b_Muffler)
        {
            Warm_Muffler.isPurchased = true;
            Warm_Muffler.go_Lock.SetActive(false);
            Warm_Muffler.go_UnlockItem.SetActive(false);
            Warm_Muffler.go_UseItem.SetActive(true);
        }
        if (info.b_Cape)
        {
            Warm_Cape.isPurchased = true;
            Warm_Cape.go_Lock.SetActive(false);
            Warm_Cape.go_UnlockItem.SetActive(false);
            Warm_Cape.go_UseItem.SetActive(true);
        }


        if (info.b_Diet >= 1)
        {
            GW_Diet.isPurchased = true;
            GW_Diet.go_Lock.SetActive(false);
            GW_Diet.go_UseItem.SetActive(true);

            StartCoroutine(GW_Diet.LoadData(info.b_Diet));
            //GW_Diet.LoadData(info.b_Diet);
        }
        if (info.b_Recycling >= 1)
        {
            GW_Recycling.isPurchased = true;
            GW_Recycling.go_Lock.SetActive(false);
            GW_Recycling.go_UseItem.SetActive(true);

            StartCoroutine(GW_Recycling.LoadData(info.b_Recycling));
            //GW_Recycling.LoadData(info.b_Diet);
        }
        if (info.b_Energy >= 1)
        {
            GW_Energy.isPurchased = true;
            GW_Energy.go_Lock.SetActive(false);
            GW_Energy.go_UseItem.SetActive(true);

            StartCoroutine(GW_Energy.LoadData(info.b_Energy));
            //GW_Energy.LoadData(info.b_Diet);
        }
        if (info.b_RenewableEnergy >= 1)
        {
            GW_RenewableEnergy.isPurchased = true;
            GW_RenewableEnergy.go_Lock.SetActive(false);
            GW_RenewableEnergy.go_UseItem.SetActive(true);

            StartCoroutine(GW_RenewableEnergy.LoadData(info.b_RenewableEnergy));
            //GW_RenewableEnergy.LoadData(info.b_Diet);
        }
        if (info.b_Transport >= 1)
        {
            GW_Transport.isPurchased = true;
            GW_Transport.go_Lock.SetActive(false);
            GW_Transport.go_UseItem.SetActive(true);

            StartCoroutine(GW_Transport.LoadData(info.b_Transport));
            //GW_Transport.LoadData(info.b_Diet);
        }
        if (info.b_Industry >= 1)
        {
            GW_Industry.isPurchased = true;
            GW_Industry.go_Lock.SetActive(false);
            GW_Industry.go_UseItem.SetActive(true);

            StartCoroutine(GW_Industry.LoadData(info.b_Industry));
            //GW_Industry.LoadData(info.b_Diet);
        }
        if (info.b_MarineProtection >= 1)
        {
            GW_MarineProtection.isPurchased = true;
            GW_MarineProtection.go_Lock.SetActive(false);
            GW_MarineProtection.go_UseItem.SetActive(true);

            StartCoroutine(GW_MarineProtection.LoadData(info.b_MarineProtection));
            //GW_MarineProtection.LoadData(info.b_Diet);
        }
        if (info.b_ForestAndSoil >= 1)
        {
            GW_ForestAndSoil.isPurchased = true;
            GW_ForestAndSoil.go_Lock.SetActive(false);
            GW_ForestAndSoil.go_UseItem.SetActive(true);

            StartCoroutine(GW_ForestAndSoil.LoadData(info.b_ForestAndSoil));
            //GW_ForestAndSoil.LoadData(info.b_Diet);
        }


    }

    public void SwitchValue_Cocoa(bool isOn = false)
    {
        Warm_Cocoa.gameObject.SetActive(isOn);
    }
    public void SwitchValue_Handwarmer(bool isOn = false)
    {
        Warm_Handwarmer.gameObject.SetActive(isOn);
    }

    public void SwitchValue_Cushion(bool isOn = false)
    {
        Warm_Cushion.gameObject.SetActive(isOn);
    }

    public void SwitchValue_Beanie(bool isOn = false)
    {
        Warm_Beanie.gameObject.SetActive(isOn);
    }
    public void SwitchValue_Muffler(bool isOn = false)
    {
        Warm_Muffler.gameObject.SetActive(isOn);
    }

    public void SwitchValue_Cape(bool isOn = false)
    {
        Warm_Cape.gameObject.SetActive(isOn);
    }

    public void SwitchValue_Shrimp(bool isOn = false)
    {
        Food_Shrimp.gameObject.SetActive(isOn);
    }
    public void SwitchValue_SmallOctopus(bool isOn = false)
    {
        Food_SmallOctopus.gameObject.SetActive(isOn);
    }

    public void SwitchValue_Sardine(bool isOn = false)
    {
        Food_Sardine.gameObject.SetActive(isOn);
    }

    public void SwitchValue_Mackerel(bool isOn = false)
    {
        Food_Mackerel.gameObject.SetActive(isOn);
    }

    public void SwitchValue_Squid(bool isOn = false)
    {
        Food_Squid.gameObject.SetActive(isOn);
    }

    public void SwitchValue_Octopus(bool isOn = false)
    {
        Food_Octopus.gameObject.SetActive(isOn);
    }

    public WarmItem GetCocoa()
    {
        return Warm_Cocoa;
    }

    public WarmItem GetCushion()
    {
        return Warm_Cushion;
    }

    public WarmItem GetHandwarmer()
    {
        return Warm_Handwarmer;
    }

    public WarmItem GetCape()
    {
        return Warm_Cape;
    }

    public WarmItem GetMuffler()
    {
        return Warm_Muffler;
    }

    public WarmItem GetBeanie()
    {
        return Warm_Beanie;
    }

    public Food GetShrimp()
    {
        return Food_Shrimp;
    }

    public Food GetSardine()
    {
        return Food_Sardine;
    }

    public Food GetSmallOctopus()
    {
        return Food_SmallOctopus;
    }

    public Food GetSquid()
    {
        return Food_Squid;
    }

    public Food GetOctopus()
    {
        return Food_Octopus;
    }

    public Food GetMackerel()
    {
        return Food_Mackerel;
    }

    public GlobalWarming GetDiet()
    {
        return GW_Diet;
    }

    public GlobalWarming GetRecycling()
    {
        return GW_Recycling;
    }

    public GlobalWarming GetEnergy()
    {
        return GW_Energy;
    }

    public GlobalWarming GetRenewableEnergy()
    {
        return GW_RenewableEnergy;
    }

    public GlobalWarming GetTransport()
    {
        return GW_Transport;
    }

    public GlobalWarming GetIndustry()
    {
        return GW_Industry;
    }

    public GlobalWarming GetMarineProtection()
    {
        return GW_MarineProtection;
    }

    public GlobalWarming GetForestAndSoil()
    {
        return GW_ForestAndSoil;
    }
}