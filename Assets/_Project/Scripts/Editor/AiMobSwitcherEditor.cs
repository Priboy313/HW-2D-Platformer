using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

[CustomEditor(typeof(AIMobSwitcher))]
public class AIMobSwitcherEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		AIMobSwitcher switcher = (AIMobSwitcher)target;

		AIMobBase[] aiControllers = switcher.GetComponents<AIMobBase>();

		if (aiControllers.Length == 0)
		{
			EditorGUILayout.HelpBox("Add AI Controllers!", MessageType.Warning);
			
			return;
		}

		EditorGUILayout.Space();
		EditorGUILayout.LabelField("AI Configuration", EditorStyles.boldLabel);

		DrawAIControllerSelector(switcher, aiControllers);
		DrawAggressiveToggle(switcher);

		DrawRuntimeInfo(switcher);
	}

	private void DrawAIControllerSelector(AIMobSwitcher switcher, AIMobBase[] controllers)
	{
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

		int newIndex = EditorGUILayout.Popup("Default Behavior", currentIndex, options.ToArray());

		if (newIndex != currentIndex)
		{
			Undo.RecordObject(switcher, "Change AI Controller");

			switcher.CurrentController = newIndex == 0 ? null : controllers[newIndex - 1];

			EditorUtility.SetDirty(switcher);
		}
	}

	private void DrawAggressiveToggle(AIMobSwitcher switcher)
	{
		bool newAggressive = EditorGUILayout.Toggle("Is Aggressive", switcher.IsAggressive);

		if (newAggressive != switcher.IsAggressive)
		{
			Undo.RecordObject(switcher, "Toggle Aggressive");
			switcher.IsAggressive = newAggressive;
			EditorUtility.SetDirty(switcher);
		}
	}

	private void DrawRuntimeInfo(AIMobSwitcher switcher)
	{
		if (Application.isPlaying && switcher.CurrentController != null)
		{
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Runtime Info", EditorStyles.boldLabel);

			using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
			{
				EditorGUILayout.LabelField("Current State:", switcher.CurrentController.GetType().Name, EditorStyles.helpBox);
				EditorGUILayout.LabelField("Aggressive Mode:", switcher.IsAggressive ? "ON" : "OFF", EditorStyles.miniLabel);
			}
		}
	}
}
