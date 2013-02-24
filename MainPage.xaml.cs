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
            GameMain gameStart = new GameMain();
            gameStart.init();
            agent = gameStart.getAgent();
            Dictionary<int, Room> roomInfo = gameStart.getRooms();
            wumpus = gameStart.getWumpus();
            bat = gameStart.getBats();
            pit1 = gameStart.getPits()[0];
            pit2 = gameStart.getPits()[1];
            int[] paths = agent.getRoomPath();
            optionA.Content = "Room " + paths[0];
            optionB.Content = "Room " + paths[1];
            optionC.Content = "Room " + paths[2];
            foreach (KeyValuePair<int, Room> entry in roomInfo)
            {
                int tmpKey = entry.Key;
                Room tmpRoom = entry.Value;
                consoleOutput.Text += "-----------------------\n";
                consoleOutput.Text += "Key value is " + tmpKey + "\n";
                consoleOutput.Text += "Room number is " + tmpRoom.getRoomNum() + "\n";
                consoleOutput.Text += "Room id is " + tmpRoom.getId() + "\n";
                consoleOutput.Text += "Room left is " + tmpRoom.getLeft().getRoomNum() + "\n";
                consoleOutput.Text += "Room right is " + tmpRoom.getRight().getRoomNum() + "\n";
                consoleOutput.Text += "Room behind is " + tmpRoom.getBack().getRoomNum() + "\n";
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
            Button clicked = (Button)sender;
            String room = (String) clicked.Content;
            int roomNum = int.Parse(room.Substring(room.Length - 2));
            int[] rmList = agent.getRoomPath();
            optionA.Content = "Room " + rmList[0];
            optionB.Content = "Room " + rmList[1];
            optionC.Content = "Room " + rmList[2];

            if (player_mode.Equals(AgentMode.move))
            {
                String[] result = agent.move(roomNum);
                //String[] result = { "2" };
                if (result.Length == 1)
                {
                    gameOver = true;
                    Frame rootFrame = Window.Current.Content as Frame;
                    
                    rootFrame.Navigate(typeof(BasicPage1), gameOver);
                }
                consoleOutput.Text = "We moved to room " + agent.getRoom().getRoomNum() +"\n";
            }
            else if (player_mode.Equals(AgentMode.shoot))
            {
                String result = agent.shoot(roomNum);
                //String result = "win";
                if(result.Equals("win")){
                    gameOver = false;
                    Frame rootFrame = Window.Current.Content as Frame;
                    rootFrame.Navigate(typeof(BasicPage1), gameOver);
                } 
            }
            optionA.Visibility = Visibility.Collapsed;
            optionB.Visibility = Visibility.Collapsed;
            optionC.Visibility = Visibility.Collapsed;
            returnBtn.Visibility = Visibility.Collapsed;
            moveBtn.Visibility = Visibility.Visible;
            shootBtn.Visibility = Visibility.Visible;
            loop();
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

            consoleOutput.Text = "Currently in Room "+ agent.getRoom().getRoomNum();
        }

        private void loop()
        {
            //consoleOutput.Text += "Currently in Room " + agent.getRoom().getRoomNum() + "\n";
            consoleOutput.Text += "Wumpus is in Room " + wumpus.getRoom().getRoomNum() + "\n";
            consoleOutput.Text += "Bat is in Room " + bat.getRoom().getRoomNum() + "\n";
            consoleOutput.Text += "Pit 1 is in Room " + pit1.getRoom().getRoomNum() + "\n";
            consoleOutput.Text += "Pit 2 is in Room " + pit2.getRoom().getRoomNum() + "\n";
            List<String> sense = agent.sense();
            if (sense != null)
            {
                foreach (String s in sense)
                {
                    consoleOutput.Text += s;
                    consoleOutput.Text += "\n";
                }
            }
        }

    }
}
