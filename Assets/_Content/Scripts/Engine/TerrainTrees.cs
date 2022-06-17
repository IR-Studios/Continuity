using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainTrees : MonoBehaviour
{
public Transform player;
  
 private TreeInstance[] paryTrees;
 private Vector3 pvecTerrainPosition;
 private Vector3 pvecTerrainSize;
 private GameObject pgobTreeCollide;
 private Vector3 pvecCollideScale;
 private bool  pbooCollideWithTrees = false;
  
 void  Start (){
     // Get the terrain's position
     pvecTerrainPosition = Terrain.activeTerrain.transform.position;
      
     // Get the terrain's size from the terrain data
     pvecTerrainSize = Terrain.activeTerrain.terrainData.size;
     // Get the tree instances
     paryTrees = Terrain.activeTerrain.terrainData.treeInstances;
      
     // Get the invisible capsule having the capsule collider that makes the nearest tree solid
     pgobTreeCollide = GameObject.Find("Tree"); // This is a capsule having a capsule collider, but when the flier hits it we want it to be reported that the flier hit a tree.
      
     // Are there trees and a tree collider?
     if ((pgobTreeCollide != null) && (paryTrees.Length > 0))
     {
         // Set a flag to make this script useful
         pbooCollideWithTrees = true;
         // Get the original local scale of the capsule. This is manually matched to the scale of the prototype of the tree.
         pvecCollideScale = pgobTreeCollide.transform.localScale;
     }
     // No need to use this script
     else 
     {
         Debug.LogWarning( "NO CAPSULE NAMED TREE FOUND, OR NO TERRAIN TREES, DESTROYING SCRIPT..." );
         Destroy(this);
     }
     
     // has the player been assigned in the Inspector?
     if ( !player ) 
     {
         Debug.LogWarning( "NO PLAYER OBJECT IN THE INSPECTOR, DESTROYING SCRIPT..." );
         Destroy(this);
     }
 }
  
 void  Update (){
     int L;
     TreeInstance triTree;
     
     //Vector3 vecFlier = sctFly.svecXYZ; // My protagonist's position, passed by a static variable in a script called sctFly.
     Vector3 vecFlier = player.position; // using the player transform, dropped in the inspector
     
     float fltProximity;
     float fltNearest = 9999.9999f; // Farther, to start, than is possible in my game.
     Vector3 vec3;
     Vector3 vecTree;
     int intNearestPntr = 0;
      
     // Test the flag
     if (pbooCollideWithTrees == true)
     {
         // Find the nearest tree to the flier
         for (L = 0; L < paryTrees.Length; L++)
         {
             // Get the tree instance
             triTree = paryTrees[L];
             // Get the normalized tree position
             vecTree = triTree.position;
             // Get the world coordinates of the tree position
             vec3 = (Vector3.Scale(pvecTerrainSize, vecTree) + pvecTerrainPosition);
             // Calculate the proximity
             fltProximity = Vector3.Distance(vecFlier, vec3);
             // Nearest so far?
             if (fltProximity < fltNearest)
             {
                 // Remember the nearest
                 fltNearest = fltProximity;
                 // Remember the index
                 intNearestPntr = L;
             }
         }
         // Get the closest tree
         triTree = paryTrees[intNearestPntr];
         // Get the normalized tree position of the closest tree
         vecTree = triTree.position;
         // Get the world coordinates of the tree position
         vec3 = (Vector3.Scale(pvecTerrainSize, vecTree) + pvecTerrainPosition);
         // Scale the capsule having the capsule collider that represents a solid tree
         pgobTreeCollide.transform.localScale = (pvecCollideScale * triTree.heightScale);
         // Add some height to position the capsule correctly on the tree
         vec3.y += pgobTreeCollide.transform.localScale.y;
         // Position the capsule having the capsule collider at the nearest tree
         pgobTreeCollide.transform.position = vec3;
     }
 }
}
