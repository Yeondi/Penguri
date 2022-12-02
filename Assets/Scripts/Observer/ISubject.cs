using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISubject
{
    void AddObserver(Observer o); // ������ �߰�
    void RemoveObserver(Observer o); // ������ ����
    void Notify(string __EventType1, string __EventType2, int __Amount = 0); // ���������� �����ϴ� �Լ�
}
