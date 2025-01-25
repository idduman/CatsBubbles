using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    
    [SerializeField] private List<Color> _colorTypes;

    public Color GetColor(ColorType colorType)
    {
        if((int)colorType > _colorTypes.Count - 1)
            throw new IndexOutOfRangeException("color type index out of bound");

        return _colorTypes[(int)colorType];
    }
}