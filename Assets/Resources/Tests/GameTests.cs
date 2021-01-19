using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

namespace Tests
{
    public class GameTests
    {
        public GridHandler gridHandler;
        public InputHandler inputHandler;
        public string sceneName = "2048";

        #region Helpers
        public IEnumerator ResetScene()
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
            var waitForScene = new WaitForSceneLoaded(sceneName);
            yield return waitForScene;

            gridHandler = GameObject.Find("Grid")?.GetComponent<GridHandler>();
            inputHandler = GameObject.Find("Grid")?.GetComponent<InputHandler>();
            gridHandler.isTestSetup = true;
            gridHandler.ResetGrid();
            yield return new WaitForFixedUpdate();
        }
        private int BlocksInPlay()
        {
            int inPlay = 0;
            for (int x = 0; x < gridHandler.sizeX; x++)
            {
                for (int y = 0; y < gridHandler.sizeY; y++)
                {
                    if (gridHandler.IsOccupied(x, y))
                    {
                        inPlay++;
                    }
                }
            }
            return inPlay;
        }
        private void Do(int xDir, int yDir)
        {
            gridHandler.MergeAll(xDir, yDir);
            gridHandler.MoveAll(xDir, yDir);
        }
        #endregion

        [UnityTest]
        public IEnumerator GameEnviromentInitializedProperly()
        {
            yield return ResetScene();

            bool gridInitialized = gridHandler != null;
            bool inputInitialized = inputHandler != null;

            Assert.IsTrue(gridInitialized && inputInitialized);
        }

        [UnityTest]
        public IEnumerator VictoryConditionsDetected()
        {
            yield return ResetScene();

            //No blocks in play
            Assert.IsFalse(gridHandler.IsGameWon());

            //No winning blocks in play
            gridHandler.ResetGrid();
            gridHandler.SpawnBlock();
            gridHandler.SpawnBlock();
            gridHandler.SpawnBlock();
            Assert.IsFalse(gridHandler.IsGameWon());

            //Only winning Block in play
            gridHandler.ResetGrid();
            gridHandler.NewBlock(0, 0, gridHandler.targetValue);
            Assert.IsTrue(gridHandler.IsGameWon());

            //Mix of blocks in play
            gridHandler.ResetGrid();
            gridHandler.NewBlock(0, 0, gridHandler.targetValue);
            gridHandler.SpawnBlock();
            gridHandler.SpawnBlock();
            gridHandler.SpawnBlock();
            Assert.IsTrue(gridHandler.IsGameWon());

            //Winning block value higher then target value
            gridHandler.ResetGrid();
            gridHandler.NewBlock(0, 0, gridHandler.targetValue*2);
            Assert.IsTrue(gridHandler.IsGameWon());
        }

        [UnityTest]
        public IEnumerator GameOverConditionsDetected()
        {
            yield return ResetScene();

            int i = 2;
            for (int x = 0; x < gridHandler.sizeX; x++)
            {
                for (int y = 0; y < gridHandler.sizeY; y++)
                {
                    gridHandler.NewBlock(x, y, i);
                    i++;
                }
            }
            Assert.IsTrue(gridHandler.IsGameOver());
        }

        [UnityTest]
        public IEnumerator BlocksMovedProperly()
        {
            yield return ResetScene();

            //left to right
            gridHandler.ResetGrid();
            for (int y = 0; y < gridHandler.sizeY; y++)
            {
                gridHandler.NewBlock(0, y, 2);
            }
            Do(1, 0);
            for (int y = 0; y < gridHandler.sizeY; y++)
            {
                Assert.IsTrue(gridHandler.IsOccupied(gridHandler.sizeX - 1, y));
                Assert.IsFalse(gridHandler.IsOccupied(0, y));
            }

            //right to left
            Do(-1, 0);
            for (int y = 0; y < gridHandler.sizeY; y++)
            {
                Assert.IsFalse(gridHandler.IsOccupied(gridHandler.sizeX - 1, y));
                Assert.IsTrue(gridHandler.IsOccupied(0, y));
            }

            //top to bottom
            gridHandler.ResetGrid();
            for (int x = 0; x < gridHandler.sizeX; x++)
            {
                gridHandler.NewBlock(x, 0, 2);
            }
            Do(0, 1);
            for (int x = 0; x < gridHandler.sizeX; x++)
            {
                Assert.IsTrue(gridHandler.IsOccupied(x, gridHandler.sizeY - 1));
                Assert.IsFalse(gridHandler.IsOccupied(x, 0));
            }

            //bottom to top
            Do(0, -1);
            for (int x = 0; x < gridHandler.sizeX; x++)
            {
                Assert.IsFalse(gridHandler.IsOccupied(x, gridHandler.sizeY - 1));
                Assert.IsTrue(gridHandler.IsOccupied(x, 0));
            }

            //moved into edge
            gridHandler.ResetGrid();
            gridHandler.NewBlock(0, 0, 2);
            Do(-1, 0);
            Assert.IsTrue(gridHandler.IsOccupied(0, 0));

            //moved into another block
            gridHandler.ResetGrid();
            gridHandler.NewBlock(0, 0, 2);
            gridHandler.NewBlock(1, 0, 4);
            Do(-1, 0);
            Assert.IsTrue(gridHandler.IsOccupied(0, 0));
            Assert.IsTrue(gridHandler.IsOccupied(1, 0));
        }

        [UnityTest]
        public IEnumerator BlocksMergedProperly()
        {
            yield return ResetScene();

            //Two same value blocks
            gridHandler.ResetGrid();
            gridHandler.NewBlock(0, 0, 2);
            gridHandler.NewBlock(1, 0, 2);
            Do(-1, 0);
            Assert.AreEqual(gridHandler.At(0, 0).value, 4);
            Assert.IsFalse(gridHandler.IsOccupied(1, 0));

            //Two diffrent value blocks
            gridHandler.ResetGrid();
            gridHandler.NewBlock(0, 0, 2);
            gridHandler.NewBlock(1, 0, 4);
            Do(-1, 0);
            Assert.IsTrue(gridHandler.IsOccupied(0, 0));
            Assert.IsTrue(gridHandler.IsOccupied(1, 0));

            //A row of 4 mergable blocks
            gridHandler.ResetGrid();
            for (int y = 0; y < 4; y++)
            {
                gridHandler.NewBlock(0, y, 2);
            }
            Do(0, -1);
            Assert.AreEqual(gridHandler.At(0, 0).value, 4);
            Assert.AreEqual(gridHandler.At(0, 1).value, 4);
            Assert.IsFalse(gridHandler.IsOccupied(0, 2));
            Assert.IsFalse(gridHandler.IsOccupied(0, 3));

            //A row of 3 mergable blocks
            gridHandler.ResetGrid();
            for (int y = 0; y < 3; y++)
            {
                gridHandler.NewBlock(0, y, 2);
            }
            Do(0, -1);
            Assert.AreEqual(gridHandler.At(0, 0).value, 4);
            Assert.AreEqual(gridHandler.At(0, 1).value, 2);
            Assert.IsFalse(gridHandler.IsOccupied(0, 2));
        }

        [UnityTest]
        public IEnumerator BlocksSpawnedProperly()
        {
            yield return ResetScene();
            int totalSpace = gridHandler.sizeX * gridHandler.sizeY;

            for (int i = 0; i<totalSpace; i++)
            {
                gridHandler.SpawnBlock();

                Assert.AreEqual(i+1, BlocksInPlay());
            }
        }
        

    }
}
