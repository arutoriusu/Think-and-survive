using GameCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.Item
{
    [CreateAssetMenu(fileName = "ItemDescriptions", menuName = "ItemDescriptions", order = 51)]
    public class ItemDescriptions : ScriptableObject
    {
        [SerializeField] private List<ItemDescription> _listItems;

        public List<ItemDescription> ListItems => _listItems;

    }
}
