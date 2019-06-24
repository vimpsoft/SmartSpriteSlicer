﻿using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    internal class ControlPanelView : ViewBase
    {
        private readonly GUIStyle _backgroundStyle;
        private readonly ControlPanelWindow _subWindow;

        public ControlPanelView(SmartSpriteSlicerWindow model) : base(model)
        {
            _backgroundStyle = model.Skin.GetStyle("ControlPanel");
            _subWindow = new ControlPanelWindow(model);
        }

        public override void OnGUI(Rect position)
        {
            base.OnGUI(position);

            //_model.BeginWindows();
            _model.ControlPanelRect = GUILayout.Window(0, _model.ControlPanelRect, _subWindow.WindowContentCallback, new GUIContent("Layout Settings"));
            if (_model.ControlPanelRect.x < 0)
                _model.ControlPanelRect.x = 0;
            if (_model.ControlPanelRect.y < 0)
                _model.ControlPanelRect.y = 0;
            if (_model.ControlPanelRect.width != SmartSpriteSlicerWindow.MaxContolPanelWidth)
                _model.ControlPanelRect.width = SmartSpriteSlicerWindow.MaxContolPanelWidth;
            if (_model.ControlPanelRect.x >= _model.position.width - _model.ControlPanelRect.width)
                _model.ControlPanelRect.x = _model.position.width - _model.ControlPanelRect.width;
            if (_model.ControlPanelRect.y >= _model.position.height - 16)
                _model.ControlPanelRect.y = _model.position.height - 16;
            var yMax = _model.ControlPanelRect.yMax;
            _model.ControlPanelRect.height = 0;

            var windowRect = _model.ControlPanelRect;
            windowRect.yMax = yMax;
            if (Event.current.type == EventType.MouseUp && windowRect.Contains(Event.current.mousePosition))
                Event.current.Use();
        }
    }
}
