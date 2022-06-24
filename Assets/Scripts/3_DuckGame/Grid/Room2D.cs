using System;
using System.Collections.Generic;
using GameFramework;
using UnityEngine;


[SelectionBase]
public class Room2D : Entity{
    public Grid2D OwnerGrid2D { get; set; }

    public Vector2Int GridId { get; set; }

    private Room2D LeftRoom => OwnerGrid2D.GetRoom(GridId + Vector2Int.left);
    private Room2D RightRoom => OwnerGrid2D.GetRoom(GridId + Vector2Int.right);
    private Room2D TopRoom => OwnerGrid2D.GetRoom(GridId + Vector2Int.up);
    private Room2D BottomRoom => OwnerGrid2D.GetRoom(GridId + Vector2Int.down);

    private readonly List<Entity> _roomObjects = new List<Entity>();

    private bool _setup;

    private bool _active = true;

    private List<Entity> _roomObjectsSpawned = new List<Entity>();

    private void Start(){
        Setup();
    }

    private void Update(){
        if (_active)
            CheckOutOfRoomConfinedObjects();
    }

    private void CheckOutOfRoomConfinedObjects(){
        List<Entity> outOfRoomConfinedObjects = new List<Entity>();

        for (int i = 0; i < _roomObjectsSpawned.Count; i++){
            if (_roomObjectsSpawned[i].gameObject.activeSelf){
                if (IsOutOfRoom(_roomObjectsSpawned[i].transform)){
                    IConfinable confinableEntity = _roomObjectsSpawned[i] as IConfinable;
                    confinableEntity?.OnExitRoom();
                    outOfRoomConfinedObjects.Add(_roomObjectsSpawned[i]);
                }
            }
        }
        RemoveRoomObjects(outOfRoomConfinedObjects);
    }

    private void RemoveRoomObjects(List<Entity> roomObjects){
        for (int i = 0; i < roomObjects.Count; i++){
            _roomObjectsSpawned.Remove(roomObjects[i]);
        }
    }

    private bool IsOutOfRoom(Transform objTransform){
        Grid2D owner = OwnerGrid2D;
        if (!owner) owner = GetComponentInParent<Grid2D>();
        if (owner){
            float objX = objTransform.position.x;
            float objY = objTransform.position.y;
            float roomX = transform.position.x;
            float roomY = transform.position.y;
            float halfGridSize = owner.GridSize * 0.5f;

            if (objX > (roomX + halfGridSize) || objX < (roomX - halfGridSize) || objY > (roomY + halfGridSize) || objY < (roomY - halfGridSize)){
                return true;
            }
        }
        return false;
    }

    private void Setup(){
        if (_setup) return;
        _setup = true;
        var entities = GetComponentsInChildren<Entity>(true);
        for (var i = 0; i < entities.Length; i++)
        {
            var item = entities[i];
            if (item is IRoomObject)
            {
                _roomObjects.Add(item);
            }
        }
    }
        
#if UNITY_EDITOR
    private void OnDrawGizmosSelected(){
        if (!transform.hasChanged || UnityEditor.Selection.activeGameObject != gameObject) return;

        Grid2D owner = OwnerGrid2D;
        if (!owner) owner = GetComponentInParent<Grid2D>();
        if (owner) SnapToGrid(owner.GridSize);
    }
#endif

    private void SnapToGrid(int gridSize){
        transform.localPosition = new Vector3(
            Mathf.Round(transform.localPosition.x / gridSize) * gridSize,
            Mathf.Round(transform.localPosition.y / gridSize) * gridSize,
            Mathf.Round(transform.localPosition.z / gridSize) * gridSize
        );
    }

    private void OnSpawnRoomObject(Entity entity){
        IConfinable confinableEntity = entity as IConfinable;
        if(confinableEntity != null)
            _roomObjectsSpawned.Add(entity);
    }

    public void Active(){
        if (_active) return;
        OwnerLevel.OnSpawned += OnSpawnRoomObject;
        Setup();
        _active = true;
        for (var i = 0; i < _roomObjects.Count; i++){
            var item = _roomObjects[i];
            if (item is IRoomObject roomObject){
                roomObject.EnterRoom();
            }
        }
    }

    public void Disable(){
        if (!_active) return;
        OwnerLevel.OnSpawned -= OnSpawnRoomObject;
        DisableSpawnedObjects();
        Setup();
        _active = false;
        for (var i = 0; i < _roomObjects.Count; i++){
            var item = _roomObjects[i];
            if (item is IRoomObject roomObject){
                roomObject.ExitRoom();
            }
        }
    }

    private void DisableSpawnedObjects(){
        for (int i = 0; i < _roomObjectsSpawned.Count; i++){
            if (_roomObjectsSpawned[i].gameObject.activeSelf){
                IConfinable roomConfinedObjectSpawned = _roomObjectsSpawned[i] as IConfinable;
                roomConfinedObjectSpawned?.OnExitRoom();
            }
        }
        _roomObjectsSpawned.Clear();
    }
}
