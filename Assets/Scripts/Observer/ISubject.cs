using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISubject
{
    void AddObserver(Observer o); // 옵저버 추가
    void RemoveObserver(Observer o); // 옵저버 삭제
    void Notify(string __EventType1, string __EventType2, int __Amount = 0); // 옵저버에게 연락하는 함수
}
