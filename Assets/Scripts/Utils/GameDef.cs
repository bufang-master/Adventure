using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Scripts
{
    public static class GameDef
    {
        public const string IdleAnimationClip = "idleSword";
        public const string AttackAnimationClip1 = "swordStrike1";
        public const string AttackAnimationClip2 = "swordStrike2";
        public const string AttackAnimationClip3 = "swordStrike3";
        public const string DefanseAnimationClip = "shieldBlock";
        public const string DieAnimationClip = "die1";

        public const string winEffect = "Audio/win";
        public const string failEffect = "Audio/fail";
        public const string turnPageEffect = "Audio/turnPage";
        public const string impackEffect = "Audio/impack";
        public const string beheadEffect = "Audio/behead";
        public const string swooshEffect = "Audio/swoosh";
        public const string coinEffect = "Audio/coin";
        public const string clickEffect = "Audio/click";
        public const string shiftEffect = "Audio/shift";
        public const string findEffect = "Audio/find";
        public const string walkEffect = "Audio/walk";
        public const string skill1Effect = "Audio/skill1";
        public const string skill2Effect = "Audio/skill2";
        public const string skillCancleEffect = "Audio/skillCancle";
        public const string skillANDEffect = "Audio/skillAND";


        public static bool isFight;
    }
}
