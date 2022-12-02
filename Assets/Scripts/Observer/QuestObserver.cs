using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObserver : Observer
{
    GameObject obj;

    public QuestObserver(GameObject obj)
    {
        this.obj = obj;
    }

    public override void OnNotify(string __EventType1, string __EventType2, int __Amount = 0)
    {
        if (QuestHandler.sharedInstance.getKey() != "")
        {
            QuestHandler.sharedInstance.ProgressOfQuest(__Amount);
        }
    }
}
