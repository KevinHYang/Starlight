using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntTheWumpus
{
    public class GameMain
    {
        Boolean gameRunning = false;
        Boolean gameStatus;

        Dictionary<int, Room> RoomHold = new Dictionary<int, Room>(); // <id, Room>
        Dictionary<int, int> RoomNumbers = new Dictionary<int, int>(); // <id, room number>

        Agent JamesBond = new Agent();
        Wumpus TheBeast = new Wumpus();
        Bat SuperBats = new Bat();
        Pit Pit1 = new Pit();
        Pit Pit2 = new Pit();

        public void init()
        {
            InitiateRooms();
            LinkRooms();
            RandomizeRooms();
            CycleCreature();
        }

        private void InitiateRooms()
        {
            for (int i = 1; i < 20; i++)
            {
                RoomHold.Add(i, new Room());
                RoomHold[i].setId(i);
            }
        }

        private void LinkRooms()
        {
            for (int i = 1; i <= 20; i++)
            {
                switch (i)
                {
                    case 1:
                        RoomHold[i].setBack(RoomHold[6]);
                        RoomHold[i].setLeft(RoomHold[5]);
                        RoomHold[i].setRight(RoomHold[2]);
                        break;
                    case 2:
                        RoomHold[i].setBack(RoomHold[8]);
                        RoomHold[i].setLeft(RoomHold[1]);
                        RoomHold[i].setRight(RoomHold[3]);
                        break;
                    case 3:
                        RoomHold[i].setBack(RoomHold[10]);
                        RoomHold[i].setLeft(RoomHold[2]);
                        RoomHold[i].setRight(RoomHold[4]);
                        break;
                    case 4:
                        RoomHold[i].setBack(RoomHold[20]);
                        RoomHold[i].setLeft(RoomHold[3]);
                        RoomHold[i].setRight(RoomHold[5]);
                        break;
                    case 5:
                        RoomHold[i].setBack(RoomHold[18]);
                        RoomHold[i].setLeft(RoomHold[4]);
                        RoomHold[i].setRight(RoomHold[1]);
                        break;
                    case 6:
                        RoomHold[i].setBack(RoomHold[13]);
                        RoomHold[i].setLeft(RoomHold[1]);
                        RoomHold[i].setRight(RoomHold[7]);
                        break;
                    case 7:
                        RoomHold[i].setBack(RoomHold[11]);
                        RoomHold[i].setLeft(RoomHold[6]);
                        RoomHold[i].setRight(RoomHold[8]);
                        break;
                    case 8:
                        RoomHold[i].setBack(RoomHold[9]);
                        RoomHold[i].setLeft(RoomHold[7]);
                        RoomHold[i].setRight(RoomHold[2]);
                        break;
                    case 9:
                        RoomHold[i].setBack(RoomHold[12]);
                        RoomHold[i].setLeft(RoomHold[8]);
                        RoomHold[i].setRight(RoomHold[10]);
                        break;
                    case 10:
                        RoomHold[i].setBack(RoomHold[17]);
                        RoomHold[i].setLeft(RoomHold[9]);
                        RoomHold[i].setRight(RoomHold[3]);
                        break;
                    case 11:
                        RoomHold[i].setBack(RoomHold[14]);
                        RoomHold[i].setLeft(RoomHold[7]);
                        RoomHold[i].setRight(RoomHold[12]);
                        break;
                    case 12:
                        RoomHold[i].setBack(RoomHold[16]);
                        RoomHold[i].setLeft(RoomHold[11]);
                        RoomHold[i].setRight(RoomHold[9]);
                        break;
                    case 13:
                        RoomHold[i].setBack(RoomHold[18]);
                        RoomHold[i].setLeft(RoomHold[6]);
                        RoomHold[i].setRight(RoomHold[14]);
                        break;
                    case 14:
                        RoomHold[i].setBack(RoomHold[15]);
                        RoomHold[i].setLeft(RoomHold[13]);
                        RoomHold[i].setRight(RoomHold[11]);
                        break;
                    case 15:
                        RoomHold[i].setBack(RoomHold[19]);
                        RoomHold[i].setLeft(RoomHold[14]);
                        RoomHold[i].setRight(RoomHold[16]);
                        break;
                    case 16:
                        RoomHold[i].setBack(RoomHold[17]);
                        RoomHold[i].setLeft(RoomHold[15]);
                        RoomHold[i].setRight(RoomHold[12]);
                        break;
                    case 17:
                        RoomHold[i].setBack(RoomHold[20]);
                        RoomHold[i].setLeft(RoomHold[16]);
                        RoomHold[i].setRight(RoomHold[10]);
                        break;
                    case 18:
                        RoomHold[i].setBack(RoomHold[19]);
                        RoomHold[i].setLeft(RoomHold[5]);
                        RoomHold[i].setRight(RoomHold[13]);
                        break;
                    case 19:
                        RoomHold[i].setBack(RoomHold[20]);
                        RoomHold[i].setLeft(RoomHold[18]);
                        RoomHold[i].setRight(RoomHold[15]);
                        break;
                    case 20:
                        RoomHold[i].setBack(RoomHold[19]);
                        RoomHold[i].setLeft(RoomHold[17]);
                        RoomHold[i].setRight(RoomHold[4]);
                        break;
                }
            }
        }

        private void RandomizeRooms()
        {
            Random rand = new Random();

            List<int> RoomsList = new List<int>();
            for (int i = 1; i <= 20; i++)
            {
                RoomsList.Add(i);
            }

            for (int i = RoomsList.Count; i <= 1; i--)
            {
                int randomIndex = rand.Next(i)+1;
                int number = RoomsList.IndexOf(randomIndex);
                RoomsList.Insert(randomIndex, RoomsList.IndexOf(i));
                RoomsList.Insert(i, number);
                
                RoomNumbers.Add(i, number);
                RoomHold[i].setRoomNum(number);
            }
        }

        private Boolean PlaceCreature(Creature myCreature)
        {
            Random rand = new Random();
            int randomIndex = rand.Next(19) + 1;

            if (RoomHold[randomIndex].getCreature() == null)
            {
                RoomHold[randomIndex].setCreature(myCreature);
                if (myCreature.Equals(JamesBond))
                {
                    JamesBond.setRoom(RoomHold[randomIndex]);
                }
                return false;
            }
            else
            {
                return true;
            }
        }

        private void CycleCreature()
        {
            Boolean agentPlaced = true;
            Boolean wumpusPlaced = true;
            Boolean batPlaced = true;
            Boolean pit1Placed = true;
            Boolean pit2Placed = true;

            while (agentPlaced)
            {
                agentPlaced = PlaceCreature(JamesBond);
            }

            while (wumpusPlaced)
            {
                wumpusPlaced = PlaceCreature(TheBeast);
            }

            while (batPlaced)
            {
                batPlaced = PlaceCreature(SuperBats);
            }

            while (pit1Placed)
            {
                pit1Placed = PlaceCreature(Pit1);
            }

            while (pit2Placed)
            {
                pit2Placed = PlaceCreature(Pit2);
            }
        }
    }
}
