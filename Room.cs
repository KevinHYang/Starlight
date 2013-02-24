using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{
    public class Room
    {
        protected int id = 0;
        protected int roomNum = 0;
        protected Creature creature = null;
        protected Room left = null;
        protected Room right = null;
        protected Room back = null;

        public int getId()
        {
            return id;
        }

        public void setId(int id)
        {
            this.id = id;
        }

        public int getRoomNum()
        {
            return roomNum;
        }

        public void setRoomNum(int roomNum)
        {
            this.roomNum = roomNum;
        }

        public Creature getCreature()
        {
            return creature;
        }

        public void setCreature(Creature creature)
        {
            this.creature = creature;
        }

        public Room getLeft()
        {
            return left;
        }

        public void setLeft(Room left)
        {
            this.left = left;
        }

        public Room getRight()
        {
            return right;
        }

        public void setRight(Room right)
        {
            this.right = right;
        }

        public Room getBack()
        {
            return back;
        }

        public void setBack(Room back){
            this.back = back;
        }

        public void roomReset(Room back)
        {
            Room temp = this.back;
            this.back = back;
            left = temp;
        }

        public Room()
        {

        }

        public Room(int id)
        {
            this.id = id;
        }
        
    }
}
