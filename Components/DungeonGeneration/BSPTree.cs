using System;
using UnityEngine;

public class BspTree {
    public RectInt container;
    public RectInt room;
    public BspTree left;
    public BspTree right;
    public bool IsHorizontal;
    public BspTree(RectInt container, bool IsHorizontal) {
        this.container = container;
        this.IsHorizontal = IsHorizontal;
    }
    public bool IsLeaf() {
        return left == null && right == null;
    }

    public bool IsInternal() {
        return left != null || right != null;
    }

    internal static BspTree Split(int numberOfIterations, RectInt container, bool IsHorizontal) {
        var node = new BspTree(container, IsHorizontal);
        if (numberOfIterations == 0) return node;
        var splittedContainers = node.SplitContainer(container);
        node.left = Split(numberOfIterations - 1, splittedContainers[0], !IsHorizontal);
        node.right = Split(numberOfIterations - 1, splittedContainers[1], !IsHorizontal);

        return node;
    }
    private RectInt[] SplitContainer(RectInt container) {
        RectInt c1, c2;
        if (!IsHorizontal) {
            // vertical
            c1 = new RectInt(container.x, container.y, container.width, (int)UnityEngine.Random.Range(container.height * 0.4f, container.height * 0.6f));
            c2 = new RectInt(container.x, container.y + c1.height, container.width, container.height - c1.height);
        }
        else {
            // horizontal 
            c1 = new RectInt(container.x, container.y, (int)UnityEngine.Random.Range(container.width * 0.4f, container.width * 0.6f), container.height);
            c2 = new RectInt(container.x + c1.width, container.y, container.width - c1.width, container.height);
        }
        return new RectInt[] { c1, c2 };
    }

    public static void GenerateRoomsInsideContainersNode(BspTree node) {
        // should create rooms for leafs
        if (node.IsLeaf()) {
            var randomX = UnityEngine.Random.Range(DungeonGenerator._minimalRoomDelta, node.container.width / 5);
            var randomY = UnityEngine.Random.Range(DungeonGenerator._minimalRoomDelta, node.container.height / 5);
            int roomX = node.container.x + randomX;
            int roomY = node.container.y + randomY;
            int roomW = node.container.width - (int)(randomX * UnityEngine.Random.Range(1.5f, 2f));
            int roomH = node.container.height - (int)(randomY * UnityEngine.Random.Range(1.5f, 2f));
            node.room = new RectInt(roomX, roomY, roomW, roomH);
        }
        else {
            if (node.left != null) GenerateRoomsInsideContainersNode(node.left);
            if (node.right != null) GenerateRoomsInsideContainersNode(node.right);
        }
    }

}