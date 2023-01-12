using Unity.Netcode.Transports.UNET;
using Unity.Netcode;
using UnityEngine;
using TMPro;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Collections.Generic;

namespace HelloWorld
{
    public class HelloWorldManager : MonoBehaviour
    {
        public static bool isStarted = false;
        public Canvas lobby_canvas;
        public Canvas game_Canvas;
        public Canvas host_IPadress;
        public TMP_InputField input_ipAdress;
        public TMP_Text host_IPadress_textbox;
        private string ipAdress = "127.0.0.1";

        private int pieceNumber;
        public static bool turnClient = false;

        public static bool isClient = false;
        public static bool isHost = false;
        public static bool isServer = false;

        public static bool gameEnded = false;
        public static bool blueWon = false;
        public static bool redWon = false;
        public static bool draw = false;

        public void ShowIPAdressToHost()
        {
            string adress = "";
            var strHostName = "";
            strHostName = System.Net.Dns.GetHostName();
            var ipEntry = System.Net.Dns.GetHostEntry(strHostName);
            IPAddress[] addr = ipEntry.AddressList;
            foreach (var item in addr)
            {
                IPAddress address;
                if (IPAddress.TryParse(item.ToString(), out address))
                {
                    switch (address.AddressFamily)
                    {
                        case System.Net.Sockets.AddressFamily.InterNetwork:
                            adress = item.ToString();
                            break;
                        case System.Net.Sockets.AddressFamily.InterNetworkV6:
                            adress = item.ToString();
                            break;
                        default:
                            // umm... yeah... I'm going to need to take your red packet and...
                            break;
                    }
                }
            }
            host_IPadress_textbox.text = adress;
            host_IPadress.enabled = true;
        }
        public void UpdateIPAdress()
        {
            ipAdress = input_ipAdress.text;
        }
        public void StartHost()
        {
            isStarted = true;
            lobby_canvas.enabled = false;
            game_Canvas.enabled = true;
            NetworkManager.Singleton.StartHost();
            isHost = true;

            var playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
            var player = playerObject.GetComponent<HelloWorldPlayer>();
            player.hidePlayerPrefabs();
            player.spawnPieces();

            Client_Camera.Change_Cameras();
        }
        public void StartClient()
        {
            isStarted = true;
            lobby_canvas.enabled = false;
            game_Canvas.enabled = true;
            UNetTransport transport = NetworkManager.Singleton.GetComponent<UNetTransport>();
            transport.ConnectAddress = ipAdress;
            NetworkManager.Singleton.StartClient();
            isClient = true;

            Client_Camera.Change_Cameras();
        }
        public void StartServer()
        {
            isStarted = true;
            lobby_canvas.enabled = false;
            game_Canvas.enabled = true;
            NetworkManager.Singleton.StartServer();
            isServer = true;
        }

        List<int> movedBluePieces = new List<int>();
        bool successBlue;
        List<int> movedRedPieces = new List<int>();
        bool successRed;
        GameObject button;

        public Canvas blueVictoryCanvas;
        public Canvas redVictoryCanvas;
        public Canvas drawCanvas;

        public static int[,] Grid = new int[3, 3] { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };

        public void setPiece(int pieceNumber)
        {
            this.pieceNumber = pieceNumber;
        }

        public void movePiece(int position)
        {
            var playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
            var player = playerObject.GetComponent<HelloWorldPlayer>();
            if (isHost && !movedBluePieces.Contains(pieceNumber))
            {
                successBlue = player.moveBluePiece(pieceNumber, getCoordinatesBlue(position), getAreaCoordinatesBlue(position));
                if (successBlue)
                {
                    movedBluePieces.Add(pieceNumber);
                    button = GameObject.Find("Move " + pieceNumber);
                    button.SetActive(false);
                }
            }
            if (isClient && !movedRedPieces.Contains(pieceNumber))
            {
                successRed = player.moveRedPiece(pieceNumber, getCoordinatesRed(position), getAreaCoordinatesRed(position));
                if (successRed)
                {
                    movedRedPieces.Add(pieceNumber);
                    button = GameObject.Find("Move " + pieceNumber);
                    button.SetActive(false);
                }
            }
        }

        private void Update()
        {
            Check_if_game_is_won();
            if (gameEnded)
                endTheGame();
        }

        private void Check_if_game_is_won()
        {
            if (Grid[0, 0] + Grid[0, 1] + Grid[0, 2] == 3)
            {
                HelloWorldManager.blueWon = true;
                HelloWorldManager.gameEnded = true;
            }
            if (Grid[1, 0] + Grid[1, 1] + Grid[1, 2] == 3)
            {
                HelloWorldManager.blueWon = true;
                HelloWorldManager.gameEnded = true;
            }
            if (Grid[2, 0] + Grid[2, 1] + Grid[2, 2] == 3)
            {
                HelloWorldManager.blueWon = true;
                HelloWorldManager.gameEnded = true;
            }
            if (Grid[0, 0] + Grid[1, 0] + Grid[2, 0] == 3)
            {
                HelloWorldManager.blueWon = true;
                HelloWorldManager.gameEnded = true;
            }
            if (Grid[0, 1] + Grid[1, 1] + Grid[2, 1] == 3)
            {
                HelloWorldManager.blueWon = true;
                HelloWorldManager.gameEnded = true;
            }
            if (Grid[0, 2] + Grid[1, 2] + Grid[2, 2] == 3)
            {
                HelloWorldManager.blueWon = true;
                HelloWorldManager.gameEnded = true;
            }
            if (Grid[0, 0] + Grid[1, 1] + Grid[2, 2] == 3)
            {
                HelloWorldManager.blueWon = true;
                HelloWorldManager.gameEnded = true;
            }
            if (Grid[0, 2] + Grid[1, 1] + Grid[2, 0] == 3)
            {
                HelloWorldManager.blueWon = true;
                HelloWorldManager.gameEnded = true;
            }

            if (Grid[0, 0] + Grid[0, 1] + Grid[0, 2] == -3)
            {
                HelloWorldManager.redWon = true;
                HelloWorldManager.gameEnded = true;
            }
            if (Grid[1, 0] + Grid[1, 1] + Grid[1, 2] == -3)
            {
                HelloWorldManager.redWon = true;
                HelloWorldManager.gameEnded = true;
            }
            if (Grid[2, 0] + Grid[2, 1] + Grid[2, 2] == -3)
            {
                HelloWorldManager.redWon = true;
                HelloWorldManager.gameEnded = true;
            }
            if (Grid[0, 0] + Grid[1, 0] + Grid[2, 0] == -3)
            {
                HelloWorldManager.redWon = true;
                HelloWorldManager.gameEnded = true;
            }
            if (Grid[0, 1] + Grid[1, 1] + Grid[2, 1] == -3)
            {
                HelloWorldManager.redWon = true;
                HelloWorldManager.gameEnded = true;
            }
            if (Grid[0, 2] + Grid[1, 2] + Grid[2, 2] == -3)
            {
                HelloWorldManager.redWon = true;
                HelloWorldManager.gameEnded = true;
            }
            if (Grid[0, 0] + Grid[1, 1] + Grid[2, 2] == -3)
            {
                HelloWorldManager.redWon = true;
                HelloWorldManager.gameEnded = true;
            }
            if (Grid[0, 2] + Grid[1, 1] + Grid[2, 0] == -3)
            {
                HelloWorldManager.redWon = true;
                HelloWorldManager.gameEnded = true;
            }
        }

        private void endTheGame()
        {
            if (blueWon)
            {
                blueVictoryCanvas.enabled = true;
                game_Canvas.enabled = false;
            }
            else if (redWon)
            {
                redVictoryCanvas.enabled = true;
                game_Canvas.enabled = false;
            }
            else
            {
                drawCanvas.enabled = true;
                game_Canvas.enabled = false;
            }
        }

        private Object_Coordinates getAreaCoordinatesBlue(int position)
        {
            if (position == 1)
                return Area_Coordinates.p1;
            if (position == 2)
                return Area_Coordinates.p2;
            if (position == 3)
                return Area_Coordinates.p3;
            if (position == 4)
                return Area_Coordinates.p4;
            if (position == 5)
                return Area_Coordinates.p5;
            if (position == 6)
                return Area_Coordinates.p6;
            if (position == 7)
                return Area_Coordinates.p7;
            if (position == 8)
                return Area_Coordinates.p8;
            if (position == 9)
                return Area_Coordinates.p9;
            return new Object_Coordinates();
        }
        private Object_Coordinates getAreaCoordinatesRed(int position)
        {
            if (position == 1)
                return Area_Coordinates.p9;
            if (position == 2)
                return Area_Coordinates.p8;
            if (position == 3)
                return Area_Coordinates.p7;
            if (position == 4)
                return Area_Coordinates.p6;
            if (position == 5)
                return Area_Coordinates.p5;
            if (position == 6)
                return Area_Coordinates.p4;
            if (position == 7)
                return Area_Coordinates.p3;
            if (position == 8)
                return Area_Coordinates.p2;
            if (position == 9)
                return Area_Coordinates.p1;
            return new Object_Coordinates();
        }
        private Vector3 getCoordinatesBlue(int position)
        {
            if (position == 1)
                return Coordinates.p1;
            if (position == 2)
                return Coordinates.p2;
            if (position == 3)
                return Coordinates.p3;
            if (position == 4)
                return Coordinates.p4;
            if (position == 5)
                return Coordinates.p5;
            if (position == 6)
                return Coordinates.p6;
            if (position == 7)
                return Coordinates.p7;
            if (position == 8)
                return Coordinates.p8;
            if (position == 9)
                return Coordinates.p9;
            return new Vector3(0, 0, 0);
        }
        private Vector3 getCoordinatesRed(int position)
        {
            if (position == 1)
                return Coordinates.p9;
            if (position == 2)
                return Coordinates.p8;
            if (position == 3)
                return Coordinates.p7;
            if (position == 4)
                return Coordinates.p6;
            if (position == 5)
                return Coordinates.p5;
            if (position == 6)
                return Coordinates.p4;
            if (position == 7)
                return Coordinates.p3;
            if (position == 8)
                return Coordinates.p2;
            if (position == 9)
                return Coordinates.p1;
            return new Vector3(0, 0, 0);
        }
    }
}