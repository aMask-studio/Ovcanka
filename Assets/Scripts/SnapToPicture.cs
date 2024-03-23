using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnapToPicture : MonoBehaviour
{
    [SerializeField] ScrollRect _scrollRect;
    [SerializeField] RectTransform _contentPanel;
    [SerializeField] RectTransform _sampleListPictures;
    [SerializeField] HorizontalLayoutGroup _hlg;

    [SerializeField] Image[] _pagination;
    [SerializeField] Color[] _colorsForPagination;
    [SerializeField] Color _mainPaginationColor;

    [SerializeField] GameObject _btnLeft;
    [SerializeField] GameObject _btnRight;

    [SerializeField] float _snapForce;
    public int currentItem;

    bool _isSnapped;
    bool _momentalSwipe;
    float _snapSpeed;
    float _previousMousePos;
    float _currentMousePos;
    void OnEnable()
    {
        _contentPanel.localPosition = new Vector3(0 - (currentItem * (_sampleListPictures.rect.width + _hlg.spacing)), 
            _contentPanel.localPosition.y,
            _contentPanel.localPosition.z);
        ChangePagination(currentItem);
        currentItem = 0;
    }
    private void OnDisable()
    {
        _momentalSwipe = false;
        _isSnapped = false;
        _snapSpeed = 0;
        _btnLeft.SetActive(true);
        _btnRight.SetActive(true);
    }
    void Update()
    {
        //_previousItem = currentItem;
        if(!_momentalSwipe)
            currentItem = Mathf.RoundToInt((0 - _contentPanel.localPosition.x / (_sampleListPictures.rect.width + _hlg.spacing)));
        /*if (currentItem != _previousItem)
            ChangePagination(currentItem);*/

        /*if (Input.GetMouseButtonDown(0))
            _previousMousePos = Input.mousePosition.x;
        if (Input.GetMouseButtonUp(0))
        {
            _currentMousePos = Input.mousePosition.x;
            if(_currentMousePos > _previousMousePos && currentItem > 0)
            {
                currentItem--;
            }
            else if(_currentMousePos < _previousMousePos && currentItem < 5)
            {
                currentItem++;
            }
            _isTouched = true;
            _momentalSwipe = true;
        }*/
        if (_momentalSwipe) //(_scrollRect.velocity.magnitude < 600 && !_isSnapped && _isTouched == true) || 
        {
            _isSnapped = true;
            ChangePagination(currentItem);
            _scrollRect.velocity = Vector2.zero;
            _snapSpeed += _snapForce * Time.deltaTime;
            _contentPanel.localPosition = new Vector3(
                Mathf.MoveTowards(_contentPanel.localPosition.x, 0 - (currentItem * (_sampleListPictures.rect.width + _hlg.spacing)),_snapSpeed/150),
                _contentPanel.localPosition.y,
                _contentPanel.localPosition.z);
            _contentPanel.localPosition = Vector3.Lerp(_contentPanel.localPosition, new Vector3(
                0 - (currentItem * (_sampleListPictures.rect.width + _hlg.spacing)),
                _contentPanel.localPosition.y,
                _contentPanel.localPosition.z),_snapSpeed/90);
            if(_contentPanel.localPosition.x == 0 - (currentItem* (_sampleListPictures.rect.width + _hlg.spacing)))
            {
                _momentalSwipe = false;
                _isSnapped = false;
                _snapSpeed = 0;
            }
        }
        /*if (_scrollRect.velocity.magnitude > 600)
        {
            _isSnapped = false;
            _snapSpeed = 0;
        }*/
    }
    public void SetPreviousPos()
    {
        _previousMousePos = Input.mousePosition.x;
    }
    public void SetCurrentPos()
    {
        if (_isSnapped)
            return;
        _currentMousePos = Input.mousePosition.x;
        if (_currentMousePos > _previousMousePos + 10 && currentItem > 0)
        {
            currentItem--;
        }
        else if (_currentMousePos < _previousMousePos - 10 && currentItem < 5)
        {
            currentItem++;
        }
        //_isSnapped = true;
        _momentalSwipe = true;
    }
    public void ChangePicture(bool isLeft)
    {
        if (_isSnapped)
            return;
        if (isLeft)
        {
            currentItem--;
        }
        else
        {
            currentItem++;
        }
        //_isSnapped = true;
        _momentalSwipe = true;
        ChangePagination(currentItem);
    }
    private void ChangePagination(int currentItem)
    {
        CheckPictureId();
        for (int i = 0; i < _pagination.Length; i++)
        {
            if(i != currentItem)
            {
                _pagination[i].color = _mainPaginationColor;
            }
            else
            {
                _pagination[i].color = _colorsForPagination[i];
            }
        }
    }
    private void CheckPictureId()
    {
        if (currentItem == 0)
            _btnLeft.SetActive(false);
        else if(currentItem == 5)
            _btnRight.SetActive(false);
        else
        {
            _btnLeft.SetActive(true);
            _btnRight.SetActive(true);
        }
    }
}
