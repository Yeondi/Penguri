using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStatusManager : MonoBehaviour
{
    GameObject Warm_Cocoa;
    GameObject Warm_Cushion;
    GameObject Warm_Blanket;
    GameObject Warm_Heater;
    GameObject Warm_Muffler;
    GameObject Warm_Beanie;

    GameObject Food_Shrimp;
    GameObject Food_Sardine;
    GameObject Food_SmallOctopus;
    GameObject Food_Squid;
    GameObject Food_Octopus;
    GameObject Food_Mackerel;

    GameObject Foods;
    GameObject Items;

    void Start()
    {
        Items = GameObject.Find("Canvas").transform.Find("Temp Contents").transform.Find("Temp Layers").gameObject;
        Foods = GameObject.Find("Canvas").transform.Find("Food Contents").transform.Find("Food Layers").gameObject;

        Warm_Cocoa = Items.transform.Find("Temp P0").transform.Find("Warm_Cocoa").gameObject;
        Warm_Cushion = Items.transform.Find("Temp P0").transform.Find("Warm_Cushion").gameObject;
        Warm_Blanket = Items.transform.Find("Temp P0").transform.Find("Warm_Blanket").gameObject;
        Warm_Heater = Items.transform.Find("Temp P1").transform.Find("Warm_Heater").gameObject;
        Warm_Muffler = Items.transform.Find("Temp P1").transform.Find("Warm_Muffler").gameObject;
        Warm_Beanie = Items.transform.Find("Temp P1").transform.Find("Warm_Beanie").gameObject;

        Food_Shrimp = Foods.transform.Find("Foods P0").transform.Find("Shrimp").gameObject;
        Food_Sardine = Foods.transform.Find("Foods P0").transform.Find("Sardine").gameObject;
        Food_SmallOctopus = Foods.transform.Find("Foods P0").transform.Find("SmallOctopus").gameObject;
        Food_Squid = Foods.transform.Find("Foods P1").transform.Find("Squid").gameObject;
        Food_Octopus = Foods.transform.Find("Foods P1").transform.Find("Octopus").gameObject;
        Food_Mackerel = Foods.transform.Find("Foods P1").transform.Find("Mackerel").gameObject;
    }

    public void SwitchValue_Cocoa(bool isOn = false)
    {
        Warm_Cocoa.GetComponentInChildren<DragEvent>().setIsOn(isOn);
    }

    public void SwitchValue_Cushion(bool isOn = false)
    {
        Warm_Cushion.GetComponentInChildren<DragEvent>().setIsOn(isOn);
    }

    public void SwitchValue_Blanket(bool isOn = false)
    {
        Warm_Blanket.GetComponentInChildren<DragEvent>().setIsOn(isOn);
    }

    public void SwitchValue_Heater(bool isOn = false)
    {
        Warm_Heater.GetComponentInChildren<DragEvent>().setIsOn(isOn);
    }

    public void SwitchValue_Muffler(bool isOn = false)
    {
        Warm_Muffler.GetComponentInChildren<DragEvent>().setIsOn(isOn);
    }

    public void SwitchValue_Beanie(bool isOn = false)
    {
        Warm_Beanie.GetComponentInChildren<DragEvent>().setIsOn(isOn);
    }

    public void SwitchValue_Shrimp(bool isOn = false)
    {
        Food_Shrimp.GetComponentInChildren<DragEvent>().setIsOn(isOn);
    }

    public void SwitchValue_Sardine(bool isOn = false)
    {
        Food_Sardine.GetComponentInChildren<DragEvent>().setIsOn(isOn);
    }

    public void SwitchValue_SmallOctopus(bool isOn = false)
    {
        Food_SmallOctopus.GetComponentInChildren<DragEvent>().setIsOn(isOn);
    }

    public void SwitchValue_Squid(bool isOn = false)
    {
        Food_Squid.GetComponentInChildren<DragEvent>().setIsOn(isOn);
    }

    public void SwitchValue_Octopus(bool isOn = false)
    {
        Food_Octopus.GetComponentInChildren<DragEvent>().setIsOn(isOn);
    }

    public void SwitchValue_Mackerel(bool isOn = false)
    {
        Food_Mackerel.GetComponentInChildren<DragEvent>().setIsOn(isOn);
    }

    public GameObject GetCocoa()
    {
        return Warm_Cocoa;
    }

    public GameObject GetCushion()
    {
        return Warm_Cushion;
    }

    public GameObject GetBlanket()
    {
        return Warm_Blanket;
    }

    public GameObject GetHeater()
    {
        return Warm_Heater;
    }

    public GameObject GetMuffler()
    {
        return Warm_Muffler;
    }

    public GameObject GetBeanie()
    {
        return Warm_Beanie;
    }

    public GameObject GetShrimp()
    {
        return Food_Shrimp;
    }

    public GameObject GetSardine()
    {
        return Food_Sardine;
    }

    public GameObject GetSmallOctopus()
    {
        return Food_SmallOctopus;
    }

    public GameObject GetSquid()
    {
        return Food_Squid;
    }

    public GameObject GetOctopus()
    {
        return Food_Octopus;
    }

    public GameObject GetMackerel()
    {
        return Food_Mackerel;
    }
}
