using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnapToStories : MonoBehaviour
{
    [SerializeField] ScrollRect _scrollRect;
    [SerializeField] RectTransform _contentPanel;
    [SerializeField] RectTransform _sampleListPictures;
    [SerializeField] HorizontalLayoutGroup _hlg;

    [SerializeField] int _storiesCount;
    [SerializeField] Image[] _pagination;
    [SerializeField] Color _colorForPagination;
    [SerializeField] Color _mainPaginationColor;
    [SerializeField] Color _sideStoryColor;

    [SerializeField] float _snapForce;
    public int currentItem;

    RectTransform thisRect;
    RectTransform LeftRect;
    RectTransform RightRect;

    bool _momentalSwipe;
    float _snapSpeed;
    float _previousMousePos;
    float _currentMousePos;
    void OnEnable()
    {
        currentItem = 0;
        _contentPanel.localPosition = new Vector3(0 - (currentItem * (_sampleListPictures.rect.width + _hlg.spacing)),
            _contentPanel.localPosition.y,
            _contentPanel.localPosition.z);

        thisRect = _contentPanel.GetChild(currentItem + 1).GetChild(0).GetComponent<RectTransform>();
        thisRect.GetComponent<Image>().color = Color.white;
        thisRect.localPosition = new Vector2(0, 0);
        thisRect.sizeDelta = new Vector2(542, 871);
        thisRect.GetComponent<Button>().interactable = false;

        RightRect = _contentPanel.GetChild(currentItem + 2).GetChild(0).GetComponent<RectTransform>();
        RightRect.GetComponent<Button>().interactable = true;
        RightRect.GetComponent<Image>().color = _sideStoryColor;
        RightRect.localPosition = new Vector2(-78f, 0);
        RightRect.sizeDelta = new Vector2(374, 589);

        thisRect = null;
        LeftRect = null;
        RightRect = null;
        ChangePagination(currentItem);
    }
    void Update()
    {
        if (!_momentalSwipe)
        {
            currentItem = Mathf.RoundToInt((0 - _contentPanel.localPosition.x / (_sampleListPictures.rect.width + _hlg.spacing)));

            if (Input.GetMouseButtonDown(0))
                SetPreviousPos();
            if (Input.GetMouseButtonUp(0))
                SetCurrentPos();
        }
        else
        {
            ChangePagination(currentItem); //смена нумерации историй
            _scrollRect.velocity = Vector2.zero;
            _snapSpeed += _snapForce * Time.deltaTime;
            _contentPanel.localPosition = new Vector3(
                Mathf.MoveTowards(_contentPanel.localPosition.x, 0 - (currentItem * (_sampleListPictures.rect.width + _hlg.spacing)), _snapSpeed),
                _contentPanel.localPosition.y,
                _contentPanel.localPosition.z); //перемещение по ленте историй

            thisRect = _contentPanel.GetChild(currentItem + 1).GetChild(0).GetComponent<RectTransform>(); //определение позиции историй (те что слева и справа меньше по размерам)
            if (currentItem > 0)
                LeftRect = _contentPanel.GetChild(currentItem).GetChild(0).GetComponent<RectTransform>();
            else
                LeftRect = null;
            if (currentItem + 1 < _storiesCount)
                RightRect = _contentPanel.GetChild(currentItem + 2).GetChild(0).GetComponent<RectTransform>();
            else
                RightRect = null;

            thisRect.GetComponent<Image>().color = Color.white; //параметры для конкретной истории (изменение цвета, позиции и размера)
            thisRect.localPosition = new Vector2(Mathf.MoveTowards(thisRect.localPosition.x, 0, _snapSpeed), 0);
            thisRect.sizeDelta = new Vector2(Mathf.MoveTowards(thisRect.rect.width, 542, _snapSpeed), Mathf.MoveTowards(thisRect.rect.height, 871, _snapSpeed));
            thisRect.GetComponent<Button>().interactable = false;

            if (LeftRect) //параметры для соседних главной историй
                SetSideStoryParams(LeftRect, 78f);
            if (RightRect)
                SetSideStoryParams(RightRect, -78f);

            if (_contentPanel.localPosition.x == 0 - (currentItem * (_sampleListPictures.rect.width + _hlg.spacing)))
            {
                _momentalSwipe = false;
                _snapSpeed = 0;
            }
        }
    }
    private void SetSideStoryParams(RectTransform Rect, float pos)
    {
        Rect.GetComponent<Button>().interactable = true;
        Rect.GetComponent<Image>().color = _sideStoryColor;
        Rect.localPosition = new Vector2(Mathf.MoveTowards(Rect.localPosition.x, pos, _snapSpeed), 0);
        Rect.sizeDelta = new Vector2(Mathf.MoveTowards(Rect.rect.width, 374, _snapSpeed), Mathf.MoveTowards(Rect.rect.height, 589, _snapSpeed));
    }
    public void SetPreviousPos() //для управления свайпом (сами методы вызываются из Event Trigger на объекте панели историй)
    {
        _previousMousePos = Input.mousePosition.x;
    }
    public void SetCurrentPos()
    {
        _currentMousePos = Input.mousePosition.x;
        if (_currentMousePos > _previousMousePos + 10 && currentItem > 0)
        {
            currentItem--;
        }
        else if (_currentMousePos < _previousMousePos - 10 && currentItem+1 < _storiesCount)
        {
            currentItem++;
        }
        _momentalSwipe = true;
    }
    public void ChangePicture(int position)
    {
        if (position > currentItem)
        {
            currentItem++;
        }
        else if(position < currentItem)
        {
            currentItem--;
        }
        _momentalSwipe = true;
        ChangePagination(currentItem);
    }
    private void ChangePagination(int currentItem)
    {
        for (int i = 0; i < _pagination.Length; i++)
        {
            if (i != currentItem)
            {
                _pagination[i].color = _mainPaginationColor;
            }
            else
            {
                _pagination[i].color = _colorForPagination;
            }
        }
    }
}
