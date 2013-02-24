using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    class Arrow : Creature
    {

        protected Room room;

        public Arrow()
        {
        }

        public Arrow(Room room)
        {
            role = "Arrow";
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
    }
}
