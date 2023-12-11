using System.Collections.Generic;
using UnityEngine;

namespace jbzd.Common.Extensions
{
    public static class GameObjectExtension
    {
        /// <summary>
        /// Get all parents names of this game object where last element in list is first parent.
        /// </summary>
        /// <param name="gameObject">Child game object that we are looking parents of</param>
        /// <returns>list of parents</returns>
        public static List<string> GetAllParentsNames(this GameObject gameObject)
        {
            var parents = new List<string>();

            var currentParent = gameObject.transform.parent;

            while (currentParent != null)
            {
                parents.Add(currentParent.name);
                currentParent = currentParent.parent;
            }
            parents.Reverse();
                        
            return parents;
        }

        /// <summary>
        /// Get all parents references of this game object where last element in list is first parent.
        /// </summary>
        /// <param name="gameObject">Child game object that we are looking parents of</param>
        /// <returns>list of parents</returns>
        public static List<GameObject> GetAllParents(this GameObject gameObject)
        {
            var parents = new List<GameObject>();

            var currentParent = gameObject.transform.parent;

            while (currentParent != null)
            {
                parents.Add(currentParent.gameObject);
                currentParent = currentParent.parent;
            }
            parents.Reverse();
            
            return parents;
        }
    }
}