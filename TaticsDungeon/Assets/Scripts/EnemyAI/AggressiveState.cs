using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class AggressiveState : FSMState
    {
        public CharacterStats characterStats;
        public AnimationHandler animationHandler;
        public CharacterStateManager stateManager;
        public TaticalMovement taticalMovement;
        public PlayerManager[] playersList;
        public EnemyManager[] enemiesList;
        public PlayerManager target;
        Dictionary<SkillType, Skill> skillDict;

        public AggressiveState(CharacterStats stats,
            CharacterStateManager states, TaticalMovement tatical, AnimationHandler animation,
            Dictionary<SkillType, Skill> skills)
        {
            stateID = FSMStateID.Aggressive;
            characterStats = stats;
            stateManager = states;
            taticalMovement = tatical;
            animationHandler = animation;
            playersList = GameManager.instance.playersList;
            enemiesList = GameManager.instance.enemiesList;
            skillDict = skills;
        }

        public override void HandleTransitions()
        {
            
        }

        public override void Act(float delta)
        {

            if (target == null)
            {
                List<(PlayerManager, int)> playersHealthList = new List<(PlayerManager, int)>();
                    
                foreach (PlayerManager player in playersList)
                {
                    if (taticalMovement.GetMouseDistance(player.taticalMovement.currentIndex) <= characterStats.currentAP)
                    {
                        playersHealthList.Add((player, player.characterStats.currentHealth));
                    }
                }

                playersHealthList.Sort((c1, c2) => c1.Item2.CompareTo(c2.Item2));
                target = playersHealthList[0].Item1;

                List<IntVector2> directions = new List<IntVector2>
                {
                    new IntVector2(0,1),
                    new IntVector2(0,-1),
                    new IntVector2(1,0),
                    new IntVector2(-1,0)
                };

                List<IntVector2> targetIndicies = new List<IntVector2>();
                foreach (IntVector2 direction in directions)
                {
                    IntVector2 i = target.taticalMovement.currentIndex.Add(direction);

                    if (i.IsValid(taticalMovement.mapAdapter))
                    {
                        if (taticalMovement.currentNavDict.ContainsKey(i))
                            targetIndicies.Add(i);                                
                    }
                }

                //foreach(var item in taticalMovement.currentNavDict)
                //{
                //    Debug.Log(item.Key.x + "," + item.Key.y + ":" + item.Value.x + "," + item.Value.y);
                //}

                IntVector2 targetIndex = targetIndicies[0];

                taticalMovement.path = NavigationHandler.instance.GetPath(taticalMovement.currentNavDict,
                                        targetIndex, taticalMovement.currentIndex);
                    
                taticalMovement.SetTargetDestination(targetIndex, 
                    taticalMovement.GetMouseDistance(target.taticalMovement.currentIndex));
            }

            else
            {
                if (taticalMovement.transform.position == taticalMovement.moveLocation)
                {
                    if (characterStats.currentAP > 0 && stateManager.characterState=="ready")
                    {                                     
                        MeleeAttack.Activate(characterStats, animationHandler,
                            taticalMovement, skillDict[SkillType.MeleeAttack], target, delta);
                    }
                            
                    else
                        stateManager.characterState = "exhausted";
                }
                else
                {
                    taticalMovement.TraverseToDestination(delta);
                }
            }
            
        }
    }
}

