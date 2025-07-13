using System;
using System.Collections;
using System.Collections.Generic;
using Resurces;
using Unity.VisualScripting;
using UnityEngine;

public class ResurcesManager : Singleton<ResurcesManager>
{
   public GameObject Player;

   private void OnEnable()
   {
      HealResurce.SendMessage += AddResurces;
      DamageResurce.SendMessage += AddResurces;
      EssenceResurs.SendMessage += AddResurces;
      EssenceBoxResurce.SendMessage += AddResurces;
      TeleportCloudRecurce.SendMessage += AddResurces;
      TeleportCristalResurce.SendMessage += AddResurces;
      TeleportFlowerResurce.SendMessage += AddResurces;
   }

   private void OnDisable()
   {
      HealResurce.SendMessage -= AddResurces;
      DamageResurce.SendMessage -= AddResurces;
      EssenceResurs.SendMessage -= AddResurces;
      EssenceBoxResurce.SendMessage -= AddResurces;
      TeleportCloudRecurce.SendMessage -= AddResurces;
      TeleportCristalResurce.SendMessage -= AddResurces;
      TeleportFlowerResurce.SendMessage -= AddResurces;
   }

   public void AddResurces(int[] IndexMass)
   {
      switch (IndexMass[0])
      {
         case 0:
            Player.GetComponent<PlayerStats>().playerHP += IndexMass[1];
            break;
         case 1:
            Player.GetComponent<PlayerStats>().playerKillPower += IndexMass[1];
            break;
         case 2:
            Player.GetComponent<PlayerStats>().playerEssention += IndexMass[1];
            break;
         case 3:
            Player.GetComponent<PlayerStats>().playerMaxEssention += IndexMass[1];
            break;
         case 4:
            Player.GetComponent<PlayerStats>().playerFlowerTeleportation += IndexMass[1];
            break;
         case 5:
            Player.GetComponent<PlayerStats>().playerCristalTeleportation += IndexMass[1];
            break;
         case 6:
            Player.GetComponent<PlayerStats>().playerCloudTeleportation += IndexMass[1];
            break;
      }
   }

   public void RemoveResurces(int ResurceIndex, int ResurcesValue)
   {
      switch (ResurceIndex)
      {
         case 0:
            Player.GetComponent<PlayerStats>().playerHP -= ResurcesValue;
            break;
         case 1:
            Player.GetComponent<PlayerStats>().playerKillPower -= ResurcesValue;
            break;
         case 2:
            Player.GetComponent<PlayerStats>().playerEssention -= ResurcesValue;
            break;
         case 3:
            Player.GetComponent<PlayerStats>().playerMaxEssention -= ResurcesValue;
            break;
         case 4:
            Player.GetComponent<PlayerStats>().playerFlowerTeleportation -= ResurcesValue;
            break;
         case 5:
            Player.GetComponent<PlayerStats>().playerCristalTeleportation -= ResurcesValue;
            break;
         case 6:
            Player.GetComponent<PlayerStats>().playerCloudTeleportation -= ResurcesValue;
            break;
      }
   }
}
