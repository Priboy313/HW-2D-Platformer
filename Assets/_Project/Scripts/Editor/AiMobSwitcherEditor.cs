using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

[CustomEditor(typeof(AiMobSwitcher))]
public class AiMobSwitcherEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		AiMobSwitcher switcher = (AiMobSwitcher)target;

		AIMobBase[] controllers = switcher.GetComponents<AIMobBase>();

		if (controllers.Length == 0)
		{
			EditorGUILayout.HelpBox("Add AI Controllers!", MessageType.Warning);
			
			return;
		}

		List<string> options = new List<string> { "None" };
		options.AddRange(controllers.Select(c => c.GetType().Name).ToList());

		int currentIndex = 0;

		if (switcher.CurrentController != null)
		{
			for (int i = 0; i < controllers.Length; i++)
			{
				if (switcher.CurrentController == controllers[i])
				{
					currentIndex = i + 1;
					break;
				}
			}
		}

		EditorGUILayout.Space();
		EditorGUILayout.LabelField("AI Configuration", EditorStyles.boldLabel);
		int newIndex = EditorGUILayout.Popup("Default Behavior", currentIndex, options.ToArray());
	
		if (newIndex != currentIndex)
		{
			Undo.RecordObject(switcher, "Change AI Controller");

			if (newIndex == 0)
			{
				switcher.CurrentController = null;
			}
			else
			{
				switcher.CurrentController = controllers[newIndex - 1];
			}

			EditorUtility.SetDirty(switcher);
		}

		if (Application.isPlaying && switcher.CurrentController != null)
		{
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Runtime Active:", switcher.CurrentController.GetType().Name, EditorStyles.helpBox);
		}
	}
}
