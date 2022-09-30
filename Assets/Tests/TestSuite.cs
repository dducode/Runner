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
    public IEnumerator SetPlayScore()
    {
        Managers.gameManager.LoadScene(2);

        yield return new WaitForSeconds(1f);

        EncodedData encodedData = Managers.dataManager.GetData();
        Assert.Greater(encodedData.score, 0);
    }
}
