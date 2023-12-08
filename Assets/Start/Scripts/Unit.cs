/*
Moteur : Unity 22.3.11f1
Développeur : CHANOT Alexandre
Date de création : 28/11/2023
Date de dernière modification : 28/11/2023
Description : Classe Unit qui réprensente une unité dans le jeu. Elle étend la classe MonoBehaviour de Unity et sera la classe mère de toutes les unités du jeu.
*/

using UnityEngine;
using System.Collections.Generic;

public class Unit : MonoBehaviour
{
    // Attributs de la classe Unit
    
    // protected Game game;

    private bool isMoved; // Permet de savoir si l'unité a déjà bougé
    private bool isSelected; // Permet de savoir si l'unité est sélectionnée
    private bool waitingForAction; // Permet de savoir si l'unité attend une action
    private bool waitingForMove; // Permet de savoir si l'unité attend un déplacement

    // private Building onBuilding; // Permet de savoir si l'unité est sur un bâtiment

    private float range; // Permet de savoir la portée de l'unité
    private float attackRange; // Permet de savoir la portée d'attaque de l'unité
    private float attack; // Permet de savoir la force de l'unité
    private float defense; // Permet de savoir la défense de l'unité
    private float hitPoints; // Permet de savoir le nombre de points de vie de l'unité

    public int team; // Permet de savoir l'équipe de l'unité

    private List<Unit> unitsInRange; // Permet de savoir les unités à portée de l'unité
    private Unit currentTarget; // Permet de savoir l'unité ciblée par l'unité

    private Vector3 StartPosition; // Permet de savoir la position de départ de l'unité
    private float speed; // Permet de savoir la vitesse de déplacement de l'unité
}



