using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] GameObject _mainPanel;
    [SerializeField] GameObject _quotePanel;
    [SerializeField] GameObject _contactsPanel;
    [SerializeField] GameObject _inContactsPanel;
    [SerializeField] GameObject _storiesPanel;
    [SerializeField] SnapToPicture _scrollInContacts;
    public void OpenMain()
    {
        _mainPanel.SetActive(true);
        _quotePanel.SetActive(false);
        _contactsPanel.SetActive(false);
        _inContactsPanel.SetActive(false);
        _storiesPanel.SetActive(false);
    }
    public void OpenQuote()
    {
        _mainPanel.SetActive(false);
        _quotePanel.SetActive(true);
    }
    public void OpenContacts()
    {
        _quotePanel.SetActive(false);
        _contactsPanel.SetActive(true);
    }
    public void OpenInContacts(int idItem)
    {
        _scrollInContacts.currentItem = idItem;
        _contactsPanel.SetActive(false);
        _inContactsPanel.SetActive(true);
    }
    public void OpenStories()
    {
        _mainPanel.SetActive(false);
        _storiesPanel.SetActive(true);
    }
}
