using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Scene
{
    public class SceneController : Singleton<SceneController> {

        [SerializeField, Scene] private string Bootstrap;
        
        [SerializeField] private GameObject loadingScreen;
        //[SerializeField] private Image progressBar;

        private Queue<SceneCommand> commands = new();
        private Coroutine cr_operations;

        public void LoadScene(String sceneName) {

            if (IsSceneLoaded(sceneName)) {
                Debug.LogWarning($"{sceneName} is already loaded.");
                return;
            }

            commands.Enqueue(new SceneCommand(CommandType.Load,sceneName));
        }

        public void LoadScenes(List<String> scene_names) {
            foreach (String s in scene_names) {
                LoadScene(s);
            }
        }

        public void UnloadScene(string sceneName) {
            if (!IsSceneLoaded(sceneName)) {
                Debug.LogWarning($"{sceneName} isn't loaded.");
                return;
            }

            commands.Enqueue(new SceneCommand(CommandType.Unload,sceneName));
        }

        public void UnloadScenes()
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.name.Equals(Bootstrap)) continue;
                UnloadScene(scene.name);
            }
        }

        public void Process() {
            if (cr_operations == null) cr_operations = StartCoroutine(ProcessCR());
            else Debug.LogWarning("SceneController already working.");
        }

        private IEnumerator ProcessCR() {
            loadingScreen.SetActive(true);
            int totalCommand = commands.Count;
            float previousProgress = 0;
            
            while (commands.Count > 0) {
                SceneCommand sceneCommand = commands.Peek();
                String sceneName = sceneCommand.sceneName;

                AsyncOperation operation;
                if (sceneCommand.command == CommandType.Load) {
                    operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                }
                else {
                    operation = SceneManager.UnloadSceneAsync(sceneName);
                }
                
                while (!operation.isDone) {
                    /*float progress = Mathf.Clamp01(operation.progress / 0.9f);
                    float totalProgress = previousProgress + progress / totalCommand;
                    progressBar.DOFillAmount(totalProgress, 0.2f);*/
                    yield return null;
                }

                previousProgress += 1f / totalCommand;
                commands.Dequeue();
            }
            
            //progressBar.fillAmount = 0;
            loadingScreen.SetActive(false);

            cr_operations = null;
        }

        private bool IsSceneLoaded(String sceneName) {
            return SceneManager.GetSceneByName(sceneName).IsValid();
        }
        
        private enum CommandType {
            Load, Unload
        }
        
        private class SceneCommand {
            public CommandType command;
            public string sceneName;

            public SceneCommand(CommandType command, string sceneName) {
                this.command = command;
                this.sceneName = sceneName;
            }
        }
    }
}
