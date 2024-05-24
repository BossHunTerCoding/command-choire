using System;
using CommandChoice.Model;
using UnityEngine;

namespace CommandChoice.Component
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private GridModel model;
        [SerializeField] private GameObject _tilePrefab;
        [SerializeField] private GameObject player;

        void Awake()
        {

            player = GameObject.FindGameObjectWithTag(StaticText.TagPlayer);
        }

        void Start()
        {
            GenerateGrid();
        }

        void GenerateGrid()
        {
            Vector3 posPlayer = player.GetComponent<Transform>().position;
            for (int x = 0; x < model.Width; x++)
            {
                for (int y = 0; y < model.Height; y++)
                {
                    var spawnedTileUpRight = Instantiate(_tilePrefab, new Vector3(posPlayer.x + x, posPlayer.y + y, 20), Quaternion.identity);
                    var spawnedTileUpLeft = Instantiate(_tilePrefab, new Vector3(posPlayer.x - x, posPlayer.y + y, 20), Quaternion.identity);
                    var spawnedTileDownLeft = Instantiate(_tilePrefab, new Vector3(posPlayer.x - x, posPlayer.y - y, 20), Quaternion.identity);
                    var spawnedTileDownRight = Instantiate(_tilePrefab, new Vector3(posPlayer.x + x, posPlayer.y - y, 20), Quaternion.identity);
                    SettingGrid(spawnedTileUpRight, x, y);
                    SettingGrid(spawnedTileUpLeft, -x, y);
                    SettingGrid(spawnedTileDownLeft, -x, -y);
                    SettingGrid(spawnedTileDownRight, x, -y);
                }
            }
        }

        void SettingGrid(GameObject spawnedTile, int x, int y)
        {
            int xPositive = Math.Abs(x);
            int yPositive = Math.Abs(y);
            var isOffset = (xPositive % 2 == 0 && yPositive % 2 != 0) || (xPositive % 2 != 0 && yPositive % 2 == 0);
            spawnedTile.GetComponent<SpriteRenderer>().color = isOffset ? model.FirstColor : model.SecondColor;
            spawnedTile.transform.SetParent(transform);
            spawnedTile.name = $"Tile {x} {y}";
        }
    }
}