using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Sequence = DG.Tweening.Sequence;

public abstract class AbstractUI : MonoBehaviour
{
    [System.Serializable]
    public enum EaseType
    {
        None, Fade, Zoom
    }

    [SerializeField] Transform _mainPanel;
    [SerializeField] EaseType _easeType;
    [SerializeField] Ease _easeIn;
    [SerializeField] Ease _easeOut;
    [SerializeField] float _duration = 0.25f;
    bool _active;
    float _fadeFrom, _fadeTo;
    CanvasGroup _canvasGroup;
    Sequence _sequence;
    private Guid uid;
    public UnityAction OnEnableUI;
    public UnityAction OnDisableUI;

    public bool Active
    {
        get
        {
            return _active;
        }
    }

    protected void OnEnable()
    {
        AnimationScene(true);
        AfterEnable();
        OnEnableUI?.Invoke();
    }

    public virtual void Open(object @object)
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        AnimationScene(false);
    }

    private void AnimationScene(bool active)
    {
        if (_sequence != null)
        {
            _sequence.Kill(false);
            _sequence = null;
        }
        _active = active;
        Ease current = active ? _easeIn : _easeOut;
        DoAnimation(_active, current);
    }

    private void DoAnimation(bool active, Ease current)
    {
        switch (_easeType)
        {
            case EaseType.None:
                {
                    _mainPanel.gameObject.SetActive(active);
                    break;
                }
            case EaseType.Fade:
                {
                    _fadeFrom = active ? 0 : 1;
                    _fadeTo = 1 - _fadeFrom;
                    if (_canvasGroup == null)
                    {
                        _canvasGroup = _mainPanel.AddComponent<CanvasGroup>();
                    }
                    _canvasGroup.alpha = _fadeFrom;
                    _mainPanel.gameObject.SetActive(true);
                    StartSequeneAnimation(_canvasGroup.DOFade(_fadeTo, _duration)
                        .SetEase(current)
                        .OnComplete(() =>
                    {
                        if (!_active)
                        {
                            AfterDisable();
                        }
                    }));
                    break;
                }
            case EaseType.Zoom:
                {
                    _fadeFrom = active ? 0 : 1;
                    _fadeTo = 1 - _fadeFrom;
                    _mainPanel.localScale = Vector3.one * _fadeFrom;
                    _mainPanel.gameObject.SetActive(true);
                    StartSequeneAnimation(_mainPanel.DOScale(_fadeTo, _duration)
                        .SetEase(current)
                        .OnComplete(() =>
                    {
                        if (!_active)
                        {
                            AfterDisable();
                        }
                    }));
                    break;
                }
        }
    }

    private void StartSequeneAnimation<T1, T2, TPlugOptions>(TweenerCore<T1, T2, TPlugOptions> tweener) where TPlugOptions : struct, IPlugOptions
    {
        if (_sequence == null) // only create if there was none before.
        {
            _sequence = DOTween.Sequence();
            _sequence.Append(tweener);
            uid = System.Guid.NewGuid();
            _sequence.id = uid;
            Debug.Log("sequence id now:" + _sequence.id);
        }
        _sequence.Play();
    }

    public virtual void AfterEnable()
    {
    }
    public virtual void AfterDisable()
    {
        _mainPanel.gameObject.SetActive(false);
        DOTween.Kill(uid);
        _sequence = null;
        OnDisableUI.Invoke();
    }

}
