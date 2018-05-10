using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PitfallRoomManager : MonoBehaviour
{
    private List<PitfallRoom> pitfallRooms = new List<PitfallRoom>();

    public enum States { Inactive, Active };
    private States state = States.Inactive;

    void Start()
    {
        foreach (Transform child in transform)
        {
            PitfallRoom room = child.GetComponent<PitfallRoom>();
            pitfallRooms.Add(room);
        }
    }

    public void Update()
    {
        switch (state)
        {
            case States.Inactive:
                break;

            case States.Active:
                bool allRoomsUp = true;
                foreach (PitfallRoom room in pitfallRooms)
                {
                    if (room.GetState != PitfallRoom.States.Up)
                    {
                        allRoomsUp = false;
                        break;
                    }
                }

                if (allRoomsUp)
                {
                    state = States.Inactive;
                }
                break;
        }
    }

    public void Activate()
    {
        if (state == States.Inactive)
        {
            int room1 = Random.Range(0, pitfallRooms.Count);
            int room2;
            do
            {
                room2 = Random.Range(0, pitfallRooms.Count);
            }
            while (room2 == room1);

            for (int i = 0; i < pitfallRooms.Count; i++)
            {
                if (i == room1 || i == room2)
                {
                    continue;
                }
                else
                {
                    pitfallRooms[i].Drop();
                }
            }

            state = States.Active;
        }
    }
}
