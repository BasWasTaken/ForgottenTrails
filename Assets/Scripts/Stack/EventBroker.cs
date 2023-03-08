using Behaviour;
using Structures;
using System.Collections.Generic;

namespace Utility
{
    /// <summary>
    /// Handles communication between classes without them having to etablish a connection first.
    /// </summary>
    public static class EventBroker
    {
        public delegate void QueueCreaturesForMapScanAction(params CreatureType[] creatureTypes);
        public static event QueueCreaturesForMapScanAction QueueCreaturesForMapScanEvent;
        /// <summary>
        /// Adds the given creatures to the queue for the next time the map is scanned, and set a flag to start the next scan.
        /// </summary>
        /// <param name="creatureTypes">The given creatures to be added</param>
        public static void QueueCreaturesForMapScan(params CreatureType[] creatureTypes)
        {
            QueueCreaturesForMapScanEvent?.Invoke(creatureTypes);
        }
        /// <summary>
        /// Adds the given creatures to the queue for the next time the map is scanned, and set a flag to start the next scan.
        /// </summary>
        /// <param name="creatureTypes">The given creatures to be added</param>
        public static void QueueCreaturesForMapScan(List<CreatureType> creatureTypes)
        {
            QueueCreaturesForMapScanEvent?.Invoke(creatureTypes.ToArray());
        }
        public delegate void StructurePlacedAction(Tower structure);
        public static event StructurePlacedAction StructurePlacedEvent;
        /// <summary>
        /// Lets the system know a structure has been placed. 
        /// </summary>
        /// <param name="structure">The structure that was placed.</param>
        public static void StructurePlacedTrigger(Tower structure)
        {
            StructurePlacedEvent?.Invoke(structure);
            MapScanNudge(updateExisting: true);
        }
        
        public delegate void MapScanAction(bool findNewCreatures = false, bool updateExisting = false);
        public static event MapScanAction MapScanEvent;
        /// <summary>
        /// Nudges the <see cref="PathFinding.MapScanner"/> to scan the map and create new graphs for the queued creature types. 
        /// </summary>
        /// <param name="findNewCreatures">Check for new creatures to add to the queue first.</param>
        /// <param name="updateExisting">If true, also re-scan the map for all existing creature graphs.</param>
        public static void MapScanNudge(bool findNewCreatures = false, bool updateExisting = false)
        {
            MapScanEvent?.Invoke(findNewCreatures,updateExisting);
        }
        /// <summary>
        /// Signal that the <see cref="PathFinding.MapScanner"/> has finished scanning the map and created a graph for the given creature types.
        /// </summary>
        /// <param name="types">The creature types that have been updated.</param>
        public delegate void GraphCreatedAction(HashSet<CreatureType> types);
        public static event GraphCreatedAction GraphCreatedEvent;
        public static void GraphCreatedTrigger(HashSet<CreatureType> types)
        {
            GraphCreatedEvent?.Invoke(types);
        }

        public delegate void PlayerHitAction();
        public static event PlayerHitAction PlayerHitEvent;
        public static void PlayerHitTrigger()
        {
            PlayerHitEvent?.Invoke();
        }

        public delegate void EnemyKilledAction(EnemyCreature enemy, int bounty, bool self);
        public static event EnemyKilledAction EnemyKilledEvent;
        public static void EnemyKilledTrigger(EnemyCreature enemy, int bounty, bool self) 
        {
            EnemyKilledEvent?.Invoke(enemy, bounty, self); 
        }

        public delegate void WaveStartAction(int waveNum, float duration, bool overtime =false);
        public static event WaveStartAction WaveStartEvent;
        public static void WaveStartTrigger(int waveNum, float duration, bool overtime=false)
        {
            WaveStartEvent?.Invoke(waveNum, duration, overtime);
        }

        public delegate void GameEndAction(bool victory);
        public static event GameEndAction GameEndEvent;
        public static void GameEndTrigger(bool victory)
        {
            GameEndEvent?.Invoke(victory);
        }
    }
}
