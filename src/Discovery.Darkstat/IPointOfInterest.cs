using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Discovery.Darkstat
{
    /// <summary>
    /// Can be a player owned base, a npc base or a mining field
    /// </summary>
    public interface IPointOfInterest
    {
        /// <summary>
        /// Returns the human readable name of the entity.
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Returns the internal name of the entity.
        /// </summary>
        public string Nickname { get; }
        /// <summary>
        /// Return the affiliated faction internal name of the entity.
        /// </summary>
        public string FactionNickname { get; }
        /// <summary>
        /// Return region, where the base is located.
        /// </summary>
        public string RegionName { get; }
        /// <summary>
        /// Return the human readable system name, where the base is located.
        /// </summary>
        public string SystemName { get; }
        /// <summary>
        /// Return the internal system name, where the base is located.
        /// </summary>
        public string SystemNickname { get; }
        /// <summary>
        /// Return the position of the entity.
        /// </summary>
        public Vector3? Position { get; }
    }
}
