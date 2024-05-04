using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace DMZ.Editor
{
	public class PlayFromScene : EditorWindow
	{
		[MenuItem("Tools/Play From Scene")]
		private static void Open()
		{
			GetWindow<PlayFromScene>();
		}

		private void OnGUI()
		{
			EditorSceneManager.playModeStartScene = (SceneAsset)EditorGUILayout.ObjectField(
				new GUIContent("Play from this scene"),
				EditorSceneManager.playModeStartScene, typeof(SceneAsset), false) as SceneAsset;

			GUILayout.BeginHorizontal();
			GUILayout.Label($"current selected start scene: \n{EditorSceneManager.playModeStartScene}");
			if (GUILayout.Button("Clear")) EditorSceneManager.playModeStartScene = null;
			if (GUILayout.Button("Close")) Close();
			
			GUILayout.EndHorizontal();
		}
	}
}

// public class SceneSwitchPlay : EditorWindow
// {
// 	private static string currentScene;
//
// 	[MenuItem("Play/Play_StartUp")]
// 	public static void RunMainScene()
// 	{
// 		//EditorSceneManager.Getscene
// 		currentScene = EditorApplication.currentScene;
// 		// save scene name
// 		// on stop play load current scene name
//
// 		// what to do if current scene has changes? save it or not?
// 		
// 		
// 		EditorApplication.OpenScene("Assets/Scenes/StartUp.unity");
// 		EditorApplication.isPlaying = true;
// 		
// 		EditorSceneManager.sceneOpening
// 	}
// }

/*public static class PlayButtonScript //: Editor
{
	private const string STARTUP_SCENE = "Assets/Scenes/StartUp.unity";
	private static string lastEditorScene;

	[MenuItem("Play/Play_StartUp")]
	public static void PlayFromStartUpScene()
	{
		EditorPrefs.SetString("lastEditorScene", EditorSceneManager.GetActiveScene().path);

		//currentSceneName = EditorApplication.currentScene;
		lastEditorScene = SceneManager.GetActiveScene().path;

		if (SceneManager.GetActiveScene().isDirty)
		{
			if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
			{
				return;
			}
		}

		// var openScene = EditorSceneManager.OpenScene(STARTUP_SCENE, OpenSceneMode.Single);
		// //EditorSceneManager.playModeStartScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(STARTUP_SCENE);
		//
		// if (openScene.IsValid())
		// //if (true)
		// {
		// 	EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
		// 	EditorApplication.isPlaying = true;
		// }
		// else
		// {
		// 	Debug.LogError($"Could not start scene {STARTUP_SCENE}");
		// }

		// EditorSceneManager.playModeStartScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(STARTUP_SCENE);
		// EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
			// EditorApplication.isPlaying = true;

		// Загрузка сцены
		var myWantedStartScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(STARTUP_SCENE);

		// Установка стартовой сцены
		if (myWantedStartScene != null)
		{
			EditorSceneManager.playModeStartScene = myWantedStartScene;
		}
		else
		{
			Debug.Log("Could not find Scene asset: " + STARTUP_SCENE);
		}
	}

	private static void OnPlayModeStateChanged(PlayModeStateChange state)
	{
		Debug.LogError($"state switched {state}");
		//if (state == PlayModeStateChange.ExitingPlayMode)
		if (state == PlayModeStateChange.EnteredEditMode)
		{
			EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
			Debug.LogWarning($"Return to scene name {lastEditorScene}");
			//EditorSceneManager.OpenScene(lastEditorScene, OpenSceneMode.Single);

			//EditorSceneManager.OpenScene(EditorPrefs.GetString("PreviousScene", "Assets/Scenes/DefaultScene.unity"));
			//EditorSceneManager.OpenScene(EditorPrefs.GetString("lastEditorScene"));
			EditorSceneManager.LoadScene(EditorSceneManager.playModeStartScene.name);
		}
	}
}*/
