using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using TMPro;

public class NTP : MonoBehaviour
{
    public static NTP sharedInstance;

    [SerializeField]
    string m_ServerUrl = "time.windows.com";
    //string m_ServerUrl = "time.google.com";

    DateTime m_ServerTime = DateTime.MinValue;
    public DateTime ServerTime => m_ServerTime;

    public Action<DateTime> OnTimeUpdated { get; set; }

    Thread m_GetTimeThread;

    private Coroutine m_TimeCorrection;



    [SerializeField]
    TMP_Text TimeText;
    [SerializeField]
    TMP_Text Device_TimeText;



    private void Awake()
    {
        if (sharedInstance != null && sharedInstance != this)
            Destroy(gameObject);
        else
            sharedInstance = this;

        DontDestroyOnLoad(gameObject);

    }

    private void OnEnable()
    {
        StopThread();
        // �ڷ�ƾ ���� ������� ����¡�� �ɸ��Ƿ� ������ ����Ͽ� ó��
        m_GetTimeThread = new Thread(() =>
        {
            while (true)
            {
                try
                {
                    var ntpData = new byte[48];
                    ntpData[0] = 0x1B;

                    var addresses = Dns.GetHostEntry(m_ServerUrl).AddressList;
                    var ipEndPoint = new IPEndPoint(addresses[0], 123);

                    using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
                    {
                        socket.Connect(ipEndPoint);

                        socket.ReceiveTimeout = 3000;

                        socket.Send(ntpData);
                        socket.Receive(ntpData);
                        socket.Close();
                    }

                    const byte serverReplyTime = 40;
                    ulong intPart = BitConverter.ToUInt32(ntpData, serverReplyTime);
                    ulong fractPart = BitConverter.ToUInt32(ntpData, serverReplyTime + 4);

                    intPart = SwapEndianness(intPart);
                    fractPart = SwapEndianness(fractPart);

                    var milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);
                    var networkDateTime = (new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc)).AddMilliseconds((long)milliseconds);

                    // ToLocalTime() �Լ��� ���ؼ� UTC �ð����� �ڵ����� �ѱ� �ð����� ����ȴ�.
                    m_ServerTime = networkDateTime.ToLocalTime();
                    OnTimeUpdated?.Invoke(m_ServerTime);
                    Debug.Log("�ð� ����ȭ : " + m_ServerTime);
                }
                catch (Exception e)
                {
                    // ���� �ʱⰪ�� �ٸ��ٸ� �ѹ��̶� ���� �ð��� �޾ƿԴٴ� ���̹Ƿ� �ʱⰪ�� ���� ���� �ð��� ����մϴ�.
                    if (m_ServerTime == DateTime.MinValue)
                    {
                        m_ServerTime = DateTime.Now;
                        Debug.Log("���� �ð��� �������� �� ������ �߻��߽��ϴ�. ���� �ð����� ��ü�մϴ�. : " + e);
                        // ������ ����
                        Thread.CurrentThread.Join();
                    }
                    else
                    {
                        Debug.Log("���� �ð��� �������� �� ������ �߻��߽��ϴ�. ���� �ð��� ������ ����� �ֽ��ϴ�. : " + e);
                    }
                }
                // �ð� �������� 4�� �ȿ� 2�� �̻��� ȣ���� �� ��� ���� �� �� ������
                // 10�ʰ��� �����̸� ������.
                Thread.Sleep(10000);
            }
        });
        m_GetTimeThread?.Start();
        StartTimeCorrection();
    }

    private void OnDisable()
    {
        StopThread();
        StopTimeCorrection();
    }

    void StartTimeCorrection()
    {
        StopTimeCorrection();
        m_TimeCorrection = StartCoroutine(CO_TimeCorrection());
    }

    void StopTimeCorrection()
    {
        if (m_TimeCorrection != null)
            StopCoroutine(m_TimeCorrection);
    }

    IEnumerator CO_TimeCorrection()
    {
        var cahcedWait = new WaitForSeconds(1);
        yield return new WaitUntil(() => m_ServerTime != DateTime.MinValue);
        while (true)
        {
            yield return cahcedWait;
            m_ServerTime = m_ServerTime.AddSeconds(1);
            //TimeText.text = m_ServerTime.ToString();
            //Device_TimeText.text = DateTime.Now.ToString("yyyy-MM-dd ") + DateTime.Now.ToString("T");
            try
            {
                Device_TimeText.text = DateTime.Now.ToString("HH:mm:ss");
            }
            catch(System.Exception e) { }
            //Debug.Log("�ð� ���� : " + m_ServerTime);
        }
    }

    void StopThread()
    {
        // ������ ��� ����
        if (m_GetTimeThread != null)
            m_GetTimeThread?.Abort();
    }

    static uint SwapEndianness(ulong x)
    {
        return (uint)(((x & 0x000000ff) << 24) +
                       ((x & 0x0000ff00) << 8) +
                       ((x & 0x00ff0000) >> 8) +
                       ((x & 0xff000000) >> 24));
    }

    public DateTime getServerTime()
    {
        return m_ServerTime;
    }
}
