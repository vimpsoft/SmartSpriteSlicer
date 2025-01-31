﻿using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Vis.SpriteEditorPro
{
    [Serializable]
    public struct SpriteChunk
    {
        [SerializeField]
        private string _name;
        public string Name => _name;
        [SerializeField]
        private int _id;
        public int Id => _id;
        [SerializeField]
        private Color _color;
        public Color Color => _color;
        [SerializeField]
        private Color _textColor;
        public Color TextColor => _textColor;
        [SerializeField]
        private Vector2Int _size;
        public Vector2Int Size => _size;

        public string GetHumanFriendlyName() => string.IsNullOrEmpty(_name) ? $"{_size.x}x{_size.y}" : Name;

        /// <summary>
        /// Ctor with only neccessary properties
        /// </summary>
        /// <param name="id"></param>
        /// <param name="size"></param>
        public SpriteChunk(int id, Vector2Int size)
        {
            _id = id;
            _size = size;
            _color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            _textColor = Color.white;
            _name = string.Empty;
        }

        /// <summary>
        /// Ctor with full set of properties
        /// </summary>
        /// <param name="id"></param>
        /// <param name="size"></param>
        /// <param name="color"></param>
        /// <param name="name"></param>
        public SpriteChunk(int id, Vector2Int size, Color color, Color textColor, string name)
        {
            _id = id;
            _name = name;
            _size = size;
            _color = color;
            _textColor = textColor;
        }

        public SpriteChunk SetSize(Vector2Int newSize) => new SpriteChunk(_id, newSize, _color, _textColor, _name);
        public SpriteChunk SetColor(Color newColor) => new SpriteChunk(_id, _size, newColor, _textColor, _name);
        public SpriteChunk SetTextColor(Color newColor) => new SpriteChunk(_id, _size, _color, newColor, _name);
        public SpriteChunk SetName(string newName) => new SpriteChunk(_id, _size, _color, _textColor, newName);
    }
}
