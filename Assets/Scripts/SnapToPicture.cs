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

    [SerializeField] float _snapForce;
    public int currentItem;

    bool _isSnapped;
    float _snapSpeed;
    int _previousItem;
    void OnEnable()
    {
        _contentPanel.localPosition = new Vector3(0 - (currentItem * (_sampleListPictures.rect.width + _hlg.spacing)), 
            _contentPanel.localPosition.y,
            _contentPanel.localPosition.z);
        ChangePagination(currentItem);
        currentItem = 0;
        _isSnapped = false;
    }
    void Update()
    {
        _previousItem = currentItem;
        currentItem = Mathf.RoundToInt((0 - _contentPanel.localPosition.x / (_sampleListPictures.rect.width + _hlg.spacing)));
        if (currentItem != _previousItem)
            ChangePagination(currentItem);
        if(_scrollRect.velocity.magnitude < 300 && !_isSnapped)
        {
            _scrollRect.velocity = Vector2.zero;
            _snapSpeed += _snapForce * Time.deltaTime;
            _contentPanel.localPosition = new Vector3(
                Mathf.MoveTowards(_contentPanel.localPosition.x, 0 - (currentItem * (_sampleListPictures.rect.width + _hlg.spacing)),_snapSpeed),
                _contentPanel.localPosition.y,
                _contentPanel.localPosition.z);
            if(_contentPanel.localPosition.x == 0 - (currentItem* (_sampleListPictures.rect.width + _hlg.spacing)))
                _isSnapped = true;
        }
        if (_scrollRect.velocity.magnitude > 300)
        {
            _isSnapped = false;
            _snapSpeed = 0;
        }
    }
    private void ChangePagination(int currentItem)
    {
        for (int i = 0; i < _pagination.Length; i++)
        {
            if(i != currentItem)
            {
                _pagination[i].color = new Color(0.5490196f, 0.5529412f, 0.5607843f);
            }
            else
            {
                _pagination[i].color = _colorsForPagination[i];
            }
        }
    }
}
