using Server;
using System;
using System.Collections.Generic;
using System.Text;
using static Define;

class Player
{
    //public ClientSession Session { get; }

    public string Name { get; set; }
    protected MoveDir _moveDir;
    public float PosX { get; set; }
    public float PosY { get; set; }
    public float SpeedPerSec { get; set; }



}

