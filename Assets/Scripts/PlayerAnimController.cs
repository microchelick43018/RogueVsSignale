using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerAnimController
{
    public static class Params
    {
        public const string Speed = "Speed";
        public const string Hit = "Hit";
    }

    public static class States
    {
        public const string Hit = "Hit";
        public const string Run = "Run";
        public const string Walk = "Walk";
        public const string Idle = "Idle";
    }
}
