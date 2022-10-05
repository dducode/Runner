using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;
using UnityEngine.SceneManagement;
using Assets.Scripts.Security;

public class TestSuite
{
    [SetUp]
    public void Setup()
    {
        SceneManager.LoadScene(0);
    }

    [TearDown]
    public void TearDown()
    {
        
    }

    [UnityTest]
    public IEnumerator SetPlayerScore()
    {
        Managers.gameManager.LoadScene(2);

        yield return new WaitWhile(() => SceneManager.GetActiveScene().buildIndex != 2);

        EncodedData encodedData = Managers.dataManager.GetData();
        Assert.Greater(encodedData.score, 0);
    }

    [UnityTest]
    public IEnumerator CollectCoin()
    {
        Managers.gameManager.LoadScene(2);

        yield return new WaitWhile(() => SceneManager.GetActiveScene().buildIndex != 2);

        EncodedData encodedData = Managers.dataManager.GetData();
        int moneys = encodedData.money;
        GameObject coinPrefab = AssetDatabase.LoadAssetAtPath(
            "Assets/Prefabs/Others/Coin.prefab", typeof(GameObject)) as GameObject;
        GameObject coin = MonoBehaviour.Instantiate(coinPrefab);
        Transform player = MonoBehaviour.FindObjectOfType<PlayerMovement>().transform;
        coin.transform.position = player.position;

        yield return new WaitForFixedUpdate();

        encodedData = Managers.dataManager.GetData();
        Assert.Greater(encodedData.money, moneys);
    }

    [UnityTest]
    public IEnumerator CollectBronzeStar()
    {
        Managers.gameManager.LoadScene(2);

        yield return new WaitWhile(() => SceneManager.GetActiveScene().buildIndex != 2);

        GameObject starPrefab = AssetDatabase.LoadAssetAtPath(
            "Assets/Prefabs/Stars/Bronze_star.prefab", typeof(GameObject)) as GameObject;
        GameObject star = MonoBehaviour.Instantiate(starPrefab);
        Transform player = MonoBehaviour.FindObjectOfType<PlayerMovement>().transform;
        star.transform.position = player.position;

        yield return new WaitForFixedUpdate();

        EncodedData encodedData = Managers.dataManager.GetData();
        bool condition = encodedData.multiplierBonus is 2;
        Assert.IsTrue(condition);
    }
}
