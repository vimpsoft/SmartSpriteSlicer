﻿using UnityEditor;
using UnityEditor.Presets;
using UnityEngine;

namespace Vis.SpriteEditorPro
{
    internal class TopPanelView : LayoutViewBase
    {
        private readonly GUIStyle _panelStyle;

        private float _validWidthCache;

        public TopPanelView(SpriteEditorProWindow model) : base(model)
        {
            _panelStyle = _model.Skin.GetStyle("ControlTopPanel");
        }

        public override void OnGUILayout()
        {
            base.OnGUILayout();

            EditorGUILayout.BeginVertical(_panelStyle);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(new GUIContent($"<i><size=14>Preset:</size></i>", "Saved layout info"), _model.RichTextStyle, GUILayout.Width(58f));
            var newPreset = (Preset)EditorGUILayout.ObjectField(_model.SlicingSettingsPreset, typeof(Preset), false, GUILayout.Width(128f));
            if (newPreset != _model.SlicingSettingsPreset)
            {
                Undo.RecordObject(_model, $"Preset changed");
                _model.SlicingSettingsPreset = newPreset;
                EditorUtility.SetDirty(_model);
            }
            if (_model.SlicingSettingsPreset != null)
            {
                if (!_model.SlicingSettingsPreset.DataEquals(_model.SlicingSettings))
                {
                    EditorGUILayout.LabelField(new GUIContent($"*modified"), _model.RichTextStyle, GUILayout.Width(68f));
                    if (GUILayout.Button(new GUIContent($"Save", "Save values to preset")))
                    {
                        if (Selection.activeObject == _model.SlicingSettingsPreset)
                            Selection.activeObject = null; //There's an issue when preset we're saving into is opened in inspector: saving wouldn't work, inspector somehow overrides it's values with it's own. So we just close inspector in that case in order to be able to save.
                        Undo.RecordObject(_model.SlicingSettingsPreset, "Preset values updated");
                        _model.SlicingSettingsPreset.UpdateProperties(_model.SlicingSettings);
                        EditorUtility.SetDirty(_model.SlicingSettingsPreset);
                        AssetDatabase.SaveAssets();
                    }
                    if (GUILayout.Button(new GUIContent($"Reset", "Apply preset values")))
                    {
                        _model.SlicingSettingsPreset.ApplyTo(_model.SlicingSettings);
                    }
                }
            }
            if (GUILayout.Button(new GUIContent($"Save as...")))
            {
                var absolutePath = EditorUtility.SaveFilePanel($"Save preset as", Application.dataPath, $"SpriteSlicePreset", "preset");
                var relativePath = $"Assets/{absolutePath.Substring(Application.dataPath.Length)}";
                var preset = new Preset(_model.SlicingSettings);
                AssetDatabase.CreateAsset(preset, relativePath);
                Undo.RecordObject(_model, $"Preset changed");
                _model.SlicingSettingsPreset = preset;
                EditorUtility.SetDirty(_model);
                AssetDatabase.SaveAssets();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (!ControlPanelWindow.Extracted && GUILayout.Button(new GUIContent($"Extract windows", $"Make windows hover")))
            {
                ControlPanelWindow.Extracted = true;
                SpritePreviewWindow.Extracted = true;
            }
            else if (ControlPanelWindow.Extracted && GUILayout.Button(new GUIContent($"Embed windows", $"Make windows embedded in main window")))
            {
                ControlPanelWindow.Extracted = false;
                SpritePreviewWindow.Extracted = false;
            }
            if ((_model.ControlPanelTab == ControlPanelTabs.ManualSlicing && !_model.SlicingSettings.HaveChunkGroups()) ||
                (_model.ControlPanelTab == ControlPanelTabs.ScriptableSclicing &&
                (string.IsNullOrEmpty(_model.SlicingSettings.ScriptabeSlicingTestText) ||
                !_model.SlicingSettings.HasWholeSetOfNodes() ||
                !_model.SlicingSettings.HasAllNodesSeparated() ||
                !_model.SlicingSettings.NodesDeepTestPassed())))
                GUI.enabled = false;
            if (GUILayout.Button(new GUIContent($"Apply", $"Slice texture")))
                _model.Slice();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }
    }
}