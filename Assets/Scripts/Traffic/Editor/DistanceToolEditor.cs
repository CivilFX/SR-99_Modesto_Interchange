using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CivilFX.TrafficV5
{
    [CustomEditor(typeof(DistanceTool))]
    public class DistanceToolEditor : Editor
    {
        private DistanceTool tool;

        private SerializedObject so;
        private float dis;
        private void OnEnable()
        {
            tool = (DistanceTool)target;

            so = serializedObject;

            so.Update();

            var sp = so.FindProperty("nodes");


            if (sp.arraySize == 0)
            {
                sp.ClearArray();
                sp.InsertArrayElementAtIndex(0);
                sp.InsertArrayElementAtIndex(1);

                sp.GetArrayElementAtIndex(0).vector3Value = tool.transform.position;
                sp.GetArrayElementAtIndex(1).vector3Value = tool.transform.position + new Vector3(5, 0, 5);
            }
            so.ApplyModifiedProperties();
            
        }

        public override void OnInspectorGUI()
        {
            so.Update();
            SerializedProperty scriptNameProp = so.FindProperty("m_Script");
            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUILayout.PropertyField(scriptNameProp);
            }

            var pp = serializedObject.FindProperty("nodes");
            EditorGUILayout.PropertyField(pp, true);

            pp = serializedObject.FindProperty("drawSphere");
            EditorGUILayout.PropertyField(pp, true);

            so.ApplyModifiedProperties();
        }


        private void OnSceneGUI()
        {
            so.Update();

            var node0 = so.FindProperty("nodes").GetArrayElementAtIndex(0);
            var node1 = so.FindProperty("nodes").GetArrayElementAtIndex(1);

            //node 0
            Handles.Label(node0.vector3Value, new GUIContent("Begin"), new GUIStyle{ fontSize = 20, fontStyle = FontStyle.Bold});
            node0.vector3Value = Handles.PositionHandle(node0.vector3Value, Quaternion.identity);

            //node 1
            Handles.Label(node1.vector3Value, new GUIContent("End"), new GUIStyle { fontSize = 20, fontStyle = FontStyle.Bold});
            node1.vector3Value = Handles.PositionHandle(node1.vector3Value, Quaternion.identity);


            dis = Vector3.Distance(node0.vector3Value, node1.vector3Value);
            Handles.Label(Vector3.Lerp(node0.vector3Value, node1.vector3Value, 0.5f), new GUIContent(dis.ToString()), new GUIStyle { fontSize = 30, fontStyle = FontStyle.Bold });
            Handles.DrawLine(node0.vector3Value, node1.vector3Value);

            so.ApplyModifiedProperties();
        }

    }



}