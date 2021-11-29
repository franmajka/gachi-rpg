using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonGenerator : MonoBehaviour {
    [Header("Generator Properies")]
    public static readonly int _minimalRoomDelta = 2;
    [Range(1, 4)] [SerializeField] private int _corridorThickness;
    private int _roomSize = 64;
    private int _roomGenerateIterations = 4;

    [Header("Blocks container&Level TileMap")]
    [SerializeField] private BlockContainer _currentBlockContainer;
    [SerializeField] private byte[,] _byteMap;
    private BspTree _binaryTree;
    private void OnEnable() {
        _byteMap = new byte[_roomSize, _roomSize];
        Array.Clear(_byteMap, 0, _byteMap.Length);
        GenerateDungeon();
    }
    private void GenerateTreeWithContainers() {
        bool IsHorizontal = UnityEngine.Random.Range(0, 2) == 0;
        _binaryTree = BspTree.Split(_roomGenerateIterations, new RectInt(0, 0, _roomSize, _roomSize), IsHorizontal);
    }
    private void GenerateCorridors(BspTree BinaryTree) {
        if (BinaryTree.IsInternal()) {
            RectInt leftContainer = BinaryTree.left.container;
            RectInt rightContainer = BinaryTree.right.container;
            Vector2 leftCenter = leftContainer.center;
            Vector2 rightCenter = rightContainer.center;
            Vector2 direction = (rightCenter - leftCenter).normalized;
            while (Vector2.Distance(leftCenter, rightCenter) > 1) {
                if (direction.Equals(Vector2.right)) {
                    for (int j = 0; j < _corridorThickness; j++) {
                        _byteMap[(int)leftCenter.x, (int)leftCenter.y + j] = 1;
                    }
                }
                else if (direction.Equals(Vector2.up)) {
                    for (int j = 0; j < _corridorThickness; j++) {
                        _byteMap[(int)leftCenter.x + j, (int)leftCenter.y] = 1;
                    }
                }
                leftCenter.x += direction.x;
                leftCenter.y += direction.y;
            }
        }
        if (BinaryTree.right != null) GenerateCorridors(BinaryTree.right);
        if (BinaryTree.left != null) GenerateCorridors(BinaryTree.left);

    }
    private void GenerateRooms(BspTree BinaryTree) {
        if (BinaryTree.IsLeaf()) {
            for (int i = BinaryTree.room.x; i < BinaryTree.room.xMax; i++) {
                for (int j = BinaryTree.room.y; j < BinaryTree.room.yMax; j++) {
                    _byteMap[i, j] = 1;
                }
            }
        }
        else {
            if (BinaryTree.left != null) GenerateRooms(BinaryTree.left);
            if (BinaryTree.right != null) GenerateRooms(BinaryTree.right);
        }
    }
    private GameObject GetContainerTypeByNeighbors(int x, int y) {
        //8 cases + 1 default + 1 without object
        if (_byteMap[x, y] == 0) {
            Debug.Log("X");
            return null;
        }
        if (_byteMap[x - 1, y] == 0 && _byteMap[x, y - 1] == 0) return _currentBlockContainer._topLeftBlock;
        if (_byteMap[x - 1, y] != 0 && _byteMap[x + 1, y] != 0 && _byteMap[x, y + 1] == 0) return _currentBlockContainer._topMiddleBlock;
        if (_byteMap[x - 1, y] == 0 && _byteMap[x, y + 1] == 0) return _currentBlockContainer._topRightBlock;
        if (_byteMap[x - 1, y] == 0 && _byteMap[x, y - 1] != 0 && _byteMap[x, y + 1] != 0) return _currentBlockContainer._middleLeftBlock;
        if (_byteMap[x + 1, y] == 0 && _byteMap[x, y - 1] != 0 && _byteMap[x, y + 1] != 0) return _currentBlockContainer._middleRightBlock;
        if (_byteMap[x, y - 1] == 0 && _byteMap[x + 1, y] == 0) return _currentBlockContainer._downLeftBlock;
        if (_byteMap[x - 1, y] != 0 && _byteMap[x + 1, y] != 0 && _byteMap[x, y - 1] == 0) return _currentBlockContainer._downMiddleBlock;
        if (_byteMap[x, y + 1] == 0 && _byteMap[x + 1, y] == 0) return _currentBlockContainer._downRightBlock;
        return _currentBlockContainer._middleMiddleBlock;
    }
    private void GenerateMap() {
        for(int i = 0; i < _roomSize; i++) {
            for (int j = 0; j < _roomSize; j++) {
                var Prefab = GetContainerTypeByNeighbors(i, j);
                if (Prefab)
                    Instantiate(Prefab, new Vector3(i, 0, j), Prefab.transform.rotation);
            }
        }
    }
    private void GenerateDungeon() {
        GenerateTreeWithContainers();
        BspTree.GenerateRoomsInsideContainersNode(_binaryTree);
        GenerateCorridors(_binaryTree);
        GenerateRooms(_binaryTree);
        string x = "";
        for (int i = 0; i < 64; i++) {
            for (int j = 0; j < 64; j++) {
                x += _byteMap[i, j];
            }
            x += "\n";
        }
        Debug.Log(x);
        GenerateMap();
    }
}
