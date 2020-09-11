using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class SkillObjectInterface : MonoBehaviour
    {
        public SkillAbstract skillScript;

        public void Activate(CharacterStats characterStats,AnimationHandler animationHandler, 
            TaticalMovement taticalMovement,float delta)
        {
<<<<<<< Updated upstream
            skillScript.Activate(delta);
=======
            skillScript.Activate( delta);
>>>>>>> Stashed changes
        }

        public void Excute(CharacterStats characterStats,AnimationHandler animationHandler,
            TaticalMovement taticalMovement,float delta, GridCell targetCell)
        {
            skillScript.Excute(delta, targetCell);
        }
    }
}

