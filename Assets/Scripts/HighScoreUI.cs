using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighScoreUI : MonoBehaviour
{
    public TMP_Text _text;
    private UserService _userService;
    void Start()
    {
        ServiceLocator.Resolve<UserService>(service =>
        {
            this._userService = service as UserService;
        });
    }

    // Update is called once per frame
    void Update()
    {
        _text.text = _userService?.HighScore(this._userService.CurrentIdGame).ToString();
    }
}
