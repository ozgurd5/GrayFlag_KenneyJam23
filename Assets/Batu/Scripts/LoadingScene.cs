using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
   public GameObject LoadingScreen;
   public GameObject MainScreen;
   public GameObject enemy1;
   public GameObject enemy2;
   public GameObject enemy3;

   public void LoadScene(int sceneId)
   {
      StartCoroutine(LoadSceneAsync(sceneId));
   }

   IEnumerator LoadSceneAsync(int sceneId)
   {
      enemy1.SetActive(false);
      enemy2.SetActive(false);
      enemy3.SetActive(false);
      MainScreen.SetActive(false);
      LoadingScreen.SetActive(true);

      AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);

      while (!operation.isDone)
      {
         yield return null;
      }
   }
}
