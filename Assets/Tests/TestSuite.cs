using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;
using UnityEngine.SceneManagement;

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
    public IEnumerator SetPlayScore()
    {
        GameManager.gameManager.LoadScene(2);

        yield return new WaitForSeconds(1f);

        EncodedData encodedData = GameManager.dataManager.GetGameData();
        Assert.Greater(encodedData.score, 0);
    }
}
