# Simple A\* Pathfinding Algorithm. Unity3D and C#

![](simple.gif)

(In the above gif, red object acting as a player, and green object is acting as target object. All the small yellow dots represent the shortest path to the target object from player) 

Class `PathFinder` is not a MonoBehavior class. You need to use it in your MonoBehavior class. PathFinder requires size (e.g Capsules's collider radius) to calculate collision while finding path, also `LayerMask` obstacles to detect obstacle GameObjects.
PathFinder class doesn't require a grid. It generates nodes while searching the path. And it will keep in mind size of the object you have provided.

### Todos

- [ ] MinHeap to find node with lowest score
- [ ] Smooth moving along the shortest paths
- [ ] Search in Y axis
- [ ] Freeze axis (e.g object can move only in X|Y|Z axis)
- [ ] Caching
