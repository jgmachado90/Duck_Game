using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;
using UnityEngine.Serialization;

public class Grid2D : MonoBehaviour{
    [SerializeField] private CameraSnapFollow camera2D;
    private bool _initialized;
    Vector2Int _mapSize;
    private Room2D[,] _rooms;
    public Room2D[,] Rooms => _rooms;

    private Room2D[] _childrenRooms;

    public Room2D[] ChildrenRooms{
        get{
            InitializeGrid();
            return _childrenRooms;
        }
    }

    private Room2D _leftmost, _rightmost;
    private Room2D _bottommost, _topmost;

    [SerializeField] private int gridSize;

    public int GridSize => gridSize;

    private void OnValidate(){
        InitializeCamera();
    }

    private void Awake(){
        InitializeCamera();
        InitializeGrid();
    }
        
    private void InitializeCamera(){
        if (!camera2D) camera2D = GetComponentInChildren<CameraSnapFollow>();
        if (camera2D) camera2D.SetGridSize(GridSize);
    }

    private void InitializeGrid(){
        if (_initialized) return;
        _initialized = true;
        Setup();
    }

    private void Setup(){
        _childrenRooms = transform.GetComponentsInChildren<Room2D>();
        SetLimits();
        SetMapSize();
        SetRooms();
    }

    private void SetRooms(){
        _rooms = new Room2D[_mapSize.x, _mapSize.y];

        for (int i = 0; i < _childrenRooms.Length; i++){
            var gridId = GetGridPos(_childrenRooms[i].transform.position, _leftmost.transform.position,
                _bottommost.transform.position);

            Assert.IsNull(_rooms[gridId.x, gridId.y],
                $"Room '{_childrenRooms[i]?.name}' overlaps room '{_rooms[gridId.x, gridId.y]?.name}'");

            _childrenRooms[i].GridId = gridId;
            _rooms[gridId.x, gridId.y] = _childrenRooms[i];
        }
    }

    private void SetMapSize(){
        _mapSize = new Vector2Int(
            1 + Mathf.RoundToInt((_rightmost.transform.position.x - _leftmost.transform.position.x) / GridSize),
            1 + Mathf.RoundToInt((_topmost.transform.position.y - _bottommost.transform.position.y) / GridSize)
        );
    }

    private void SetLimits(){
        _leftmost = _rightmost = null;
        _bottommost = _topmost = null;
        for (int i = 0; i < _childrenRooms.Length; i++){
            _childrenRooms[i].OwnerGrid2D = this;

            Vector3 roomPos = _childrenRooms[i].transform.position;
            if (!_leftmost || roomPos.x < _leftmost.transform.position.x){
                _leftmost = _childrenRooms[i];
            }

            if (!_rightmost || (roomPos.x > _rightmost.transform.position.x)){
                _rightmost = _childrenRooms[i];
            }

            if (!_bottommost || (roomPos.y < _bottommost.transform.position.y)){
                _bottommost = _childrenRooms[i];
            }

            if (!_topmost || (roomPos.y > _topmost.transform.position.y)){
                _topmost = _childrenRooms[i];
            }
        }
    }

    public Room2D GetRoom(Vector2Int id){
        bool outOfBounds = id.x < 0 || id.x >= _mapSize.x || id.y < 0 || id.y >= _mapSize.y;
        return outOfBounds ? null : Rooms[id.x, id.y];
    }

    public Room2D GetRoom(Vector3 position){
        return GetRoom(GetGridPos(position, _leftmost.transform.position, _bottommost.transform.position));
    }

    private Vector2Int GetGridPos(Vector3 position, Vector3 leftmost, Vector3 bottommost){
        return new Vector2Int(
            Mathf.RoundToInt((position.x - leftmost.x) / GridSize),
            Mathf.RoundToInt((position.y - bottommost.y) / GridSize));
    }
        
    private void OnDrawGizmos(){
        Setup();
        int extraGrids = 2;
        float halfGridSize = (float)GridSize/2;
        var minLeft = _leftmost.transform.position;
        var minBottom = _bottommost.transform.position;
        float bottom =  (minBottom.y - halfGridSize) - (extraGrids * halfGridSize);
        float left = (minLeft.x - halfGridSize) - (extraGrids * halfGridSize);
        float length = ((_rooms.GetLength(0)-1) + extraGrids) * GridSize;
        float height = ((_rooms.GetLength(1)-1) + extraGrids)* GridSize;
            
        Vector3 posIni = new Vector3(left,bottom,transform.position.z);

        for (int i = 0; i <= _rooms.GetLength(0) + extraGrids; i++){
            DrawVerticalLine(posIni, i, height,left);
            for (int j = 0; j <= _rooms.GetLength(1)+ extraGrids; j++){
                DrawHorizontalLine(posIni, j, length,bottom);
            }
        }
    }

    private void DrawHorizontalLine(Vector3 posIni, int j, float lenght, float bottommost){
        Vector3 endPos;
        Vector3 posY = posIni;
        posY.y = bottommost + GridSize * j;
        endPos = posY;
        endPos.x = posY.x + lenght + GridSize;
        Gizmos.DrawLine(posY, endPos);
    }

    private void DrawVerticalLine(Vector3 posIni, int i, float height, float leftmost){
        Vector3 posX = posIni;
        posX.x = leftmost + GridSize * i;
        Vector3 endPos = posX;
        endPos.y = posIni.y + height + GridSize;
        Gizmos.DrawLine(posX, endPos);
    }
}
