
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum PlanetTypes
{
    Asteroid01, Asteroid02, Asteroid03, PlanetOfMushrooms,
    RockAndGrass, RockGrassTree, Rock, TreeSapling,
    Grass, Water, LevitatingMushroom, Sun, LevitatingApple,
    RockWater, Jupiter, Soil, Earth, None
}
public static class PlanetCollisionsStore
{
    public static List<List<PlanetTypes>> collisionTypes = new List<List<PlanetTypes>>()
    {
        new List<PlanetTypes>(){PlanetTypes.Soil, PlanetTypes.Water, PlanetTypes.Grass},     
        new List<PlanetTypes>(){PlanetTypes.Grass, PlanetTypes.Water, PlanetTypes.Earth},
        new List<PlanetTypes>(){PlanetTypes.Earth, PlanetTypes.LevitatingMushroom, PlanetTypes.PlanetOfMushrooms},
        
        new List<PlanetTypes>(){PlanetTypes.Water, PlanetTypes.Rock, PlanetTypes.RockWater},
        new List<PlanetTypes>(){PlanetTypes.Grass, PlanetTypes.RockWater, PlanetTypes.RockAndGrass},
        new List<PlanetTypes>(){PlanetTypes.RockAndGrass, PlanetTypes.TreeSapling, PlanetTypes.RockGrassTree},
    };

    public static PlanetTypes GetNewPlanetType(PlanetTypes p1, PlanetTypes p2)
    {
        //return all lists that contain p1 and p2 at indices 0 or 1, not 2
        Debug.Log(p1);
        Debug.Log(p2);
        var list = collisionTypes.FirstOrDefault(x => (x.IndexOf(p1)==0 && x.IndexOf(p2)==1) || (x.IndexOf(p1)==1 && x.IndexOf(p2)==0));
        
        if (list == null || list.Count==0)
            return PlanetTypes.None;
        return list[2];
    }
}
