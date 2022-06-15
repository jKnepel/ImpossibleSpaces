using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance { get; private set; }

    [SerializeField] private Player _player;
    [SerializeField] private Room[] _editorRooms;

    private Dictionary<float, Room> _rooms = new Dictionary<float, Room>();
    private Dictionary<float, Room> _currentInteriorAreas = new Dictionary<float, Room>();
    private Dictionary<float, Room> _currentTransitionAreas = new Dictionary<float, Room>();
    private Dictionary<float, int> _visibleRooms = new Dictionary<float, int>();

	private void Awake()
	{
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        foreach (Room room in _editorRooms)
		{
            _rooms.Add(room.ID, room);
		}
    }

    public void RemoveRoomsOnTransition(Room[] rooms)
	{
        foreach (Room room in rooms)
		{
            if (_visibleRooms.ContainsKey(room.ID) && _visibleRooms[room.ID] == 1)
			{
                _visibleRooms.Remove(room.ID);
                _rooms[room.ID].gameObject.SetActive(false);
			} 
            else
			{
                _visibleRooms[room.ID] -= 1;
			}
		}
	}

    public void AddRoomsOnTransition(Room[] rooms)
    {
        foreach (Room room in rooms)
        {
            if (!_visibleRooms.ContainsKey(room.ID))
            {
                _visibleRooms.Add(room.ID, 1);
                _rooms[room.ID].gameObject.SetActive(true);
            }
            else
            {
                _visibleRooms[room.ID] += 1;
            }
        }
    }
}
