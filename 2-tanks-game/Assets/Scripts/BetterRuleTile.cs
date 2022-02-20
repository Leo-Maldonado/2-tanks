using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

    [CreateAssetMenu]
    public class BetterRuleTile : RuleTile<BetterRuleTile.Neighbor>
    {
        public class Neighbor : RuleTile.TilingRule.Neighbor
        {
            public const int Null = 3;
            public const int NotNull = 4;
        }

        public List<TileBase> Siblings = new List<TileBase>();

        public override bool RuleMatch(int neighbor, TileBase other)
        {
            switch (neighbor)
            {
                case BetterRuleTile.Neighbor.This:
                    {
                        return other == this || Siblings.Contains(other);
                    }
                case BetterRuleTile.Neighbor.NotThis:
                    {
                        return other != this && !Siblings.Contains(other);
                    }
            }

            return base.RuleMatch(neighbor, other);
        }
    }