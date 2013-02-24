using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{
    public class Agent : Creature
    {
        protected int maxArrows = 5;
        protected int maxRoomsToShoot = 5;
        public Room room = null;
        public Agent()
        {
            role = "Agent";
        }

        public Agent(Room room)
        {
            role = "";
            this.room = room;
        }

        public Room getRoom()
        {
            return room;
        }

        public void setRoom(Room room)
        {
            this.room = room;
        }

        public String[] move(int roomNum)
        {
            String[] result = null;
            Room nextRoom = getNextRoom(roomNum);
            Creature creature = nextRoom.getCreature();
            if (creature == null)
            {
                //nextRoom.roomReset(this.room);
                this.room.setCreature(null);
                this.room = nextRoom;
                this.room.setCreature(this);
                result = new String[3] { room.getLeft().getRoomNum().ToString(), room.getRight().getRoomNum().ToString(), room.getBack().getRoomNum().ToString() };
            }
            else
            {
                if (creature.getRole().Equals("Bat")) {
                    //result = new String[1] { "Bat attacked you, you ran into other room." };
                    result = new String[1] { "Bat is still in the lab under research process...." };
                    // Move to other random room and reset rooms.
                } else {
                    result = new String[1] { "You are killed. Sorry but you lost...." };
                }
            }
            return result;
        }

        public List<String> sense()
        {
            List<String> senses = new List<String>();
            Creature creature = null;
            creature = room.getLeft().getCreature();
            if (creature != null)
            {
                senses.Add(getFeeling(creature));
            }
            creature = room.getRight().getCreature();
            if (creature != null)
            {
                senses.Add(getFeeling(creature));
            }
            creature = room.getBack().getCreature();
            if (creature != null)
            {
                senses.Add(getFeeling(creature));
            }
            return senses;
        }

        public String getFeeling(Creature creature)
        {
            String role = creature.getRole();
            if (role.Equals("Wumpus")) {
                return "I smell a Wumpus.";
            } else if (role.Equals("Bat")) {
                return "Bats nearby.";
            } else if (role.Equals("Pit")) {
                return "I feel a draft.";
            }
            return "";
        }

        public String shoot(int roomNum)
        {
            Random random = new Random();
            int nextRoomNum = 0;
            Room[] rooms = new Room[5];
            Room nextRoom = getNextRoom(roomNum);
            if (nextRoom==null) {
                return "Cannot Shoot";
            }
            maxArrows--;
            rooms[0] = nextRoom;

            int i = 1;
            while (i < maxRoomsToShoot)
            {
                nextRoomNum = random.Next(1, 2);
                if (nextRoomNum == 1) {
                    nextRoomNum = nextRoom.getLeft().getRoomNum();
                } else {
                    nextRoomNum = nextRoom.getRight().getRoomNum();
                }
                nextRoom = getNextRoom(nextRoomNum);
                rooms[i] = nextRoom;
                i++;
            }

            for (int j = 0; j < rooms.Length; j++ )
            {
                Creature creature = rooms[j].getCreature();
                String role = creature.getRole();
                if (role.Equals("Wumpus")) {
                    return "win";
                }
            }
            if (maxArrows == 0) {
                return "lose";
            }
            return "Nothing happened";
        }
        
        public int[] getRoomPath()
        {
            Room tempRoom = room.getLeft();
            int leftRoomNum = tempRoom.getRoomNum();
            int rightRoomNum = room.getRight().getRoomNum();
            int backRoomNum = room.getBack().getRoomNum();
            int[] roomPath = { leftRoomNum, rightRoomNum, backRoomNum };
            return roomPath;
        }

        public Room getNextRoom(int roomNum)
        {
            if (room.getLeft().getRoomNum() == roomNum)
            {
                return room.getLeft();
            }
            else if (room.getRight().getRoomNum() == roomNum)
            {
                return room.getRight();
            }
            else
            {
                return null;
            }
        }

    }
}
