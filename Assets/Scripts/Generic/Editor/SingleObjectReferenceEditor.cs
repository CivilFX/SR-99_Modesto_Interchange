using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace CivilFX.Generic2
{

    [CustomEditor(typeof(SingleObjectReference))]
    public class SingleObjectReferenceEditor : Editor
    {
        SerializedObject so;

        private void OnEnable()
        {
            so = serializedObject;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            so.Update();

            if (so.FindProperty("referencedObject").objectReferenceValue == null) {
                so.FindProperty("referencedObject").objectReferenceValue = ((SingleObjectReference)target).gameObject;
            }

            if (string.IsNullOrEmpty(so.FindProperty("name").stringValue)) {
                so.FindProperty("name").stringValue = ((SingleObjectReference)target).gameObject.name;
            }

            so.ApplyModifiedProperties();

        }
    }
}