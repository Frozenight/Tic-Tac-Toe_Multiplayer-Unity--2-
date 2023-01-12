using Unity.Netcode;
using UnityEngine;
using UnityEngine.Networking;
using Unity.Netcode.Transports.UNET;
using System.Text.RegularExpressions;

namespace HelloWorld
{
    public class HelloWorldPlayer : NetworkBehaviour
    {
        public static NetworkVariable<Vector3> turn_Value = new NetworkVariable<Vector3>();
        public static NetworkVariable<Vector3> win_Value = new NetworkVariable<Vector3>();
        public static NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
        public static NetworkVariable<Vector3> Blue1Position = new NetworkVariable<Vector3>();
        public static NetworkVariable<Vector3> Blue2Position = new NetworkVariable<Vector3>();
        public static NetworkVariable<Vector3> Blue3Position = new NetworkVariable<Vector3>();
        public static NetworkVariable<Vector3> Blue4Position = new NetworkVariable<Vector3>();
        public static NetworkVariable<Vector3> Blue5Position = new NetworkVariable<Vector3>();
        public static NetworkVariable<Vector3> Blue6Position = new NetworkVariable<Vector3>();
        public static NetworkVariable<Vector3> Red1Position = new NetworkVariable<Vector3>();
        public static NetworkVariable<Vector3> Red2Position = new NetworkVariable<Vector3>();
        public static NetworkVariable<Vector3> Red3Position = new NetworkVariable<Vector3>();
        public static NetworkVariable<Vector3> Red4Position = new NetworkVariable<Vector3>();
        public static NetworkVariable<Vector3> Red5Position = new NetworkVariable<Vector3>();
        public static NetworkVariable<Vector3> Red6Position = new NetworkVariable<Vector3>();
        public static NetworkVariable<bool> gameHasStarted = new NetworkVariable<bool>();
        private NetworkVariable<bool> turn = new NetworkVariable<bool>();

        private Vector3 blueTurnPosition = new Vector3(0, 0, 0);
        private Vector3 redTurnPosition = new Vector3(0, 0, 10);

        [SerializeField]
        private GameObject blue1;
        [SerializeField]
        private GameObject blue2;
        [SerializeField]
        private GameObject blue3;
        [SerializeField]
        private GameObject blue4;
        [SerializeField]
        private GameObject blue5;
        [SerializeField]
        private GameObject blue6;
        [SerializeField]
        private GameObject red1;
        [SerializeField]
        private GameObject red2;
        [SerializeField]
        private GameObject red3;
        [SerializeField]
        private GameObject red4;
        [SerializeField]
        private GameObject red5;
        [SerializeField]
        private GameObject red6;
        [SerializeField]
        private GameObject turn_object;
        [SerializeField]
        private GameObject winCondition_object;
        [SerializeField]
        private GameObject turn_Counter;

        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                hidePlayerPrefabs();
            }
        }

        public void hidePlayerPrefabs()
        {
            if (HelloWorldManager.isServer)
            {
                var outOfSight = new Vector3(0, -5, 0);
                transform.position = outOfSight;
                Position.Value = outOfSight;
            }
            else
            {
                hidePlayerPrefabServerRpc();
            }
        }

        public void spawnPieces()
        {
            SpawnPlayersServerRpc();
        }

        public bool moveBluePiece(int pieceNumber, Vector3 position, Object_Coordinates area_position)
        {
            bool succes = false;
            GameObject turn_obj = GameObject.Find("turn_Object(Clone)");
            if (turn_obj.transform.position == blueTurnPosition)
            {
                if (Check_for_placePiece(pieceNumber, area_position))
                {
                    moveBluePieceServerRpc(pieceNumber, position);
                    Delete(area_position);
                    update_Grid_BlueServerRpc(position);
                    update_Turn_Counter_ServerRpc();
                    changeTurnServerRpc(true);
                    succes = true;
                }
            }
            return succes;
        }
        public bool moveRedPiece(int pieceNumber, Vector3 position, Object_Coordinates area_position)
        {
            bool succes = false;
            GameObject turn_obj = GameObject.Find("turn_Object(Clone)");
            if (turn_obj.transform.position != blueTurnPosition)
            {
                if (Check_for_placePiece(pieceNumber, area_position))
                {
                    moveRedPieceServerRpc(pieceNumber, position);
                    Delete(area_position);
                    update_Grid_RedServerRpc(position);
                    update_Turn_Counter_ServerRpc();
                    changeTurnServerRpc(false);
                    succes = true;
                }
            }
            return succes;
        }

        [ServerRpc(RequireOwnership = false)]
        private void update_Turn_Counter_ServerRpc()
        {
            GameObject turn_object = GameObject.Find("TurnCounter(Clone)");
            turn_object.transform.position = new Vector3(turn_object.transform.position.x + 4, 0, 0);
        }

        private void check_for_draw()
        {
            GameObject turn_object = GameObject.Find("TurnCounter(Clone)");
            if (turn_object.transform.position.x == 48)
            {
                HelloWorldManager.draw = true;
                HelloWorldManager.gameEnded = true;
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void update_Grid_BlueServerRpc(Vector3 position)
        {
            if (position == Coordinates.p1)
            {
                win_Value.Value = new Vector3(10, 10, 10);
            }
            if (position == Coordinates.p2)
                win_Value.Value = new Vector3(10, 20, 10);
            if (position == Coordinates.p3)
                win_Value.Value = new Vector3(10, 30, 10);
            if (position == Coordinates.p4)
                win_Value.Value = new Vector3(20, 10, 10);
            if (position == Coordinates.p5)
                win_Value.Value = new Vector3(20, 20, 10);
            if (position == Coordinates.p6)
                win_Value.Value = new Vector3(20, 30, 10);
            if (position == Coordinates.p7)
                win_Value.Value = new Vector3(30, 10, 10);
            if (position == Coordinates.p8)
                win_Value.Value = new Vector3(30, 20, 10);
            if (position == Coordinates.p9)
                win_Value.Value = new Vector3(30, 30, 10);
        }

        [ServerRpc(RequireOwnership = false)]
        private void update_Grid_RedServerRpc(Vector3 position)
        {
            if (position == Coordinates.p1)
            {
                win_Value.Value = new Vector3(10, 10, -10);
            }
                if (position == Coordinates.p2)
                win_Value.Value = new Vector3(10, 20, -10);
            if (position == Coordinates.p3)
                win_Value.Value = new Vector3(10, 30, -10);
            if (position == Coordinates.p4)
                win_Value.Value = new Vector3(20, 10, -10);
            if (position == Coordinates.p5)
                win_Value.Value = new Vector3(20, 20, -10);
            if (position == Coordinates.p6)
                win_Value.Value = new Vector3(20, 30, -10);
            if (position == Coordinates.p7)
                win_Value.Value = new Vector3(30, 10, -10);
            if (position == Coordinates.p8)
                win_Value.Value = new Vector3(30, 20, -10);
            if (position == Coordinates.p9)
                win_Value.Value = new Vector3(30, 30, -10);
        }

        private void updateGrid()
        {
            GameObject win_obj = GameObject.Find("WinCondition(Clone)");
            Vector3 position = win_obj.transform.position;
            if (position.x == 10 && position.y == 10 && position.z == 10)
                HelloWorldManager.Grid[0, 0] = 1;
            if (position.x == 10 && position.y == 10 && position.z == -10)
                HelloWorldManager.Grid[0, 0] = -1;

            if (position.x == 10 && position.y == 20 && position.z == 10)
                HelloWorldManager.Grid[0, 1] = 1;
            if (position.x == 10 && position.y == 20 && position.z == -10)
                HelloWorldManager.Grid[0, 1] = -1;

            if (position.x == 10 && position.y == 30 && position.z == 10)
                HelloWorldManager.Grid[0, 2] = 1;
            if (position.x == 10 && position.y == 30 && position.z == -10)
                HelloWorldManager.Grid[0, 2] = -1;

            if (position.x == 20 && position.y == 10 && position.z == 10)
                HelloWorldManager.Grid[1, 0] = 1;
            if (position.x == 20 && position.y == 10 && position.z == -10)
                HelloWorldManager.Grid[1, 0] = -1;

            if (position.x == 20 && position.y == 20 && position.z == 10)
                HelloWorldManager.Grid[1, 1] = 1;
            if (position.x == 20 && position.y == 20 && position.z == -10)
                HelloWorldManager.Grid[1, 1] = -1;

            if (position.x == 20 && position.y == 30 && position.z == 10)
                HelloWorldManager.Grid[1, 2] = 1;
            if (position.x == 20 && position.y == 30 && position.z == -10)
                HelloWorldManager.Grid[1, 2] = -1;

            if (position.x == 30 && position.y == 10 && position.z == 10)
                HelloWorldManager.Grid[2, 0] = 1;
            if (position.x == 30 && position.y == 10 && position.z == -10)
                HelloWorldManager.Grid[2, 0] = -1;

            if (position.x == 30 && position.y == 20 && position.z == 10)
                HelloWorldManager.Grid[2, 1] = 1;
            if (position.x == 30 && position.y == 20 && position.z == -10)
                HelloWorldManager.Grid[2, 1] = -1;

            if (position.x == 30 && position.y == 30 && position.z == 10)
                HelloWorldManager.Grid[2, 2] = 1;
            if (position.x == 30 && position.y == 30 && position.z == -10)
                HelloWorldManager.Grid[2, 2] = -1;
        }

        public bool Check_for_placePiece(int pieceNumber, Object_Coordinates area_position)
        {
            bool check = true;
            GameObject blue1 = GameObject.Find("Blue 1(Clone)");
            GameObject blue2 = GameObject.Find("Blue 2(Clone)");
            GameObject blue3 = GameObject.Find("Blue 3(Clone)");
            GameObject blue4 = GameObject.Find("Blue 4(Clone)");
            GameObject blue5 = GameObject.Find("Blue 5(Clone)");
            GameObject blue6 = GameObject.Find("Blue 6(Clone)");
            GameObject red1 = GameObject.Find("Red 1(Clone)");
            GameObject red2 = GameObject.Find("Red 2(Clone)");
            GameObject red3 = GameObject.Find("Red 3(Clone)");
            GameObject red4 = GameObject.Find("Red 4(Clone)");
            GameObject red5 = GameObject.Find("Red 5(Clone)");
            GameObject red6 = GameObject.Find("Red 6(Clone)");
            if (blue1 != null)
            {
                if ((area_position.top_left.x < blue1.transform.position.x) && (area_position.top_left.y > blue1.transform.position.y) && (area_position.top_left.z > blue1.transform.position.z) &&
                        (area_position.bottom_right.x > blue1.transform.position.x) && (area_position.bottom_right.y < blue1.transform.position.y) && (area_position.bottom_right.z < blue1.transform.position.z))
                {
                    if (Compare(pieceNumber, blue1) == 1)
                        check = false;
                }
            }
            if (blue2 != null)
            {
                if ((area_position.top_left.x < blue2.transform.position.x) && (area_position.top_left.y > blue2.transform.position.y) && (area_position.top_left.z > blue2.transform.position.z) &&
        (area_position.bottom_right.x > blue2.transform.position.x) && (area_position.bottom_right.y < blue2.transform.position.y) && (area_position.bottom_right.z < blue2.transform.position.z))
                    if (Compare(pieceNumber, blue2) == 1)
                        check = false;
            }
            if (blue3 != null)
            {
                if ((area_position.top_left.x < blue3.transform.position.x) && (area_position.top_left.y > blue3.transform.position.y) && (area_position.top_left.z > blue3.transform.position.z) &&
            (area_position.bottom_right.x > blue3.transform.position.x) && (area_position.bottom_right.y < blue3.transform.position.y) && (area_position.bottom_right.z < blue3.transform.position.z))
                    if (Compare(pieceNumber, blue3) == 1)
                        check = false;
            }
            if (blue4 != null)
            {
                if ((area_position.top_left.x < blue4.transform.position.x) && (area_position.top_left.y > blue4.transform.position.y) && (area_position.top_left.z > blue4.transform.position.z) &&
        (area_position.bottom_right.x > blue4.transform.position.x) && (area_position.bottom_right.y < blue4.transform.position.y) && (area_position.bottom_right.z < blue4.transform.position.z))
                    if (Compare(pieceNumber, blue4) == 1)
                        check = false;
            }
            if (blue5 != null)
            {
                if ((area_position.top_left.x < blue5.transform.position.x) && (area_position.top_left.y > blue5.transform.position.y) && (area_position.top_left.z > blue5.transform.position.z) &&
        (area_position.bottom_right.x > blue5.transform.position.x) && (area_position.bottom_right.y < blue5.transform.position.y) && (area_position.bottom_right.z < blue5.transform.position.z))
                    if (Compare(pieceNumber, blue5) == 1)
                        check = false;
            }
            if (blue6 != null)
            {
                if ((area_position.top_left.x < blue6.transform.position.x) && (area_position.top_left.y > blue6.transform.position.y) && (area_position.top_left.z > blue6.transform.position.z) &&
        (area_position.bottom_right.x > blue6.transform.position.x) && (area_position.bottom_right.y < blue6.transform.position.y) && (area_position.bottom_right.z < blue6.transform.position.z))
                    if (Compare(pieceNumber, blue6) == 1)
                        check = false;
            }
            if (red1 != null)
            {
                if ((area_position.top_left.x < red1.transform.position.x) && (area_position.top_left.y > red1.transform.position.y) && (area_position.top_left.z > red1.transform.position.z) &&
        (area_position.bottom_right.x > red1.transform.position.x) && (area_position.bottom_right.y < red1.transform.position.y) && (area_position.bottom_right.z < red1.transform.position.z))
                    if (Compare(pieceNumber, red1) == 1)
                        check = false;
            }
            if (red2 != null)
            {
                if ((area_position.top_left.x < red2.transform.position.x) && (area_position.top_left.y > red2.transform.position.y) && (area_position.top_left.z > red2.transform.position.z) &&
        (area_position.bottom_right.x > red2.transform.position.x) && (area_position.bottom_right.y < red2.transform.position.y) && (area_position.bottom_right.z < red2.transform.position.z))
                    if (Compare(pieceNumber, red2) == 1)
                        check = false;
            }
            if (red3 != null)
            {
                if ((area_position.top_left.x < red3.transform.position.x) && (area_position.top_left.y > red3.transform.position.y) && (area_position.top_left.z > red3.transform.position.z) &&
        (area_position.bottom_right.x > red3.transform.position.x) && (area_position.bottom_right.y < red3.transform.position.y) && (area_position.bottom_right.z < red3.transform.position.z))
                    if (Compare(pieceNumber, red3) == 1)
                        check = false;
            }
            if (red4 != null)
            {
                if ((area_position.top_left.x < red4.transform.position.x) && (area_position.top_left.y > red4.transform.position.y) && (area_position.top_left.z > red4.transform.position.z) &&
(area_position.bottom_right.x > red4.transform.position.x) && (area_position.bottom_right.y < red4.transform.position.y) && (area_position.bottom_right.z < red4.transform.position.z))
                    if (Compare(pieceNumber, red4) == 1)
                        check = false;
            }
            if (red5 != null)
            {
                if ((area_position.top_left.x < red5.transform.position.x) && (area_position.top_left.y > red5.transform.position.y) && (area_position.top_left.z > red5.transform.position.z) &&
(area_position.bottom_right.x > red5.transform.position.x) && (area_position.bottom_right.y < red5.transform.position.y) && (area_position.bottom_right.z < red5.transform.position.z))
                    if (Compare(pieceNumber, red5) == 1)
                        check = false;
            }
            if (red6 != null)
            {
                if ((area_position.top_left.x < red6.transform.position.x) && (area_position.top_left.y > red6.transform.position.y) && (area_position.top_left.z > red6.transform.position.z) &&
(area_position.bottom_right.x > red6.transform.position.x) && (area_position.bottom_right.y < red6.transform.position.y) && (area_position.bottom_right.z < red6.transform.position.z))
                    if (Compare(pieceNumber, red6) == 1)
                        check = false;
            }
            return check;
        }
        
        public int Compare(int pieceBeingPut, GameObject pieceAlreadyPut)
        {
            string number = Regex.Match(pieceAlreadyPut.name, @"\d+").Value;
            if (int.Parse(number) < pieceBeingPut)
                return 1;
            return 0;
        }
        public void Delete(Object_Coordinates area_position)
        {
            GameObject blue1 = GameObject.Find("Blue 1(Clone)");
            GameObject blue2 = GameObject.Find("Blue 2(Clone)");
            GameObject blue3 = GameObject.Find("Blue 3(Clone)");
            GameObject blue4 = GameObject.Find("Blue 4(Clone)");
            GameObject blue5 = GameObject.Find("Blue 5(Clone)");
            GameObject blue6 = GameObject.Find("Blue 6(Clone)");
            GameObject red1 = GameObject.Find("Red 1(Clone)");
            GameObject red2 = GameObject.Find("Red 2(Clone)");
            GameObject red3 = GameObject.Find("Red 3(Clone)");
            GameObject red4 = GameObject.Find("Red 4(Clone)");
            GameObject red5 = GameObject.Find("Red 5(Clone)");
            GameObject red6 = GameObject.Find("Red 6(Clone)");
            if (blue1 != null)
            {
                if ((area_position.top_left.x < blue1.transform.position.x) && (area_position.top_left.y > blue1.transform.position.y) && (area_position.top_left.z > blue1.transform.position.z) &&
                        (area_position.bottom_right.x > blue1.transform.position.x) && (area_position.bottom_right.y < blue1.transform.position.y) && (area_position.bottom_right.z < blue1.transform.position.z))
                {
                    DestroyServerRpc("Blue 1(Clone)");
                }
            }
            if (blue2 != null)
            {
                if ((area_position.top_left.x < blue2.transform.position.x) && (area_position.top_left.y > blue2.transform.position.y) && (area_position.top_left.z > blue2.transform.position.z) &&
        (area_position.bottom_right.x > blue2.transform.position.x) && (area_position.bottom_right.y < blue2.transform.position.y) && (area_position.bottom_right.z < blue2.transform.position.z))
                    DestroyServerRpc("Blue 2(Clone)");
            }
            if (blue3 != null)
            {
                if ((area_position.top_left.x < blue3.transform.position.x) && (area_position.top_left.y > blue3.transform.position.y) && (area_position.top_left.z > blue3.transform.position.z) &&
            (area_position.bottom_right.x > blue3.transform.position.x) && (area_position.bottom_right.y < blue3.transform.position.y) && (area_position.bottom_right.z < blue3.transform.position.z))
                    DestroyServerRpc("Blue 3(Clone)");
            }
            if (blue4 != null)
            {
                if ((area_position.top_left.x < blue4.transform.position.x) && (area_position.top_left.y > blue4.transform.position.y) && (area_position.top_left.z > blue4.transform.position.z) &&
        (area_position.bottom_right.x > blue4.transform.position.x) && (area_position.bottom_right.y < blue4.transform.position.y) && (area_position.bottom_right.z < blue4.transform.position.z))
                    DestroyServerRpc("Blue 4(Clone)");
            }
            if (blue5 != null)
            {
                if ((area_position.top_left.x < blue5.transform.position.x) && (area_position.top_left.y > blue5.transform.position.y) && (area_position.top_left.z > blue5.transform.position.z) &&
        (area_position.bottom_right.x > blue5.transform.position.x) && (area_position.bottom_right.y < blue5.transform.position.y) && (area_position.bottom_right.z < blue5.transform.position.z))
                    DestroyServerRpc("Blue 5(Clone)");
            }
            if (blue6 != null)
            {
                if ((area_position.top_left.x < blue6.transform.position.x) && (area_position.top_left.y > blue6.transform.position.y) && (area_position.top_left.z > blue6.transform.position.z) &&
        (area_position.bottom_right.x > blue6.transform.position.x) && (area_position.bottom_right.y < blue6.transform.position.y) && (area_position.bottom_right.z < blue6.transform.position.z))
                    DestroyServerRpc("Blue 6(Clone)");
            }
            if (red1 != null)
            {
                if ((area_position.top_left.x < red1.transform.position.x) && (area_position.top_left.y > red1.transform.position.y) && (area_position.top_left.z > red1.transform.position.z) &&
        (area_position.bottom_right.x > red1.transform.position.x) && (area_position.bottom_right.y < red1.transform.position.y) && (area_position.bottom_right.z < red1.transform.position.z))
                    DestroyServerRpc("Red 1(Clone)");
            }
            if (red2 != null)
            {
                if ((area_position.top_left.x < red2.transform.position.x) && (area_position.top_left.y > red2.transform.position.y) && (area_position.top_left.z > red2.transform.position.z) &&
        (area_position.bottom_right.x > red2.transform.position.x) && (area_position.bottom_right.y < red2.transform.position.y) && (area_position.bottom_right.z < red2.transform.position.z))
                    DestroyServerRpc("Red 2(Clone)");
            }
            if (red3 != null)
            {
                if ((area_position.top_left.x < red3.transform.position.x) && (area_position.top_left.y > red3.transform.position.y) && (area_position.top_left.z > red3.transform.position.z) &&
        (area_position.bottom_right.x > red3.transform.position.x) && (area_position.bottom_right.y < red3.transform.position.y) && (area_position.bottom_right.z < red3.transform.position.z))
                    DestroyServerRpc("Red 3(Clone)");
            }
            if (red4 != null)
            {
                if ((area_position.top_left.x < red4.transform.position.x) && (area_position.top_left.y > red4.transform.position.y) && (area_position.top_left.z > red4.transform.position.z) &&
(area_position.bottom_right.x > red4.transform.position.x) && (area_position.bottom_right.y < red4.transform.position.y) && (area_position.bottom_right.z < red4.transform.position.z))
                    DestroyServerRpc("Red 4(Clone)");
            }
            if (red5 != null)
            {
                if ((area_position.top_left.x < red5.transform.position.x) && (area_position.top_left.y > red5.transform.position.y) && (area_position.top_left.z > red5.transform.position.z) &&
(area_position.bottom_right.x > red5.transform.position.x) && (area_position.bottom_right.y < red5.transform.position.y) && (area_position.bottom_right.z < red5.transform.position.z))
                    DestroyServerRpc("Red 5(Clone)");
            }
            if (red6 != null)
            {
                if ((area_position.top_left.x < red6.transform.position.x) && (area_position.top_left.y > red6.transform.position.y) && (area_position.top_left.z > red6.transform.position.z) &&
(area_position.bottom_right.x > red6.transform.position.x) && (area_position.bottom_right.y < red6.transform.position.y) && (area_position.bottom_right.z < red6.transform.position.z))
                    DestroyServerRpc("Red 6(Clone)");
            }
        }


        [ServerRpc(RequireOwnership = false)]
        void DestroyServerRpc(string obj)
        {
            GameObject piece = GameObject.Find(obj);
            Destroy(piece);
        }
        [ServerRpc]
        void changeTurnServerRpc(bool whoseTurn)
        {
            if (whoseTurn)
            turn_Value.Value = redTurnPosition;
            if (!whoseTurn)
                turn_Value.Value = blueTurnPosition;
        }

        [ServerRpc(RequireOwnership = false)]
        void moveBluePieceServerRpc(int pieceNumber, Vector3 position)
        {
            if (pieceNumber == 1)
                Blue1Position.Value = position;
            if (pieceNumber == 2)
                Blue2Position.Value = position;
            if (pieceNumber == 3)
                Blue3Position.Value = position;
            if (pieceNumber == 4)
                Blue4Position.Value = position;
            if (pieceNumber == 5)
                Blue5Position.Value = position;
            if (pieceNumber == 6)
                Blue6Position.Value = position;
        }

        [ServerRpc(RequireOwnership = false)]
        void moveRedPieceServerRpc(int pieceNumber, Vector3 position)
        {
            if (pieceNumber == 1)
                Red1Position.Value = position;
            if (pieceNumber == 2)
                Red2Position.Value = position;
            if (pieceNumber == 3)
                Red3Position.Value = position;
            if (pieceNumber == 4)
                Red4Position.Value = position;
            if (pieceNumber == 5)
                Red5Position.Value = position;
            if (pieceNumber == 6)
                Red6Position.Value = position;
        }

        [ServerRpc]
        void hidePlayerPrefabServerRpc(ServerRpcParams rpcParams = default)
        {
            Position.Value = new Vector3(0, -5, 0);
        }

        [ServerRpc(RequireOwnership = false)]
        void SpawnPlayersServerRpc(ServerRpcParams rpcParams = default)
        {
            GameObject blue1Obj = Instantiate(blue1, Vector3.zero, Quaternion.identity);
            blue1Obj.GetComponent<NetworkObject>().Spawn();
            Blue1Position.Value = new Vector3(16f, 2f, -21.9f);

            GameObject blue2Obj = Instantiate(blue2, Vector3.zero, Quaternion.identity);
            blue2Obj.GetComponent<NetworkObject>().Spawn();
            Blue2Position.Value = new Vector3(12, 2f, -21.9f);

            GameObject blue3Obj = Instantiate(blue3, Vector3.zero, Quaternion.identity);
            blue3Obj.GetComponent<NetworkObject>().Spawn();
            Blue3Position.Value = new Vector3(8f, 2f, -21.9f);

            GameObject blue4Obj = Instantiate(blue4, Vector3.zero, Quaternion.identity);
            blue4Obj.GetComponent<NetworkObject>().Spawn();
            Blue4Position.Value = new Vector3(4f, 2f, -21.9f);

            GameObject blue5Obj = Instantiate(blue5, Vector3.zero, Quaternion.identity);
            blue5Obj.GetComponent<NetworkObject>().Spawn();
            Blue5Position.Value = new Vector3(0f, 2f, -21.9f);

            GameObject blue6Obj = Instantiate(blue6, Vector3.zero, Quaternion.identity);
            blue6Obj.GetComponent<NetworkObject>().Spawn();
            Blue6Position.Value = new Vector3(-4f, 2f, -21.9f);

            GameObject red1Obj = Instantiate(red1, Vector3.zero, Quaternion.identity);
            red1Obj.GetComponent<NetworkObject>().Spawn();
            Red1Position.Value = new Vector3(-28f, 2f, 22.6f);

            GameObject red2Obj = Instantiate(red2, Vector3.zero, Quaternion.identity);
            red2Obj.GetComponent<NetworkObject>().Spawn();
            Red2Position.Value = new Vector3(-24, 2f, 22.6f);

            GameObject red3Obj = Instantiate(red3, Vector3.zero, Quaternion.identity);
            red3Obj.GetComponent<NetworkObject>().Spawn();
            Red3Position.Value = new Vector3(-20f, 2f, 22.6f);

            GameObject red4Obj = Instantiate(red4, Vector3.zero, Quaternion.identity);
            red4Obj.GetComponent<NetworkObject>().Spawn();
            Red4Position.Value = new Vector3(-16f, 2f, 22.6f);

            GameObject red5Obj = Instantiate(red5, Vector3.zero, Quaternion.identity);
            red5Obj.GetComponent<NetworkObject>().Spawn();
            Red5Position.Value = new Vector3(-12f, 2f, 22.6f);

            GameObject red6Obj = Instantiate(red6, Vector3.zero, Quaternion.identity);
            red6Obj.GetComponent<NetworkObject>().Spawn();
            Red6Position.Value = new Vector3(-8f, 2f, 22.6f);

            GameObject turnObj = Instantiate(turn_object, Vector3.zero, Quaternion.identity);
            turnObj.GetComponent<NetworkObject>().Spawn();
            turn_Value.Value = new Vector3(0f, 0f, 0f);

            GameObject winObj = Instantiate(winCondition_object, Vector3.zero, Quaternion.identity);
            winObj.GetComponent<NetworkObject>().Spawn();
            win_Value.Value = new Vector3(0f, 0f, 0f);

            GameObject turnCounterObj = Instantiate(turn_Counter, Vector3.zero, Quaternion.identity);
            turnCounterObj.GetComponent<NetworkObject>().Spawn();

            gameHasStarted.Value = true;
        }

        void Update()
        {
            check_for_draw();
            updateGrid();
            if (HelloWorldManager.isHost)
            {
                if (HelloWorldPlayer.gameHasStarted.Value)
                {
                    GameObject Player = GameObject.Find("Player(Clone)");
                    Player.transform.position = HelloWorldPlayer.Position.Value;
                    GameObject Blue1 = GameObject.Find("Blue 1(Clone)");
                    if (Blue1 != null)
                    Blue1.transform.position = HelloWorldPlayer.Blue1Position.Value;
                    GameObject Blue2 = GameObject.Find("Blue 2(Clone)");
                    if (Blue2 != null)
                        Blue2.transform.position = HelloWorldPlayer.Blue2Position.Value;
                    GameObject Blue3 = GameObject.Find("Blue 3(Clone)");
                    if (Blue3 != null)
                        Blue3.transform.position = HelloWorldPlayer.Blue3Position.Value;
                    GameObject Blue4 = GameObject.Find("Blue 4(Clone)");
                    if (Blue4 != null)
                        Blue4.transform.position = HelloWorldPlayer.Blue4Position.Value;
                    GameObject Blue5 = GameObject.Find("Blue 5(Clone)");
                    if (Blue5 != null)
                        Blue5.transform.position = HelloWorldPlayer.Blue5Position.Value;
                    GameObject Blue6 = GameObject.Find("Blue 6(Clone)");
                    if (Blue6 != null)
                        Blue6.transform.position = HelloWorldPlayer.Blue6Position.Value;
                    GameObject Red1 = GameObject.Find("Red 1(Clone)");
                    if (Red1 != null)
                        Red1.transform.position = HelloWorldPlayer.Red1Position.Value;
                    GameObject Red2 = GameObject.Find("Red 2(Clone)");
                    if (Red2 != null)
                        Red2.transform.position = HelloWorldPlayer.Red2Position.Value;
                    GameObject Red3 = GameObject.Find("Red 3(Clone)");
                    if (Red3 != null)
                        Red3.transform.position = HelloWorldPlayer.Red3Position.Value;
                    GameObject Red4 = GameObject.Find("Red 4(Clone)");
                    if (Red4 != null)
                        Red4.transform.position = HelloWorldPlayer.Red4Position.Value;
                    GameObject Red5 = GameObject.Find("Red 5(Clone)");
                    if (Red5 != null)
                        Red5.transform.position = HelloWorldPlayer.Red5Position.Value;
                    GameObject Red6 = GameObject.Find("Red 6(Clone)");
                    if (Red6 != null)
                        Red6.transform.position = HelloWorldPlayer.Red6Position.Value;
                    GameObject turn_obj = GameObject.Find("turn_Object(Clone)");
                    turn_obj.transform.position = turn_Value.Value;

                    GameObject win_obj = GameObject.Find("WinCondition(Clone)");
                    win_obj.transform.position = win_Value.Value;
                }
            }
        }

    }
    public class Coordinates : MonoBehaviour
    {
        public static Vector3 p1 = new Vector3(-20f, 0, 13.6f);
        public static Vector3 p2 = new Vector3(-6.6f, 0, 13.6f);
        public static Vector3 p3 = new Vector3(5.5f, 0, 13.6f);
        public static Vector3 p4 = new Vector3(-20f, 0, 0.8f);
        public static Vector3 p5 = new Vector3(-6.6f, 0, 0.8f);
        public static Vector3 p6 = new Vector3(5.5f, 0, 0.8f);
        public static Vector3 p7 = new Vector3(-20f, 0, -11.3f);
        public static Vector3 p8 = new Vector3(-6.6f, 0, -11.3f);
        public static Vector3 p9 = new Vector3(5.5f, 0, -11.3f);
    }

    public class Area_Coordinates : MonoBehaviour
    {
        public static Object_Coordinates p1 = new Object_Coordinates(new Vector3(-22, 2, 15), new Vector3(-10, -2, 4));
        public static Object_Coordinates p2 = new Object_Coordinates(new Vector3(-10, 2, 15), new Vector3(0, -2, 5));
        public static Object_Coordinates p3 = new Object_Coordinates(new Vector3(0, 2, 15), new Vector3(11, -2, 5));
        public static Object_Coordinates p4 = new Object_Coordinates(new Vector3(-22, 2, 4.5f), new Vector3(-10, -2, -4));
        public static Object_Coordinates p5 = new Object_Coordinates(new Vector3(-10, 2, 5), new Vector3(1, -2, -5));
        public static Object_Coordinates p6 = new Object_Coordinates(new Vector3(1, 2, 5), new Vector3( 11, -2, -5));
        public static Object_Coordinates p7 = new Object_Coordinates(new Vector3(-21, 2, -5), new Vector3(-10, -2, -15));
        public static Object_Coordinates p8 = new Object_Coordinates(new Vector3(-10.5f, 2, -5), new Vector3(0.7f, -2, -15));
        public static Object_Coordinates p9 = new Object_Coordinates(new Vector3(0.7f, 2, -5), new Vector3(11.4f, -2, -15));
    }

    public class Object_Coordinates
    {
        public Vector3 top_left { get; set; }
        public Vector3 bottom_right { get; set; }

        public Object_Coordinates(Vector3 top_left, Vector3 bottom_right)
        {
            this.top_left = top_left;
            this.bottom_right = bottom_right;
        }
        public Object_Coordinates()
        {
            this.top_left = new Vector3(0, 0, 0);
            this.bottom_right = new Vector3(0, 0, 0);
        }
    }
}