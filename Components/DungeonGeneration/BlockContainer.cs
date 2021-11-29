using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DungeonContainer")]
public class BlockContainer : ScriptableObject
{
    public GameObject _topLeftBlock;
    public GameObject _topMiddleBlock;
    public GameObject _topRightBlock;
    public GameObject _middleLeftBlock;
    public GameObject _middleMiddleBlock;
    public GameObject _middleRightBlock;
    public GameObject _downLeftBlock;
    public GameObject _downMiddleBlock;
    public GameObject _downRightBlock;
}
