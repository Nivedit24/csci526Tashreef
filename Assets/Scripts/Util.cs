using System.Collections;
using System.Collections.Generic;
using UnityEngine;


class Util
{
    public static Dictionary<string, int> enemyTagToDamage = new Dictionary<string, int>()
{
    {"Tornado", 10},
    {"DeathFloor",25},
    {"DemonFireball", 25},
    {"lightning", 25},
    {"EarthMonster", 25},
    {"AcidDrop", 5},
    {"IceMonster", 10},
    {"VolcanoBall", 25},
    {"WaterBody", 5},
    {"Demon", 20},
    {"Boulder", 10},
    {"BossFireball", 50},
    {"BossBoulder", 20}
};

}
public enum State
{
    Normal, Hover, Shielded, Dead, Gone
}

public enum Power
{
    Air, Fire, Water, Earth
}


