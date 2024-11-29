using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace com.sun.game.mini
{
    public struct Cell
    {
        public int key;
        public int value;

        public static implicit operator Cell(int value)
        {
            return new Cell { value = value, key = 0 };
        }

        public static implicit operator int(Cell one)
        {
            return one.value;
        }

        public static implicit operator Cell((int, int) keyValue)
        {
            return new Cell { key = keyValue.Item1, value = keyValue.Item2 };
        }

        public static Cell operator +(Cell one, int value)
        {
            one.value += value;
            return one;
        }

        public static bool operator ==(Cell one, Cell two)
        {
            return one.key == two.key && one.value == two.value;
        }

        public static bool operator !=(Cell one, Cell two)
        {
            return one.key != two.key || one.value != two.value;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType().Equals(this.GetType()))
            {
                return this == (Cell)obj;
            }
            return false;
        }
    }

    public class Map2048 : MonoBehaviour
    {
        const string ID_GAME = "mini2048";
        public static int RandomObj;
        public int _column;
        public Vector2 _padding;
        public float _margin;
        public TMP_Text _textScore;
        public GameObject prefabEnd;
        public UnityEvent<int, int> OnComplete;
        private float _size;
        private int _row;
        private Vector2 _position;
        private Cell[,] cells;
        private int x, y;
        private GUIStyle style;
        private AssetService _assetService;
        private UserService _userService;
        private RewardService _rewardService;
        private int _otherType = 0;
        private Dictionary<(int, int), Texture2D> _doneArray = new();
        private int[] _doneTypeCount;
        private int _score;
        private bool _startGame;

        void OnEnable()
        {
            _size = (Screen.width - _padding.x * 2 - _margin * (_column - 1)) / _column;
            _row = _column;
            _position.x = Screen.width / 2 - _size * _column / 2;
            _position.y = Screen.height / 2 - _size * _row / 2;
        }

        private void Awake()
        {
            ServiceLocator.Resolve<AssetService>(x =>
            {
                _assetService = x as AssetService;
                _otherType = Random.Range(0, _assetService.Length);
                _doneTypeCount = new int[_assetService.Length];
            });
            ServiceLocator.Resolve<UserService>(x =>
            {
                _userService = x as UserService;
            });
            ServiceLocator.Resolve<RewardService>(x =>
            {
                _rewardService = x as RewardService;
            });
        }

        private void Start()
        {
            cells = new Cell[_row, _column];
            Rect rect = new Rect(_position.x, _position.y, _size, _size);
            for (y = 0; y < _row; y++)
            {
                for (x = 0; x < _column; x++)
                {
                    cells[x, y] = 0;
                    rect.x += _size;
                }
                rect.x = _position.x;
                rect.y += _size;
            }
            _startGame = false;
        }

        void StartGame()
        {
            _startGame = true;
            GenCell(GestureDirection.None);
        }

        private Vector2 GetPosition(int x, int y)
        {
            var _x = _position.x + (x - 1) * _size;
            var _y = _position.y + (y - 1) * _size;
            return new Vector2(_x, _y);
        }

        private void OnGUI()
        {
            if (!Gesture.Enable || !_startGame)
                return;
            style = new GUIStyle(GUI.skin.box);
            style.border = new RectOffset(0, 0, 0, 0);
            Rect rect = new Rect(_position.x, _position.y, _size, _size);
            for (y = 0; y < _row; y++)
            {
                for (x = 0; x < _column; x++)
                {
                    var texture = _doneArray.ContainsKey((x, y)) ? _doneArray[(x, y)] : _assetService.GetTexture(cells[x, y].key);
                    if (texture == null)
                    {
                        AnimCompleteBox(x, y);
                    }
                    style.normal.background = texture;
                    GUI.Box(rect, "", style);
                    rect.x += _size + _margin;
                }
                rect.x = _position.x;
                rect.y += _size + _margin;
            }
        }

        private void Update()
        {
            _userService.currentScore = _score;
            var gesture = Gesture.Direction;
            if (gesture != GestureDirection.None)
            {
                int selected = 0;
                bool addCell = false;
                switch (gesture)
                {
                    case GestureDirection.Left:
                        for (int y = 0; y < _row; y++)
                        {
                            selected = 0;
                            for (int x = 1; x < _column; x++)
                            {
                                if (cells[x, y] == 0 || _doneArray.ContainsKey((x, y)))
                                    continue;
                                selected = CalculateX(selected, y, x, 1, ref addCell);
                            }
                        }
                        break;
                    case GestureDirection.Right:
                        for (int y = 0; y < _row; y++)
                        {
                            selected = _column - 1;
                            for (int x = _column - 1 - 1; x >= 0; x--)
                            {
                                if (cells[x, y] == 0 || _doneArray.ContainsKey((x, y)))
                                    continue;
                                selected = CalculateX(selected, y, x, -1, ref addCell);
                            }
                        }
                        break;
                    case GestureDirection.Down:
                        for (int x = 0; x < _column; x++)
                        {
                            selected = _row - 1;
                            for (int y = _row - 1 - 1; y >= 0; y--)
                            {
                                if (cells[x, y] == 0 || _doneArray.ContainsKey((x, y)))
                                    continue;
                                selected = CalculateY(selected, x, y, -1, ref addCell);
                            }
                        }
                        break;
                    case GestureDirection.Up:
                        for (int x = 0; x < _column; x++)
                        {
                            selected = 0;
                            for (int y = 1; y < _row; y++)
                            {
                                if (cells[x, y] == 0 || _doneArray.ContainsKey((x, y)))
                                    continue;
                                selected = CalculateY(selected, x, y, 1, ref addCell);
                            }
                        }
                        break;
                }
                if (addCell)
                    GenCell(gesture);
            }
        }

        private int CalculateX(int selected, int y, int x, int add, ref bool addCell)
        {
            bool checkWhile()
            {
                if (add > 0)
                    return x > selected;
                return x < selected;
            }
            while (checkWhile())
            {
                if (cells[selected, y] == 0)
                {
                    cells[selected, y] = cells[x, y];
                    cells[x, y] = 0;
                    addCell = true;
                    break;
                }
                else if (_doneArray.ContainsKey((x, y)))
                {
                    selected += add;
                }
                else if (cells[x, y] == cells[selected, y])
                {
                    cells[selected, y]++;
                    _score += cells[selected, y];
                    _userService.SetHighScore(ID_GAME,_score);
                    cells[x, y] = 0;
                    //if (_assetService.GetTexture(cells[x, selected]) == null)
                    //{
                    //    AnimCompleteBox(x, selected);
                    //    cells[x, selected] = 0;
                    //    selected++;
                    //}
                    addCell = true;
                    break;
                }
                else
                {
                    selected += add;
                }
            }

            return selected;
        }

        private int CalculateY(int selected, int x, int y, int add, ref bool addCell)
        {

            bool checkWhile()
            {
                if (add > 0)
                    return y > selected;
                return y < selected;
            }
            while (checkWhile())
            {
                if (cells[x, selected] == 0)
                {
                    cells[x, selected] = cells[x, y];
                    cells[x, y] = 0;
                    addCell = true;
                    break;
                }
                else if (_doneArray.ContainsKey((x, y)))
                {
                    selected += add;
                }
                else if (cells[x, y] == cells[x, selected])
                {
                    cells[x, selected]++;
                    _score += cells[x, selected];
                    _userService.SetHighScore(ID_GAME, _score);
                    cells[x, y] = 0;
                    //if (_assetService.GetTexture(cells[x, selected]) == null)
                    //{
                    //    AnimCompleteBox(x, selected);
                    //    cells[x, selected] = 0;
                    //}
                    addCell = true;
                    break;
                }
                else
                {
                    selected += add;
                }
            }
            return selected;
        }

        private void AnimCompleteBox(int x, int y)
        {
            Cell cell = cells[x, y];
            _doneTypeCount[cell.key] += 1;
            //int rewardType =
            //    _rewardService.GetRewardType(_doneTypeCount[cell.key], _userService.GetPercentIncreaseRoll());
            //int rewardIndex = _mathFomularService.GetRewardIndex(rewardType);
            //var reward = _userService.UnlockCollection(rewardType, rewardIndex);
            //_doneArray.Add((x, y), reward.Detail.Thumbnail);
        }

        private void GenCell(GestureDirection direction)
        {
            List<(int, int)> list = new();
            for (x = 0; x < _column; x++)
            {
                for (y = 0; y < _row; y++)
                {
                    if (cells[x, y] == 0 && !_doneArray.ContainsKey((x, y)))
                    {
                        list.Add((x, y));
                    }
                }
            }
            if (list.Count == 0)
                EndGame();
            else
            {
                var result = list[Random.Range(0, list.Count)];
                int type = 0;
                if (RandomObj == 2)
                    type = Random.Range(0, _assetService.Length);
                else if (RandomObj == 1)
                    type = Time.frameCount % 2 == 0 ? type : type + 1;
                cells[result.Item1, result.Item2] = (type, Time.frameCount % 2 == 0 ? 1 : 2);
            }
        }

        private void EndGame()
        {
            throw new NotImplementedException();
        }
    }
}
