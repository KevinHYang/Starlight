using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace HelloWorld
{

    enum AgentMode { shoot, move, none };
   
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class MainPage : HelloWorld.Common.LayoutAwarePage
    {
        Enum player_mode = AgentMode.none;
        Boolean gameOver = false;
        Agent agent = new Agent();
        Pit pit1 = new Pit();
        Pit pit2 = new Pit();
        Wumpus wumpus = new Wumpus();
        Room room = new Room();
        Bat bat = new Bat();
        GameStart gameStart = new GameStart();

        public MainPage()
        {
            this.InitializeComponent();
        }


        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            gameStart.init();
            //agent = gameStart.getAgent();
            Dictionary<int, Room> roomInfo = gameStart.getRooms();
            wumpus = gameStart.getWumpus();
            bat = gameStart.getBats();
            pit1 = gameStart.getPits()[0];
            pit2 = gameStart.getPits()[1];
            int[] paths = gameStart.getRoomPath();
            optionA.Content = "Room " + paths[0];
            optionB.Content = "Room " + paths[1];
            optionC.Content = "Room " + paths[2];
            foreach (KeyValuePair<int, Room> entry in roomInfo)
            {
                int tmpKey = entry.Key;
                Room tmpRoom = entry.Value;
                consoleOutput.Text += "-----------------------\n";
//                consoleOutput.Text += "Key value is " + tmpKey + "\n";
                consoleOutput.Text += "Room number is " + tmpRoom.getRoomNum() + "\n";
//                consoleOutput.Text += "Room id is " + tmpRoom.getId() + "\n";
//                consoleOutput.Text += "Room left is " + tmpRoom.getLeft().getRoomNum() + "\n";
//                consoleOutput.Text += "Room right is " + tmpRoom.getRight().getRoomNum() + "\n";
//                consoleOutput.Text += "Room behind is " + tmpRoom.getBack().getRoomNum() + "\n";
                if (tmpRoom.getCreature() != null)
                {
                    consoleOutput.Text += "Contains " + tmpRoom.getCreature().getRole() + "\n";
                }
            }
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }

        private void Move_Click(object sender, RoutedEventArgs e)
        {
            player_mode = AgentMode.move;
            optionA.Visibility = Visibility.Visible;
            optionB.Visibility = Visibility.Visible;
            optionC.Visibility = Visibility.Visible;
            returnBtn.Visibility = Visibility.Visible;
            moveBtn.Visibility = Visibility.Collapsed;
            shootBtn.Visibility = Visibility.Collapsed;
        }

        private void Shoot_Click(object sender, RoutedEventArgs e)
        {
            player_mode = AgentMode.shoot;
            optionA.Visibility = Visibility.Visible;
            optionB.Visibility = Visibility.Visible;
            optionC.Visibility = Visibility.Visible;
            returnBtn.Visibility = Visibility.Visible;
            moveBtn.Visibility = Visibility.Collapsed;
            shootBtn.Visibility = Visibility.Collapsed;
        }

        private void Option_Select(object sender, RoutedEventArgs e)
        {
            consoleOutput.Text = "";
            Button clicked = (Button)sender;
            String room = (String) clicked.Content;
            int roomNum = int.Parse(room.Substring(room.Length - 2));

            //consoleOutput.Text = "Before Room:  " + gameStart.getAgent().getRoom().getRoomNum();
//            consoleOutput.Text += "";
            if (player_mode.Equals(AgentMode.move))
            {
                String[] result = gameStart.move(gameStart.getAgent(), roomNum);
                //String[] result = { "2" };
                if (result.Length == 1)
                {
                    String gameCode = result[0];
                    String gameMsg = "";
                    if (gameCode.Equals(ErrorCode.gameLost)) gameMsg = "Game Lost! "; //gameOver = true;
                    if (gameCode.Equals(ErrorCode.creatureNotFound)) gameMsg = "Creature Not Found!";
                    if (gameCode.Equals(ErrorCode.creatureMovementFailure)) gameMsg = "Creature Movement Failure!";
                    if (gameCode.Equals(ErrorCode.roomNotFound)) gameMsg = "Room Not Found!";
                    gameMsg += gameCode;
                    Frame rootFrame = Window.Current.Content as Frame;
                    rootFrame.Navigate(typeof(BasicPage1), gameMsg);
                }
                else if (result.Length == 3)
                {
                    consoleOutput.Text += "We moved to room " + gameStart.getAgent().getRoom().getRoomNum() + "\n";

                    foreach (String s in result)
                    {
                        if (! s.Equals("Empty"))
                        {
                            consoleOutput.Text += s;
                            consoleOutput.Text += "\n";
                        }
                    }
                }

            }
            else if (player_mode.Equals(AgentMode.shoot))
            {
                String result = gameStart.shoot(roomNum);
                //String result = "win";
                if(result.Equals("win")){
                    //gameOver = false;
                    Frame rootFrame = Window.Current.Content as Frame;
                    rootFrame.Navigate(typeof(BasicPage1), result);
                } 
            }
            optionA.Visibility = Visibility.Collapsed;
            optionB.Visibility = Visibility.Collapsed;
            optionC.Visibility = Visibility.Collapsed;
            returnBtn.Visibility = Visibility.Collapsed;
            moveBtn.Visibility = Visibility.Visible;
            shootBtn.Visibility = Visibility.Visible;
            int[] rmList = gameStart.getRoomPath();
            optionA.Content = "Room " + rmList[0];
            optionB.Content = "Room " + rmList[1];
            optionC.Content = "Room " + rmList[2];

            //consoleOutput.Text = "After  Room:  " + gameStart.getAgent().getRoom().getRoomNum();
            //consoleOutput.Text += "After  RoomPath:  " + gameStart.getRoomPath()[0] + "," + gameStart.getRoomPath()[1] + "," + gameStart.getRoomPath()[2];
//            consoleOutput.Text += "";
            //loop();
        }

        private void Return_Select(object sender, RoutedEventArgs e)
        {
            optionA.Visibility = Visibility.Collapsed;
            optionB.Visibility = Visibility.Collapsed;
            optionC.Visibility = Visibility.Collapsed;
            returnBtn.Visibility = Visibility.Collapsed;
            moveBtn.Visibility = Visibility.Visible;
            shootBtn.Visibility = Visibility.Visible;
            //App.Current.Exit();

            consoleOutput.Text = "Currently in Room " + gameStart.getAgent().getRoom().getRoomNum();
        }

        private void loop()
        {
            //consoleOutput.Text += "Currently in Room " + agent.getRoom().getRoomNum() + "\n";
            consoleOutput.Text += "Wumpus is in Room " + wumpus.getRoom().getRoomNum() + "\n";
            consoleOutput.Text += "Bat is in Room " + bat.getRoom().getRoomNum() + "\n";
            consoleOutput.Text += "Pit 1 is in Room " + pit1.getRoom().getRoomNum() + "\n";
            consoleOutput.Text += "Pit 2 is in Room " + pit2.getRoom().getRoomNum() + "\n";
            //String[] sense = gameStart.sense(gameStart.getAgent());
            //if (sense != null)
            /*{
                foreach (String s in sense)
                {
                    consoleOutput.Text += s;
                    consoleOutput.Text += "\n";
                }
            }*/
        }

    }
}
