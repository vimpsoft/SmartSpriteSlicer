﻿using UnityEditor;
using UnityEngine;

namespace Vis.SpriteEditorPro
{
    internal class DraggableButtonView : ViewBase
    {
        public DraggableButtonView(SpriteEditorProWindow model) : base(model)
        {
        }

        public override void OnGUI(Rect position)
        {
            base.OnGUI(position);

            if (DragableButton.IsDragging && Event.current.type == EventType.Repaint)
            {
                var originalColor = GUI.backgroundColor;
                GUI.backgroundColor = DragableButton.DragableColor;
                DragableButton.DraggingStyle.Draw(DragableButton.DraggingPosition, DragableButton.DraggingContent, 0);
                GUI.backgroundColor = originalColor;

                EditorGUIUtility.AddCursorRect(DragableButton.AcceptDragArea, MouseCursor.ArrowPlus);
            }
        }
    }
}