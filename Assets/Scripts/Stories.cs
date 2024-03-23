using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stories : MonoBehaviour
{
    [SerializeField] Sprite[] _storiesSprites;
    [SerializeField] Image[] _pagiantion;
    [SerializeField] Image[] _storiesImages;
    [SerializeField] RectTransform _contentPanel;
    [SerializeField] float _snapForce;
    public int currentStory;
    float _previousMousePos;
    float _currentMousePos;
    float _snapSpeed;
    public void OnEnable()
    {
        currentStory = 0;
        _storiesImages[1].sprite = _storiesSprites[currentStory];
        _storiesImages[0].gameObject.SetActive(false);
        _storiesImages[2].gameObject.SetActive(true);
        _storiesImages[2].sprite = _storiesSprites[currentStory + 1];
        ChangePagination(currentStory);
    }
    private void Update()
    {
        /*if (Input.GetMouseButtonUp(0))
        {
            _currentMousePos = Input.mousePosition.x;
            if (_currentMousePos > _previousMousePos && currentStory > 0)
            {
                PreviousStory();
            }
            else if (_currentMousePos < _previousMousePos && currentStory+1 < _storiesSprites.Length)
            {
                NextStory();
            }
        }*/
        _snapSpeed += _snapForce * Time.deltaTime;
        _contentPanel.localScale = new Vector3(
            Mathf.MoveTowards(_contentPanel.localScale.x, 1, _snapSpeed), 
            Mathf.MoveTowards(_contentPanel.localScale.y, 1, _snapSpeed), 
            _contentPanel.localScale.z);
    }
    public void SetPreviousPos()
    {
        _previousMousePos = Input.mousePosition.x;
    }
    public void SetCurrentPos()
    {
        _currentMousePos = Input.mousePosition.x;
        if (_currentMousePos > _previousMousePos + 10 && currentStory > 0)
        {
            PreviousStory();
        }
        else if (_currentMousePos < _previousMousePos - 10 && currentStory + 1 < _storiesSprites.Length)
        {
            NextStory();
        }
    }
    public void NextStory()
    {
        currentStory++;
        if (currentStory < _storiesSprites.Length + 1)
        {
            ActivateSideStories();
            ChangePagination(currentStory);
            _storiesImages[0].sprite = _storiesSprites[currentStory-1];
            _storiesImages[1].sprite = _storiesSprites[currentStory];
            if (currentStory + 1 < _storiesSprites.Length)
                _storiesImages[2].sprite = _storiesSprites[currentStory + 1];
            else
                _storiesImages[2].gameObject.SetActive(false);
        }
    }
    public void PreviousStory()
    {
        currentStory--;
        if (currentStory < _storiesSprites.Length + 1)
        {
            ActivateSideStories();
            ChangePagination(currentStory);
            if (currentStory - 1 >= 0)
                _storiesImages[0].sprite = _storiesSprites[currentStory - 1];
            else
                _storiesImages[0].gameObject.SetActive(false);
            _storiesImages[1].sprite = _storiesSprites[currentStory];
            _storiesImages[2].sprite = _storiesSprites[currentStory + 1];

        }
    }
    private void ActivateSideStories()
    {
        _storiesImages[0].gameObject.SetActive(true);
        _storiesImages[2].gameObject.SetActive(true);
    }
    private void ChangePagination(int currentItem)
    {
        for (int i = 0; i < _pagiantion.Length; i++)
        {
            if (i != currentItem)
            {
                _pagiantion[i].color = new Color(0.5490196f, 0.5529412f, 0.5607843f);
            }
            else
            {
                _pagiantion[i].color = new Color(0.8509804f, 0.8509804f, 0.8509804f);
            }
        }
    }
}
