using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] GameObject _mainPanel;
    [SerializeField] GameObject _quotePanel;
    [SerializeField] GameObject _contactsPanel;
    [SerializeField] GameObject _inContactsPanel;
    [SerializeField] GameObject[] _storiesPanel;
    [SerializeField] SnapToPicture _scrollInContacts;

    [SerializeField] GameObject _txtLoading;
    public void OpenMain()
    {
        //_mainPanel.SetActive(true);
        _quotePanel.SetActive(false);
        _contactsPanel.SetActive(false);
        _inContactsPanel.SetActive(false);
        foreach(var i in _storiesPanel)
            i.SetActive(false);
    }
    public void OpenQuote()
    {
        //_mainPanel.SetActive(false);
        _quotePanel.SetActive(true);
        _txtLoading.SetActive(false);
    }
    public void OpenContacts()
    {
        _quotePanel.SetActive(false);
        _inContactsPanel.SetActive(false);
        _contactsPanel.SetActive(true);
    }
    public void OpenInContacts(int idItem)
    {
        _scrollInContacts.currentItem = idItem;
        _contactsPanel.SetActive(false);
        _inContactsPanel.SetActive(true);
    }
    public void OpenStories(int i)
    {
        //_mainPanel.SetActive(false);
        _storiesPanel[i].SetActive(true);
    }
}
