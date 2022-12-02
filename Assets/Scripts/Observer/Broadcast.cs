using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Broadcast : MonoBehaviour, ISubject
{
    public static Broadcast sharedInstance = null;
    List<Observer> observers = new List<Observer>();

    private void Awake()
    {
        if (sharedInstance != null && sharedInstance != this)
            Destroy(gameObject);
        else
            sharedInstance = this;

        DontDestroyOnLoad(gameObject);

        Observer QuestObserver = new QuestObserver(this.gameObject);

        AddObserver(QuestObserver);

    }

    public void AddObserver(Observer o)
    {
        observers.Add(o);
    }

    public void RemoveObserver(Observer o)
    {
        if (observers.IndexOf(o) > 0) observers.Remove(o);
    }

    public void Notify(string __EventType1, string __EventType2,int __Amount = 0)
    {
        foreach (Observer o in observers)
        {
            o.OnNotify(__EventType1, __EventType2,__Amount);
        }
    }
}
