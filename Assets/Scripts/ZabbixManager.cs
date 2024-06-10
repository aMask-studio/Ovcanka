using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using Zabbix_Sender;

public class ZabbixManager : MonoBehaviour
{
    public string IP_Sender; //192.168.0.1
    public string IP_Recipient; //95.188.79.124
    public string Key; //test_key

    [SerializeField] float _time;
    [SerializeField] float _maxTime;
    void Start()
    {
        SendData();
    }
    void Update()
    {
        _time += Time.deltaTime;
        if (_time >= _maxTime)
        {
            _time = 0;
            SendData();
        }
    }
    public void SendData()
    {
        IPAddress[] localIPs = Dns.GetHostAddresses(System.Net.Dns.GetHostName());

        Console.WriteLine("IP Addresses of the local machine:");
        foreach (IPAddress addr in localIPs)
        {
            if (addr.AddressFamily == AddressFamily.InterNetwork) //IPv4 Addresses
            {
                IP_Sender = addr.ToString();
                Debug.Log(addr);
            }
        }

        ZS_Request r = new ZS_Request(IP_Sender, Key, Application.version);
        Debug.Log(r.Send(IP_Recipient).response);
    }
}
