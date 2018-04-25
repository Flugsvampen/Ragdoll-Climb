﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using XInputDotNetPure;

public class PlayerInfoSingleton : MonoBehaviour
{
    private static PlayerInfoSingleton m_instance;

    public static PlayerInfoSingleton instance
    {
        get
        {
            // If an instance of this script doesn't exist already
            if (m_instance == null)
            {
                // Creates a GameObject based of this class
                //GameObject prefab = (GameObject)Resources.Load("PlayerInfoSingleton");
                GameObject prefab = new GameObject();
                prefab.AddComponent<PlayerInfoSingleton>();
                // Instantiates the created GameObject
                GameObject created = Instantiate(prefab);
                created.name = "PlayerInfoSingleton";
                // Prevents the object from being destroyed when changing scenes
                DontDestroyOnLoad(created);
                // Assigns an instance of this script
                m_instance = created.GetComponent<PlayerInfoSingleton>();
            }

            return m_instance;
        }
    }

    public bool debug = true;
	public int playerAmount = 0;
    public List<PlayerIndex> playerIndexes;
    public Color[] colors = new Color[4];
    public int[] characterIndex = new int[4];
    public string selectedLevel = "RandomGeneratedLevelWithPrefabs";

    // Multiplayer
    public enum Difficulties { VeryEasy, Easy, Medium, Hard, VeryHard, Mix }
    public enum Lengths { Short = 1,  Medium, Long, Humongous, Gigantic }
    public Difficulties levelDifficulty = Difficulties.Mix;
    public Lengths levelLength = Lengths.Medium;

    // Singleplayer
    public List<List<int>> stars = new List<List<int>>();

    
    public void Save()
    {
        // Binary formatter that will serialize the Data class
        BinaryFormatter bf = new BinaryFormatter();

        // The file that the data will be stored in
        FileStream file = File.Create(Application.persistentDataPath + "/savefile.dat");

        // Creates an instance of PlayerData
        Data data = new Data();

        // Assigns all variables in Data to its correspondants values in this singleton
        data.stars = stars;

        // Stores the data in the file
        bf.Serialize(file, data);

        // Closes file
        file.Close();
    }


    public void Load()
    {
        // Checks if save file exists
        if (File.Exists(Application.persistentDataPath + "/savefile.dat"))
        {
            // Binary formatter that will deserialize the save file
            BinaryFormatter bf = new BinaryFormatter();

            // An instance of the save file
            FileStream file = File.Open(Application.persistentDataPath + "/savefile.dat", FileMode.Open);

            // An instance of the PlayerData class that is initialized with the savefiles data
            Data data = (Data)bf.Deserialize(file);

            // Closes file
            file.Close();

            // Assigns all variables to be saved/loaded in this singleton to its correspondant values in Data
            stars = data.stars;
        }
    }


    [System.Serializable]
    class Data
    {
        public List<List<int>> stars = new List<List<int>>();
    }
}
