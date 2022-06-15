using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomLoader : MonoBehaviour
{
	[SerializeField] private Room[] _visibleRooms;

	public void OnTriggerEnter(Collider other)
	{
		if (other.GetComponent<Player>() != null)
		{
			RoomManager.Instance.AddRoomsOnTransition(_visibleRooms);
		}
	}

	public void OnTriggerExit(Collider other)
	{
		if (other.GetComponent<Player>() != null)
		{
			RoomManager.Instance.RemoveRoomsOnTransition(_visibleRooms);
		}
	}
}
