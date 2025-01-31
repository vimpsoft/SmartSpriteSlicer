﻿using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Vis.SpriteEditorPro
{
    internal class GroupsMainPanelView : LayoutViewBase
    {
        private readonly GUIStyle _panelStyle;
        private readonly GUIStyle _panelDragAcceptanceStyle;
        private readonly GUIStyle _blobStyle;
        private readonly GUIStyle _selectedBlobStyle;

        public GroupsMainPanelView(SpriteEditorProWindow model) : base(model)
        {
            _panelStyle = model.Skin.GetStyle("GroupsMainPanel");
            _panelDragAcceptanceStyle = model.Skin.GetStyle("GroupsMainPanelDragAcceptence");
            _blobStyle = model.Skin.GetStyle("BlobStyle");
            _selectedBlobStyle = model.Skin.GetStyle("SelectedBlobStyle");
        }

        public override void OnGUILayout()
        {
            base.OnGUILayout();

            EditorGUILayout.BeginVertical(DragableButton.IsDragging && DragableButton.ReadyToDrop ? _panelDragAcceptanceStyle : _panelStyle);
            if (_model.SlicingSettings.ChunkGroups.Count == 0)
            {
                if (_model.SlicingSettings.Chunks.Count == 0)
                    EditorGUILayout.LabelField(new GUIContent($"<i><color=#000000>Groups panel empty.</color></i>"), _model.RichTextStyle);
                else
                    EditorGUILayout.LabelField(new GUIContent($"<i><color=#000000>Drag'n'Drop some chunks here to create a group.</color></i>"), _model.RichTextStyle);
            }
            else
            {
                var reorderableListResult = ReorderableBlobList.Draw(_model.SlicingSettings.ChunkGroups, _model.SelectedGroupIndex, (int)WindowWidth - 30, getBlobContent, getBlobColor, getBlobStyle, getSelectedBlobStyle);
                if (reorderableListResult.clicked.Id != 0)
                {

                }
                if (reorderableListResult.reordered)
                    Undo.RecordObject(_model.SlicingSettings, $"Chunk groups reordered");
                _model.SlicingSettings.ChunkGroups = reorderableListResult.list;
                if (reorderableListResult.reordered)
                {
                    if (_model.SelectedGroupIndex >= 0)
                        _model.SelectedGroupIndex = reorderableListResult.selected;
                    _model.SlicingSettings.UpdateScriptableSlicingLayoutHash();
                    _model.Repaint();
                    EditorUtility.SetDirty(_model.SlicingSettings);
                }
                else
                    _model.SelectedGroupIndex = reorderableListResult.selected;
                if (reorderableListResult.clicked.Id != 0 && !reorderableListResult.reordered)
                    onGroupClick(reorderableListResult.clicked);
            }
            EditorGUILayout.EndVertical();
        }

        private Color getBlobColor(SpriteGroup group)
        {
            switch (group.Flavor)
            {
                case SpriteGroupFlavor.Group:
                    return _model.SlicingSettings.Chunks.Where(chunk => chunk.Id == group.ChunkId).First().Color;
                case SpriteGroupFlavor.EndOfLine:
                case SpriteGroupFlavor.EmptySpace:
                default:
                    return Color.white * 0.95f;
            }
        }

        private GUIContent getBlobContent(SpriteGroup group)
        {
            switch (group.Flavor)
            {
                case SpriteGroupFlavor.Group:
                    return getBlobContent(group, _model.SlicingSettings.Chunks.Where(chunk => chunk.Id == group.ChunkId).First());
                case SpriteGroupFlavor.EndOfLine:
                    return new GUIContent($"<color=#000000><i>End of line</i></color>");
                case SpriteGroupFlavor.EmptySpace:
                default:
                    return new GUIContent($"<color=#000000><i>Empty space</i></color>");
            }
        }

        private GUIStyle getSelectedBlobStyle(SpriteGroup group) => formatStyle(_selectedBlobStyle, group);
        private GUIStyle getBlobStyle(SpriteGroup group) => formatStyle(_blobStyle, group);
        private GUIStyle formatStyle(GUIStyle style, SpriteGroup group)
        {
            switch (group.Flavor)
            {
                case SpriteGroupFlavor.Group:
                    style.normal.textColor = _model.SlicingSettings.Chunks.Where(chunk => chunk.Id == group.ChunkId).First().TextColor;
                    break;
                case SpriteGroupFlavor.EndOfLine:
                    style.normal.textColor = Color.black;
                    break;
                case SpriteGroupFlavor.EmptySpace:
                    style.normal.textColor = Color.white * 0.25f;
                    break;
                default:
                    break;
            }
            return style;
        }

        private void onGroupClick(SpriteGroup group)
        {
            if (_model.EditedGroupId == group.Id)
            {
                _model.EditedGroupId = 0;
                _model.SelectedGroupIndex = -1;
            }
            else
                _model.EditedGroupId = group.Id;
        }

        private GUIContent getBlobContent(SpriteGroup group, SpriteChunk chunk)
        {
            if (group.UseCustomName)
                return new GUIContent(group.CustomName);
            return new GUIContent($"{chunk.GetHumanFriendlyName()}");
        }
    }
}