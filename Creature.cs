using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{
    public abstract class Creature
    {
        protected int id;
        protected String role;
        public void move() {}
        public Room room = null;
        public String getRole() { return role; }
        public void setRoom(Room room)
        {
            this.room = room;
        }

        public Room getRoom()
        {
            return room;
        }

    }
}
