using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] UI ui;
    [SerializeField] float _time;
    [SerializeField] float _maxTime;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _time = 0;
        }
        else
        {
            _time += Time.deltaTime;
            if (_time >= _maxTime)
            {
                _time = 0;
                ui.OpenMain();
            }
        }
    }
}
