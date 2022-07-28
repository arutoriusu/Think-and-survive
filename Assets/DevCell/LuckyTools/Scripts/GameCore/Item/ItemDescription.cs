using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.Item
{
    [Serializable]
    public class ItemDescription
    {
        public int id;
        public string type;
        public string name;
        public string itemClass;
        public int damage;
        public int durability;
        public int satisfyingHunger;
        public int kindleValue;

        private string[] interactObjects = new string[] {
            "BlockStone(Clone)",
            "BlockStone",
            "Dead Tree(Clone)",
            "Dead Tree",
            "tree_1(Clone)",
            "tree_1",
            "Fir_v1_1(Clone)",
            "Fir_v1_1"
        };
        public string[] InteractObjects { get => interactObjects; set => interactObjects = value; }
    }
}
